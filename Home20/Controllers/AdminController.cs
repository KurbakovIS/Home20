﻿using Home20.Entity;
using Home20.Entity.Data;
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
        private readonly IFoodData foodData;
        public AdminController(IWebHostEnvironment environment, IFoodData FoodData)
        {
            hostingEnvironment = environment;
            foodData = FoodData;
        }

        public async Task<IActionResult> Index()
        {
            var foods = await foodData.GetFoods();
            return View(foods);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            if (Id == 0)
                return BadRequest();

            try
            {
              var findFood = await foodData.GetFood(Id);
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

                try
                {
                 await foodData.UpdateFood(food.Id, food);
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
                await foodData.DeleteFood(Id);
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
                try
                {
                   await foodData.AddFood(food);
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
       
    }
    
}