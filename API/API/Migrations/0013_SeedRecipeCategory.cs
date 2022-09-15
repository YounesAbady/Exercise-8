using FluentMigrator;

namespace API.Migrations
{
    [Migration(13)]
    public class _0013_SeedRecipeCategory : Migration
    {
        public record recipe_category
        {
            public Guid id { get; set; }
            public string data { get; set; }
            public Guid recipe_id { get; set; }
            public bool is_active { get; set; }
        }
        List<recipe_category> recipeCategories = new()
        {
            new recipe_category
            {
                id = Guid.NewGuid(),
                data = "French",
                recipe_id = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                is_active = true
            },
            new recipe_category
            {
                id = Guid.NewGuid(),
                data = "English",
                recipe_id = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                is_active=true
            },
            new recipe_category
            {
                id = Guid.NewGuid(),
                data = "Italian",
                recipe_id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                is_active = true
            },
            new recipe_category
            {
                id = Guid.NewGuid(),
                data = "Egyptian",
                recipe_id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                is_active = true
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (recipe_category recipeCategory in recipeCategories)
            {
                Insert.IntoTable(tableName: "recipe_category").Row(recipeCategory);
            }
        }
    }
}
