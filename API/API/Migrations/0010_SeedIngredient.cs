using FluentMigrator;

namespace API.Migrations
{
    [Migration(10)]
    public class _0010_SeedIngredient : Migration
    {
        public record Ingredient
        {
            public Guid Id { get; set; }
            public string Data { get; set; }
            public Guid RecipeId { get; set; }
        }
        List<Ingredient> ingredients = new()
        {
            new Ingredient
            {
                Id = Guid.NewGuid(),
                Data = "ing1 for test1",
                RecipeId = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644")
            },
            new Ingredient
            {
                Id = Guid.NewGuid(),
                Data = "ing2 for test1",
                RecipeId = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644")
            },
            new Ingredient
            {
                Id = Guid.NewGuid(),
                Data = "ing1 for test2",
                RecipeId = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c")
            },
            new Ingredient
            {
                Id = Guid.NewGuid(),
                Data = "ing2 for test2",
                RecipeId = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c")
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (Ingredient ingredient in ingredients)
            {
                Insert.IntoTable(tableName: "Ingredient").Row(ingredient);
            }
        }
    }
}
