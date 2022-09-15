using FluentMigrator;

namespace API.Migrations
{
    [Migration(9)]
    public class _0009_SeedRecipe : Migration
    {
        public record recipe
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public Guid user_id { get; set; }
            public bool is_active { get; set; }
        }
        List<recipe> recipes = new()
        {
            new recipe
            {
                id=Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                title="Test1",
                user_id=Guid.Parse("0b89bb77-33ef-471d-8390-59b3969d86ae"),
                is_active=true
            },
            new recipe
            {
                id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                title = "Test2",
                user_id=Guid.Parse("0b89bb77-33ef-471d-8390-59b3969d86ae"),
                is_active = true
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (recipe recipe in recipes)
            {
                Insert.IntoTable(tableName: "recipe").Row(recipe);
            }
        }
    }
}
