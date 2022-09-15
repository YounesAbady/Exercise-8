using FluentMigrator;

namespace API.Migrations
{
    [Migration(12)]
    public class _0012_SeedCategories : Migration
    {
        public record category
        {
            public Guid id { get; set; }
            public string data { get; set; }
            public bool is_active { get; set; }
        }
        List<category> categories = new()
        {
            new category
            {
                id = Guid.NewGuid(),
                data = "French",
                is_active = true
            },
            new category
            {
                id = Guid.NewGuid(),
                data = "Egyptian",
                is_active=true
            },
            new category
            {
                id = Guid.NewGuid(),
                data = "Italian",
                is_active=true
            },
            new category
            {
                id = Guid.NewGuid(),
                data = "English",
                is_active=true
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (category category in categories)
            {
                Insert.IntoTable(tableName: "category").Row(category);
            }
        }
    }
}
