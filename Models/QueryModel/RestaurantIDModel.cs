namespace RMS_API.Models.QueryModel
{
    public class RestaurantIDModel
    {
        public string RestaurantRegistrationID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 OfferID { get; set; }
    }

    public class SearchRestaurantIDModel
    {
        public string RestaurantRegistrationID { get; set; }
        public string Name { get; set; }
    }
}
