using FluentMigrator;

namespace API.Migrations
{
    [Migration(5)]
    public class _0005_AddInstruction : Migration
    {
        public override void Down()
        {
            Delete.Table("Instruction");
        }

        public override void Up()
        {
            Create.Table("Instruction")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Data").AsString().NotNullable()
                .WithColumn("RecipeId").AsGuid().NotNullable().ForeignKey("Recipe", "Id").OnDelete(System.Data.Rule.Cascade);
        }
    }
}
