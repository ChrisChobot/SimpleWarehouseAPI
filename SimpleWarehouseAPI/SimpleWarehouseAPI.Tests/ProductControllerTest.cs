using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using SimpleWarehouseAPI;
using SimpleWarehouseAPI.Controllers;
using SimpleWarehouseAPI.Models;
using SimpleWarehouseAPI.Services;
using SimpleWarehouseAPI.Tests;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Tests
{
    public class ProductControllerTest
    {
       // ProductController controller;

        private static Mock<Microsoft.EntityFrameworkCore.DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<Microsoft.EntityFrameworkCore.DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }

        [SetUp]
        public void Setup()
        {
            //Product product1 = new Product()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "test Product 1",
            //    Price = 3
            //};

            //Product product2 = new Product()
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "test Product 2",
            //    Price = 5
            //};

            //var products = new List<Product>
            //{
            //product1,
            //product2
            //}.AsQueryable();
            //var mockSet = new Mock<DbSet<Product>>();



            //mockSet.As<IAsyncEnumerable<Product>>()
            //   .Setup(m => m.GetEnumerator())
            //   .Returns(new TestAsyncEnumerator<Product>(products.GetEnumerator()));

            ////mockSet.As<IQueryable<Product>>()
            ////    .Setup(m => m.Provider)
            ////    .Returns(new TestAsyncQueryProvider<Product>(products.Provider));

            //mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            //mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            //mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            //mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => products.GetEnumerator());

            //var contextOptions = new DbContextOptions<ClientDbContext>();
            //var mockContext = new Mock<ClientDbContext>(contextOptions);
            //mockContext.Setup(c => c.Set<Product>()).Returns(mockSet.Object);



            //mockSet.As<IDbAsyncEnumerable<Product>>()
            //    .Setup(m => m.GetAsyncEnumerator())
            //    .Returns(new TestDbAsyncEnumerator<Product>(data.GetEnumerator()));

            //var productsMock = CreateDbSetMock(products);

            //var productContextMock = new Mock<ClientDbContext>();
            //productContextMock.Object.Products = productsMock.Result.Object;
            //  productContextMock.Setup(x => x.Products).Returns(productsMock.Object);

         //   var productService = new ProductService(mockContext.Object);
        //    controller = new ProductController(productService);
            //   var controller = new ProductController();
        }


        [Test]
        public void GetProductList()
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

            var data = new List<Product>
            {
            product1,
            product2
            }.AsQueryable();

            var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<Product>>();
            mockSet.As<IAsyncEnumerable<Product>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<Product>(data.GetEnumerator()));

            mockSet.As<IQueryable<Product>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Product>(data.Provider));

            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ClientDbContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var service = new ProductService(mockContext.Object);
         //   var Products = await service.GetAllProductsAsync();


            // IList<Product> products = new List<Product>
            // {
            // product1,
            // product2
            // };

            // var productsMock = CreateDbSetMock(products);

            // var productContextMock = new Mock<ClientDbContext>();
            //// productContextMock.Object.Products = productsMock.Result.Object;
            //   productContextMock.Setup(x => x.Products).Returns(productsMock.Object);

            // var productService = new ProductService(productContextMock.Object);
            // var controller = new ProductController(productService);
             var controller = new ProductController(service);

            List<Product> product = controller.Get().Result;

            Assert.IsTrue(product != null);
            Assert.AreEqual(product.Count, 2);
            Assert.IsTrue(product != null);
        }

        [Test]
        public async Task GetProduct()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "test Product 1",
                Price = 3
            };

            var mockRepository = new Mock<IProductService>();
            mockRepository.Setup(x => x.GetProduct((Guid)product.Id))
                        .ReturnsAsync(product);
            var controller = new ProductController(mockRepository.Object);

            Product response = await controller.Get((Guid)product.Id);

            Assert.IsTrue(response != null);
            Assert.AreEqual(response.Id, product.Id);
            Assert.AreEqual(response.Name, product.Name);
            Assert.AreEqual(response.Price, product.Price);
        }

        [Test]
        public async Task GetProductWithBadIdReturnsNull()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "test Product 1",
                Price = 3
            };

            var mockRepository = new Mock<IProductService>();
            mockRepository.Setup(x => x.GetProduct((Guid)product.Id))
                        .ReturnsAsync(product);
            var controller = new ProductController(mockRepository.Object);

            Product response = await controller.Get(Guid.NewGuid());

            Assert.IsTrue(response == null);
        }
    }
}