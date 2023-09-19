
using SURAKSHA.Database.Repository;
using SURAKSHA;
using SURAKSHA.Models;
using SURAKSHA.Models.QueryModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using RMS_API.Models.ViewModel;
using RMS_API.Models.QueryModel;
using Microsoft.Extensions.Azure;

namespace SURAKSHA_API.Database.Repository
{
    
    public class RMSRepository
    {
        private readonly ILogger<RMSRepository> _logger;
        private string conn = AppSettingsHelper.Setting(Key: "ConnectionStrings:DevConn");
        private IConfiguration _conConfig;
        

        public RMSRepository(ILogger<RMSRepository> logger,IConfiguration configuration)
        {
            _logger = logger;
            _conConfig = configuration;
        }

        public async Task<List<RestaurantViewAPIModel>> RestaurantListAPI(RestaurantListModel resList)
        {
            List<RestaurantViewAPIModel> restaurantViewAPIModels = new List<RestaurantViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@current_Lat",resList.Current_Lat),
                new SqlParameter("@current_Log",resList.Current_Log),
                new SqlParameter("@filter",resList.FilterRange)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "RestaurantList", param);
                restaurantViewAPIModels = AppSettingsHelper.ToListof<RestaurantViewAPIModel>(dataSet.Tables[0]);

                restaurantViewAPIModels.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return restaurantViewAPIModels;
        }

        public async Task<List<RestaurantViewAPIModel>> SearchRestaurantListAPI(SearchRestaurantModel resList)
        {
            List<RestaurantViewAPIModel> restaurantViewAPIModels = new List<RestaurantViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@current_Lat",resList.Current_Lat),
                new SqlParameter("@current_Log",resList.Current_Log),
                new SqlParameter("@Name",resList.Name)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "SearchRestaurantList", param);
                restaurantViewAPIModels = AppSettingsHelper.ToListof<RestaurantViewAPIModel>(dataSet.Tables[0]);

                restaurantViewAPIModels.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return restaurantViewAPIModels;
        }

        public async Task<List<RestaurantViewAPIModel>> FavouritehRestaurantListAPI(FavouriteRestaurantListSearchModel resList)
        {
            List<RestaurantViewAPIModel> restaurantViewAPIModels = new List<RestaurantViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@current_Lat",resList.Current_Lat),
                new SqlParameter("@current_Log",resList.Current_Log),
                new SqlParameter("@UserID",resList.UserID)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "FevouriteRestaurantList", param);
                restaurantViewAPIModels = AppSettingsHelper.ToListof<RestaurantViewAPIModel>(dataSet.Tables[0]);

                restaurantViewAPIModels.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return restaurantViewAPIModels;
        }

        public async Task<UserDetailModel> GetUserDetailAPI(MobileNoCheck Mobile_no)
        {
            UserDetailModel ?UserDetailAPIModels = new UserDetailModel();
            try
            {
                SqlParameter[] param ={
                new SqlParameter("@Mobile_NO",Mobile_no.MobileNo)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "Get_User_Detail", param);
                UserDetailAPIModels = AppSettingsHelper.ToSingleObject<UserDetailModel>(dataSet.Tables[0]);
                string ContainerUrl = _conConfig["URL:containerURL"];
                if (UserDetailAPIModels!=null)
                UserDetailAPIModels.IMAGE = ContainerUrl + UserDetailAPIModels.IMAGE;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return UserDetailAPIModels;
        }
        public async Task<List<ProductViewAPIModel>> GetProductList(RestaurantIDModel restaurantRegistrationID)
        {
            List<ProductViewAPIModel> productViewAPIModel = new List<ProductViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@RegistrationID", restaurantRegistrationID.RestaurantRegistrationID),
                new SqlParameter("@Userid", restaurantRegistrationID.UserID)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "Restaurent_Product_List", param);
                productViewAPIModel = AppSettingsHelper.ToListof<ProductViewAPIModel>(dataSet.Tables[0]);
                productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return  productViewAPIModel;
        }

        public async Task<int> ManageFevouriteRestaurantsList(ManageRestaurantFavouritesModel FevRes)
        {
            int retStatus = 0;
            SqlParameter sqlParameter = new SqlParameter();
            SqlParameter parmretStatus = new SqlParameter();
            parmretStatus.ParameterName = "@Ret_Status";
            parmretStatus.DbType = DbType.Int32;
            parmretStatus.Size = 8;
            parmretStatus.Direction = ParameterDirection.Output;
            SqlParameter[] param ={
                new SqlParameter("@RestaurantRegID",FevRes.RestaurantRegID),
                new SqlParameter("@UserID",FevRes.UserID),
                    parmretStatus};
            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "ManageFevouriteRestaurants", param);

                if (param[2].Value != DBNull.Value)// status
                    retStatus = Convert.ToInt32(param[2].Value);
                else
                    retStatus = 0;
            }
            catch (Exception ex)
            {
                retStatus = -1;
            }



            return retStatus;
        }

        //public async Task<int> CheckMobileNoAPI(MobileNoCheck mobileno)
        //{
        //    List<RestaurantViewAPIModel> restaurantViewAPIModels = new List<RestaurantViewAPIModel>();
        //    int retStatus = 0;
        //    SqlParameter parmretStatus = new SqlParameter();
        //    parmretStatus.ParameterName = "@Ret_Status";
        //    parmretStatus.DbType = DbType.Int32;
        //    parmretStatus.Size = 8;
        //    parmretStatus.Direction = ParameterDirection.Output;

        //    SqlParameter[] param ={
        //    new SqlParameter("@MobileNO",(object) mobileno.MobileNo),
        //    parmretStatus
        //    };
        //    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "Check_MobileNo", param);

        //    if (param[10].Value != DBNull.Value)// status
        //        retStatus = Convert.ToInt32(param[10].Value);
        //    else
        //        retStatus = 0;
        //    return retStatus;

        //}

    }
}
