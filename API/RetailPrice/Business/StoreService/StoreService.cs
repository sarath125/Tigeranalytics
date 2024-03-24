using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RetailPrice.DTO;
using RetailPrice.Models;

namespace RetailPrice.Business.StoreService
{
    public class StoreService : IStoreService
    {
        private readonly RetailPriceContext _context;
        private readonly IMapper _mapper;

        public StoreService(RetailPriceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StoreDto>> GetAllStoresAsync()
        {
            var stores = await _context.Stores.ToListAsync();
            return _mapper.Map<IEnumerable<StoreDto>>(stores);
        }

        public async Task<StoreDto> GetStoreByIdAsync(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            return _mapper.Map<StoreDto>(store);
        }

        public async Task AddStoreAsync(StoreDto storeDto)
        {
            var store = _mapper.Map<Store>(storeDto); // Map the DTO to the entity
            _context.Stores.Add(store); // Add the entity, not the DTO
            await _context.SaveChangesAsync();
        }


        public async Task UpdateStoreAsync(StoreDto storeDto)
        {
            var store = _mapper.Map<Store>(storeDto);
            _context.Stores.Update(store);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStoreAsync(int id)
        {
            var store = await _context.Stores.Include(s => s.Pricings).SingleOrDefaultAsync(s => s.StoreId == id);
            if (store != null)
            {
                // If needed, remove related pricings
                _context.Pricings.RemoveRange(store.Pricings);

                // Remove the store
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
            }
        }
    }
}
