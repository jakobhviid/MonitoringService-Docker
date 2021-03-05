using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonitoringService.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DockerHosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerName = table.Column<string>(type: "text", nullable: false),
                    CommandRequestTopic = table.Column<string>(type: "text", nullable: false),
                    CommandResponseTopic = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_DockerHosts", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "DockerContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DockerHostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContainerId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DockerContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DockerContainers_DockerHosts_DockerHostId",
                        column: x => x.DockerHostId,
                        principalTable: "DockerHosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatsRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DockerContainerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuUsage = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    NumOfCpu = table.Column<int>(type: "integer", nullable: false),
                    SystemCpuUsage = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CpuPercentage = table.Column<double>(type: "double precision", nullable: false),
                    MemoryPercentage = table.Column<double>(type: "double precision", nullable: false),
                    NetInputBytes = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    NetOutputBytes = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DiskInputBytes = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    DiskOutputBytes = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatsRecords_DockerContainers_DockerContainerId",
                        column: x => x.DockerContainerId,
                        principalTable: "DockerContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DockerContainerId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Health = table.Column<string>(type: "text", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusRecords_DockerContainers_DockerContainerId",
                        column: x => x.DockerContainerId,
                        principalTable: "DockerContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DockerContainers_ContainerId_DockerHostId",
                table: "DockerContainers",
                columns: new[] {"ContainerId", "DockerHostId"},
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DockerContainers_DockerHostId",
                table: "DockerContainers",
                column: "DockerHostId");

            migrationBuilder.CreateIndex(
                name: "IX_DockerHosts_ServerName",
                table: "DockerHosts",
                column: "ServerName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatsRecords_DockerContainerId_UpdateTime",
                table: "StatsRecords",
                columns: new[] {"DockerContainerId", "UpdateTime"},
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusRecords_DockerContainerId_UpdateTime",
                table: "StatusRecords",
                columns: new[] {"DockerContainerId", "UpdateTime"},
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatsRecords");

            migrationBuilder.DropTable(
                name: "StatusRecords");

            migrationBuilder.DropTable(
                name: "DockerContainers");

            migrationBuilder.DropTable(
                name: "DockerHosts");
        }
    }
}