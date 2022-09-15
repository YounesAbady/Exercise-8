using FluentMigrator;

namespace API.Migrations
{
    [Migration(7)]
    public class _0007_AddCategory : Migration
    {
        public override void Down()
        {
            Delete.Table("category");
        }

        public override void Up()
        {
            Create.Table("category")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("data").AsString().NotNullable()
                .WithColumn("is_active").AsBoolean().NotNullable();
        }
    }
}
