using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Game
    {

        public int Id { get; set; }

        public String Name { get; set; }
        public ICollection<GameReview> Reviews { get; set; }
    }
}
