using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class SeededDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0fceab49-2493-4df6-ad3a-82ca11467fce", "5da97df7-a66c-4084-a452-c43c5506a4f0", "User", "USER" },
                    { "8cceac4d-ec33-4f9d-b6e4-29833083b4e7", "a9c1da06-0a38-44d8-b17a-6dde890629bc", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "90ec3121-3328-42ef-8117-1569cf3e70a7", 0, "6c86160b-39d2-463c-823d-d5e7650ed81d", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", null, "AQAAAAEAACcQAAAAEKOw4T6m4JmdePueUV44ADveXn7Gpuc4bqrfaGruPjuTQrTVEoO7a39G7CVNq8jWmw==", null, false, "8abee83c-c147-4ee8-8bc0-d6e7c6afd142", false, null },
                    { "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab", 0, "ca933236-e1b4-4bd1-97cf-485b0c8bb6e1", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", null, "AQAAAAEAACcQAAAAEIH8RHpjenLDWfp4fInPK0EjO92Ydjpvvr4w7fxh54z28gcCDq0ImXdt8zJJrLYzhQ==", null, false, "a73cfbd9-47c0-4f44-bc24-d80f419caec9", false, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "0fceab49-2493-4df6-ad3a-82ca11467fce", "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "8cceac4d-ec33-4f9d-b6e4-29833083b4e7", "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0fceab49-2493-4df6-ad3a-82ca11467fce", "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8cceac4d-ec33-4f9d-b6e4-29833083b4e7", "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90ec3121-3328-42ef-8117-1569cf3e70a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0fceab49-2493-4df6-ad3a-82ca11467fce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8cceac4d-ec33-4f9d-b6e4-29833083b4e7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab");
        }
    }
}
