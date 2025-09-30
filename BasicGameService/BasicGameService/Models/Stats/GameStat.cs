namespace BasicGameService.Models.Stats
{
    public class GameStat
    {
        public int GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public int TimesPlayed { get; set; }
    }
}
