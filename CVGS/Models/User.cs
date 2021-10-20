using CVGS.Controllers;
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
        public int Id { get; set; }

        [Required]
        public String UserRole { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        public String DisplayName { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime JoinDate { get; set; }

        public String Gender { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,20}$", ErrorMessage = "Minimum 6 Max 20 characters atleast 1 Alphabet, 1 Number and 1 Special Character and avoid space")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public String VerififcationToken { get; set; }

        public Boolean ReceivePromomotionalEmails { get; set; }

        public String FavoritePlatform { get; set; }

        public String FavoriteGameCategory { get; set; }

        public int LoginAttempts { get; set; }

        public DateTime LockedOut { get; set; }

        public ICollection<GameReview> Reviews { get; set; }

        public ICollection<CreditCard> CreditCards { get; set; }

        [NotMapped]
        public int CaptchaId { get; set; }

        [NotMapped]
        public String CaptchaAnswer { get; set; }

        [NotMapped]
        public String CaptchaQuestion { get; set; }

        public bool IsEmployee()
        {
            return UserRole == "EMPLOYEE";
        }

        public static readonly List<string> Platforms = new List<string>()
        {
            "PC",
            "Xbox",
            "Playstation",
            "Nintendo Switch"
        };

    }
}

