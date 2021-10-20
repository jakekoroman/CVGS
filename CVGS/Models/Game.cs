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

        public static readonly List<string> Categories = new List<string>()
        {
            "Sandbox",
            "RTS",
            "Shooters",
            "MOBA",
            "RPG",
            "Simulation",
            "Sport",
            "Puzzle",
            "Party",
            "Action Adventure",
            "Horror",
            "Suvival",
            "Platformer"
        };
    }
}
