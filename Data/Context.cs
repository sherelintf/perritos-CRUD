using Microsoft.EntityFrameworkCore;
using CRUD.Models;

namespace CRUD.Data
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}
