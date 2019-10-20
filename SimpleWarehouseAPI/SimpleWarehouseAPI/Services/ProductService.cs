using Microsoft.EntityFrameworkCore;
using SimpleWarehouseAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWarehouseAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ClientDbContext _context;

        public ProductService(ClientDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            if (await _context.Products.CountAsync() > 0)
            {
                return await _context.Products.ToListAsync();
            }
            else
            {
                return null;
            }      
        }
        
        public async Task<Product> GetProduct(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(x_ => x_.Id == id);
        }

        public async Task<Guid?> AddProduct(ProductCreateInputModel model)
        {
            Product product = new Product(model);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }
        
        public async Task UpdateProduct(ProductUpdateInputModel model)
        {
            Product foundedProduct = await _context.Products.FirstOrDefaultAsync(x_ => x_.Id == model.Id);

            if (foundedProduct != null)
            {
                foundedProduct.Name = model.Name;
                foundedProduct.Price = model.Price;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(Guid id)
        {
            Product foundedProduct = await _context.Products.FirstOrDefaultAsync(x_ => x_.Id == id);

            if (foundedProduct != null)
            {
                _context.Products.Remove(foundedProduct);
                await _context.SaveChangesAsync();
            }
        }
    }
}
