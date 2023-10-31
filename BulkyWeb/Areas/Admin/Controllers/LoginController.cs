
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost,ActionName("Login")]
        public IActionResult UserLogin() { 
            return View();
        }
    }
}
