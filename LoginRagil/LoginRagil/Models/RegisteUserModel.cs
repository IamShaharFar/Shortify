using System.ComponentModel.DataAnnotations;
using LoginRagil.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace LoginRagil.Models
{
    public class RegisteUserModel
    {

        [StringLength(20, MinimumLength = 2, ErrorMessage = "This fiels need to be at least 2 chars")]
        [Required(ErrorMessage = "Required field")]

        public string? Name { get; set; }

        [CustomEmailValidation(ErrorMessage = "Email address is already taken.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Required field")]

        public string? Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "This fiels need to be at least 4 chars")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [MustBeTrue(ErrorMessage = "This field must be checked.")]
        public bool AgreeServices { get; set; }
    }
}
