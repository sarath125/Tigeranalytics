using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RetailPrice.DTO;
using RetailPrice.Models;

namespace RetailPrice.Business.ProductCategoryService
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly RetailPriceContext _context;
        private readonly IMapper _mapper;

        public ProductCategoryService(RetailPriceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllProductCategoriesAsync()
        {
            var categories = await _context.ProductCategories.ToListAsync();
            return _mapper.Map<IEnumerable<ProductCategoryDto>>(categories);
        }

        public async Task<ProductCategoryDto> GetProductCategoryByIdAsync(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            return _mapper.Map<ProductCategoryDto>(category);
        }

        public async Task AddProductCategoryAsync(ProductCategoryDto productCategoryDto)
        {
            var category = _mapper.Map<ProductCategory>(productCategoryDto);
            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductCategoryAsync(ProductCategoryDto productCategoryDto)
        {
            var category = _mapper.Map<ProductCategory>(productCategoryDto);
            _context.ProductCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductCategoryAsync(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category != null)
            {
                _context.ProductCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
