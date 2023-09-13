using System.Diagnostics.CodeAnalysis;

namespace RMS_API.Models.QueryModel
{
    public class MasterRestaurantRegitrationModel
    {
        [NotNull]
        public string Restaurantinfo { get; set; } = string.Empty;

        //[NotNull]
        //public ModelFile file { get; set; }

        [NotNull]
        public IFormFile ImageFile { get; set; }

    }
    public class RestaurantRegitrationModel
    {
        public string registrationID { get; set; }
        public string Password { get; set; }

        public string RestaurentName { get; set; }
        public string MobileNo { get; set; }
        public string MailID { get; set; }
        public string Address { get; set; }
        public string Area_Code { get; set; }
        public string Image { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Timings { get; set; }

    }
}
