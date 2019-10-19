using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleWarehouseAPI.Models
{
    public class Product
    {
        [Required]
        public Guid? Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }

        public Product()
        {
            Name = string.Empty;
            Price = Decimal.Zero;
        }

        public Product(ProductCreateInputModel createModel)
        {
            Name = createModel.Name;
            Price = createModel.Price;
        }

        public Product(ProductUpdateInputModel updateModel)
        {
            Id = updateModel.Id;
            Name = updateModel.Name;
            Price = updateModel.Price;
        }
    }
}
