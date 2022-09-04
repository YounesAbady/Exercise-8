using FluentMigrator;

namespace Backend.Migrations
{
    [Migration(4)]
    public class AddIngredientTable_4 : Migration
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
                .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("Recipe", "Id");
        }
    }
}
