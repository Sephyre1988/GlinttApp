using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{
    public class EditProductModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public long Version { get; set; }
    }
}
