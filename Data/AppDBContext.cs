using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Models;
using WebApiAutores.Models;

namespace WebApiAutores.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public AppDBContext(DbContextOptions options) : base(options) { }
    }
}
