using System.Net;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using static Shared.ErrorModels.ErrorDetails;

namespace Store.Api.Factories
{
    public class ApiResponseFactory
    {
        public static ActionResult CustomValidationErrorResponse(ActionContext actionContext)
        {
            var errors = actionContext.ModelState.Where(error => error.Value.Errors.Any())
                .Select(error => new ValidationError
                {
                    key = error.Key,
                    Errors = error.Value.Errors.Select(error => error.ErrorMessage)
                });
            var validationResponse = new ValidationErrorResponse
            {
                StatusCode=(int)HttpStatusCode.BadRequest,
                Errors = errors,
                ErrorMessage="Validation Failed"
            };

            return new BadRequestObjectResult(validationResponse);
        }
    }
}
