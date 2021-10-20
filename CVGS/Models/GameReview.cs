using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class GameReview
    {

        public int Id { get; set; }

        public String Content { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        //The user who left the review
        public int UserId { get; set; }

        public bool Approved { get; set; }
    }
}