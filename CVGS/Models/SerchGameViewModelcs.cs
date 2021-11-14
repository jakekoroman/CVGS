using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class SearchGameViewModel
    {

        public String Name { get; set; }

        public List<Game> Games { get; set; }

        public Game Game { get; set; }
    }
}
