using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Home20.Entity.ReqModel
{
    public class ManipulationFood
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public IFormFile Img { get; set; }
    }
}
