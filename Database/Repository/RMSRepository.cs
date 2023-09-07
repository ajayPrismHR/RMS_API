
using SURAKSHA.Database.Repository;
using SURAKSHA;
using SURAKSHA.Models;
using SURAKSHA.Models.QueryModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using RMS_API.Models.ViewModel;

namespace SURAKSHA_API.Database.Repository
{
    
    public class RMSRepository
    {
        private readonly ILogger<RMSRepository> _logger;
        private string conn = AppSettingsHelper.Setting(Key: "ConnectionStrings:DevConn");
        public RMSRepository(ILogger<RMSRepository> logger)
        {
            _logger = logger;
        }

        public async Task<List<RestaurantViewAPIModel>> RestaurantListAPI()
        {
            List<RestaurantViewAPIModel> restaurantViewAPIModels = new List<RestaurantViewAPIModel>();
            try
            {
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "RestaurantList");
                restaurantViewAPIModels = AppSettingsHelper.ToListof<RestaurantViewAPIModel>(dataSet.Tables[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return restaurantViewAPIModels;
        }
        public async Task<List<ProductViewAPIModel>> GetProductList()
        {
            List<ProductViewAPIModel> productViewAPIModel = new List<ProductViewAPIModel>();
            try
            {
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "All_Product_List");
                productViewAPIModel = AppSettingsHelper.ToListof<ProductViewAPIModel>(dataSet.Tables[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return  productViewAPIModel;
        }

    }
}
