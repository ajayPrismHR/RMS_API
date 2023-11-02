namespace RMS_API.Models.ViewModel
{
    public class RestaurantViewAPIModel
    {
         public string RegistrationID { get; set; } = string.Empty;
         public string RestaurentName { get; set; } = string.Empty;
         public string MobileNo { get; set; } = string.Empty;      
         public string MailID { get; set; } = string.Empty;
         public string Address { get; set; } = string.Empty;
         public string Area_Code { get; set; } = string.Empty;
         public string Image { get; set; } = string.Empty;
         public double Lat { get; set; } 
         public double Long { get; set; }
        public double Distance { get; set; } 
        public string Favourite { get; set; }

    }
}
