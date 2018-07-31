using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademiadecodigoWarehouseApi.Controllers.Products
{
    public class ActivateProductModel
    {
        [Required]
        public long Version { get; set; }
    }
}
