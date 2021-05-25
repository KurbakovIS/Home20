using Home20.Entity.Models;
using Home20.Entity.ReqModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Home20.Entity.Data
{
    public class FoodDataApi: IFoodData
    {
        private HttpClient httpClient { get; set; }
        public FoodDataApi()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Отправляет запрос на получение списка позиций в меню
        /// </summary>
        /// <returns>позиции в меню </returns>
        public async Task<IEnumerable<Food>> GetFoods()
        {
            string url = $"http://localhost:5000/api/Foods";

            string json =await httpClient.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<IEnumerable<Food>>(json);

            return res;
        }

        /// <summary>
        /// Отсылает запрос на удаление позиции в меню
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteFood(int Id)
        {
            string url = @"http://localhost:5000/api/Foods";

            var res = await httpClient.DeleteAsync(url);
            Console.WriteLine(res);
        }

        /// <summary>
        /// Отправляет запрос на получени позиции в меню
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Food> GetFood(int Id)
        {
            string url = $"http://localhost:5000/api/Foods/{Id}";

            string json = await httpClient.GetStringAsync(url);

            return JsonConvert.DeserializeObject<Food>(json);
        }
    }
}
