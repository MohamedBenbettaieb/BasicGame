namespace BasicGameService.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Game title
        public GameType gameType { get; set; } 
    }
}
