using Microsoft.AspNetCore.Mvc;
using MVCApp1.Services;

namespace MVCApp1.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminOperations _adminOp;

        public AdminController(AdminOperations adminOp)
        {
            _adminOp = adminOp;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var admin = _adminOp.GetLoggedInUser();

            var users = _adminOp.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult UserDetails(string username)
        {
            var admin = _adminOp.GetLoggedInUser();

            var user = _adminOp.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Requests()
        {
            var admin = _adminOp.GetLoggedInUser();

            var requests = _adminOp.GetAdminRequests();
            return View(requests);
        }

        [HttpGet]
        public IActionResult DeleteAccountRequests()
        {
            var admin = _adminOp.GetLoggedInUser();

            var requests = _adminOp.GetDeleteAccountRequests();
            return View("DeleteAccountRequests", requests);
        }

        [HttpPost]
        public IActionResult ApproveAdminRequest(string username, bool approve)
        {
            var admin = _adminOp.GetLoggedInUser();

            _adminOp.ApproveAdminRole(username, approve);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ApproveDeleteRequest(string username, bool approve)
        {
            var admin = _adminOp.GetLoggedInUser();

            if (approve)
            {
                _adminOp.DeleteUserAccount(username);
            }
            else
            {
                _adminOp.RejectDeleteRequest(username);
            }
            return RedirectToAction("Dashboard");
        }
    }
}
