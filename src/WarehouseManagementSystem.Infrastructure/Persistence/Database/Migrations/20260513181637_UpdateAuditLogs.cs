using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseManagementSystem.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportTicketEntity",
                table: "SupportTicketEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductEntity",
                table: "ProductEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditLogEntity",
                table: "AuditLogEntity");

            migrationBuilder.RenameTable(
                name: "SupportTicketEntity",
                newName: "SupportTickets");

            migrationBuilder.RenameTable(
                name: "ProductEntity",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "AuditLogEntity",
                newName: "AuditLogs");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTicketEntity_Status_Priority",
                table: "SupportTickets",
                newName: "IX_SupportTickets_Status_Priority");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTicketEntity_Status",
                table: "SupportTickets",
                newName: "IX_SupportTickets_Status");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTicketEntity_PublicId",
                table: "SupportTickets",
                newName: "IX_SupportTickets_PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTicketEntity_Priority",
                table: "SupportTickets",
                newName: "IX_SupportTickets_Priority");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTicketEntity_CreatedByUserId_Status",
                table: "SupportTickets",
                newName: "IX_SupportTickets_CreatedByUserId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTicketEntity_CreatedByUserId",
                table: "SupportTickets",
                newName: "IX_SupportTickets_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTicketEntity_CreatedAt",
                table: "SupportTickets",
                newName: "IX_SupportTickets_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEntity_SupplierId",
                table: "Products",
                newName: "IX_Products_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEntity_SKU",
                table: "Products",
                newName: "IX_Products_SKU");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEntity_Name",
                table: "Products",
                newName: "IX_Products_Name");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEntity_IsDeleted",
                table: "Products",
                newName: "IX_Products_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_ProductEntity_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogEntity_SubjectType_SubjectId",
                table: "AuditLogs",
                newName: "IX_AuditLogs_SubjectType_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogEntity_PublicId",
                table: "AuditLogs",
                newName: "IX_AuditLogs_PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogEntity_CreatedAt",
                table: "AuditLogs",
                newName: "IX_AuditLogs_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogEntity_ActorId",
                table: "AuditLogs",
                newName: "IX_AuditLogs_ActorId");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActorName",
                table: "AuditLogs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportTickets",
                table: "SupportTickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportTickets",
                table: "SupportTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "ActorName",
                table: "AuditLogs");

            migrationBuilder.RenameTable(
                name: "SupportTickets",
                newName: "SupportTicketEntity");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "ProductEntity");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                newName: "AuditLogEntity");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_Status_Priority",
                table: "SupportTicketEntity",
                newName: "IX_SupportTicketEntity_Status_Priority");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_Status",
                table: "SupportTicketEntity",
                newName: "IX_SupportTicketEntity_Status");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_PublicId",
                table: "SupportTicketEntity",
                newName: "IX_SupportTicketEntity_PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_Priority",
                table: "SupportTicketEntity",
                newName: "IX_SupportTicketEntity_Priority");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_CreatedByUserId_Status",
                table: "SupportTicketEntity",
                newName: "IX_SupportTicketEntity_CreatedByUserId_Status");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_CreatedByUserId",
                table: "SupportTicketEntity",
                newName: "IX_SupportTicketEntity_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportTickets_CreatedAt",
                table: "SupportTicketEntity",
                newName: "IX_SupportTicketEntity_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SupplierId",
                table: "ProductEntity",
                newName: "IX_ProductEntity_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SKU",
                table: "ProductEntity",
                newName: "IX_ProductEntity_SKU");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Name",
                table: "ProductEntity",
                newName: "IX_ProductEntity_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Products_IsDeleted",
                table: "ProductEntity",
                newName: "IX_ProductEntity_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "ProductEntity",
                newName: "IX_ProductEntity_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogs_SubjectType_SubjectId",
                table: "AuditLogEntity",
                newName: "IX_AuditLogEntity_SubjectType_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogs_PublicId",
                table: "AuditLogEntity",
                newName: "IX_AuditLogEntity_PublicId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogEntity",
                newName: "IX_AuditLogEntity_CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_AuditLogs_ActorId",
                table: "AuditLogEntity",
                newName: "IX_AuditLogEntity_ActorId");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportTicketEntity",
                table: "SupportTicketEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductEntity",
                table: "ProductEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditLogEntity",
                table: "AuditLogEntity",
                column: "Id");
        }
    }
}
