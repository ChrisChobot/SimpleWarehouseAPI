using SimpleWarehouseAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWarehouseAPI.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProduct(Guid id);
        Task<Guid?> AddProduct(ProductCreateInputModel model);
        Task UpdateProduct(ProductUpdateInputModel model);
        Task DeleteProduct(Guid id);
    }
}
