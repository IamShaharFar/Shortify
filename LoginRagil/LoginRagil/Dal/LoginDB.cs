using Microsoft.EntityFrameworkCore;
using LoginRagil.Models;

namespace LoginRagil.NewFolder
{
    public class LoginDB : DbContext
    {
        public DbSet<LUser> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<UrlPair> UrlPairs { get; set; }
        public LoginDB(DbContextOptions<LoginDB> options) : base(options)
        {
            
        }
    }
}
