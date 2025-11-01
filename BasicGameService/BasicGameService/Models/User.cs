using BasicGameService.Models.Enums;

namespace BasicGameService.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserRole Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public List<Session>? Sessions { get; set; }

    }
}
