using CVGS.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers
{
    public class ContextController : Controller
    {
        public readonly DBContext context;

        public ContextController(DBContext context)
        {
            this.context = context;
        }
    }
}
