using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        //post /api/orders create order => order(basketid , deliverymethodid , shipping address)AUTH
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreatedOrderDto>>CreateOrder(OrderDto orderDto , CancellationToken ct = default)
        {
          return ToActionResult( await _orderService.CreateOrderAsync(orderDto, GetEmailFromToken(), ct));
        }

        //get /api/orders/ delivery method => list of delivery methods

        //get /api/orders - orders of user => [email] => user orders for logged in user
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CreatedOrderDto>>>GetAllOrders(CancellationToken ct)
        {
            return ToActionResult(await _orderService.GetAllOrdersForUserAsync(GetEmailFromToken(), ct));
        }

        //get /api/orders/{id} - order of user => id => user order for logged in user
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CreatedOrderDto>>GetSpecificOrderById(Guid id , CancellationToken ct)
        {
            return ToActionResult(await _orderService.GetOrderByIdAndEmailForUserAsync(id, GetEmailFromToken(), ct));
        }
    }
}
