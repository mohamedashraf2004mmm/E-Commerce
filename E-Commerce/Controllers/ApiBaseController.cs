using E_Commerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            //success
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.data);
            }
            else
            {
                //fail
                return ToProblem(result.Errors);

            }
        }

        protected static ObjectResult ToProblem(IReadOnlyList<Error> errors)
        {
            var firstError = errors[0];
            var statusCode = firstError.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };
            var problem = new ProblemDetails()
            {
                Status = statusCode,
                Title = firstError.code,
                Detail = firstError.Description,
                Extensions = { ["errors"] = errors }
            };
            return new ObjectResult(problem);
        }

        protected static ActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
                return new OkResult();
            else
                return ToProblem(result.Errors);
        }
    }
}
