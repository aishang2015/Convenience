using Microsoft.AspNetCore.Mvc;

using System.Linq;

namespace Convience.Fluentvalidation
{
    public static class BadRequestExtension
    {
        public static BadRequestObjectResult BadRequestResult(this ControllerBase controller, params string[] errorMsgs)
        {
            errorMsgs.ToList().ForEach(msg =>
                controller.ModelState.AddModelError(string.Empty, msg));
            return controller.BadRequest(controller.ModelState);
        }


        public static BadRequestObjectResult BadRequestResult(this ControllerBase controller, params (string, string)[] errorMsgs)
        {
            errorMsgs.ToList().ForEach(msg =>
                controller.ModelState.AddModelError(msg.Item1, msg.Item2));
            return controller.BadRequest(controller.ModelState);
        }
    }
}
