using FluentMigrator;

namespace API.Migrations
{
    [Migration(9)]
    public class _0009_SeedRecipe : Migration
    {
        public record Recipe
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public Guid UserId { get; set; }
        }
        List<Recipe> recipes = new()
        {
            new Recipe
            {
                Id=Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                Title="Test1",
                UserId=Guid.Parse("0b89bb77-33ef-471d-8390-59b3969d86ae")
            },
            new Recipe
            {
                Id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                Title = "Test2",
                UserId=Guid.Parse("0b89bb77-33ef-471d-8390-59b3969d86ae")
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (Recipe recipe in recipes)
            {
                Insert.IntoTable(tableName: "Recipe").Row(recipe);
            }
        }
    }
}
