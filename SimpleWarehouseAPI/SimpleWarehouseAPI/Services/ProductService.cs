using Microsoft.EntityFrameworkCore;
using SimpleWarehouseAPI.Models;
using SimpleWarehouseAPI.Validator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWarehouseAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly MainDbContext _context;

        public ProductService(MainDbContext context)
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
            if (ProductValidator.IsIdOk(id))
            {
                return await _context.Products.FirstOrDefaultAsync(x_ => x_.Id == id);
            }
            else
            {
                return null;
            }
        }

        public async Task<Guid?> AddProduct(ProductCreateInputModel model)
        {
            if (ProductValidator.ValidateData(model, out _))
            {
                Product product = new Product(model);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product.Id;
            }
            else
            {
                return null;
            }
        }
        
        public async Task UpdateProduct(ProductUpdateInputModel model)
        {
            if (ProductValidator.ValidateData(model, _context, out _))
            {
                Product foundedProduct = await _context.Products.FirstOrDefaultAsync(x_ => x_.Id == model.Id);
                foundedProduct.Name = model.Name;
                foundedProduct.Price = model.Price;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(Guid id)
        {
            if (ProductValidator.IsIdOk(id))
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
}
