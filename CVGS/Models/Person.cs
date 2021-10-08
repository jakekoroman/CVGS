using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS.Models
{
    /// <summary>
    /// Represents an person entity on the system, other classes will inherit the class
    /// to reuse shared properties such as FirstName & LastName
    /// </summary>
    public class Person
    {

       public Person()
        {
            Addresses = new List<Address>();
        }

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

        public virtual ICollection<Address> Addresses { get; set; }


    }
}
