using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Address
    {


        public int ID { get; set; }

        public String Country { get; set; }

        public String City { get; set; }

        public String PostalCode { get; set; }

        public String Street { get; set; }

        public String Province { get; set; }

        public virtual Person Person { get; set; }

    }
}
