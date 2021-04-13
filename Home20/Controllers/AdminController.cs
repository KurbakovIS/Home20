using Home20.Entity;
using Home20.Entity.Models;
using Home20.Entity.ReqModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Home20.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public AdminController(IWebHostEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateFood(CreateFood food)
        {

            if (ModelState.IsValid)
            {
                if (food.Img != null)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, GetUniqueFileName(food.Img.FileName));
                    try
                    {
                        food.Img.CopyTo(new FileStream(filePath, FileMode.Create));
                        using var db = new DataContext();
                        db.Foods.Add(
                            new Food
                            {
                                Name = food.Name,
                                Description = food.Description,
                                Price = Convert.ToDecimal(food.Price),
                                Img = filePath
                            }
                            );
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return Redirect("~/");
                }
            }
            else
            {
                // handle what to do when model validation fails
            }
            return Json(new { status = "error", message = "something wrong!" });

            

            
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}