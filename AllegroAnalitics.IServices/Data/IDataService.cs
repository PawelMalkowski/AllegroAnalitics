using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AllegroAnalitics.IServices.Request;

namespace AllegroAnalitics.IServices.Data
{
    public interface IDataService
    {
        Task<int> AddOrder(AddOrder addOrder, string userId);
        Task<List<TimerForOrders>> GetAllRequestTime();
        Task UpdateData(uint OrderId);
        Task<double> GetAverage(GetAverage getAverage, string userId);
        Task<double> GetMinimum(GetAverage getAverage, string userId);
        Task<List<MinimumResult>> GetMinimumInTime(GetMinimumInTime getMinimumInTime, string userId);
        Task<List<MinimumResult>> GetAverageInTime(GetMinimumInTime getMinimumInTime, string userId);
        Task<List<Order>> GetOrders(string userId);
        Task<List<Product>> GetOcasion(string userId);
    }
}
