namespace RMS_API.Models.QueryModel
{
    public class RatingsModel
    {
        public Int64 UserID { get; set; } 
        public string RestroID { get; set; }
        public Int64 OrderID { get; set; }
        public int Ratings { get; set;}
        public string Review { get; set; }
    }
    public class GetRatingsModel
    {
        public string RegistrationID { get; set; }
        public decimal AverageRating { get; set; }
    }
    public class GetOrderWiseRatingsModel
    {
        public string OrderID { get; set; }
        public decimal Ratings { get; set; }
    }

    public class OrderWiseRatingsModel
    {
        public string OrderID { get; set; }
        public decimal UserID { get; set; }
    }
}
