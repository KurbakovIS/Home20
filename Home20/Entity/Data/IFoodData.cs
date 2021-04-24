using Home20.Entity.Models;
using Home20.Entity.ReqModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Home20.Entity.Data
{
    public interface IFoodData
    {
        Task<IEnumerable<Food>> GetFoods();
        Task AddFood(CreateFood food);
        Task DeleteFood(int Id);
        Task<Food> GetFood(int Id);
        Task UpdateFood(int id, UpdateFood food);
    }
}
