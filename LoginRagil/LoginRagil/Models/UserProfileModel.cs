using System.ComponentModel.DataAnnotations;

namespace LoginRagil.Models
{
    public class UserProfileModel
    {
        [StringLength(20, MinimumLength = 2, ErrorMessage = "This fiels need to be at least 2 chars")]
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Old password is required")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "This fiels need to be at least 4 chars")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }


        public string Errors { get; set; } = "";
    }
}
