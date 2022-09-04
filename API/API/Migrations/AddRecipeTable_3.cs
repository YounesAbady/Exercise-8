using FluentMigrator;

namespace Backend.Migrations
{
    [Migration(3)]
    public class AddRecipeTable_3 : Migration
    {
        public override void Down()
        {
            Delete.Table("Recipe");
        }

        public override void Up()
        {
            Create.Table("Recipe")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("User", "Id");
        }
    }
}
