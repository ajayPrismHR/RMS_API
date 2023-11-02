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
using Azure.Communication.Sms;
using System.Collections.Generic;

namespace SURAKSHA.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RMSController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration  _configuration;


        #region Constructor
        public RMSController(ILogger<RMSController> logger, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _configuration = configuration;

        }
        #endregion


        #region ProductList 
        [HttpPost]
        [Route("ProductList")]

        public async Task<IActionResult> ProductList(RestaurantIDModel restaurantRegistrationID)
        {
            _logger.LogInformation("Start : ProductList");
            RMSController rmsController = this;
            RMSRepository rMSRepository    = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<ProductViewAPIModel> productViewAPIModels = await rMSRepository.GetProductList(restaurantRegistrationID);

            _logger.LogInformation("Exit : ProductList");
            return Ok(productViewAPIModels);

        }
        #endregion

        #region SearchProductList 
        [HttpPost]
        [Route("SearchProductList")]

        public async Task<IActionResult> SearchProductList(SearchRestaurantIDModel restaurantRegistrationID)
        {
            _logger.LogInformation("Start : ProductList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<ProductViewAPIModel> productViewAPIModels = await rMSRepository.GetProductListSearch(restaurantRegistrationID);

            _logger.LogInformation("Exit : ProductList");
            return Ok(productViewAPIModels);

        }
        #endregion

        #region FrequentlyOrderedProductOrder 
        [HttpPost]
        [Route("FrequentlyOrderedProductOrder")]

        public async Task<IActionResult> FrequentlyOrderedProductOrder(UserIDModel UserId)
        {
            _logger.LogInformation("Start : ProductList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<FrequentlyProductViewAPIModel> productViewAPIModels = await rMSRepository.FrequentlyOrderedProductOrderSearch(UserId);

            _logger.LogInformation("Exit : ProductList");
            return Ok(productViewAPIModels);

        }
        #endregion

        #region OrderMasterList 
        [HttpPost]
        [Route("OrderMasterList")]

        public async Task<IActionResult> OrderMastertList(UserIDModel UserId)
        {
            _logger.LogInformation("Start : OrderMasterList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<OrderListViewAPIModel> orderListViewAPIModel = await rMSRepository.GetOrderMastertList(UserId);

            _logger.LogInformation("Exit : OrderMasterList");
            return Ok(orderListViewAPIModel);

        }
        #endregion

        #region OrderDetailList 
        [HttpPost]
        [Route("OrderDetailList")]

        public async Task<IActionResult> OrderDetailList(OrderIDModel OrderId)
        {
            _logger.LogInformation("Start : OrderMastertList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<OrderDetailViewAPIModel> orderListViewAPIModel = await rMSRepository.GetOrderDetailList(OrderId);

            _logger.LogInformation("Exit : OrderMastertList");
            return Ok(orderListViewAPIModel);

        }
        #endregion

        #region OrderCOunt 
        [HttpPost]
        [Route("OrderCOunt")]

        public async Task<IActionResult> OrderCOunt(UserIDModel UserId)
        {
            _logger.LogInformation("Start : OrderCOunt");
            RMSController rmsController = this;
            ReturnStatusModel returnStatus = new ReturnStatusModel();
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            int OrderCount = await rMSRepository.OrderCountList(UserId);
            returnStatus.response = OrderCount;
            returnStatus.status = "Total Order Count";
            return Ok(returnStatus);
           
            _logger.LogInformation("Exit : OrderCOunt");
           
        }
        #endregion

        #region RestaurantList 
        [HttpPost]
        [Route("RestaurantList")]

        public async Task<IActionResult> RestaurantList(RestaurantListModel resList)
        {
            _logger.LogInformation("Start : RestaurantList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<RestaurantViewAPIModel> productViewAPIModels = await rMSRepository.RestaurantListAPI(resList);
            _logger.LogInformation("Exit : RestaurantList");
            return Ok(productViewAPIModels);

        }
        #endregion

        #region SearchRestaurantList 
        [HttpPost]
        [Route("SearchRestaurantList")]

        public async Task<IActionResult> SearchRestaurantList(SearchRestaurantModel resList)
        {
            _logger.LogInformation("Start : SearchRestaurantList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<RestaurantViewAPIModel> productViewAPIModels = await rMSRepository.SearchRestaurantListAPI(resList);
            _logger.LogInformation("Exit : SearchRestaurantList");
            return Ok(productViewAPIModels);

        }
        #endregion

        #region FavouritehRestaurantList 
        [HttpPost]
        [Route("FavouritehRestaurantList")]

        public async Task<IActionResult> FavouritehRestaurantList(FavouriteRestaurantListSearchModel resList)
        {
            _logger.LogInformation("Start : FavouriteRestaurantList");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List<RestaurantViewAPIModel> productViewAPIModels = await rMSRepository.FavouritehRestaurantListAPI(resList);
            _logger.LogInformation("Exit : FavouriteRestaurantList");
            return Ok(productViewAPIModels);

        }
        #endregion

        #region RestaurantList 
        [HttpPost]
        [Route("GetUserDetail")]

        public async Task<IActionResult> GetUserDetail(MobileNoCheck Mobile_no)
        {
            _logger.LogInformation("Start : UserDetail");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            UserDetailModel UserDetailAPIModels = await rMSRepository.GetUserDetailAPI(Mobile_no);
            _logger.LogInformation("Exit : UserDetail");
            return Ok(UserDetailAPIModels);

        }
        #endregion

        #region OfferList 
        [HttpPost]
        [Route("GetOfferDetail")]

        public async Task<IActionResult> GetOfferDetail()
        {
            _logger.LogInformation("Start : OfferDetail");
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            List < OfferDetailModel> OfferDetailAPIModels = await rMSRepository.GetOfferDetailAPI();
            _logger.LogInformation("Exit : OfferDetail");
            return Ok(OfferDetailAPIModels);

        }
        #endregion

        [HttpPost]
        [Route("SendSMS")]
        public IActionResult  SendSMS()
        {

            string connectionString = "endpoint=https://respmsmsapp.unitedstates.communication.azure.com/;accesskey=Z8ZbdJFadcJWWV7j6qaAlBAFWHG5pK+4Gf4MGIAe2cvyQWHYyFTcoDaEqbyCdDHm93MXeiLV1lZGz+c3l7WWNA==";
            SmsClient smsClient = new SmsClient(connectionString);
            smsClient.Send(
                
                from: "+919166188989", 
                to:  new string[] { "+919828031268" }, 
                message: "Test message from Punch Mate",
                options: new SmsSendOptions(enableDeliveryReport: true)
                {
                    Tag = "food",


                });   

            return Ok();
        }

        [HttpPost]
        [Route("ManageFevouriteRestaurants")]
        public async Task<IActionResult> ManageFevouriteRestaurants(ManageRestaurantFavouritesModel FevRes)
        {
            int retStatus = 0;
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            ReturnStatusModel returnStatus = new ReturnStatusModel();
            UserViewAPIModel userViewApiModel = new UserViewAPIModel();
            retStatus = await rMSRepository.ManageFevouriteRestaurantsList(FevRes);
            if (retStatus == 1)
            {
                returnStatus.response = 1;
                returnStatus.status = "Successfully updated to Favourites";
                return Ok(returnStatus);
            }
            else
            {
                returnStatus.response = 0;
                returnStatus.status = "Error Occured";
                return Ok(returnStatus);
            }

        }


        #region CreateOrder 
        [HttpPost]
        [Route("CreateOrder")]

        public async Task<IActionResult> CreateOrder(OrderMasterModel model)
        {
            _logger.LogInformation("Start : CreateOrder");
            ReturnStatusModel1 returnStatus = new ReturnStatusModel1();
            RMSController rmsController = this;
            RMSRepository rMSRepository = new RMSRepository(rmsController._loggerFactory.CreateLogger<RMSRepository>(), _configuration);
            Int64 orderId = await rMSRepository.CreateOrder(model);
            _logger.LogInformation("Exit : CreateOrder");
            if (orderId >= 1)
            {
                returnStatus.response = orderId;
                returnStatus.status = "Order Placed Successfully";
                return Ok(returnStatus);
            }
            else
            {
                returnStatus.response = 0;
                returnStatus.status = "Error Occured";
                return Ok(returnStatus);
            }
            

        }
        #endregion

      
    }

}
