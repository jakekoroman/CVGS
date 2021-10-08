using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS.Models
{
    public class Game
    {


        public int ID{ get; set; }

        [Required]
        public String Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;


        public virtual Employee Creator { get; set; }

        
    }
}
