using System;
using System.Collections.Generic;
using System.Text;
using AllegroAnalitics.IData.Data;
using System.Threading.Tasks;
using AllegroAnalitics.Domain.Order;
using AllegroAnalitics.Data.Sql.DAO;
using AllegroAnalitics.Common;
using AllegroAnalitics.Common.Object;
using System.Linq;
using System.Globalization;

namespace AllegroAnalitics.Data.Sql.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly AllegroAnaliticsDbContext _context;

        public DataRepository(AllegroAnaliticsDbContext context)
        {
            _context = context;
        }

        async Task<int> IDataRepository.AddOrder(Domain.Order.Order order, string userId)
        {
            DAO.Order order1 = new DAO.Order()
            {
                UserId = userId,
                SerchedPhrase= order.Name,
                Parameters = order.ParametrList.Select(x => new Parameter { Name = x.Key, Value = x.Value }).ToList(),
                Categories = order.CattegoryId.Select(x => new DAO.Category { CategoryAllegroId = (uint)x }).ToList(),
                BannedWords = order.BannedWords.Select(x => new BannedWord { Word = x }).ToList(),
                NecessaryWords = order.NecessaryWords.Select(x =>new NecessaryWord { Word=x}).ToList()
            };
            await _context.AddAsync(order1);
            await _context.SaveChangesAsync();
            saveProducts(order1);
            return 0;
        }

        async Task saveProducts(DAO.Order order)
        {
            RequestsResult products =await DowloadData(order);
            products = FiltrResults(products, order.OrderId); 
            DAO.Request request = new Request
            {
                CountRecords = (uint)products.iteamList.Count,
                firstPromotedId = products.firstPromotedId,
                firstRegularId = products.firstRegularId,
                firstRegularOffset = products.firstRegularOffset,
                Timestamp = DateTime.Now,
                OrderId= order.OrderId,
                Products = products.iteamList.Select(x => new DAO.Product
                {
                    AllegroProductId = x.id,
                    ProductName = x.name,
                    Price = double.Parse(x.sellingMode.price.amount, CultureInfo.InvariantCulture),
                    DeliveryPrice = double.Parse(x.delivery.lowestPrice.amount, CultureInfo.InvariantCulture),
                    BuyNow = x.sellingMode.format == "BUY_NOW",
                    AvilableIteams = (uint)x.stock.available,
                    CategorryId= uint.Parse(x.category.id)

                }).ToList()
            };
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();

        }
        async Task<RequestsResult> DowloadData(DAO.Order order)
        {
            RequestProduct requestProduct = new RequestProduct();
            RequestData requestData = new RequestData
            {
                name = order.SerchedPhrase,
                parametrLists = order.Parameters.Select(x => new KeyValuePair<string, string>(x.Name, x.Value)).ToList(),
                cattegoriesId = order.Categories.Select(x => x.CategoryId).ToList()
            };
            return await requestProduct.MakeRequest(requestData);
        }

        public async Task<List<TimerForOrders>> GetAllRequestTime()
        {
            var timerForOrders = _context.Request.GroupBy(x => new { x.OrderId }).Select(x => new TimerForOrders
            {
                Orderid = x.Key.OrderId,
                Date = x.Max(row=>row.Timestamp)
            }).ToList();
            return timerForOrders;
        }

        public async Task UpdateData(uint OrderId)
        {
            try
            {
                var order = _context.Order.Where(x => x.OrderId == OrderId).FirstOrDefault();
                Request lastRequest = _context.Request.Where(x => x.OrderId == OrderId).OrderByDescending(x => x.Timestamp).FirstOrDefault();
                var newRequest = new DAO.Request
                {
                    Timestamp = DateTime.Now,
                    OrderId = OrderId
                };
                await _context.AddAsync(newRequest);
                await _context.SaveChangesAsync();
                var minimum = _context.Request.Join(
                    _context.Product,
                    request => request.RequestId,
                    product => product.RequestId,
                    (request, product) => new
                    {
                        RequestId = request.RequestId,
                        OrderId = request.OrderId,
                        ProductId = product.ProductId,
                        ProductPrice = product.Price
                    }
                    ).Where(x => x.OrderId == OrderId).Min(x => x.ProductPrice);
                RequestData requestData = new RequestData
                {
                    name = order.SerchedPhrase,
                    parametrLists = order.Parameters.Select(x => new KeyValuePair<string, string>(x.Name, x.Value)).ToList(),
                    cattegoriesId = order.Categories.Select(x => x.CategoryId).ToList(),
                    firstPromotedId = lastRequest.firstPromotedId,
                    firstRegularId = lastRequest.firstRegularId,
                    firstRegularOffset = lastRequest.firstRegularOffset
                };

                RequestProduct requestProduct = new RequestProduct();
                var products = await requestProduct.MakeRequest(requestData);
                products = FiltrResults(products, OrderId);
                newRequest.CountRecords = (uint)products.iteamList.Count;
                newRequest.firstPromotedId = products.firstPromotedId;
                newRequest.firstRegularId = products.firstRegularId;
                newRequest.firstRegularOffset = products.firstRegularOffset;
                newRequest.Products = products.iteamList.Select(x => new DAO.Product
                {
                    AllegroProductId = x.id,
                    ProductName = x.name,
                    Price = double.Parse(x.sellingMode.price.amount, CultureInfo.InvariantCulture),
                    DeliveryPrice = double.Parse(x.delivery.lowestPrice.amount, CultureInfo.InvariantCulture),
                    BuyNow = x.sellingMode.format == "BUY_NOW",
                    AvilableIteams = (uint)x.stock.available,
                    CategorryId = uint.Parse(x.category.id)
                }).ToList();
                await _context.SaveChangesAsync();

                var a = newRequest.Products.OrderBy(x => x.Price).First();
                if (a.Price < minimum)
                {
                    var promotion = new Occasion
                    {
                        OrderId = OrderId,
                        OccasionId = a.ProductId
                    };
                    _context.Add(promotion);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {

            }
        }
        private RequestsResult FiltrResults(RequestsResult requestResult, uint OrderId)
        {
            List<string> bannedWords = _context.BannedWord.Where(x => x.OrderId == OrderId)
                .Select(x => x.Word).ToList();
            foreach(var bannedWord in bannedWords)
            {
                requestResult.iteamList = requestResult.iteamList
                    .Where(x => !x.name.ToUpper().Contains(bannedWord.ToUpper())).ToList();
            }
            List<string> necessaryWords = _context.NecessaryWord.Where(x => x.OrderId == OrderId)
                .Select(x => x.Word).ToList();
            foreach (var necessaryWord in necessaryWords)
            {
                requestResult.iteamList = requestResult.iteamList
                    .Where(x => x.name.ToUpper().Contains(necessaryWord.ToUpper())).ToList();
            }
            return requestResult;
        }

        public async Task<double> GetAverage(GetAverage getAverage, string userID)
        {
            try
            {
                double average = _context.Order.Join(
                    _context.Request,
                    order => order.OrderId,
                    request => request.Order.OrderId,
                    (order, request) => new
                    {
                        RequestId = request.RequestId,
                        RequestTimeStamp = request.Timestamp,
                        OrderId = order.OrderId,
                        UserId = order.UserId
                    }
                    ).Join(
                    _context.Product,
                    request => request.RequestId,
                    products => products.Request.RequestId,
                    (request, products) => new
                    {
                        RequestId = request.RequestId,
                        RequestTimeStamp = request.RequestTimeStamp,
                        OrderId = request.OrderId,
                        UserId = request.UserId,
                        Price = products.Price
                    }
                    ).Where(x => x.UserId == userID && x.RequestTimeStamp > getAverage.StartDate && x.OrderId == getAverage.OrderId)
                    .Average(x => x.Price);
                return average;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<double> GetMinimum(GetAverage getAverage, string userID)
        {
            try
            {
                double minimum = _context.Order.Join(
                    _context.Request,
                    order => order.OrderId,
                    request => request.Order.OrderId,
                    (order, request) => new
                    {
                        RequestId = request.RequestId,
                        RequestTimeStamp = request.Timestamp,
                        OrderId = order.OrderId,
                        UserId = order.UserId
                    }
                    ).Join(
                    _context.Product,
                    request => request.RequestId,
                    products => products.Request.RequestId,
                    (request, products) => new
                    {
                        RequestId = request.RequestId,
                        RequestTimeStamp = request.RequestTimeStamp,
                        OrderId = request.OrderId,
                        UserId = request.UserId,
                        Price = products.Price
                    }
                    ).Where(x => x.UserId == userID && x.RequestTimeStamp > getAverage.StartDate && x.OrderId == getAverage.OrderId)
                    .Min(x => x.Price);
                return minimum;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<List<MinimumResult>> GetMinimumInTime(GetMinimum getMinimum, string userID)
        {
            try
            {
                var minimum = _context.Order.Join(
                        _context.Request,
                        order => order.OrderId,
                        request => request.Order.OrderId,
                        (order, request) => new
                        {
                            RequestId = request.RequestId,
                            RequestTimeStamp = request.Timestamp,
                            OrderId = order.OrderId,
                            UserId = order.UserId
                        }
                        ).Join(
                        _context.Product,
                        request => request.RequestId,
                        products => products.Request.RequestId,
                        (request, products) => new
                        {
                            RequestId = request.RequestId,
                            RequestTimeStamp = request.RequestTimeStamp,
                            OrderId = request.OrderId,
                            UserId = request.UserId,
                            Price = products.Price
                        }
                        ).Where(x => x.UserId == userID && x.RequestTimeStamp > getMinimum.StartDate
                        && x.RequestTimeStamp < getMinimum.StopDate & x.OrderId == getMinimum.OrderId)
                        .GroupBy(x => new { x.RequestId, x.RequestTimeStamp }).Select(x => new MinimumResult
                        {
                            Date = x.Key.RequestTimeStamp,
                            Value = x.Min(y => y.Price)
                        }).ToList();

                return minimum;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<MinimumResult>> GetAverageInTime(GetMinimum getMinimum, string userID)
        {
            try
            {
                var minimum = _context.Order.Join(
                        _context.Request,
                        order => order.OrderId,
                        request => request.Order.OrderId,
                        (order, request) => new
                        {
                            RequestId = request.RequestId,
                            RequestTimeStamp = request.Timestamp,
                            OrderId = order.OrderId,
                            UserId = order.UserId
                        }
                        ).Join(
                        _context.Product,
                        request => request.RequestId,
                        products => products.Request.RequestId,
                        (request, products) => new
                        {
                            RequestId = request.RequestId,
                            RequestTimeStamp = request.RequestTimeStamp,
                            OrderId = request.OrderId,
                            UserId = request.UserId,
                            Price = products.Price
                        }
                        ).Where(x => x.UserId == userID && x.RequestTimeStamp > getMinimum.StartDate
                        && x.RequestTimeStamp < getMinimum.StopDate & x.OrderId == getMinimum.OrderId)
                        .GroupBy(x => new { x.RequestId, x.RequestTimeStamp }).Select(x => new MinimumResult
                        {
                            Date = x.Key.RequestTimeStamp,
                            Value = x.Average(y => y.Price)
                        }).ToList();

                return minimum;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<IData.Data.Order>> GetOrders(string userId)
        {
            var orders = _context.Order.Where(x => x.UserId == userId).Select(x => new IData.Data.Order
            {
                id = x.OrderId,
                Name = x.SerchedPhrase
            }).ToList();
            foreach(var order in orders)
            {
                order.CattegoryId = _context.Category.Where
                     (x => x.OrderId == order.id).Select(x => x.CategoryAllegroId).ToList();
                order.ParametrList = _context.Parameter.Where
                     (x => x.OrderId == order.id).Select(x => new KeyValuePair<string,string>(x.Name,x.Value))
                     .ToList();
                order.NecessaryWords = _context.NecessaryWord.Where
                    (x => x.OrderId == order.id).Select(x => x.Word).ToList();
                order.BannedWords = _context.BannedWord.Where
                    (x => x.OrderId == order.id).Select(x => x.Word).ToList();
            }
            return orders;
        }

        public async Task<List<IData.Data.Product>> GetOcasion(string userId)
        {
            var orders = _context.Order.Where(x => x.UserId == userId).Select(x => x.OrderId).ToList();
            List<IData.Data.Product> products = new List<IData.Data.Product>();
            List<uint> ocasionProductsId = new List<uint>();
            foreach(var order in orders)
            {
                ocasionProductsId.AddRange(_context.Occasions.Where(x => x.OrderId == order).Select(x => x.ProductId));
            }
            foreach(var ocasionProductId in ocasionProductsId)
            {
                products.Add(_context.Product.Where(x => x.ProductId == ocasionProductId).Select(x => new IData.Data.Product
                {
                    AllegroProductId = x.AllegroProductId,
                    AvilableIteams = x.AvilableIteams,
                    BuyNow = x.BuyNow,
                    CategorryId = x.CategorryId,
                    DeliveryPrice = x.DeliveryPrice,
                    Price = x.Price,
                    ProductName = x.ProductName
                }).FirstOrDefault());
            }
            return products;
        }
    }
}
