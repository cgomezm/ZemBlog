using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zemblog.Domain;
using Zemblog.Domain.Constants;

namespace ZemBlogMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            if (!new string[] { "escritor", "editor" }.Contains(user.Username) || user.Password != "pass123")
            {
                ModelState.AddModelError("credentials", "Invalid credentials");
                user.Password = string.Empty;
                return View();
            }

            var role = user.Username == "escritor" ? Constants.WriterRole : Constants.EditorRole;

            var userClaims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            return Redirect("/Posts");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return Redirect("/Posts");
            }

            return Redirect("/Login");
        }
    }
}
