using FluentMigrator;

namespace Backend.Migrations
{
    [Migration(5)]
    public class AddInstructionTable_5 : Migration
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
