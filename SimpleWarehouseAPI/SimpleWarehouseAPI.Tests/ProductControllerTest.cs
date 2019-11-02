using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleWarehouseAPI;
using SimpleWarehouseAPI.Controllers;
using SimpleWarehouseAPI.Models;
using SimpleWarehouseAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class ProductControllerTest
    {
        #region Init methods
        private ProductController GetController(out Product[] products)
        {
            products = GetSampleProducts();
            var context = GetContext(products);
            var service = new ProductService(context);
            var controller = new ProductController(service);

            return controller;
        }

        private Product[] GetSampleProducts()
        {
            Product product1 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "test Product 1",
                Price = 3
            };

            Product product2 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "test Product 2",
                Price = 5
            };

            return new Product[]
                {
                product1,
                product2
                };
        }

        private MainDbContext GetContext(Product[] products)
        {
            var options = new DbContextOptionsBuilder<MainDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDatabase")
              .Options;
            var context = new MainDbContext(options);

            foreach (var id in context.Products.Select(e => e.Id))
            {
                var entity = new Product { Id = id };
                context.Products.Attach(entity);
                context.Products.Remove(entity);
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return context;
        }

        private bool AreProductsSame(Product response, Product source)
        {
            Assert.IsNotNull(response);
            Assert.IsNotNull(source);
           
            return
                response.Id.Equals(source.Id) &&
                response.Name.Equals(source.Name) &&
                response.Price.Equals(source.Price);
                
        }
        #endregion

        #region Tests
        [Test]
        public async Task GetProductList()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);
            List<Product> responseProducts = await controller.Get();

            Assert.Greater(dbProducts.Length, 0);
            Assert.IsTrue(responseProducts != null);
            Assert.AreEqual(responseProducts.Count, dbProducts.Length);

            for (int i = 0; i < responseProducts.Count; i++)
            {
                Product sourceProduct = dbProducts[i];
                Product foundedProduct = responseProducts.Find((x) => x.Id == sourceProduct.Id);

                Assert.IsNotNull(foundedProduct);
                Assert.IsTrue(AreProductsSame(foundedProduct, sourceProduct));
            }
        }

        [Test]
        public async Task GetProduct()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);

            Assert.Greater(dbProducts.Length, 0);

            Random random = new Random();
            int randomIndex = random.Next(0, dbProducts.Length);
            var sourceProduct = dbProducts[randomIndex];

            Product responseProduct = await controller.Get((Guid)sourceProduct.Id);

            Assert.IsTrue(responseProduct != null);
            Assert.IsTrue(AreProductsSame(responseProduct, sourceProduct));
        }

        [Test]
        public async Task GetProductWithBadIdReturnsNull()
        {
            var controller = GetController(out _);

            Product response = await controller.Get(Guid.NewGuid());

            Assert.IsTrue(response == null);
        }


        [Test]
        public async Task AddProduct()
        {
            ProductCreateInputModel newProductCreateModel = new ProductCreateInputModel()
            {
                Name = "new test Product",
                Price = 9
            };

            var controller = GetController(out _);

            Guid? response = await controller.Post(newProductCreateModel);

            Assert.IsTrue(response != null);
            Assert.IsTrue(response != Guid.Empty);

            Product responseProduct = await controller.Get((Guid)response);
            Product sourceProduct = new Product(newProductCreateModel);
            sourceProduct.Id = response;

            Assert.IsTrue(AreProductsSame(responseProduct, sourceProduct));
        }

        [Test]
        public async Task AddProductWithTooLongName()
        {
            ProductCreateInputModel newProductCreateModel = new ProductCreateInputModel()
            {
                Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Price = 9
            };

            Product[] dbProducts;
            var controller = GetController(out dbProducts);
            Assert.Greater(dbProducts.Length, 0);

            Guid? response = await controller.Post(newProductCreateModel);

            Assert.IsTrue(response == null);

            List<Product> responseProducts = await controller.Get();
            Assert.AreEqual(dbProducts.Length, responseProducts.Count);
        }

        [Test]
        public async Task AddProductWithMinusPrice()
        {
            ProductCreateInputModel newProductCreateModel = new ProductCreateInputModel()
            {
                Name = "minus price",
                Price = -5
            };

            Product[] dbProducts;
            var controller = GetController(out dbProducts);
            Assert.Greater(dbProducts.Length, 0);

            Guid? response = await controller.Post(newProductCreateModel);

            Assert.IsTrue(response == null);

            List<Product> responseProducts = await controller.Get();
            Assert.AreEqual(dbProducts.Length, responseProducts.Count);
        }

        [Test]
        public async Task UpdateProduct()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);
            Assert.Greater(dbProducts.Length, 0);

            Random random = new Random();
            int randomIndex = random.Next(0, dbProducts.Length);
            var sourceProduct = dbProducts[randomIndex];

            ProductUpdateInputModel newProductUpdateModel = new ProductUpdateInputModel()
            {
                Id = sourceProduct.Id,
                Name = "new test Product",
                Price = 9
            };

            await controller.Put(newProductUpdateModel);

            Product updatedProduct = new Product(newProductUpdateModel);
            Product responseProduct = await controller.Get((Guid)updatedProduct.Id);

            Assert.IsTrue(AreProductsSame(responseProduct, updatedProduct));
        }

        [Test]
        public async Task UpdateProductWithBadId()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);
            Assert.Greater(dbProducts.Length, 0);

            Random random = new Random();
            int randomIndex = random.Next(0, dbProducts.Length);
            var sourceProduct = dbProducts[randomIndex];

            ProductUpdateInputModel newProductUpdateModel = new ProductUpdateInputModel()
            {
                Id = Guid.NewGuid(),
                Name = "new id",
                Price = 9
            };

            await controller.Put(newProductUpdateModel);

            Product updatedProduct = new Product(newProductUpdateModel);
            Product responseProduct = await controller.Get((Guid)updatedProduct.Id);

            Assert.IsNull(responseProduct);
        }

        [Test]
        public async Task UpdateProductWithTooLongName()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);
            Assert.Greater(dbProducts.Length, 0);

            Random random = new Random();
            int randomIndex = random.Next(0, dbProducts.Length);
            var sourceProduct = dbProducts[randomIndex];

            ProductUpdateInputModel newProductUpdateModel = new ProductUpdateInputModel()
            {
                Id = sourceProduct.Id,
                Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Price = 9
            };

            await controller.Put(newProductUpdateModel);

            Product updatedProduct = new Product(newProductUpdateModel);
            Product responseProduct = await controller.Get((Guid)updatedProduct.Id);

            Assert.IsFalse(AreProductsSame(responseProduct, updatedProduct));
        }

        [Test]
        public async Task UpdateProductWithMnusPrice()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);
            Assert.Greater(dbProducts.Length, 0);

            Random random = new Random();
            int randomIndex = random.Next(0, dbProducts.Length);
            var sourceProduct = dbProducts[randomIndex];

            ProductUpdateInputModel newProductUpdateModel = new ProductUpdateInputModel()
            {
                Id = sourceProduct.Id,
                Name = "minus price",
                Price = -8
            };

            await controller.Put(newProductUpdateModel);

            Product updatedProduct = new Product(newProductUpdateModel);
            Product responseProduct = await controller.Get((Guid)updatedProduct.Id);

            Assert.IsFalse(AreProductsSame(responseProduct, updatedProduct));
        }

        [Test]
        public async Task DeleteProduct()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);

            Assert.Greater(dbProducts.Length, 0);

            Random random = new Random();
            int randomIndex = random.Next(0, dbProducts.Length);
            var sourceProduct = dbProducts[randomIndex];

            await controller.Delete((Guid)sourceProduct.Id);

            List<Product> responseProducts = await controller.Get();
            Assert.AreEqual(dbProducts.Length - 1, responseProducts.Count);

            Product response = await controller.Get((Guid)sourceProduct.Id);
            Assert.IsTrue(response == null);
        }


        [Test]
        public async Task DeleteProductWithBadId()
        {
            Product[] dbProducts;
            var controller = GetController(out dbProducts);

            Assert.Greater(dbProducts.Length, 0);

            await controller.Delete(Guid.NewGuid());

            List<Product> responseProducts = await controller.Get();
            Assert.AreEqual(dbProducts.Length, responseProducts.Count);
        }
        #endregion
    }
}