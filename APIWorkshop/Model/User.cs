using System.ComponentModel.DataAnnotations;

namespace APIWorkshop.Model
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string? ResetToken { get; set; } = null;
        
        public DateTime? ResetTokenExpiry { get; set; } = null;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastLogin { get; set; }
    }
}
