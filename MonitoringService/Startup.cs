using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonitoringService.Application;
using MonitoringService.Domain;
using MonitoringService.Infrastructure;
using MonitoringService.Infrastructure.Gateways;
using MonitoringService.Infrastructure.Repositories;

namespace MonitoringService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // The serializer settings are added to avoid cycles in the object graph
            // (example: post -> blog -> post -> blog ...)
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // environment variables check
            var connectionString = Environment.GetEnvironmentVariable("MONITORING_POSTGRES_CONNECTION_STRING");
            if (connectionString == null)
            {
                // The below commented line should be used when using EF Core to generate migrations, as the tool does
                // not use environment variables and therefore the connection string is never found.
                // connectionString =
                //     "Host=localhost;Port=5433;Database=monitoring_db;Username=monitoring;Password=Monitoring_database_password1";
                Console.WriteLine("'MONITORING_POSTGRES_CONNECTION_STRING' Database Connection string not found");
                System.Environment.Exit(1);
            }

            services.AddDbContext<DockerHostContext>(options =>
            {
                options.UseNpgsql(connectionString, options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null
                ));
            });

            services.AddTransient<IDockerHostRepository, DockerHostRepository>();
            services.AddTransient<IDockerContainerRepository, DockerContainerRepository>();
            services.AddTransient<IStatsRecordRepository, StatsRecordRepository>();
            services.AddTransient<IStatusRecordRepository, StatusRecordRepository>();
            services.AddTransient<IKafkaProducerGateway, KafkaProducerGateway>();

            services.AddScoped<IDockerHostService, DockerHostService>();
            services.AddScoped<IDockerContainerService, DockerContainerService>();
            services.AddScoped<IStatsRecordService, StatsRecordService>();
            services.AddScoped<IStatusRecordService, StatusRecordService>();

            services.AddHostedService<KafkaConsumerBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            await UpdateDatabase(app, logger);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // Ensures an updated database to the latest migration
        private async Task UpdateDatabase(IApplicationBuilder app, ILogger<Startup> logger)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DockerHostContext>())
                {
                    // Npgsql resiliency strategy does not work with Database.EnsureCreated() and Database.Migrate().
                    // Therefore a retry pattern is implemented for this purpose 
                    // if database connection is not ready it will retry 3 times before finally quiting
                    var retryCount = 3;
                    var currentRetry = 0;
                    while (true)
                    {
                        try
                        {
                            logger.LogInformation("Attempting database migration");

                            context.Database.Migrate();

                            logger.LogInformation("Database migration & connection successful");

                            break; // just break if migration is successful
                        }
                        catch (Npgsql.NpgsqlException)
                        {
                            logger.LogError("Database migration failed. Retrying in 5 seconds ...");

                            currentRetry++;

                            if (currentRetry == retryCount
                            ) // Here it is possible to check the type of exception if needed with an OR. And exit if it's a specific exception.
                            {
                                // We have tried as many times as retryCount specifies. Now we throw it and exit the application
                                logger.LogCritical($"Database migration failed after {retryCount} retries");
                                throw;
                            }
                        }

                        // Waiting 5 seconds before trying again
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
                }
            }
        }
    }
}