using LoginRagil.NewFolder;
using System.ComponentModel.DataAnnotations;

namespace LoginRagil.Attributes
{
    public class CustomEmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = validationContext.GetService<LoginDB>();
            var email = value as string;

            if (dbContext.Users.Any(u => u.Email == email))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
