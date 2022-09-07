using FluentMigrator;
using Microsoft.AspNetCore.Identity;

namespace API.Migrations
{
    [Migration(8)]
    public class _0008_SeedUser : Migration
    {
        public record User
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string PasswordHash { get; set; }
            public Guid RefreshTokenId { get; set; }
        }
        static PasswordHasher<User> hasher = new();
        public override void Down()
        {
        }

        public override void Up()
        {
            Insert.IntoTable(tableName: "User").Row(new
            {
                Id = Guid.Parse("0b89bb77-33ef-471d-8390-59b3969d86ae"),
                Name = "Abady",
                PasswordHash = hasher.HashPassword(new User(), "123")
            });
        }
    }
}
