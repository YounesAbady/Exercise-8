using FluentMigrator;

namespace API.Migrations
{
    [Migration(13)]
    public class _0013_SeedRecipeCategory : Migration
    {
        public record RecipeCategory
        {
            public Guid Id { get; set; }
            public string Data { get; set; }
            public Guid RecipeId { get; set; }
        }
        List<RecipeCategory> recipeCategories = new()
        {
            new RecipeCategory
            {
                Id = Guid.NewGuid(),
                Data = "French",
                RecipeId = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644")
            },
            new RecipeCategory
            {
                Id = Guid.NewGuid(),
                Data = "English",
                RecipeId = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644")
            },
            new RecipeCategory
            {
                Id = Guid.NewGuid(),
                Data = "Italian",
                RecipeId = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c")
            },
            new RecipeCategory
            {
                Id = Guid.NewGuid(),
                Data = "Egyption",
                RecipeId = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c")
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (RecipeCategory recipeCategory in recipeCategories)
            {
                Insert.IntoTable(tableName: "RecipeCategory").Row(recipeCategory);
            }
        }
    }
}
