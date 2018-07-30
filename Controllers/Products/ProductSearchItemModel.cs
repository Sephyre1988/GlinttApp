using System;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{
    public class ProductSearchItemModel {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CurrentStock { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public string UpdatedBy { get; set;} 
        public bool IsActive{get;set;}

    }


}