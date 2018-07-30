using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{

    [Route ("api/products")]

    public class ProductsController : ControllerBase, IProductsEndpoint {

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
            
            var result = new []{
                new ProductSearchItemModel{
                    Code = "111",
                    Name = "Bola",
                    Price = 10.5m,
                    CurrentStock = 50,
                    UpdatedOn = DateTimeOffset.Now.AddMinutes(-32),
                    UpdatedBy = "ruben.ribeiro",
                    IsActive = true,
                },

                new ProductSearchItemModel{
                    Code = "222",
                    Name = "Bola de futebol",
                    Price = 50.5m,
                    CurrentStock = 10,
                    UpdatedOn = DateTimeOffset.Now.AddHours(-50),
                    UpdatedBy = "Marcolino",
                    IsActive = false,
                }
            };

            IEnumerable<ProductSearchItemModel> filterItems = result;

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

    }


}