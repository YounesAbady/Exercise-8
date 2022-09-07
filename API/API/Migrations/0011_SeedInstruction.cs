using FluentMigrator;

namespace API.Migrations
{
    [Migration(11)]
    public class _0011_SeedInstruction : Migration
    {
        public record Instruction
        {
            public Guid Id { get; set; }
            public string Data { get; set; }
            public Guid RecipeId { get; set; }
        }
        List<Instruction> instructions = new()
        {
            new Instruction
            {
                Id = Guid.NewGuid(),
                Data = "ins1 for test1",
                RecipeId = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644")
            },
            new Instruction
            {
                Id = Guid.NewGuid(),
                Data = "ins2 for test1",
                RecipeId = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644")
            },
            new Instruction
            {
                Id = Guid.NewGuid(),
                Data = "ins1 for test2",
                RecipeId = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c")
            },
            new Instruction
            {
                Id = Guid.NewGuid(),
                Data = "ins2 for test2",
                RecipeId = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c")
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (Instruction instruction in instructions)
            {
                Insert.IntoTable(tableName: "Instruction").Row(instruction);
            }
        }
    }
}
