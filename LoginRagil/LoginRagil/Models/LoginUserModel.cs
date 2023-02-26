namespace LoginRagil.Models
{
        public class LoginUserModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Error { get; set; } = "";
        }
}
