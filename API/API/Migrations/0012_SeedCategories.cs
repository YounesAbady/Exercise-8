using FluentMigrator;

namespace API.Migrations
{
    [Migration(12)]
    public class _0012_SeedCategories : Migration
    {
        public record Category
        {
            public Guid Id { get; set; }
            public string Data { get; set; }
        }
        List<Category> categories = new()
        {
            new Category
            {
                Id = Guid.NewGuid(),
                Data = "French"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Data = "Egyption"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Data = "Italian"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Data = "English"
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (Category category in categories)
            {
                Insert.IntoTable(tableName: "Category").Row(category);
            }
        }
    }
}
