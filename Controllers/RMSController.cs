using SURAKSHA.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using SURAKSHA.Models.QueryModel;
using SURAKSHA_API.Database.Repository;
using System.Data;
using RMS_API.Models.QueryModel;
using SURAKSHA.Database.Repository;
using System.Text.Json;
using RMS_API.Models.ViewModel;

namespace SURAKSHA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RMSController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        
        #region Constructor
        public RMSController(ILogger<RMSController> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;

        }
        #endregion


        #region ProductList 
        [HttpPost]
        [Route("ProductList")]

        public async Task<IActionResult> ProductList()
        {
            _logger.LogInformation("Start : ProductList");
            RMSController rmsController = this;
            RMSRepository rMSRepository    = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>());
            List<ProductViewAPIModel> productViewAPIModels = await rMSRepository.GetProductList();

            _logger.LogInformation("Exit : UserRegistration");
            return Ok(productViewAPIModels);

        }
        #endregion

        #region RestaurantList 
        [HttpPost]
        [Route("RestaurantList")]

        public async Task<IActionResult> RestaurantList()
        {
            _logger.LogInformation("Start : RestaurantList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>());
            List<RestaurantViewAPIModel> productViewAPIModels = await rMSRepository.RestaurantListAPI();
            _logger.LogInformation("Exit : RestaurantList");
            return Ok(productViewAPIModels);

        }
        #endregion
    }

}
