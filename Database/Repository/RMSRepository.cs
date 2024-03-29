﻿
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
                restaurantViewAPIModels.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                restaurantViewAPIModels.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);


                //restaurantViewAPIModels.ForEach(x => x.Image = ContainerUrl + x.Image);
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

                restaurantViewAPIModels.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                restaurantViewAPIModels.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);
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
                restaurantViewAPIModels.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                restaurantViewAPIModels.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);
                //restaurantViewAPIModels.ForEach(x => x.Image = ContainerUrl + x.Image);
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
                if (UserDetailAPIModels != null)
                {
                    if (UserDetailAPIModels.IMAGE == "")
                    {
                        UserDetailAPIModels.IMAGE = ContainerUrl + "no photo.jpg";
                    }
                    else
                    {
                        UserDetailAPIModels.IMAGE = ContainerUrl + UserDetailAPIModels.IMAGE;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return UserDetailAPIModels;
        }

        public async Task<List<OfferDetailModel>> GetOfferDetailAPI()
        {
            //OfferDetailModel? OfferDetailAPIModels = new OfferDetailModel();
            List<OfferDetailModel> OfferDetailAPIModels = new List<OfferDetailModel>();
            try
            {
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "GetOfferMaster");
                OfferDetailAPIModels = AppSettingsHelper.ToListof<OfferDetailModel>(dataSet.Tables[0]);
                string ContainerUrl = _conConfig["URL:containerURL"];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return OfferDetailAPIModels;
        }
        public async Task<List<ProductViewAPIModel>> GetProductList(RestaurantIDModel restaurantRegistrationID)
        {
            List<ProductViewAPIModel> productViewAPIModel = new List<ProductViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@RegistrationID", restaurantRegistrationID.RestaurantRegistrationID)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "Restaurent_Product_List_New", param);
                productViewAPIModel = AppSettingsHelper.ToListof<ProductViewAPIModel>(dataSet.Tables[0]);

                productViewAPIModel.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                productViewAPIModel.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return  productViewAPIModel;
        }

        

        public async Task<List<AllProductViewAPIModel>> GetAllProductList()
        {
            List<AllProductViewAPIModel> productViewAPIModel = new List<AllProductViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "All_Product_List");
                productViewAPIModel = AppSettingsHelper.ToListof<AllProductViewAPIModel>(dataSet.Tables[0]);

                productViewAPIModel.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                productViewAPIModel.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return productViewAPIModel;
        }

        public async Task<List<ProductViewAPIModel>> GetProductListSearch(SearchRestaurantIDModel restaurantRegistrationID)
        {
            List<ProductViewAPIModel> productViewAPIModel = new List<ProductViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@RegistrationID", restaurantRegistrationID.RestaurantRegistrationID),
                new SqlParameter("@Name", restaurantRegistrationID.Name)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "Search_Restaurent_Product_List", param);
                productViewAPIModel = AppSettingsHelper.ToListof<ProductViewAPIModel>(dataSet.Tables[0]);

                productViewAPIModel.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                productViewAPIModel.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return productViewAPIModel;
        }

        public async Task<List<FrequentlyProductViewAPIModel>> FrequentlyOrderedProductOrderSearch(UserIDModel UserId)
        {
            List<FrequentlyProductViewAPIModel> productViewAPIModel = new List<FrequentlyProductViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];

                SqlParameter[] param ={
                new SqlParameter("@USERID", UserId.UserID)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "Top_10_FrequentlyOrderedITMES", param);
                productViewAPIModel = AppSettingsHelper.ToListof<FrequentlyProductViewAPIModel>(dataSet.Tables[0]);

                productViewAPIModel.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                productViewAPIModel.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return productViewAPIModel;
        }


        public async Task<List<OrderListViewAPIModel>> GetOrderMastertList(UserIDModel UserId)
        {
            List<OrderListViewAPIModel> orderListViewAPIModel = new List<OrderListViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@userID", UserId.UserID)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "OrderMaster", param);
                orderListViewAPIModel = AppSettingsHelper.ToListof<OrderListViewAPIModel>(dataSet.Tables[0]);
                orderListViewAPIModel.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                orderListViewAPIModel.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);
                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return orderListViewAPIModel;
        }


        public async Task<List<OrderDetailViewAPIModel>> GetOrderDetailList(OrderIDModel OrderId)
        {
            List<OrderDetailViewAPIModel> orderListViewAPIModel = new List<OrderDetailViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@OrderID", OrderId.OrderID)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "OrderDetail", param);
                orderListViewAPIModel = AppSettingsHelper.ToListof<OrderDetailViewAPIModel>(dataSet.Tables[0]);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return orderListViewAPIModel;
        }

        public async Task<List<OrderDetailViewAPIModel>> GetOrderDetaitoRestaurantlList(OrderIDForRestaurantModel OrderId)
        {
            List<OrderDetailViewAPIModel> orderListViewAPIModel = new List<OrderDetailViewAPIModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@OrderID", OrderId.OrderID),
                new SqlParameter("@RestaurantID", OrderId.RestaurantID),
                new SqlParameter("@UserID", OrderId.UserID)};
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "GetOrderDetail", param);
                orderListViewAPIModel = AppSettingsHelper.ToListof<OrderDetailViewAPIModel>(dataSet.Tables[0]);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return orderListViewAPIModel;
        }

        public async Task<int> OrderCountList(UserIDModel UserId)
        {
            int retStatus = 0;
            try
            {
                
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter parmretStatus = new SqlParameter();
                parmretStatus.ParameterName = "@orderCount";
                parmretStatus.DbType = DbType.Int32;
                parmretStatus.Size = 8;
                parmretStatus.Direction = ParameterDirection.Output;
                SqlParameter[] param ={
                new SqlParameter("@Costumer_ID", UserId.UserID),
                new SqlParameter("@Restr_ID", UserId.Restaurant_ID),parmretStatus};
                int Ocount= SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "CheckForFeeOrder", param);
                if (param[2].Value != DBNull.Value)// status
                    retStatus = Convert.ToInt32(param[2].Value);
                else
                retStatus = 0;
                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return retStatus;
        }


        public async Task<int> SubmitRestaurantRatings(RatingsModel Ratings)
        {
            int retStatus = 0;
            try
            {

                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter parmretStatus = new SqlParameter();
                parmretStatus.ParameterName = "@RetStatus";
                parmretStatus.DbType = DbType.Int32;
                parmretStatus.Size = 8;
                parmretStatus.Direction = ParameterDirection.Output;
                SqlParameter[] param ={
                new SqlParameter("@UserID", Ratings.UserID),
                new SqlParameter("@RestroID", Ratings.RestroID),
                new SqlParameter("@OrderID", Ratings.OrderID),
                new SqlParameter("@Ratings", Ratings.Ratings),
                new SqlParameter("@Review", Ratings.Review),parmretStatus};
                int Ocount = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "SubmitRatings", param);
                if (param[4].Value != DBNull.Value)// status
                    retStatus = Convert.ToInt32(param[4].Value);
                else
                    retStatus = 0;
                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return retStatus;
        }

        public async Task<List<GetRatingsModel>> GetRestaurantRatingsList()
        {
            List<GetRatingsModel> RatingListViewAPIModel = new List<GetRatingsModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
               
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "GetRestroRatings");
                RatingListViewAPIModel = AppSettingsHelper.ToListof<GetRatingsModel>(dataSet.Tables[0]);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return RatingListViewAPIModel;
        }

        public async Task<List<GetOrderWiseRatingsModel>> GetOrderWiseRestaurantRatingsList(OrderWiseRatingsModel RR)
        {
            List<GetOrderWiseRatingsModel> RatingListViewAPIModel = new List<GetOrderWiseRatingsModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                SqlParameter[] param ={
                new SqlParameter("@UserID", RR.UserID),
                new SqlParameter("@orderID", RR.OrderID) };
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "GetOrderWiseRestroRatings",param);
                RatingListViewAPIModel = AppSettingsHelper.ToListof<GetOrderWiseRatingsModel>(dataSet.Tables[0]);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return RatingListViewAPIModel;
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

        public async Task<int> UpdateOrderStatusList(OrderStatusUpdateModel OrderStatus)
        {
            int retStatus = 0;
            SqlParameter sqlParameter = new SqlParameter();
            SqlParameter parmretStatus = new SqlParameter();
            parmretStatus.ParameterName = "@Ret_status";
            parmretStatus.DbType = DbType.Int32;
            parmretStatus.Size = 8;
            parmretStatus.Direction = ParameterDirection.Output;
            SqlParameter[] param ={
                new SqlParameter("@OrderID",OrderStatus.OrderID),
                new SqlParameter("@Status",OrderStatus.Status),
                    parmretStatus};
            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "UpdateOrderStatus", param);

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
        public async Task<Int64> CreateOrder(OrderMasterModel oMM)
        {
            Int64 retOrderId = 0;
            SqlParameter parmretOrderId = new SqlParameter();
            parmretOrderId.ParameterName = "@Orderid";
            parmretOrderId.DbType = DbType.Int64;
            parmretOrderId.Size = 8;
            parmretOrderId.Direction = ParameterDirection.Output;

            SqlParameter parmretStatus = new SqlParameter();
            parmretStatus.ParameterName = "@Status";
            parmretStatus.DbType = DbType.String;
            parmretStatus.Size = 200;
            parmretStatus.Direction = ParameterDirection.Output;


            SqlParameter[] param ={
                new SqlParameter("@Restro_ID", oMM.Restro_ID),
                new SqlParameter("@Costumer_ID",oMM.Costumer_ID),
                new SqlParameter("@Order_Date",oMM.Order_Date),
                new SqlParameter("@QR_Details",oMM.QR_Details),
                new SqlParameter("@Is_Finalized",oMM.Is_Finalized),parmretOrderId,parmretStatus};
            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "InsertOrderTransationMaster", param);

                if (parmretOrderId.Value != DBNull.Value)
                {
                    retOrderId = Convert.ToInt64(parmretOrderId.Value);
                }

                OrderDetailsModel orderDetailsModel;
                foreach (var item in oMM.orderDetailsModels)
                {
                    orderDetailsModel = new OrderDetailsModel();
                    orderDetailsModel.ProductID = item.ProductID;
                    orderDetailsModel.Quantity = item.Quantity;
                    orderDetailsModel.OfferID = item.OfferID;
                    orderDetailsModel.Price = item.Price;
                    await CreateOrderDetails(retOrderId,orderDetailsModel);
                }

            }
            catch (Exception ex)
            {
                retOrderId = -1;
            }



            return retOrderId;
        }

        public async Task<string> CreateOrderDetails(Int64 orderId, OrderDetailsModel oDM)
        {
            string retStatus = string.Empty;

            SqlParameter parmretStatus = new SqlParameter();
            parmretStatus.ParameterName = "@Status";
            parmretStatus.DbType = DbType.String;
            parmretStatus.Size = 200;
            parmretStatus.Direction = ParameterDirection.Output;


            SqlParameter[] param ={
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@ProductID",oDM.ProductID),
                new SqlParameter("@OfferID",oDM.OfferID),
                new SqlParameter("@Quantity",oDM.Quantity),
                new SqlParameter("@Price",oDM.Price),parmretStatus};
            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "InsertOrderDetails", param);

                if (parmretStatus.Value != DBNull.Value)
                {
                    retStatus = Convert.ToString(parmretStatus.Value.ToString());
                }
                
            }
            catch (Exception ex)
            {
                return retStatus ;
            }
            return retStatus;
        }

    }
}
