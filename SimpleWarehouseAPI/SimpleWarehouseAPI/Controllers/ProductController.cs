﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleWarehouseAPI.Models;
using SimpleWarehouseAPI.Services;

namespace SimpleWarehouseAPI.Controllers
{
    [Route("product/")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<Product>> Get()
        {
           return await _service.GetAllProducts();
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(Guid id)
        {
            return await _service.GetProduct(id);
        }

        [HttpPost]
        public async Task<Guid?> Post([FromBody] ProductCreateInputModel model)
        {
            return await _service.AddProduct(model);
        }

        [HttpPut]
        public async Task Put([FromBody] ProductUpdateInputModel model)
        {
            await _service.UpdateProduct(model);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _service.DeleteProduct(id);
        }
    }
}
