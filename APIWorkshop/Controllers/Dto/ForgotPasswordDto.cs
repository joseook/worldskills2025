using System.ComponentModel.DataAnnotations;

namespace APIWorkshop.Controllers.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
