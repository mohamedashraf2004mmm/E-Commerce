using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiBaseController
    {
        //post /api/orders create order => order(basketid , deliverymethodid , shipping address)AUTH

        //get /api/orders/ delivery method => list of delivery methods

        //get /api/orders - orders of user => [email] => user orders for logged in user

        //get /api/orders/{id} - order of user => id => user order for logged in user
    }
}
