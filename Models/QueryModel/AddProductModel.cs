using System.Diagnostics.CodeAnalysis;

namespace RMS_API.Models.QueryModel
{
    public class MasterAddProductModel
    {
        [NotNull]
        public string Productinfo { get; set; } = string.Empty;

        //[NotNull]
        //public ModelFile file { get; set; }

        [NotNull]
        public IFormFile ImageFile { get; set; }

    }
    public class AddProductModel
    {
        public Int64 PID { get; set; }
        public string ProductName { get; set; }
        public Boolean isVeg { get; set; }
        public Boolean IsAlcoholic { get; set; }
        public string Image { get; set; }
        public string RestaurantRegID { get; set; }
        public decimal PRate { get; set; }
        public Int64 offerID { get; set; }
    }
}
