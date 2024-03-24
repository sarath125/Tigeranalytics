using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailPrice.Business.ProductCategoryService;
using RetailPrice.DTO;

namespace RetailPrice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        // GET: api/productcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetAllProductCategories()
        {
            var categories = await _productCategoryService.GetAllProductCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/productcategories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryDto>> GetProductCategoryById(int id)
        {
            var category = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // POST: api/productcategories
        [HttpPost]
        public async Task<ActionResult<ProductCategoryDto>> AddProductCategory(ProductCategoryDto productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productCategoryService.AddProductCategoryAsync(productCategory);
            return CreatedAtAction(nameof(GetProductCategoryById), new { id = productCategory.CategoryId }, productCategory);
        }

        // PUT: api/productcategories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory(int id, ProductCategoryDto productCategory)
        {
            if (id != productCategory.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                await _productCategoryService.UpdateProductCategoryAsync(productCategory);
            }
            catch (Exception)
            {
                if (await _productCategoryService.GetProductCategoryByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/productcategories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            var category = await _productCategoryService.GetProductCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _productCategoryService.DeleteProductCategoryAsync(id);
            return NoContent();
        }
    }
}
