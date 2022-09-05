using FluentMigrator;

namespace Backend.Migrations
{
    [Migration(7)]
    public class AddAllCategoriesTable_7 : Migration
    {
        public override void Down()
        {
            Delete.Table("AllCategories");
        }

        public override void Up()
        {
            Create.Table("AllCategories")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Data").AsString().NotNullable();
        }
    }
}
