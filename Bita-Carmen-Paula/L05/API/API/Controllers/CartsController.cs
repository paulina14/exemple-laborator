using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Lab1.Domain.Models;
using Lab1.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartsController : ControllerBase
    {
        private ILogger<CartsController> logger;

        public CartsController(ILogger<CartsController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromServices] IProductsRepository productsRepo) =>
            await productsRepo.TryGetAllProducts().Match(
            Succ: GetAllProductsHandleSuccess,
            Fail: GetAllProductsHandleError
        );
        private ObjectResult GetAllProductsHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }
        private OkObjectResult GetAllProductsHandleSuccess(List<Product> products) =>
            Ok(products.Select(prod => new
            {
                ProductCode = prod.code.Value,
                ProductAmount = prod.amount.Value
            }));

    }
}
