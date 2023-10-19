namespace RMS_API.Models.QueryModel
{
    public class OrderDetailsModel
    {
        public string ProductID { get; set; }
        public string OfferID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        
    }
}
