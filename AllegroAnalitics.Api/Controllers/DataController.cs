using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AllegroAnalitics.Api.BindingModels;
using AllegroAnalitics.IServices.Data;
using AllegroAnalitics.IServices.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AllegroAnalitics.Api.Validation;

namespace AllegroAnalitics.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/data")]
    [EnableCors()]
    public class DataController : Controller
    {
        private readonly IDataService _dataService;
        private readonly UserManager<IdentityUser> _userManger;
        public DataController(IDataService dataService, UserManager<IdentityUser> userManager)
        {
            _dataService = dataService;
            _userManger = userManager;
        }
      

        [Authorize()]
        [Route("AddOrder", Name = "AddOrder")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddOrder([FromBody]BindingModels.AddOrder addOrder)
        {
            IServices.Request.AddOrder Order = new IServices.Request.AddOrder()
            {
                Name = addOrder.Name,
                CattegoryId = addOrder.CattegoryId,
                ParametrList = addOrder.ParametrList.Select(x => new KeyValuePair<string, string>(x.Name, x.Value)).ToList(),
                BannedWords = addOrder.BannedWords,
                NecessaryWords = addOrder.NecessaryWords
            };
            var user = await _userManger.GetUserAsync(User);
            int Id = await _dataService.AddOrder(Order,user.Id);
            return Ok();
        }


        [Authorize (Roles = "Administrator")]
        [Route("GetAllRequestTime", Name = "GetAllRequestTime")]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetAllRequestTime()
        {
           var TimingOrder= await _dataService.GetAllRequestTime();
           return Ok(new { TimingOrder });
        }

        [Authorize]
        [Route("GetOrders", Name = "GetOrders")]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetOrders()
        {
            var user = await _userManger.GetUserAsync(User);
            var orders =await _dataService.GetOrders(user.Id);
            return Ok(orders);
        }
        [Authorize]
        [Route("GetOcasion", Name = "GetOcasion")]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetOcasion()
        {
            var user = await _userManger.GetUserAsync(User);
            var ocassions = await _dataService.GetOcasion(user.Id);
            return Ok(ocassions);
        }

        [Authorize]
        [Route("GetAverage", Name = "GetAverage")]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetAverage([FromBody] GetAvarage getAvarage)
        {
            var user = await _userManger.GetUserAsync(User);
            GetAverage average = new GetAverage
            {
                OrderID = getAvarage.OrderID,
                StartDate = getAvarage.StartDate
            };
            double Average = await _dataService.GetAverage(average,user.Id);
            if (Average < 0)
            {
                string badData = "Somethin is bad check data and try again";
                return BadRequest(new { badData });
            }
            return Ok(new { Average });
        }

        [Authorize]
        [Route("GetMinimumInTime", Name = "GetMinimumInTime")]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetMinimumInTime([FromBody] BindingModels.GetMinimumInTime getMinimumInTime)
        {
            if (getMinimumInTime.StopDate == DateTime.MinValue) getMinimumInTime.StopDate = DateTime.Now;
            var user = await _userManger.GetUserAsync(User);
            IServices.Request.GetMinimumInTime minimum = new IServices.Request.GetMinimumInTime
            {
                OrderID = getMinimumInTime.OrderID,
                StartDate = getMinimumInTime.StartDate,
                StopDate = getMinimumInTime.StopDate
            };
            var Minimum = await _dataService.GetMinimumInTime(minimum, user.Id);
            if (Minimum==null)
            {
                string badData = "Somethin is bad check data and try again";
                return BadRequest(new { badData });
            }
            return Ok(new { Minimum });
        }
       

        [Authorize]
        [Route("GetAverageInTime", Name = "GetAverageInTime")]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetAverageInTime([FromBody] BindingModels.GetMinimumInTime getAverageInTime)
        {
            if (getAverageInTime.StopDate == DateTime.MinValue) getAverageInTime.StopDate = DateTime.Now;
            var user = await _userManger.GetUserAsync(User);
            IServices.Request.GetMinimumInTime average = new IServices.Request.GetMinimumInTime
            {
                OrderID = getAverageInTime.OrderID,
                StartDate = getAverageInTime.StartDate,
                StopDate = getAverageInTime.StopDate
            };
            var Average = await _dataService.GetAverageInTime(average, user.Id);
            if (Average == null)
            {
                string badData = "Somethin is bad check data and try again";
                return BadRequest(new { badData });
            }
            return Ok(new { Average });
        }

        [Authorize]
        [Route("GetMinimum", Name = "GetMinimum")]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetMinimum([FromBody] GetAvarage getAvarage)
        {
            var user = await _userManger.GetUserAsync(User);
            GetAverage average = new GetAverage
            {
                OrderID = getAvarage.OrderID,
                StartDate = getAvarage.StartDate
            };
            double Average = await _dataService.GetMinimum(average, user.Id);
            if (Average < 0)
            {
                string badData = "Somethin is bad check data and try again";
                return BadRequest(new { badData });
            }
            return Ok(new { Average });
        }

        [Authorize(Roles = "Administrator")]
        [Route("UpdateData", Name = "UpdateData")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> UpdateData([FromBody] UpdateData updateData)
        {
            await _dataService.UpdateData(updateData.id);
            return Ok();
        }
    }
}
