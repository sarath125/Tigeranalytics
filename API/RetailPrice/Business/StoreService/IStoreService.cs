using RetailPrice.DTO;

namespace RetailPrice.Business.StoreService
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDto>> GetAllStoresAsync();
        Task<StoreDto> GetStoreByIdAsync(int id);
        Task AddStoreAsync(StoreDto store);
        Task UpdateStoreAsync(StoreDto store);
        Task DeleteStoreAsync(int id);
    }
}
