using FluentMigrator;

namespace API.Migrations
{
    [Migration(6)]
    public class _0006_AddRecipeCategory : Migration
    {
        public override void Down()
        {
            Delete.Table("RecipeCategory");
        }

        public override void Up()
        {
            Create.Table("RecipeCategory")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Data").AsString().NotNullable()
                .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("Recipe", "Id").OnDelete(System.Data.Rule.Cascade);
        }
    }
}
