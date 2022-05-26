using FormsAuthentication.Models;
using FormsAuthentication.Service;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
namespace FormsAuthentication.Controllers
{
    public class AccountController : Controller
    {
        Repository repository = new Repository();
        // GET: Account
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated) //check if authenticated user is available
            {
                return RedirectToAction("Index", "Home"); //if yes redirect to HomeController - means user is still log in
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(LoginViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var result = await repository.Login(model);
            if (result.resultCode == 200)
            {
                FormsAuthentication.SetAuthCookie(model.Email, false);
                return RedirectToAction("Index", "Home", result.Data); 
            }
            else
            {
                ModelState.AddModelError("", result.message);
            }

            return View();
        }
        public ActionResult Register() //Register
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Register(LoginViewModel model) //Register
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var result = await repository.register(model);

            if (result.resultCode == 200)
            {
                return RedirectToAction("Index"); //redirect to login form
            }
            else
            {
                ModelState.AddModelError("", result.message);
            }

            return View();
        }
        public ActionResult Logout() //Register
        {
            return RedirectToAction("Index"); //redirect to index
        }
    }
}