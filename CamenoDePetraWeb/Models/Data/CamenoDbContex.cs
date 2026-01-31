
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CamenoDePetraWeb.Models.Data
{
    public class ERPDbContext : IdentityDbContext<IdentityUser>
    {
        public ERPDbContext(DbContextOptions<ERPDbContext> options)
            : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }
    }
}


