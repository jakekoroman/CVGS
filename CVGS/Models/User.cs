using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [DataType(DataType.Password)]
        public String Password { get; set; }

        public String VerififcationToken { get; set; }


        public String Country { get; set; }

        public String City { get; set; }

        public String PostalCode { get; set; }

        public String Street { get; set; }

        public String Province { get; set; }

        public Platform FavoritePlatform { get; set; }
        public GameCategory FavoriteGameCategory { get; set; }

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

