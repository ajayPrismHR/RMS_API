namespace RMS_API.Models.ViewModel
{
    public class ProductViewAPIModel
    {
        public Int64 PID { get; set; }
        public string PName { get; set; } = string.Empty;
        public bool Is_Veg { get; set; } = false;
        public bool Is_Alcoholic { get; set; } = false;
        public string Image { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public Int64 OrderedQuntity { get; set; } 
        public decimal PRate { get; set; } 
    }

    public class OrderListViewAPIModel
    {
        public Int64 Order_ID { get; set; }
        public string RestaurentName { get; set; } = string.Empty;
        public DateTime Order_Date { get; set; }
        public string OrderStatus { get; set; }

        public string Image { get; set; }
    }

    public class OrderDetailViewAPIModel
    {
        public Int64 OrderID { get; set; }
        public string PName { get; set; } = string.Empty;
        public DateTime Order_Date { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }

    public class FrequentlyProductViewAPIModel
    {
        public string PNAME { get; set; }
        public Int64 ProductID { get; set; }
        public string UserName { get; set; }
        public Int64 OrderedCount { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool Is_Alcoholic { get; set; } 
        public bool Is_Veg { get; set; }
        public decimal PRate { get; set; }
        public string RestaurentName { get; set; }
        public string RegistrationID { get; set; }
    }
}
