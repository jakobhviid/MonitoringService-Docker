using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonitoringService.Infrastructure.Gateways
{
    public class KafkaProducerGateway : IKafkaProducerGateway
    {
        public readonly string BootstrapServers =
            Environment.GetEnvironmentVariable("MONITORING_KAFKA_URL") ??
            "kafka1.cfei.dk:9092,kafka2.cfei.dk:9092,kafka3.cfei.dk:9092";
        private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        
        public async Task SendMessageAsync(string topic, object messageToSerialize, IProducer<Null, string> p)
        {
            try
            {
                var deliveryReport = await p.ProduceAsync(
                    topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(messageToSerialize, _jsonSettings) });

                Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
            }
        }
        
        // This method creates it own kafka producer and disposes it after it has been used
        public async Task SendMessageAsync(string topic, object messageToSerialize)
        {
            ProducerConfig producerConfig = new ProducerConfig { BootstrapServers = BootstrapServers, Acks = Acks.Leader };

            using (var p = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                try
                {
                    var deliveryReport = await p.ProduceAsync(
                        topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(messageToSerialize, _jsonSettings) });

                    Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");
                }
                catch (ProduceException<string, string> e)
                {
                    Console.WriteLine($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
                }
            }
        }
    }
}