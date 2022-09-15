using FluentMigrator;

namespace API.Migrations
{
    [Migration(2)]
    public class _0002_AddUser : Migration
    {
        public override void Down()
        {
            Delete.Table("user");
        }

        public override void Up()
        {
            Create.Table("user")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("name").AsString().NotNullable().Unique()
                .WithColumn("password_hash").AsString().NotNullable()
                .WithColumn("refresh_token_id").AsGuid().Nullable().ForeignKey("refresh_token", "id")
                .WithColumn("is_active").AsBoolean().NotNullable();
        }
    }
}
