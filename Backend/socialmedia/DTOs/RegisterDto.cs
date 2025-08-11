using System.ComponentModel.DataAnnotations;

namespace socialmedia.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]{5,}$", ErrorMessage = "Username can only contain letters, numbers, and underscores, and must be at least 5 characters long.")]
        public string Username { get; set; }

        public IFormFile? Photo { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        [ Required(ErrorMessage = "Title is required or write N/A")]
        public string title { get; set; }
        [Required(ErrorMessage = "Bio is required, tell us about yourself")]
        public string bio { get; set; }
    }
}
