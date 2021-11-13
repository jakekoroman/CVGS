using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Family
    {
        public int Id { get; set; }

        public int FamilyListId { get; set; }

        // User ID of the family member 
        public int UserId { get; set; }
    }
}
