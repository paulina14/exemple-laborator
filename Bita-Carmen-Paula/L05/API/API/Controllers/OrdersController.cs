using API.Models;
using Lab1.Domain;
using Lab1.Domain.Models;
using Lab1.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private ILogger<OrdersController> logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrderLines([FromServices] IOrderLineRepository ordersRepo) =>
        await ordersRepo.TryGetExistingProducts().Match(
            Succ: GetAllOrderLinesHandleSucces,
            Fail: GetAllOrderLinesHandleError
        );
        private ObjectResult GetAllOrderLinesHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }
        private OkObjectResult GetAllOrderLinesHandleSucces(List<CalculatedPayment> orders) =>
            Ok(orders.Select(order => new
            {
                ProductCode = order.productCode.Value,
                order.productAmount.Value,
                order.productPrice,
                order.finalPrice,
                order.adress
            }));

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromServices] PayCartWorkflow payCartWorkflow, [FromBody]InputOrder[] orders)
        {
            var unvalidatedOrders = orders.Select(MapInputOrderToUnvalidatedOrder)
                                            .ToList()
                                            .AsReadOnly();
            PayShopppingCartCommand command = new PayShopppingCartCommand(unvalidatedOrders);
            var result = await payCartWorkflow.ExecuteAsync(command);
            return result.Match<IActionResult>(
                    whenCartPaidFailEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                    whenCartPaidSuccededEvent: succesEvent => Ok()
            );
        }
        private static UnvalidatedProductsCart MapInputOrderToUnvalidatedOrder(InputOrder order) => new UnvalidatedProductsCart(
            ProductCode: order.Code,
            ProductAmount: order.Quantity,
            ProductPrice: order.Price,
            Adress: order.Adress);

    }
}
