﻿namespace RMS_API.Models.ViewModel
{
    public class ProductViewAPIModel
    {
        public string PName { get; set; } = string.Empty;
        public bool Is_Veg { get; set; } = false;
        public bool Is_Alcoholic { get; set; } = false;
        public string Image { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
