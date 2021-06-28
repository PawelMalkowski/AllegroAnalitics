using System;
using System.Collections.Generic;
using System.Text;
using AllegroAnalitics.IServices.Data;
using AllegroAnalitics.IData.Data;
using System.Threading.Tasks;
using AllegroAnalitics.IServices.Request;
using System.Linq;

namespace AllegroAnalitics.Services.Data
{
    public class DataService : IDataService
    {
        private readonly IDataRepository _dataRepository;

        public DataService(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        public async Task<int> AddOrder(AddOrder addOrder, string userId)
        {
            var order = new Domain.Order.Order(addOrder.Name, addOrder.CattegoryId, addOrder.ParametrList,
                addOrder.BannedWords, addOrder.NecessaryWords);
            int Id = await _dataRepository.AddOrder(order,userId);
            return Id;
        }

        public async Task<List<IServices.Data.TimerForOrders>> GetAllRequestTime()
        {
            var timerForOrders = await _dataRepository.GetAllRequestTime();
            return timerForOrders.Select(x => new IServices.Data.TimerForOrders
            {
                Orderid = x.Orderid,
                Date = x.Date
            }).ToList();
        }

        public Task<double> GetAverage(GetAverage getAverage, string userId)
        {
            var Average = new Domain.Order.GetAverage(getAverage.OrderID, getAverage.StartDate);
            return _dataRepository.GetAverage(Average, userId);

        }

        public Task<double> GetMinimum(GetAverage getAverage, string userId)
        {
            var Minimum = new Domain.Order.GetAverage(getAverage.OrderID, getAverage.StartDate);
            return _dataRepository.GetMinimum(Minimum, userId);
        }

        public async Task<List<IServices.Data.MinimumResult>> GetMinimumInTime(GetMinimumInTime getMinimumInTime, string userId)
        {
            var Minimum = new Domain.Order.GetMinimum(getMinimumInTime.OrderID, getMinimumInTime.StartDate,getMinimumInTime.StopDate);
            var min= await _dataRepository.GetMinimumInTime(Minimum, userId);
            return min.Select(x => new IServices.Data.MinimumResult
            {
                Date = x.Date,
                Value = x.Value
            }).ToList();
        }
        public async Task<List<IServices.Data.MinimumResult>> GetAverageInTime(GetMinimumInTime getAverageInTime, string userId)
        {
            var Minimum = new Domain.Order.GetMinimum(getAverageInTime.OrderID, getAverageInTime.StartDate, getAverageInTime.StopDate);
            var min = await _dataRepository.GetAverageInTime(Minimum, userId);
            return min.Select(x => new IServices.Data.MinimumResult
            {
                Date = x.Date,
                Value = x.Value
            }).ToList();
        }

        public async Task UpdateData(uint OrderId)
        {
            await _dataRepository.UpdateData(OrderId);
        }

        public async Task<List<IServices.Request.Order>> GetOrders(string userId)
        {
            var Order = await _dataRepository.GetOrders(userId);
            var order = Order.Select(x => new IServices.Request.Order
            {
                id = x.id,
                Name = x.Name,
                BannedWords = x.BannedWords,
                CattegoryId = x.CattegoryId,
                NecessaryWords = x.NecessaryWords,
                ParametrList = x.ParametrList,
            }).ToList();
            return order;
        }
        public async Task<List<IServices.Data.Product>> GetOcasion(string userId)
        {
            var Ocasion = await _dataRepository.GetOcasion(userId);
            var ocasion = Ocasion.Select(x => new IServices.Data.Product
            {
              AllegroProductId= x.AllegroProductId,
              AvilableIteams= x.AvilableIteams,
              BuyNow= x.BuyNow,
              CategorryId= x.CategorryId,
              DeliveryPrice= x.DeliveryPrice,
              Price= x.Price,
              ProductName= x.ProductName,
            }).ToList();
            return ocasion;
        }
    }
}
