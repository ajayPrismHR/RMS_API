
using SURAKSHA.Database.Repository;
using SURAKSHA;
using SURAKSHA.Models;
using SURAKSHA.Models.QueryModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using SURAKSHA_API.Models.QueryModel;

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

        public RMSRepository()
        {
        }
       
    }
}
