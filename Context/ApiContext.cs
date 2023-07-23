using RicardoDevAPI;
using Microsoft.EntityFrameworkCore;

namespace RicardoDevAPI.Context
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "UserDB");
        }

        public DbSet<UserDTO> Users { get; set; }
    }
}
