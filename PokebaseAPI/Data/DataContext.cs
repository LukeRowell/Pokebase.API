using Microsoft.EntityFrameworkCore;

namespace PokebaseAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Pokemon> Pokemon => Set<Pokemon>();
    }
}
