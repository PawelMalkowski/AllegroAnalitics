using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllegroAnalitics.IData.Data
{
    public interface IDataRepository
    {
        Task<int> AddOrder(Domain.Order.Order order, string userId);
        Task<List<TimerForOrders>> GetAllRequestTime();
        Task UpdateData(uint OrderId);
        Task<double> GetAverage(Domain.Order.GetAverage getAverage, string userID);
        Task<double> GetMinimum(Domain.Order.GetAverage getAverage, string userID);
        Task<List<MinimumResult>> GetMinimumInTime(Domain.Order.GetMinimum getMinimum, string userID);
        Task<List<MinimumResult>> GetAverageInTime(Domain.Order.GetMinimum getAverage, string userID);
        Task<List<Order>> GetOrders(string userId);
        Task<List<Product>> GetOcasion(string userId);
    }
}
