namespace RMS_API.Models.QueryModel
{
    public class UserIDModel
    {
        public Int64 UserID { get; set; }
        public string Restaurant_ID { get; set; }
    }

    public class OrderIDModel
    {
        public Int64 OrderID { get; set; }
    }

    public class OrderIDForRestaurantModel
    {
        public Int64 OrderID { get; set; }
        public string RestaurantID { get; set; }
        public Int64 UserID { get; set; }
    }
}
