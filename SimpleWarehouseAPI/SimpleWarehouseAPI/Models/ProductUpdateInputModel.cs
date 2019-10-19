using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SimpleWarehouseAPI.Models
{
    public class ProductUpdateInputModel
    {
        [Required]
        [NotEmpty]
        public Guid? Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
