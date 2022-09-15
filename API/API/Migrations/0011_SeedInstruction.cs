using FluentMigrator;

namespace API.Migrations
{
    [Migration(11)]
    public class _0011_SeedInstruction : Migration
    {
        public record instruction
        {
            public Guid id { get; set; }
            public string data { get; set; }
            public Guid recipe_id { get; set; }
            public bool is_active { get; set; }
        }
        List<instruction> instructions = new()
        {
            new instruction
            {
                id = Guid.NewGuid(),
                data = "ins1 for test1",
                recipe_id = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                is_active = true
            },
            new instruction
            {
                id = Guid.NewGuid(),
                data = "ins2 for test1",
                recipe_id = Guid.Parse("dd98a0ab-8a29-4ad0-a631-ff6dea396644"),
                is_active= true
            },
            new instruction
            {
                id = Guid.NewGuid(),
                data = "ins1 for test2",
                recipe_id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                is_active=true
            },
            new instruction
            {
                id = Guid.NewGuid(),
                data = "ins2 for test2",
                recipe_id = Guid.Parse("f35ce39f-2105-482e-80b6-bddd5b0f663c"),
                is_active=true
            }
        };
        public override void Down()
        {
        }

        public override void Up()
        {
            foreach (instruction instruction in instructions)
            {
                Insert.IntoTable(tableName: "instruction").Row(instruction);
            }
        }
    }
}
