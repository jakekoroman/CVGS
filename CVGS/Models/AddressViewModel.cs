using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class AddressViewModel
    {

        public bool SameAddress { get; set; }

        public Address ShippingAddress { get; set; }
        public Address MailingAddress { get; set; }

    }
}
