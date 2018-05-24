using ChewieBot.Database.Model;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChewieBot.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ChatEvent> ChatEvents { get; set; }
        public DbSet<EventWinner> EventWinners { get; set; }

        public DatabaseContext()
            : base("DatabaseConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatEvent>()
                .Ignore(x => x.UserList)
                .Ignore(x => x.HasStarted)
                .Ignore(x => x.HasFinished);

            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<DatabaseContext>(modelBuilder);
            System.Data.Entity.Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
