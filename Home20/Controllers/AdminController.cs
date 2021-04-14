using Home20.Entity;
using Home20.Entity.Models;
using Home20.Entity.ReqModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
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

        /// <summary>
        /// Добавление позиции в меню
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateFood(CreateFood food)
        {

            if (ModelState.IsValid)
            {
                var uniqueNameFile = "";
                if (food.Img != null)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img/uploads");
                    uniqueNameFile = GetUniqueFileName(food.Img.FileName);
                    var filePath = Path.Combine(uploads, uniqueNameFile);
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);
                    food.Img.CopyTo(new FileStream(filePath, FileMode.Create));

                }

                try
                {
                    var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                    var provider = new CultureInfo("ru-RU");
                    var number = Decimal.Parse(food.Price.Replace('.',','), style, provider);
                    Console.WriteLine("'{0}' converted to {1}.", food.Price, number);

                    using var db = new DataContext();
                    db.Foods.Add(
                        new Food
                        {
                            Name = food.Name,
                            Description = food.Description,
                            Price = number,
                            Img = $"img/uploads/{uniqueNameFile}"
                        }
                        );
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return BadRequest();
                }
                return Ok();
            }
            else
                return BadRequest();
        }
        /// <summary>
        /// Присваивает уникальное имя файлу
        /// </summary>
        /// <param name="FileName">Название файла</param>
        /// <returns>Уникальное названеифайла</returns>
        private static string GetUniqueFileName(string FileName)
        {
            FileName = Path.GetFileName(FileName);
            return Path.GetFileNameWithoutExtension(FileName)
                      + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(FileName);
        }
    }
}