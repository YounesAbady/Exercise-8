using FluentMigrator;

namespace API.Migrations
{
    [Migration(5)]
    public class _0005_AddInstruction : Migration
    {
        public override void Down()
        {
            Delete.Table("instruction");
        }

        public override void Up()
        {
            Create.Table("instruction")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("data").AsString().NotNullable()
                .WithColumn("recipe_id").AsGuid().NotNullable().ForeignKey("recipe", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("is_active").AsBoolean().NotNullable();
        }
    }
}
