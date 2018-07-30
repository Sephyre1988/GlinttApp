namespace AcademiadecodigoWarehouseApi.Controllers.Products
{
    public class ProductModel{
        public long Id{get;set;}
         public string Code{get;set;}
        public string Name{get;set;}
        public string Description{get;set;}
        public decimal Price{get;set;}
        public int CurrentStock{get;set;}
        public System.DateTimeOffset CreatedOn {get;set;}
        public string CreatedBy{get;set;}
         public System.DateTimeOffset UpdatedOn {get;set;}
        public string UpdatedBy{get;set;}
         public System.DateTimeOffset? DeletedOn {get;set;}
        public string deletedBy{get;set;}
        public long Version{get;set;}
    }
}