using SURAKSHA.Database.Repository;
using SURAKSHA.Models;
using SURAKSHA.Models.QueryModel;
using SURAKSHA.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
namespace SURAKSHA.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
      
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        IConfiguration _configuration;


        public LoginController(ILogger<LoginController> logger, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("DoLogin")]
        public IActionResult DoLogin(UserRequestQueryModel modelUser)
        {
            ILogger<LoginRepository> modelLogger = _loggerFactory.CreateLogger<LoginRepository>();
            LoginRepository modelLoginRepository = new LoginRepository(modelLogger);
            UserViewModel userViewModels = new UserViewModel();
           
            userViewModels = modelLoginRepository.ValidateUser(modelUser);
            if (!string.IsNullOrEmpty(userViewModels.Emp_Name))
            {
                double expiryMins= string.IsNullOrEmpty(_configuration["Jwt:TokenValidityInMinutes"]) ? 5 : Convert.ToDouble(_configuration["Jwt:TokenValidityInMinutes"]);
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("ID", userViewModels.ID.ToString()),
                        new Claim("USER_NAME", userViewModels.Emp_Name),
                        new Claim("NAME", userViewModels.NAME),
                    };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,expires: DateTime.UtcNow.AddMinutes(expiryMins),signingCredentials: signIn);

                userViewModels.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("Login success");
                return Ok(userViewModels);
            }
            else
            {
                _logger.LogInformation("Invalid credentials");
                
                return NotFound(-1);
            }
        }

      
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserRequestQueryModel User)
        {
            int retStatus = 0;
            LoginController loginController = this;
            ReturnStatusModel returnStatus = new ReturnStatusModel();
            LoginRepository loginRepository = new LoginRepository(loginController._loggerFactory.CreateLogger<LoginRepository>());
            UserViewAPIModel userViewApiModel = new UserViewAPIModel();
            retStatus = await loginRepository.ChangePassword(User);
            if (retStatus == 1)
            {
                returnStatus.response = 1;
                returnStatus.status = "Password Changed Successfully";
                return Ok(returnStatus);
            }
            else
            {
                returnStatus.response = 0;
                returnStatus.status = "Password Not Changed";
                return Ok(returnStatus);
            }
            
        }

        [HttpPost]
        [Route("SignUP")]
        public async Task<IActionResult> SignUP(EmpSignUP User)
        {
            LoginController loginController = this;
            ReturnStatusModel returnStatus = new ReturnStatusModel();
            LoginRepository loginRepository = new LoginRepository(loginController._loggerFactory.CreateLogger<LoginRepository>());
            UserViewAPIModel userViewApiModel = new UserViewAPIModel();
            string str = await loginRepository.SignUP(User);
            returnStatus.response = 1;
            returnStatus.status = str;

            return Ok(returnStatus);
        }

    }
}
