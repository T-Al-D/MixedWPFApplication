using Microsoft.EntityFrameworkCore;

namespace MixedLibrary.DataAccess
{
    public class SQLiteContext : DbContext
    {
        // class overriden and configured
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // input Connectionstring
            optionsBuilder.UseSqlite("Data Source = MicroProtocol.db");

        }

        // define Tables in database
        public DbSet<ProtocolModel> Protocols { get; set; }
    }
}
