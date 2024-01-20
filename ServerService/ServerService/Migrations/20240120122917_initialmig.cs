using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerService.Migrations
{
    /// <inheritdoc />
    public partial class initialmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:ltree", ",,");

            migrationBuilder.CreateTable(
                name: "TreeItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    EntityValue = table.Column<string>(type: "text", nullable: false),
                    MaterializedPath = table.Column<string>(type: "ltree", nullable: false),
                    TreeKey = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sorting = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreeItems_EntityId",
                table: "TreeItems",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeItems_MaterializedPath",
                table: "TreeItems",
                column: "MaterializedPath")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_TreeItems_ParentEntityId",
                table: "TreeItems",
                column: "ParentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeItems_TreeKey",
                table: "TreeItems",
                column: "TreeKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreeItems");

            migrationBuilder.DropTable(
                name: "Trees");
        }
    }
}
