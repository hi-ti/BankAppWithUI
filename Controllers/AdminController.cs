using Microsoft.AspNetCore.Mvc;
using MVCApp1.Services;

namespace MVCApp1.Controllers
{
    public class AdminController : Controller
    {
        private readonly Operations _operations;

        public AdminController(Operations operations)
        {
            _operations = operations;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            // Get the currently logged-in user
            var admin = _operations.GetLoggedInUser();

            // Ensure the logged-in user is an admin
            if (admin == null || admin.Role != "admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Fetch all users for the admin
            var users = _operations.GetAllUsers();
            return View(users); // Pass the list of users to the Admin Dashboard view
        }

        [HttpGet]
        public IActionResult UserDetails(string username)
        {
            var admin = _operations.GetLoggedInUser();
            if (admin == null || admin.Role != "admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var user = _operations.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Render a new view to display the user's details
        }

        [HttpGet]
        public IActionResult Requests()
        {
            var admin = _operations.GetLoggedInUser();
            if (admin == null || admin.Role != "admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var requests = _operations.GetAdminRequests(); // Fetch pending admin requests
            return View(requests);
        }

        [HttpGet]
        public IActionResult DeleteAccountRequests()
        {
            var admin = _operations.GetLoggedInUser();
            if (admin == null || admin.Role != "admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var requests = _operations.GetDeleteAccountRequests(); // Fetch account deletion requests
            return View("DeleteAccountRequests", requests);
        }

        [HttpPost]
        public IActionResult ApproveAdminRequest(string username, bool approve)
        {
            // Get the logged-in user
            var admin = _operations.GetLoggedInUser();

            // Ensure the logged-in user is an admin
            if (admin == null || admin.Role != "admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Approve or reject the admin request
            _operations.ApproveAdminRole(username, approve);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ApproveDeleteRequest(string username, bool approve)
        {
            // Get the logged-in user
            var admin = _operations.GetLoggedInUser();

            // Ensure the logged-in user is an admin
            if (admin == null || admin.Role != "admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (approve)
            {
                // Delete the user account
                _operations.DeleteUserAccount(username);
            }
            else
            {
                // Reject the delete request
                _operations.RejectDeleteRequest(username);
            }

            return RedirectToAction("Dashboard");
        }
    }
}
