using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{
    public interface IProductsEndpoint{
        IReadOnlyCollection<ProductSearchItemModel> Search (
            string code,
            string name,
            decimal? minPrice,  //  ? indica que pode ser null
            decimal? maxPrice,
            bool? isActive,
            int skip = 0,       //  se não vier designado assume estes valores de paginação skip-> itens a saltar antes de apresentar
            int take = 20       //  quantidade de itens a apresentar
        );

        IActionResult Create(CreateProductModel model);
        IActionResult Get(string code);
        IActionResult Update(long id, EditProductModel model);
        IActionResult Deactivate(long id, DeactivateProductModel model);
        IActionResult Activate(long id, DeactivateProductModel model);

    }
}