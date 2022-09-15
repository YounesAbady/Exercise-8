using FluentMigrator;

namespace API.Migrations
{
    [Migration(1)]
    public class _0001_AddRefreshToken : Migration
    {
        public override void Down()
        {
            Delete.Table("refresh_token");
        }

        public override void Up()
        {
            Create.Table("refresh_token")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("token").AsString().NotNullable()
                .WithColumn("time_created").AsDateTime().NotNullable()
                .WithColumn("time_expires").AsDateTime().NotNullable()
                .WithColumn("is_active").AsBoolean().NotNullable();
        }
    }
}
