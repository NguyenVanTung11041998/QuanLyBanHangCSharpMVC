using QuanLyBanHangCSharpMVC.Helpers;
using System.Web.Mvc;

namespace QuanLyBanHangCSharpMVC.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session[Constant.UserCustomerSession] == null)
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Index" }));
            base.OnActionExecuting(filterContext);
        }
    }
}