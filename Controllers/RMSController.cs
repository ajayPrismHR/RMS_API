using SURAKSHA.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using SURAKSHA.Models.QueryModel;
using SURAKSHA_API.Database.Repository;
using System.Data;

namespace SURAKSHA.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RMSController : ControllerBase
    {


        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;


      
    }

}
