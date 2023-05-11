using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedProject.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Database
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        public DatabaseContext() : base() { }

        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<RentComment> Comments { get; set; }
        public DbSet<InventoryEntry> InventoryEntries { get; set; }
        public DbSet<RentGameLink> RentGameLinks { get; set; }
    }
}
