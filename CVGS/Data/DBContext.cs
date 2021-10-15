using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DbSet<User> Game { get; set; }
        public DbSet<User> GameReview { get; set; }
        public DbSet<User> Event { get; set; }

    }
}
