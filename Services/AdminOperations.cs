using Microsoft.AspNetCore.Mvc;
using MVCApp1.Models;

namespace MVCApp1.Services
{
    public class AdminOperations
    {
        public BankUser? _user; // Single user object
        private readonly FileServices _fileService;
        private readonly List<BankUser> _users;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminOperations(FileServices fs, IHttpContextAccessor hca)
        {
            _fileService = fs;
            _users = _fileService.LoadUsers(); // Load all users from JSON
            _httpContextAccessor = hca;
        }
        public void ApproveAdminRole(string username, bool approve)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            if (user.Role == "admin")
            {
                Console.WriteLine("User is already an admin.");
                return;
            }

            if (!user.IsAdminRequestPending)
            {
                Console.WriteLine("No admin request is pending for this user.");
                return;
            }

            if (approve)
            {
                user.Role = "admin";
                Console.WriteLine($"{user.Username} is now an admin.");
            }
            else
            {
                Console.WriteLine($"Admin request for {user.Username} was rejected.");
            }

            user.IsAdminRequestPending = false;
            SaveChanges(user);
        }

        public BankUser? GetLoggedInUser()
        {
            // Get the username of the logged-in user from the session
            var username = _httpContextAccessor.HttpContext?.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return null;

            // Find the user by username
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<BankUser> GetAllUsers()
        {
            return _users; // Return the full list of users
        }

        public List<BankUser> GetAdminRequests()
        {
            return _users.Where(u => u.IsAdminRequestPending).ToList();
        }

        public List<BankUser> GetDeleteAccountRequests()
        {
            return _users.Where(u => u.IsDeleteRequestPending).ToList();
        }

        public BankUser GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void DeleteUserAccount(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            if (!user.IsDeleteRequestPending)
            {
                Console.WriteLine("No delete request is pending for this user.");
                return;
            }

            // Remove the user from the list
            _users.Remove(user);

            // Save the updated list to the JSON file
            _fileService.SaveUsers(_users);
            Console.WriteLine($"User {username} has been deleted.");
        }

        public void RejectDeleteRequest(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            user.IsDeleteRequestPending = false;
            SaveChanges(user);
            Console.WriteLine($"Delete request for {username} was rejected.");
        }
        private async Task SaveChanges(BankUser user)
        {
            var index = _users.FindIndex(u => u.Username == user.Username);
            if (index != -1)
            {
                _users[index] = user; // Update the user in the list
                _fileService.SaveUsers(_users); // Save the entire list
                Console.WriteLine("Changes saved successfully!");
            }
        }

    }
}
