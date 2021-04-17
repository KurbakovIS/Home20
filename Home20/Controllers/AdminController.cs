using Home20.Entity;
using Home20.Entity.Models;
using Home20.Entity.ReqModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == 0)
                return BadRequest();

            try
            {
                using var db = new DataContext();
                var findFood = await db.Foods.FindAsync(Id);
                if (findFood != null)
                    return View(findFood);
                else
                    return NotFound();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFood(UpdateFood food)
        {

            if (ModelState.IsValid)
            {
                var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
                var provider = new CultureInfo("ru-RU");

                

                try
                {
                    var uniqueNameFile = "";
                    var number = Decimal.Parse(food.Price.Replace('.', ','), style, provider);

                    using var db = new DataContext();
                    var findFood = await db.Foods.FindAsync(food.Id);

                    findFood.Name = food.Name;
                    findFood.Description = food.Description;
                    findFood.Price = number;

                    if (food.Img != null)
                    {
                        uniqueNameFile = await RecordImg(food.Img);
                        findFood.Img = $"img/uploads/{uniqueNameFile}";
                    }
                       
                    
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
                    uniqueNameFile = await RecordImg(food.Img);

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
        /// Копирует в папку изображение пришедшее с клиента
        /// </summary>
        /// <param name="Img">изображение с клиента</param>
        /// <returns>уникальное название изображения</returns>
        async Task<string> RecordImg(IFormFile Img)
        {
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "img\\uploads");

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            string uniqueNameFile = GetUniqueFileName(Img.FileName);
            var filePath = Path.Combine(uploads, uniqueNameFile);
            FileStream neImg = new FileStream(filePath, FileMode.Create);
            await Img.CopyToAsync(neImg);
            neImg.Close();

            return uniqueNameFile;
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
    /// <summary>
    /// Расширение FileExtensions для ассинхронного удаления
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// ассинхронное удаление файла
        /// </summary>
        /// <param name="fi">удаляемый файл</param>
        /// <returns></returns>
        public static Task DeleteAsync(this FileInfo fi)
        {
            return Task.Factory.StartNew(() => fi.Delete());
        }
    }
}