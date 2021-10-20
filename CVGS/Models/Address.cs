using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Address
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public String Country { get; set; }

        [Required]
        public String City { get; set; }

        [Required]

        public String PostalCode { get; set; }

        [Required]
        public String Street { get; set; }

        [Required]
        public String Province { get; set; }

        public User User { get; set; }
        
        public int UserId { get; set; }
 
    }
}
