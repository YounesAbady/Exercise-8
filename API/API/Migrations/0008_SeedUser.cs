using FluentMigrator;
using Microsoft.AspNetCore.Identity;

namespace API.Migrations
{
    [Migration(8)]
    public class _0008_SeedUser : Migration
    {
        public record user
        {
            public Guid id { get; set; }
            public string name { get; set; }
            public string password_hash { get; set; }
            public Guid refresh_token_id { get; set; }
            public bool is_active { get; set; }
        }
        static PasswordHasher<user> hasher = new();
        public override void Down()
        {
        }

        public override void Up()
        {
            Insert.IntoTable(tableName: "user").Row(new
            {
                id = Guid.Parse("0b89bb77-33ef-471d-8390-59b3969d86ae"),
                name = "Abady",
                password_hash = hasher.HashPassword(new user(), "123"),
                is_active = true
            });
        }
    }
}
