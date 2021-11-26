using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Game
    {

        public int Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public String Developer { get; set; }

        public String Genre { get; set; }

        public String Platform { get; set; }

        public DateTime ReleaseDate { get; set; }

        public double Price { get; set; }

        public ICollection<GameReview> Reviews { get; set; }

        [NotMapped]
        public GameRatings UserGameRating { get; set; }

        [NotMapped]
        public double OverAllRating { get; set; }

        [NotMapped]
        public Boolean Owned { get; set; }

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
