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
        public Task<string> UserRegistration(UserRegistration User)
        {
            string retStatus = string.Empty;
            string str = Utility.EncryptText(User.Password);
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
            new SqlParameter("@RegistrationID",  User.RegistrationID),
            new SqlParameter("@FirstName",  User.FirstName),
            new SqlParameter("@LastName",  User.LastName),
            new SqlParameter("@MobileNo",  User.MobileNo),
            new SqlParameter("@MailID",  User.MailID),
            new SqlParameter("@Address",  User.Address),
            new SqlParameter("@Area_Code",  User.Area_Code),
            new SqlParameter("@UserType",  User.UserType),
            new SqlParameter("@Image",  User.Image),
            new SqlParameter("@Password",  User.Password),
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

                if (param[16].Value != DBNull.Value)// status
                    retStatus = Convert.ToString(param[16].Value );
            }
            catch (Exception ex)
            {
                retStatus = "Exception";
            }
            return Task.FromResult<string>(retStatus);
        }


    }



}
