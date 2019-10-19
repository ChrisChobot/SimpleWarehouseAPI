using System.ComponentModel.DataAnnotations;

namespace SimpleWarehouseAPI.Models
{
    public class ProductCreateInputModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
