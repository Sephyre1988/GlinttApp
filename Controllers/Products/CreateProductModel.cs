using System.ComponentModel.DataAnnotations;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{
    public class CreateProductModel{
        
        
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Code{get;set;}
        [Required]
        public string Name{get;set;}
        public string Description{get;set;}
        [Required]
        public decimal Price{get;set;}


    }

}