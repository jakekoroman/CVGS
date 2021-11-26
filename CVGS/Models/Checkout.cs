using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Checkout
    {

        public Cart Cart { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectList CreditCardsList { get; set; }

        public CreditCard CreditCard { get; set; }

        public String CreditCardNumber { get; set; }
    }
}
