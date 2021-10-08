using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CVGS.Models
{
    public class Employee: Person
    {
        public Employee()
        {
            Games = new List<Game>();
        }

        public virtual ICollection<Game> Games { get; set; }
    }




}

