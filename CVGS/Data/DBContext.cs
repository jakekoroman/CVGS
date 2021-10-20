using System;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;

namespace CVGS.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        { }

        public DbSet<User> User { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<Game> Game { get; set; }

        public DbSet<GameReview> GameReview { get; set; }

        public DbSet<CreditCard> CreditCard { get; set; }

        public DbSet<Address> Address {get; set;}
    }
}
