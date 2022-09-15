using FluentMigrator;

namespace API.Migrations
{
    [Migration(4)]
    public class _0004_AddIngredient : Migration
    {
        public override void Down()
        {
            Delete.Table("ingredient");
        }

        public override void Up()
        {
            Create.Table("ingredient")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("data").AsString().NotNullable()
                .WithColumn("recipe_id").AsGuid().NotNullable().ForeignKey("recipe", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("is_active").AsBoolean().NotNullable(); 
        }
    }
}
