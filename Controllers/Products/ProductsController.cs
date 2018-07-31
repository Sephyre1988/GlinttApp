using System;
using System.Collections.Generic;
using System.Linq;
using AcademiadecodigoWarehouseApi.Database;
using Microsoft.AspNetCore.Mvc;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{
    [Route("api/products")]
    public class ProductsController : Controller, IProductsEndpoint
    {
        private static readonly List<ProductEntity> MockProducts = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Code = "111",
                Name = "Bola",
                Description = "fantastico",
                Price = 10.5m,
                StockMoviments =
                {
                    new StockMovimentEntity
                    {
                        Price = 10.5m,
                        Quantity = 2,
                        CreatedOn = DateTimeOffset.Now.AddDays(-2),
                        CreatedBy = "ruben.ribeiro"
                    }
                },
                CreatedOn = DateTimeOffset.Now.AddDays(-30),
                CreatedBy = "ruben.ribeiro",
                UpdatedOn = DateTimeOffset.Now.AddMinutes(-32),
                UpdatedBy = "ruben.ribeiro"
            },

            new ProductEntity
            {
                Id = 2,
                Code = "222",
                Name = "Bola de futebol",
                Description = "not so Awesome",
                Price = 50.5m,
                StockMoviments =
                {
                    new StockMovimentEntity
                    {
                        Price = 50.5m,
                        Quantity = 2,
                        CreatedOn = DateTimeOffset.Now.AddDays(-20),
                        CreatedBy = "ruben.ribeiro"
                    }
                },
                CreatedOn = DateTimeOffset.Now.AddDays(-30),
                CreatedBy = "ruben.ribeiro",
                UpdatedOn = DateTimeOffset.Now.AddHours(-50),
                UpdatedBy = "Marcolino"
            }
        };

        private readonly WarehouseContext _ctx;

        public ProductsController(WarehouseContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        [Route("search")]
        [HttpGet]
        public IReadOnlyCollection<ProductSearchItemModel> Search(
            string code,
            string name,
            decimal? minPrice, //  ? indica que pode ser null
            decimal? maxPrice,
            bool? isActive,
            int skip = 0, //  se não vier designado assume estes valores de paginação skip-> itens a saltar antes de apresentar
            int take = 20 //  quantidade de itens a apresentar
        )
        {
            /* 
            var result = new []{
                
            };
            */

            var filterItems = _ctx.Set<ProductEntity>()
                .Select(e => new ProductSearchItemModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    Price = e.Price,
                    CurrentStock = e.StockMoviments.Sum(m => m.Quantity),
                    IsActive = e.DeletedOn == null,
                    UpdatedOn = e.UpdatedOn,
                    UpdatedBy = e.UpdatedBy
                });

            if (!string.IsNullOrWhiteSpace(code))
                filterItems = filterItems
                    .Where(e => e.Code.Contains(code.Trim(), StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(name))
                filterItems = filterItems
                    .Where(e => e.Name.Contains(name.Trim(), StringComparison.InvariantCultureIgnoreCase));
            if (minPrice.HasValue)
                filterItems = filterItems
                    .Where(e => e.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                filterItems = filterItems
                    .Where(e => e.Price <= maxPrice.Value);

            if (isActive.HasValue)
                filterItems = filterItems
                    .Where(e => e.IsActive == isActive.Value);

            return filterItems
                .AsPage(skip, take)
                .ToList();
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateProductModel model)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(
                    new
                    {
                        Message = "bad data format",
                        ModelState = ModelState.Select(e => new
                        {
                            e.Key,
                            Value = e.Value.Errors
                        })
                    }
                );
            {
                if (MockProducts.Any(e => e.Code.Equals(model.Code.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                    return Conflict(new
                    {
                        Message = "Duplicated Code"
                    });

                if (MockProducts.Any(e => e.Name.Equals(model.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                    return Conflict(new
                    {
                        Message = "Duplicated Name"
                    });

                var now = DateTimeOffset.Now;
                var username = User.Identity.Name;

                var newId = MockProducts.Max(e => e.Id) + 1;
                MockProducts.Add(new ProductEntity
                {
                    Id = newId,
                    Code = model.Code,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CreatedOn = now,
                    CreatedBy = username,
                    UpdatedOn = now,
                    UpdatedBy = username
                });

                return Json(new CreateProductResultModel {Id = newId});
            }
        }

        [Route("{code}")]
        [HttpGet]
        public IActionResult Get(string code)
        {
            var product = MockProducts
                .SingleOrDefault(e => e.Code.Equals(code.Trim(), StringComparison.InvariantCultureIgnoreCase));

            if (product == null) return NotFound();

            return Json(new ProductModel
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CurrentStock = product.StockMoviments.Sum(m => m.Quantity),
                CreatedOn = product.CreatedOn,
                CreatedBy = product.CreatedBy,
                UpdatedOn = product.UpdatedOn,
                UpdatedBy = product.UpdatedBy,
                DeletedOn = product.DeletedOn,
                deletedBy = product.DeletedBy,
                Version = product.Version
            });
        }

        [Route("update/{id}")]
        [HttpPost]
        public IActionResult Update([FromRoute] long id, [FromBody] EditProductModel model)
        {
            var product = MockProducts.SingleOrDefault(e => e.Id == id);

            if (product == null)
            {
                Console.WriteLine("Not found return command");
                return NotFound();
            }

            if (product.Version != model.Version)
                return Conflict(new
                {
                    Message = "there are prior changes"
                });


            if (!ModelState.IsValid)
                return UnprocessableEntity(
                    new
                    {
                        Message = "bad data format",
                        ModelState = ModelState.Select(e => new
                        {
                            e.Key,
                            Value = e.Value.Errors
                        })
                    }
                );

            if (MockProducts.Any(e =>
                e.Name.Equals(model.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                return Conflict(new
                {
                    Message = "Can't save this name"
                });

            if (MockProducts.Any(e =>
                e.Code.Equals(model.Code.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                return Conflict(new
                {
                    Message = "Can't save this code"
                });

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.Code = model.Code;
            product.UpdatedOn = DateTimeOffset.Now;
            product.UpdatedBy = User.Identity.Name;
            ++product.Version;


            return Json(new
            {
                product.Version
            });
        }

        [Route("delete/{id}")]
        [HttpPost]
        public IActionResult Deactivate([FromRoute] long id, [FromBody] DeactivateProductModel model)
        {
            var product = MockProducts.SingleOrDefault(e => e.Id == id);

            if (product == null)
            {
                Console.WriteLine("Not found return command");
                return NotFound();
            }

            if (product.Version != model.Version)
                return Conflict(new
                {
                    Message = "there are prior changes"
                });

            product.DeletedOn = product.UpdatedOn = DateTimeOffset.Now;
            product.DeletedBy = product.UpdatedBy = User.Identity.Name;
            ++product.Version;


            return Json(new
            {
                product.Version
            });
        }

        [Route("activate/{id}")]
        [HttpPost]
        public IActionResult Activate([FromRoute] long id, [FromBody] DeactivateProductModel model)
        {
            var product = MockProducts.SingleOrDefault(e => e.Id == id);

            if (product == null)
            {
                Console.WriteLine("Not found return command");
                return NotFound();
            }

            if (product.Version != model.Version)
                return Conflict(new
                {
                    Message = "there are prior changes"
                });

            product.DeletedOn = null;
            product.UpdatedOn = DateTimeOffset.Now;
            product.DeletedBy = null;
            product.UpdatedBy = User.Identity.Name;
            ++product.Version;


            return Json(new
            {
                product.Version
            });
        }
    }
}