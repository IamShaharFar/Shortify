using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LoginRagil.Models
{
    public class LUser : IdentityUser<int>
    {        
        public string? Password { get; set; }

        public bool RememberMe { get; set; }

        //identity user fields
       

        [NotMapped]
        public override string? NormalizedUserName { get; set; }

        [NotMapped]
        public override string? NormalizedEmail { get; set; }

        [NotMapped]
        public override bool EmailConfirmed { get; set; }

        [NotMapped]
        public override string? PasswordHash { get; set; }

        [NotMapped]
        public override string? SecurityStamp { get; set; }

        [NotMapped]
        public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [NotMapped]
        public override string? PhoneNumber { get; set; }

        [NotMapped]
        public override bool PhoneNumberConfirmed { get; set; }

        [NotMapped]
        public override bool TwoFactorEnabled { get; set; }

        [NotMapped]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [NotMapped]
        public override bool LockoutEnabled { get; set; }

        [NotMapped]
        public override int AccessFailedCount { get; set; }
    }
}
