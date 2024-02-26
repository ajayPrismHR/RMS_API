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

using RMS_API.Models.QueryModel;
using System.Text.Json.Nodes;
using System.Text.Json;

using RMS_API.Models.ViewModel;
using System.Security.AccessControl;
using SURAKSHA_API.Database.Repository;

namespace SURAKSHA.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
      
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IFileService _fileService;
        private string fileNametoSave { get; set; } = string.Empty;
        IConfiguration _configuration;

        #region Constructor
        public LoginController(ILogger<LoginController> logger, ILoggerFactory loggerFactory, IConfiguration configuration,IFileService fileService)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _configuration = configuration;
            _fileService = fileService;
        }
        #endregion

        #region Login 
        [HttpPost]
        [Route("DoLogin")]
        public IActionResult DoLogin(UserRequestQueryModel modelUser)
        {
            _logger.LogInformation("Start : DoLogin");
            ILogger<LoginRepository> modelLogger = _loggerFactory.CreateLogger<LoginRepository>();
            LoginRepository modelLoginRepository = new LoginRepository(modelLogger);
            UserViewModel userViewModels = new UserViewModel();
           
            userViewModels = modelLoginRepository.ValidateUser(modelUser);
            if (!string.IsNullOrEmpty(userViewModels.RegistrationID))
            {
                double expiryMins= string.IsNullOrEmpty(_configuration["Jwt:TokenValidityInMinutes"]) ? 5 : Convert.ToDouble(_configuration["Jwt:TokenValidityInMinutes"]);
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("ID", userViewModels.RegistrationID.ToString()),
                        new Claim("FIRSTNAME", userViewModels.FirstName)
                    };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,expires: DateTime.UtcNow.AddMinutes(expiryMins),signingCredentials: signIn);

                userViewModels.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("Login success");
                _logger.LogInformation("Exit : DoLogin");
                return Ok(userViewModels);
            }
            else
            {
                _logger.LogInformation("Invalid credentials");
                _logger.LogInformation("Exit : DoLogin");

                return NotFound(-1);
            }
           
        }
        #endregion

        #region Login 
        [HttpPost]
        [Route("RestaurantLogin")]
        public IActionResult RestaurantLogin(UserRequestQueryModel modelUser)
        {
            ILogger<LoginRepository> modelLogger = _loggerFactory.CreateLogger<LoginRepository>();
            LoginRepository modelLoginRepository = new LoginRepository(modelLogger);
            RestaurantViewModel restaurantViewModels = new RestaurantViewModel();

            restaurantViewModels = modelLoginRepository.ValidateRestaurant(modelUser);
            if (!string.IsNullOrEmpty(restaurantViewModels.RegistrationID))
            {
                double expiryMins = string.IsNullOrEmpty(_configuration["Jwt:TokenValidityInMinutes"]) ? 5 : Convert.ToDouble(_configuration["Jwt:TokenValidityInMinutes"]);
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("ID", restaurantViewModels.RegistrationID.ToString()),
                        new Claim("RestaurentName", restaurantViewModels.RestaurentName)
                    };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims, expires: DateTime.UtcNow.AddMinutes(expiryMins), signingCredentials: signIn);

                restaurantViewModels.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("Login success");
                return Ok(restaurantViewModels);
            }
            else
            {
                _logger.LogInformation("Invalid credentials");

                return NotFound(-1);
            }
        }
        #endregion

        #region GetLoginImages 
        [HttpPost]
        [Route("GetLoginImages")]

        public async Task<IActionResult> GetLoginImages()
        {
            _logger.LogInformation("Start : GetLoginImages");
            LoginController loginController = this;
            ILogger<LoginRepository> modelLogger = _loggerFactory.CreateLogger<LoginRepository>();
            LoginRepository loginRepository = new LoginRepository(loginController._loggerFactory.CreateLogger<LoginRepository>(), _configuration);
            List<LoginListModel> LoginImageViewAPIModels = await loginRepository.GetLoginImagesList();

            _logger.LogInformation("Exit : GetLoginImages");
            return Ok(LoginImageViewAPIModels);

        }
        #endregion

        #region UserRegistration 
        [HttpPost]
        [Route("UserRegistration")]

        public async Task<IActionResult> UserRegistration([FromForm] MasterUserRegistration user)
        {
            _logger.LogInformation("Start : UserRegistration");
            LoginController loginController = this;
            var json = user.Userinfo;

            var userObj = JsonSerializer.Deserialize<UserRegistration>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Random rnd = new Random();
            int rndmunber = rnd.Next(1, 1000);
            userObj.registrationID = "PM" + userObj.MobileNo.Substring(userObj.MobileNo.Length - 5) + rndmunber.ToString();
            ModelFile file = new ModelFile();
            file.ImageFile = user.ImageFile;
            string filename = file.ImageFile.FileName;
            FileInfo fi = new FileInfo(filename);
            string fileNametoSave = userObj.registrationID + "_" + Guid.NewGuid() + fi.Extension;
            userObj.Image = fileNametoSave;

            LoginRepository loginRepository = new LoginRepository(loginController._loggerFactory.CreateLogger<LoginRepository>());
            RMS_API.Models.Response response = await loginRepository.UserRegistration(userObj);
            if (response.Status > 0)
            {
                await _fileService.Upload(
                                            file , fileNametoSave
                                         );
            }
            _logger.LogInformation("Exit : UserRegistration");
            return Ok(response);

        }
        #endregion

        #region RestaurantRegistration 
        [HttpPost]
        [Route("RestaurantRegistration")]

        public async Task<IActionResult> RestaurantRegistration([FromForm] MasterRestaurantRegitrationModel restaurant)
        {
            _logger.LogInformation("Start : RestaurantRegistration");
            LoginController loginController = this;
            var json = restaurant.Restaurantinfo;

            var userObj = JsonSerializer.Deserialize<RestaurantRegitrationModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Random rnd = new Random();
            int rndmunber = rnd.Next(1, 1000);
            userObj.registrationID = "PM" + userObj.MobileNo.Substring(userObj.MobileNo.Length - 5) + rndmunber.ToString();
            ModelFile file = new ModelFile();
            file.ImageFile = restaurant.ImageFile;
            string filename = file.ImageFile.FileName;
            FileInfo fi = new FileInfo(filename);
            string fileNametoSave = userObj.registrationID + "_" + Guid.NewGuid() + fi.Extension;
            userObj.Image = fileNametoSave;

            LoginRepository loginRepository = new LoginRepository(loginController._loggerFactory.CreateLogger<LoginRepository>());
            RMS_API.Models.Response response = await loginRepository.RestaurantRegistrationAPI(userObj);
            if (response.Status > 0)
            {
                await _fileService.Upload(
                                            file, fileNametoSave
                                         );
            }
            _logger.LogInformation("Exit : RestaurantRegistration");
            return Ok(response);

        }
        #endregion

        #region AddProduct 
        [HttpPost]
        [Route("AddProduct")]

        public async Task<IActionResult> AddProduct([FromForm] MasterAddProductModel product)
        {
            _logger.LogInformation("Start : AddProduct");
            LoginController loginController = this;
            var json = product.Productinfo;

            var userObj = JsonSerializer.Deserialize<AddProductModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            ModelFile file = new ModelFile();
            if (userObj.PID == 0)
            {
                
                file.ImageFile = product.ImageFile;
                string filename = file.ImageFile.FileName;
                FileInfo fi = new FileInfo(filename);
                string fileNametoSave = userObj.ProductName + "_" + Guid.NewGuid() + fi.Extension;
                userObj.Image = fileNametoSave;
            }
            else
            {
                userObj.Image = "";
            }
            LoginRepository loginRepository = new LoginRepository(loginController._loggerFactory.CreateLogger<LoginRepository>());
            RMS_API.Models.Response response = await loginRepository.AddProductAPI(userObj);
            if (response.Status > 0 && userObj.PID == 0)
            {
                await _fileService.Upload(
                                            file, fileNametoSave
                                         );
            }
            _logger.LogInformation("Exit : AddProduct");
            return Ok(response);

        }
        #endregion

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
        [Route("UploadImage")]

        public async Task<IActionResult> uploadImage([FromForm] ModelFile file)
        {
            Random random = new Random();
            await _fileService.Upload(file,random.Next().ToString());
            return Ok("success");
        }
        #region Check Mobile No 
        [HttpPost]
        [Route("CheckMobileNo")]

        public async Task<IActionResult> CheckMobileNo(MobileNoCheck mobileno)
        {
            _logger.LogInformation("Start : Check Mobile No.");
            LoginController loginController = this;
            LoginRepository loginRepository = new LoginRepository(loginController._loggerFactory.CreateLogger<LoginRepository>());
            ReturnStatusModel returnStatus = new ReturnStatusModel();
            int retStatus = 0;
            retStatus = await loginRepository.CheckMobileNoAPI(mobileno);
            _logger.LogInformation("Exit : Check Mobile No.");
            if (retStatus == 1)
            {
                returnStatus.response = 1;
                returnStatus.status = "Mobile No. Exists";
                return Ok(returnStatus);
            }

            else
            {
                returnStatus.response = 0;
                returnStatus.status = "Mobile No. Does Not Exists";
                return Ok(returnStatus);
            }

        }
        #endregion

    }
}

