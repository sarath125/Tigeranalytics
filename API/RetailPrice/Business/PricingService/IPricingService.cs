using RetailPrice.DTO;
using RetailPrice.Models;

namespace RetailPrice.Business.PricingService
{
    public interface IPricingService
    {
        Task<PagedResult<PricingDtoView>> GetAllPricingsAsync(GridParameters gridParams);
        Task<PricingDto> GetPricingByIdAsync(int id);
        Task AddPricingAsync(PricingDto pricing);
        Task UpdatePricingAsync(PricingDto pricing);
        Task DeletePricingAsync(int id);

        void ProcessSKUPriceFile(string filePath);
    }
}
