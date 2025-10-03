namespace BasicGameService.DTOs
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // use string for easy JSON
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public int? CurrentSessionId { get; set; }
    }
}
