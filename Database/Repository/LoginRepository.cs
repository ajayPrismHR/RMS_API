using SURAKSHA.Database.Repository;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SURAKSHA.Models.QueryModel;
using SURAKSHA.Models.ViewModel;
using SURAKSHA.Controllers;
using Serilog;
using Microsoft.Extensions.Logging;
using SURAKSHA.Filters;
using System.Data.Common;
using System.Net;
using System.Runtime.InteropServices;
using System;
using RMS_API.Models.ViewModel;
using RMS_API.Models.QueryModel;

namespace SURAKSHA.Database.Repository
{
    public class LoginRepository 
    {
   
        private readonly ILogger<LoginRepository> _logger;

        public LoginRepository(ILogger<LoginRepository> logger)
        {
            _logger = logger;
        }

        private string conn=AppSettingsHelper.Setting(Key: "ConnectionStrings:DevConn");


        public UserViewModel ValidateUser(UserRequestQueryModel user)
        {
            List<UserViewModel> userViewModel = new List<UserViewModel>();
            UserViewModel userViewModelReturn = new UserViewModel();  
            try
            {
                SqlParameter[] param ={new SqlParameter("@Username",user.LoginId.Trim()),new SqlParameter("@Password",Utility.EncryptText(user.Password.Trim()) )};
                DataSet dataSet = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "Validate_User_API_LOGIN", param);
                userViewModel = AppSettingsHelper.ToListof<UserViewModel>(dataSet.Tables[0]);
                userViewModelReturn = userViewModel[0];
                _logger.LogInformation(conn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return userViewModelReturn;
        }

        public RestaurantViewModel ValidateRestaurant(UserRequestQueryModel user)
        {
            List<RestaurantViewModel> userViewModel = new List<RestaurantViewModel>();
            RestaurantViewModel userViewModelReturn = new RestaurantViewModel();
            try
            {
                SqlParameter[] param = { new SqlParameter("@Username", user.LoginId.Trim()), new SqlParameter("@Password", Utility.EncryptText(user.Password.Trim())) };
                DataSet dataSet = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "Validate_Restaurant_API_LOGIN", param);
                userViewModel = AppSettingsHelper.ToListof<RestaurantViewModel>(dataSet.Tables[0]);
                userViewModelReturn = userViewModel[0];
                _logger.LogInformation(conn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return userViewModelReturn;
        }

        public UserViewAPIModel ValidateUserAPI(UserRequestQueryModel user)
        {
            List<UserViewAPIModel> userViewModel = new List<UserViewAPIModel>();
            UserViewAPIModel userViewModelReturn = new UserViewAPIModel();
            try
            {
                SqlParameter[] param = { new SqlParameter("@Username", user.LoginId.Trim()), new SqlParameter("@Password", Utility.EncryptText(user.Password.Trim())) };
                DataSet dataSet = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "Validate_User_API_LOGIN", param);
                userViewModel = AppSettingsHelper.ToListof<UserViewAPIModel>(dataSet.Tables[0]);
                userViewModelReturn = userViewModel[0];
                _logger.LogInformation(conn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return userViewModelReturn;
        }

        public async Task<int> ChangePassword(UserRequestQueryModel User)
        {
            int retStatus = 0;
            string str = Utility.EncryptText(User.Password);
            SqlParameter sqlParameter = new SqlParameter();
            SqlParameter parmretStatus = new SqlParameter();
            parmretStatus.ParameterName = "@retStatus";
            parmretStatus.DbType = DbType.Int32;
            parmretStatus.Size = 8;
            parmretStatus.Direction = ParameterDirection.Output;
            SqlParameter[] param ={
                new SqlParameter("@Emp_Name",User.LoginId),
                new SqlParameter("@PASSWORD",str),
                    parmretStatus};
            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "ChangePassword", param);

                if (param[2].Value != DBNull.Value)// status
                    retStatus = Convert.ToInt32(param[2].Value);
                else
                    retStatus = 2;
            }
            catch (Exception ex)
            {
                retStatus = -1;
            }



            return retStatus;
        }
        public Task<RMS_API.Models.Response> UserRegistration(UserRegistration User)
        {
            RMS_API.Models.Response response; 
            
            SqlParameter outRetStatus = new SqlParameter();
            outRetStatus.ParameterName = "@Ret_Status";
            outRetStatus.DbType = DbType.Int32;
            outRetStatus.Size = 8;
            outRetStatus.Direction = ParameterDirection.Output;

            SqlParameter outRetMessage = new SqlParameter();
            outRetMessage.ParameterName = "@Ret_Message";
            outRetMessage.DbType = DbType.String;
            outRetMessage.Size = 1000;
            outRetMessage.Direction = ParameterDirection.Output;

            SqlParameter[] param ={
            new SqlParameter("@RegistrationID",  User.registrationID),
            new SqlParameter("@FirstName",  User.FirstName),
            new SqlParameter("@LastName",  User.LastName),
            new SqlParameter("@MobileNo",  User.MobileNo),
            new SqlParameter("@MailID",  User.MailID),
            new SqlParameter("@Address",  User.Address),
            new SqlParameter("@Area_Code",  User.Area_Code),
            new SqlParameter("@UserType",  User.UserType),
            new SqlParameter("@Image",  User.Image),
            new SqlParameter("@Password",   Utility.EncryptText(User.Password)),
            new SqlParameter("@Lat",  User.Lat),
            new SqlParameter("@Long",  User.Long),
            new SqlParameter("@Updatedby",User.Updatedby),
            new SqlParameter("@UpdateOn",User.UpdateOn),
            new SqlParameter("@RegisteredBy",User.RegisteredBy), 
            new SqlParameter("@RegisteredOn",User.RegisteredOn),
            outRetStatus , outRetMessage};


            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "User_Registration", param);
                response = new RMS_API.Models.Response();
                if (param[16].Value != DBNull.Value)// status
                    response.Status = Convert.ToInt16(param[16].Value );
                if (param[17].Value != DBNull.Value)// status
                    response.message = Convert.ToString(param[17].Value);

            }
            catch (Exception ex)
            {
                response = new RMS_API.Models.Response();
                response.Status = -1;
                response.message = "Internal Server Error";
            }
            return Task.FromResult<RMS_API.Models.Response>(response);
        }

        public async Task<int> CheckMobileNoAPI(MobileNoCheck mobileno)
        {
            List<RestaurantViewAPIModel> restaurantViewAPIModels = new List<RestaurantViewAPIModel>();
            int retStatus = 0;
            SqlParameter parmretStatus = new SqlParameter();
            parmretStatus.ParameterName = "@Ret_Status";
            parmretStatus.DbType = DbType.Int32;
            parmretStatus.Size = 8;
            parmretStatus.Direction = ParameterDirection.Output;

            SqlParameter[] param ={
            new SqlParameter("@MobileNO",(object) mobileno.MobileNo),
            parmretStatus
            };
            SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "Check_MobileNo", param);

            if (param[1].Value != DBNull.Value)// status
                retStatus = Convert.ToInt32(param[1].Value);
            else
                retStatus = 0;
            return retStatus;

        }

    }
}
