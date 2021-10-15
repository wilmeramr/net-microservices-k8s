using Microsoft.EntityFrameworkCore;
using PlatFormService.Models;

namespace PlatFormService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {


        }

        public DbSet<Platform> Platforms { get; set; }

    }
}