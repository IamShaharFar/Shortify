using System.ComponentModel.DataAnnotations;
using System.Net;

namespace LoginRagil.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }   
        [Required]
        public string Ip { get; set; }
        [Required]
        public DateTime EntryTime { get; set; }
        [Required]
        public string ShortUrl { get; set; }
    }
}