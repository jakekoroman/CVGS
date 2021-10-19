using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        [Display(Name = "Credit Card Number")]
        [Required(ErrorMessage = "required")]
        [Range(100000000000, 9999999999999999999, ErrorMessage = "must be between 12 and 19 digits")]
        [CreditCard]
        public String CardNumber { get; set; }

        public int UserId { get; set; }

        public User User;
    }
}
