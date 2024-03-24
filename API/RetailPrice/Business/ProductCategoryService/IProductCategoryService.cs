using RetailPrice.DTO;

namespace RetailPrice.Business.ProductCategoryService
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategoryDto>> GetAllProductCategoriesAsync();
        Task<ProductCategoryDto> GetProductCategoryByIdAsync(int id);
        Task AddProductCategoryAsync(ProductCategoryDto productCategory);
        Task UpdateProductCategoryAsync(ProductCategoryDto productCategory);
        Task DeleteProductCategoryAsync(int id);
    }
}
