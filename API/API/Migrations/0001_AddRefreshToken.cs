using FluentMigrator;

namespace API.Migrations
{
    [Migration(1)]
    public class _0001_AddRefreshToken : Migration
    {
        public override void Down()
        {
            Delete.Table("RefreshToken");
        }

        public override void Up()
        {
            Create.Table("RefreshToken")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Token").AsString().NotNullable()
                .WithColumn("TimeCreated").AsDateTime().NotNullable()
                .WithColumn("TimeExpires").AsDateTime().NotNullable();
        }
    }
}
