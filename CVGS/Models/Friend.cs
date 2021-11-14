using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public int FriendListId { get; set; }

        // User ID of the friend 
        public int UserId { get; set; }

        public String UserName { get; set; }
    }
}
