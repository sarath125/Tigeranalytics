namespace RetailPrice.DTO
{
    public class StoreDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Country { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
