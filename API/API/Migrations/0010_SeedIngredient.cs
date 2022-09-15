using FluentMigrator;

namespace API.Migrations
{
    [Migration(10)]
    public class _0010_SeedIngredient : Migration
    {
        public record ingredient
        {
            public Guid id { get; set; }
            public string data { get; set; }
            public Guid recipe_id { get; set; }
            public bool is_active { get; set; }
        }
        List<ingredient> ingredients = new()
        {
            new ingredient
            {
                id = Guid.NewGuid(),
                data = "ing1 for test1",
                recipe_id = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                is_active = true
            },
            new ingredient
            {
                id = Guid.NewGuid(),
                data = "ing2 for test1",
                recipe_id = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                is_active= true
            },
            new ingredient
            {
                id = Guid.NewGuid(),
                data = "ing1 for test2",
                recipe_id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                is_active = true
            },
            new ingredient
            {
                id = Guid.NewGuid(),
                data = "ing2 for test2",
                recipe_id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                is_active=true
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (ingredient ingredient in ingredients)
            {
                Insert.IntoTable(tableName: "ingredient").Row(ingredient);
            }
        }
    }
}
