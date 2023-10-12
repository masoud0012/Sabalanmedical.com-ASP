using Microsoft.AspNetCore.Mvc;

namespace SabalanMedical.UI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
