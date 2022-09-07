using FluentMigrator;

namespace API.Migrations
{
    [Migration(7)]
    public class _0007_AddCategory : Migration
    {
        public override void Down()
        {
            Delete.Table("Category");
        }

        public override void Up()
        {
            Create.Table("Category")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Data").AsString().NotNullable();
        }
    }
}
