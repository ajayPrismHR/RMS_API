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
        public async Task<string> SignUP(EmpSignUP User)
        {
            string retStatus = "";
            string str = Utility.EncryptText(User.Password);
            SqlParameter parmretStatus = new SqlParameter();
            parmretStatus.ParameterName = "@RetStatus";
            parmretStatus.DbType = DbType.Int32;
            parmretStatus.Size = 8;
            parmretStatus.Direction = ParameterDirection.Output;
            SqlParameter[] param ={
                new SqlParameter("@UserName",  User.UserName),
                new SqlParameter("@PASSWORD",  str),
                new SqlParameter("@FirstName",  User.FirstName),
                new SqlParameter("@LastName",  User.LastName),
                new SqlParameter("@Address1",  User.Address1),
                new SqlParameter("@Address2",  User.Address2),
                new SqlParameter("@Address3",  User.Address3),
                new SqlParameter("@LandMark",  User.LandMark),
                new SqlParameter("@City",  User.City),
                new SqlParameter("@State",  User.State),
                new SqlParameter("@Pincode",  User.Pincode),
                new SqlParameter("@Mobile_No",  User.Mobile_No),
                new SqlParameter("@Email",  User.Email),
                new SqlParameter("@Photo",  User.Photo),
                parmretStatus,
                
            };
            try
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "SIGNUP", param);

                if (param[13].Value != DBNull.Value)// status
                    retStatus = Convert.ToString(param[15].Value);
                else
                    retStatus = "";
            }
            catch (Exception ex)
            {
                retStatus = "Exception";
            }
            return retStatus;
        }


    }



}
