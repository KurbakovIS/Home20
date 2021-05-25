using Home20.Entity.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Home20.Controllers
{
    public class AuthController : Controller
    {
        public AuthController(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private HttpClient httpClient { get; set; }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("token");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserReg userReg)
        {
            string url = @"http://localhost:5000/api/Auth";

            var req =await httpClient.PostAsync(requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(userReg),
                Encoding.UTF8,
                mediaType: "application/json"));
            if((int)req.StatusCode ==201)
            {
                HttpContext.Response.Cookies.Append("token", "value set at " + DateTime.Now.TimeOfDay);
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine(req);


            return RedirectToAction("Registration", "Auth");
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserAuth userAuth)
        {
            string url = @"http://localhost:5000/api/Auth/login";

            var req = await httpClient.PostAsync(requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(userAuth),
                Encoding.UTF8,
                mediaType: "application/json"));
            if ((int)req.StatusCode == 200)
            {
                HttpContext.Response.Cookies.Append("token", "value set at " + DateTime.Now.TimeOfDay);
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine(req);


            return RedirectToAction("Registration", "Auth");
        }
    }
}
