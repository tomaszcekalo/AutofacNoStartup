using Microsoft.AspNetCore.Mvc;

namespace AutofacNoStartup.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesManagoController : ControllerBase
    {
        private readonly ILogger<SalesManagoController> _logger;
        private readonly ISalesManagoService salesManagoService;

        public SalesManagoController(
            ILogger<SalesManagoController> logger,
            ISalesManagoService salesManagoService)
        {
            _logger = logger;
            this.salesManagoService = salesManagoService;
        }

        [HttpGet(Name = "SalesManago")]
        public string GetSalesManago()
        {
            return salesManagoService.GetTypeString();
        }
    }
}