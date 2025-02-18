using Microsoft.AspNetCore.Mvc;
using MVCApp1.Models;
using MVCApp1.Services;

namespace MVCApp1.Controllers
{
    public class AccountController : Controller
    {
        private readonly Authentication _authService;
        private readonly TransactionService _transactionService;

        public AccountController(Authentication authService, TransactionService transactionService)
        {
            _authService = authService; // Injected via DI
            _transactionService = transactionService; // Injected via DI
        }

        public IActionResult Register()
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, int pin)
        {
            var user = _authService.Login(username, pin);
            if (user != null)
            {
                // Set the user in TransactionService
                _transactionService.SetUser(user);

                return RedirectToAction("Dashboard");
            }
            ViewBag.ErrorMessage = "Invalid credentials!";
            return View();
        }

        public IActionResult Dashboard()
        {
            // Use _transactionService to display data
            //var balance = _transactionService.CheckBalance();
            //ViewBag.Balance = balance;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(int amount)
        {
            await _transactionService.Deposit(amount);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(int amount)
        {
            await _transactionService.Withdraw(amount);
            return RedirectToAction("Dashboard");
        }
    }
}
