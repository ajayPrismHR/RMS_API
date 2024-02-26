namespace RMS_API.Models.QueryModel
{
    public class RatingsModel
    {
        public Int64 UserID { get; set; } 
        public string RestroID { get; set; }
        public int Ratings { get; set;}
        public string Review { get; set; }
    }
    public class GetRatingsModel
    {
        public string RegistrationID { get; set; }
        public decimal AverageRating { get; set; }
    }
}
