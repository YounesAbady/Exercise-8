using FluentMigrator;

namespace API.Migrations
{
    [Migration(3)]
    public class _0003_AddRecipe : Migration
    {
        public override void Down()
        {
            Delete.Table("recipe");
        }

        public override void Up()
        {
            Create.Table("recipe")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("title").AsString().NotNullable()
                .WithColumn("user_id").AsGuid().NotNullable().ForeignKey("user", "id")
                .WithColumn("is_active").AsBoolean().NotNullable();
        }
    }
}
