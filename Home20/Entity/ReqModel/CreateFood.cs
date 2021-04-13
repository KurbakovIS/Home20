using Microsoft.AspNetCore.Http;

namespace Home20.Entity.ReqModel
{
    public class CreateFood
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public IFormFile Img { get; set; }
    }
}