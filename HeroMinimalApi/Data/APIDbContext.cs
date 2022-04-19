using HeroMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HeroMinimalApi.Data
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options)
        {

        }
        public DbSet<SuperHero> SuperHeros { get; set; }
    }
}
