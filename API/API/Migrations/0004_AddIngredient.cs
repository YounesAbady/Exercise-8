using FluentMigrator;

namespace API.Migrations
{
    [Migration(4)]
    public class _0004_AddIngredient : Migration
    {
        public override void Down()
        {
            Delete.Table("Ingredient");
        }

        public override void Up()
        {
            Create.Table("Ingredient")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Data").AsString().NotNullable()
                .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("Recipe", "Id").OnDelete(System.Data.Rule.Cascade);
        }
    }
}
