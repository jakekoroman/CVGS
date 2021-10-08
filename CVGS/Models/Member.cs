using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Member : Person
    {

        public virtual Preferences Peferences { get; set; }
    }
}
