using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Preferences
    {
        public int ID;

        public Platform FavoritePlatform { get; set; }
        public GameCategory FavoriteGameCategory { get; set; }

        [Required]
        public virtual Member Member { get; set; }
         

    }

}

public enum Platform
{
    Xbox,
    PC,
}


public enum GameCategory
{
    MMORPG
}