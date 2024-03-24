using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailPrice.Business.StoreService;
using RetailPrice.DTO;

namespace RetailPrice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreDto>>> GetAllStores()
        {
            var stores = await _storeService.GetAllStoresAsync();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoreDto>> GetStoreById(int id)
        {
            var store = await _storeService.GetStoreByIdAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        [HttpPost]
        public async Task<ActionResult> AddStore(StoreDto store)
        {
            await _storeService.AddStoreAsync(store);
            return CreatedAtAction(nameof(GetStoreById), new { id = store.StoreId }, store);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStore(int id, StoreDto store)
        {
            if (id != store.StoreId)
            {
                return BadRequest();
            }
            await _storeService.UpdateStoreAsync(store);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStore(int id)
        {
            await _storeService.DeleteStoreAsync(id);
            return NoContent();
        }
    }
}
