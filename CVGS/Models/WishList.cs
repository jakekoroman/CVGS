using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class WishList
    {
        public int Id { get; set; }

        public int UserId { get; set; }
    
        public int GameId { get; set; }

        public String GameName { get; set; }
    }
}
