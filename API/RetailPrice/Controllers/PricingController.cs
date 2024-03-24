using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailPrice.Business.PricingService;
using RetailPrice.DTO;
using RetailPrice.Models;
using System.Text.Json;

namespace RetailPrice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        // GET: api/pricings
        [HttpGet]
        public async Task<ActionResult<PagedResult<PricingDtoView>>> GetAllPricings([FromQuery] GridParameters gridParams)
        {
            
            var pricings = await _pricingService.GetAllPricingsAsync(gridParams);
            return Ok(pricings);
        }

        // GET: api/pricings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PricingDto>> GetPricingById(int id)
        {
            var pricing = await _pricingService.GetPricingByIdAsync(id);
            if (pricing == null)
            {
                return NotFound();
            }
            return Ok(pricing);
        }

        // POST: api/pricings
        [HttpPost]
        public async Task<ActionResult<PricingDto>> AddPricing(PricingDto pricing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _pricingService.AddPricingAsync(pricing);
            return CreatedAtAction(nameof(GetPricingById), new { id = pricing.PricingId }, pricing);
        }

        // PUT: api/pricings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePricing(int id, PricingDto pricing)
        {
            if (id != pricing.PricingId)
            {
                return BadRequest();
            }

            try
            {
                await _pricingService.UpdatePricingAsync(pricing);
            }
            catch (Exception)
            {
                if (await _pricingService.GetPricingByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/pricings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePricing(int id)
        {
            var pricing = await _pricingService.GetPricingByIdAsync(id);
            if (pricing == null)
            {
                return NotFound();
            }

            await _pricingService.DeletePricingAsync(id);
            return NoContent();
        }


        [HttpPost]
        [Route("UploadSKUPriceFile")]
        public IActionResult UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not selected.");
            }

            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var fileNameWithTimestamp = Path.GetFileNameWithoutExtension(file.FileName) + "_" + timestamp + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileNameWithTimestamp);




            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Queue the file processing
            BackgroundJob.Enqueue(() => _pricingService.ProcessSKUPriceFile(filePath));

            return Ok();

        }


    }
}
