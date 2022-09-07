namespace API.DTO
{
    public class RecipeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public List<string> Instructions { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public Guid UserId { get; set; }
    }
}
