using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LoginRagil.Models
{
    public class UrlPair
    {
        [Key]
        public string ShortUrl { get; set; }
        [Required]
        public string FullUrl { get; set; }
        [Required]
        public int Entries { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string UrlUserEmail { get; set; } = string.Empty;
        public virtual ICollection<Entry> EntriesPc { get; set; }

        public UrlPair()
        {
            Created = DateTime.UtcNow;
            EntriesPc = new List<Entry>();
        }
    }
}
