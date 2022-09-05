using FluentMigrator;

namespace Backend.Migrations
{
    [Migration(1)]
    public class AddRefreshTokenTable_1 : Migration
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
