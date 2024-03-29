﻿using SURAKSHA.Database.Repository;
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
using System.Configuration;
using SURAKSHA_API.Database.Repository;

namespace SURAKSHA.Database.Repository
{
    public class LoginRepository 
    {
   
        private readonly ILogger<LoginRepository> _logger;
        private IConfiguration _conConfig;
        public LoginRepository(ILogger<LoginRepository> logger)
        {
            _logger = logger;
        }

        public LoginRepository(ILogger<LoginRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _conConfig = configuration;
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
        public async Task<List<LoginListModel>> GetLoginImagesList()
        {
            List<LoginListModel> LoginImageViewAPIModels = new List<LoginListModel>();
            try
            {
                string ContainerUrl = _conConfig["URL:containerURL"];
                DataSet dataSet = await SqlHelper.ExecuteDatasetAsync(conn, CommandType.StoredProcedure, "GetLoginImages");
                LoginImageViewAPIModels = AppSettingsHelper.ToListof<LoginListModel>(dataSet.Tables[0]);

                LoginImageViewAPIModels.Where(itm => String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = "no photo.jpg");
                LoginImageViewAPIModels.Where(itm => !String.IsNullOrEmpty(itm.Image)).ToList().ForEach(x => x.Image = ContainerUrl + x.Image);

                //productViewAPIModel.ForEach(x => x.Image = ContainerUrl + x.Image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return LoginImageViewAPIModels;
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
                new SqlParameter("@Mobile_no",User.LoginId),
                new SqlParameter("@Pin",str),
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
            new SqlParameter("@Image",  User.Image),
            new SqlParameter("@Password",   Utility.EncryptText(User.Password)),
            new SqlParameter("@Lat",  User.Lat),
            new SqlParameter("@Long",  User.Long),
            new SqlParameter("@Gender",  User.Gender),
            new SqlParameter("@DOB",  User.DOB),

            outRetStatus , outRetMessage};


            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "User_Registration", param);
                response = new RMS_API.Models.Response();
                if (param[13].Value != DBNull.Value)// status
                    response.Status = Convert.ToInt16(param[13].Value );
                if (param[14].Value != DBNull.Value)// status
                    response.message = Convert.ToString(param[14].Value);

            }
            catch (Exception ex)
            {
                response = new RMS_API.Models.Response();
                response.Status = -1;
                response.message = "Internal Server Error";
            }
            return Task.FromResult<RMS_API.Models.Response>(response);
        }

        public Task<RMS_API.Models.Response> RestaurantRegistrationAPI(RestaurantRegitrationModel User)
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
            new SqlParameter("@RestaurentName",  User.RestaurentName),
            new SqlParameter("@MobileNo",  User.MobileNo),
            new SqlParameter("@MailID",  User.MailID),
            new SqlParameter("@Address",  User.Address),
            new SqlParameter("@Area_Code",  User.Area_Code),
            new SqlParameter("@Image",  User.Image),
            new SqlParameter("@Password",   Utility.EncryptText(User.Password)),
            new SqlParameter("@Lat",  User.Lat),
            new SqlParameter("@Long",  User.Long),
            new SqlParameter("@Timings",  User.Timings),

            outRetStatus , outRetMessage};


            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "Restaurent_Registration", param);
                response = new RMS_API.Models.Response();
                if (param[11].Value != DBNull.Value)// status
                    response.Status = Convert.ToInt16(param[11].Value);
                if (param[12].Value != DBNull.Value)// status
                    response.message = Convert.ToString(param[12].Value);

            }
            catch (Exception ex)
            {
                response = new RMS_API.Models.Response();
                response.Status = -1;
                response.message = "Internal Server Error";
            }
            return Task.FromResult<RMS_API.Models.Response>(response);
        }


        public Task<RMS_API.Models.Response> AddProductAPI(AddProductModel Product)
        {
            RMS_API.Models.Response response;

            SqlParameter outRetStatus = new SqlParameter();
            outRetStatus.ParameterName = "@Ret_Status";
            outRetStatus.DbType = DbType.Int32;
            outRetStatus.Size = 8;
            outRetStatus.Direction = ParameterDirection.Output;


            SqlParameter[] param ={
             new SqlParameter("@PID",Product.PID),
                new SqlParameter("@Pname",Product.ProductName),
                new SqlParameter("@isVeg",Product.isVeg),
                new SqlParameter("@IsAlcoholic",Product.IsAlcoholic),
                new SqlParameter("@Image",Product.Image),
                new SqlParameter("@RestaurantRegID",Product.RestaurantRegID),
                new SqlParameter("@PRate",Product.PRate),
                new SqlParameter("@offerID",Product.offerID),
                    outRetStatus};


            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "AddProduct", param);
                response = new RMS_API.Models.Response();
                if (param[8].Value != DBNull.Value)// status
                    response.Status = Convert.ToInt16(param[8].Value);
                    response.message = "Product Added Successfully";

            }
            catch (Exception ex)
            {
                response = new RMS_API.Models.Response();
                response.Status = 0;
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
