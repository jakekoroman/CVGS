using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    [Index(nameof(Email), IsUnique = true), Index(nameof(DisplayName), IsUnique = true)]
    public class User
    {
        public int ID { get; set; }

        [Required]
        public String UserRole { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        public String DisplayName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        
        public String Gender { get; set; }

       [Required]
        public String Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Pass \"{0}\" must have no less {2} chars.", MinimumLength = 8)]
        public String Password { get; set; }

        public String VerififcationToken { get; set; }

        [Required]
        public String Country { get; set; }

        [Required]
        public String City { get; set; }

        [Required]

        public String PostalCode { get; set; }

        [Required]
        public String Street { get; set; }

        [Required]
        public String Province { get; set; }

        public Boolean ReceivePromomotionalEmails { get; set; }

        public String FavoritePlatform { get; set; }

        public String FavoriteGameCategory { get; set; }

        public ICollection<GameReview> Reviews { get; set; }

        public ICollection<CreditCard> CreditCards { get; set; }

        public bool IsEmployee()
        {
            return UserRole == "EMPLOYEE";
        }

    }
}

