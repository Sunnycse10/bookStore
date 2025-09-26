using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Book_App.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Book_App.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }


}
