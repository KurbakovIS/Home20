using Home20.Entity;
using Home20.Entity.Models;
using Home20.Entity.ReqModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Home20.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //public IActionResult CreateFood(string Name, string Description, string Price, IFormFile Img)
        public IActionResult CreateFood(CreateFood FoodReq)
        {
            using var db = new DataContext();
            db.Foods.Add(
                new Food { }
                );
            db.SaveChanges();

            return Redirect("~/");
        }
    }
}
