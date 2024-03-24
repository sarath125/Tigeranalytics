namespace RetailPrice.Models
{
    public class PricingDtoView
    {
        public int PricingID { get; set; }
        public string StoreName { get; set; }
        public int Sku { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
        public string Country { get; set; }

        public int StoreId { get; set; }
       

        
    }
}
