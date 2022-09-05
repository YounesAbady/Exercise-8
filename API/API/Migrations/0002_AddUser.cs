using FluentMigrator;

namespace Backend.Migrations
{
    [Migration(2)]
    public class AddUserTable_2 : Migration
    {
        public override void Down()
        {
            Delete.Table("User");
        }

        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable().Unique()
                .WithColumn("PasswordHash").AsString().NotNullable()
                .WithColumn("RefreshTokenId").AsGuid().Nullable().ForeignKey("RefreshToken", "Id");
        }
    }
}
