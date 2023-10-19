namespace RMS_API.Models.QueryModel
{
    public class OrderMasterModel
    {
        public string Restro_ID {  get; set; } 
        public string Costumer_ID { get; set; }
        public DateTime Order_Date { get; set; }
        public string QR_Details { get; set; }
        public bool Is_Finalized { get; set; }
    }
}
