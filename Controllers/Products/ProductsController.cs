using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AcademiadecodigoWarehouseApi.Database;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{

    [Route ("api/products")]

    public class ProductsController : ControllerBase, IProductsEndpoint {

        private static readonly List<ProductEntity> MockProducts = new List<ProductEntity>{
            new ProductEntity{
                    Id = 1,
                    Code = "111",
                    Name = "Bola",
                    Description = "fantastico",
                    Price = 10.5m,
                    StockMoviments = {
                        new StockMovimentEntity{
                            Price = 10.5m,
                            Quantity = 2,
                            CreatedOn = DateTimeOffset.Now.AddDays(-2),
                            CreatedBy = "ruben.ribeiro"
                        }
                    
                    },
                    CreatedOn = DateTimeOffset.Now.AddDays(-30),
                    CreatedBy = "ruben.ribeiro",
                    UpdatedOn = DateTimeOffset.Now.AddMinutes(-32),
                    UpdatedBy = "ruben.ribeiro",
                },

                new ProductEntity{
                    Id = 2,
                    Code = "222",
                    Name = "Bola de futebol",
                    Description= "not so Awesome",
                    Price = 50.5m,
                    StockMoviments = {
                        new StockMovimentEntity{
                            Price = 50.5m,
                            Quantity = 2,
                            CreatedOn = DateTimeOffset.Now.AddDays(-20),
                            CreatedBy = "ruben.ribeiro"
                        }
                    
                    },
                    CreatedOn = DateTimeOffset.Now.AddDays(-30),
                    CreatedBy = "ruben.ribeiro",
                    UpdatedOn = DateTimeOffset.Now.AddHours(-50),
                    UpdatedBy = "Marcolino",
                }
        };
        

        [Route ("search"), HttpGet]
        public IReadOnlyCollection<ProductSearchItemModel> Search (
            string code,
            string name,
            decimal? minPrice,  //  ? indica que pode ser null
            decimal? maxPrice,
            bool? isActive,
            int skip = 0,       //  se não vier designado assume estes valores de paginação skip-> itens a saltar antes de apresentar
            int take = 20       //  quantidade de itens a apresentar
        ) {

            var username = User.Identity.Name;
            /* 
            var result = new []{
                
            };
            */

            IEnumerable<ProductSearchItemModel> filterItems = MockProducts
            .Select(e=>new ProductSearchItemModel{
                Code = e.Code,
                Name = e.Name,
                Price = e.Price,
                CurrentStock = e.StockMoviments.Sum(m => m.Quantity),
                IsActive = e.DeletedOn == null,
                UpdatedOn = e.UpdatedOn,
                UpdatedBy = e.UpdatedBy
            });

            if(!string.IsNullOrWhiteSpace(code)){
                filterItems = filterItems
                .Where(e => e.Code.Contains(code.Trim(),StringComparison.InvariantCultureIgnoreCase));
            }
            if(!string.IsNullOrWhiteSpace(name)){
                filterItems = filterItems
                .Where(e => e.Name.Contains(name.Trim(),StringComparison.InvariantCultureIgnoreCase));
            }
            if (minPrice.HasValue)
            {
                filterItems=filterItems
                .Where(e => e.Price >= minPrice.Value);
            };

            if (maxPrice.HasValue)
            {
                filterItems=filterItems
                .Where(e => e.Price <= maxPrice.Value);
            }

            if (isActive.HasValue)
            {
                filterItems=filterItems
                .Where(e=> e.IsActive == isActive.Value);
            }
            
            return filterItems
            .AsPage(skip,take)
            .ToList();
        }

        [Route("create"), HttpPost]
        public CreateProductResultModel Create([FromBody]CreateProductModel model) {

            if (ModelState.IsValid)
            {
                
            }

            throw new NotImplementedException();

        }

    }


}