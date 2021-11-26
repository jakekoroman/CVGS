using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class OrderItem
    {

        public int Id {get; set;}

        public int OrderId { get; set; }

        public int GameId { get; set; }

    }
}
