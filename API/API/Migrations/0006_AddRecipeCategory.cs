using FluentMigrator;

namespace API.Migrations
{
    [Migration(6)]
    public class _0006_AddRecipeCategory : Migration
    {
        public override void Down()
        {
            Delete.Table("recipe_category");
        }

        public override void Up()
        {
            Create.Table("recipe_category")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("data").AsString().NotNullable()
                .WithColumn("recipe_id").AsGuid().NotNullable().ForeignKey("recipe", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("is_active").AsBoolean().NotNullable();
        }
    }
}
