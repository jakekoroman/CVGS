using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class GameRatings
    {
        public int Id { get; set; }

        public int GameID { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }
    }

}
