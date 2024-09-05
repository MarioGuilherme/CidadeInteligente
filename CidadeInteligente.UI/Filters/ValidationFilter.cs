using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CidadeInteligente.UI.Filters;

public class ValidationFilter : IActionFilter {
    public void OnActionExecuted(ActionExecutedContext context) { }

    public void OnActionExecuting(ActionExecutingContext context) {
        if (context.ModelState.IsValid) return;

        IEnumerable<string> messages = context.ModelState
            .SelectMany(ms => ms.Value!.Errors)
            .Select(e => e.ErrorMessage);

        context.Result = new BadRequestObjectResult(messages);
    }
}