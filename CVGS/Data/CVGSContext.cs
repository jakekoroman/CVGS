using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;

namespace CVGS.Data
{
    public class CVGSContext : DbContext
    {
        public CVGSContext (DbContextOptions<CVGSContext> options)
            : base(options)
        {



        }

        public DbSet<CVGS.Models.Employee> Employee { get; set; }

        public DbSet<CVGS.Models.Game> Game { get; set; }
    }
}
