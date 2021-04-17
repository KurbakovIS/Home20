using Home20.Entity;
using Home20.Entity.Models;
using Home20.Entity.ReqModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

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
            ViewBag.Foods = new DataContext().Foods;
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFood(int Id)
        {
            if(Id == 0)
                return BadRequest();

            try
            {
                using var db = new DataContext();
                var findFood = await db.Foods.FindAsync(Id);
                if (findFood != null)
                {
                    DeleteImg(findFood.Img);

                    db.Foods.Remove(findFood);
                    db.SaveChanges();
                }
                else
                    return NotFound();
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

          return  NoContent();
        }


        /// <summary>
        /// Добавление позиции в меню
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateFood(CreateFood food)
        {

            if (ModelState.IsValid)
            {
                var uniqueNameFile = "";
                var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                var provider = new CultureInfo("ru-RU");

                if (food.Img != null)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img\\uploads");

                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    uniqueNameFile = GetUniqueFileName(food.Img.FileName);
                    var filePath = Path.Combine(uploads, uniqueNameFile);
                    FileStream neImg = new FileStream(filePath, FileMode.Create);
                    await food.Img.CopyToAsync(neImg);
                    neImg.Close();
                }

                try
                {
                    var number = Decimal.Parse(food.Price.Replace('.',','), style, provider);

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
                    return BadRequest(e.Message);
                }
                return Ok();
            }
            else
                return BadRequest("Модель не валидна");
        }
        /// <summary>
        /// Присваивает уникальное имя файлу
        /// </summary>
        /// <param name="FileName">Название файла</param>
        /// <returns>Уникальное названеифайла</returns>
        string GetUniqueFileName(string FileName)
        {
            FileName = Path.GetFileName(FileName);
            return Path.GetFileNameWithoutExtension(FileName)
                      + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(FileName);
        }

        /// <summary>
        /// Удаляет картинку
        /// </summary>
        /// <param name="Img">название картинки</param>
       async void DeleteImg(string Img)
        {
            var filePath = Path.Combine(hostingEnvironment.WebRootPath, Img);
            FileInfo img = new FileInfo(filePath);
            await  img.DeleteAsync();
        }
    }
    public static class FileExtensions
    {
        public static Task DeleteAsync(this FileInfo fi)
        {
            return Task.Factory.StartNew(() => fi.Delete());
        }
    }
}