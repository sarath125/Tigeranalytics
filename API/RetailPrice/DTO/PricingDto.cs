namespace RetailPrice.DTO
{
    public class PricingDto
    {
        public int PricingId { get; set; }

        public int StoreId { get; set; }

        public int Sku { get; set; }

        public decimal Price { get; set; }

        public DateTime? Date { get; set; }
    }
}
