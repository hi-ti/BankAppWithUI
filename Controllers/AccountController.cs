using Microsoft.AspNetCore.Mvc;
using MVCApp1.Models;
using MVCApp1.Services;

namespace MVCApp1.Controllers
{
    public class AccountController : Controller
    {
        private readonly Authentication _authService;
        private readonly Operations _operations;

        public AccountController(Authentication au, Operations op)
        {
            _authService = au; // Injected via DI
            _operations = op;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Deposit()
        {
            return View();
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(BankUser user)
        {
            if (ModelState.IsValid)
            {
                _authService.Register(user.Username, user.Pin, user.Balance);
                return RedirectToAction("Login");
            }
            return View();
        }



        [HttpPost]
        public IActionResult Login(string username, int pin)
        {
            var user = _authService.Login(username, pin);
            if (user != null)
            {
                // Save the username in session
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Dashboard");
            }

            ViewBag.ErrorMessage = "Invalid credentials!";
            return View();
        }

        public IActionResult Dashboard()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login");

            var user = _authService.GetUser(username); // Get the user by username
            if (user == null) return RedirectToAction("Login");

            return View(user); // Pass the BankUser object to the view
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(int amount)
        {
            Console.WriteLine($"Received amount: ₹{amount}");
            // Debugging log or breakpoint to verify the value

            var username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login");

            var user = _authService.GetUser(username);
            if (user == null) return RedirectToAction("Login");

            await _operations.Deposit(user, amount);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(int amount)
        {
            Console.WriteLine($"Recieved amount: Rs.{amount}");

            var username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login");

            var user = _authService.GetUser(username);
            if (user == null) return RedirectToAction("Login");

            await _operations.Withdraw(user, amount);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult RequestAdminRole()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login");

            var user = _authService.GetUser(username);
            if (user == null) return RedirectToAction("Login");

            _operations.RequestAdminRole(user);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult RequestAccountDeletion()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login");

            var user = _authService.GetUser(username);
            if (user == null) return RedirectToAction("Login");

            _operations.RequestDeleteAccount(user);
            return RedirectToAction("Dashboard");
        }
    }
}
