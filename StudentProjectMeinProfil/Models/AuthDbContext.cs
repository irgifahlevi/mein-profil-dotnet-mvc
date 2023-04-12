using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentProjectMeinProfil.Models
{
    public class AuthDbContext : IdentityDbContext<AddUser>

    {
        public AuthDbContext(DbContextOptions<AuthDbContext>options) 
        : base(options) 
        {
        
        }

        public virtual DbSet<AddUser> AddUsers { get; set; }
    }
}
