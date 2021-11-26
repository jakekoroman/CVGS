using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Cart
    {


        public int Id { get; set; }

        public int UserId { get; set; }

        public ICollection<CartItem> Items { get; set; }
    }
}
