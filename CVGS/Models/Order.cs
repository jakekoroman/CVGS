using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Order
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public String Status { get; set; }

        [NotMapped]
        public List<OrderItem> OrderItems = new List<OrderItem>();

        public User User { get; set; }
    }
}
