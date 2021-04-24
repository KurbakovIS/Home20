using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Home20.Entity.Models
{
    public class UserReg:UserAuth
    {
        public string ConfirmPassword { get; set; }
    }
}
