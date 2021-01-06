using Microsoft.AspNetCore.Mvc;

using System.Linq;

namespace Convience.Util.Extension
{
    public static class ObjectResulttExtension
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
