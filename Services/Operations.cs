using MVCApp1.Models;

namespace MVCApp1.Services
{
    public class Operations
    {
        public BankUser? _user; // Single user object
        private readonly FileServices _fileService;
        private List<BankUser> _users;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Operations(FileServices fs, IHttpContextAccessor hca)
        {
            _fileService = fs;
            _users = _fileService.LoadUsers(); // Load all users from JSON
            _httpContextAccessor = hca;
        }
        public void SetUser(BankUser user)
        {
            _user = _users.FirstOrDefault(u => u.Username == user.Username);

            if (_user == null)
            {
                Console.WriteLine("User not found!");
            }
        }

        public async Task Deposit(BankUser user, int amount)
        {
            if (user == null)
            {
                Console.WriteLine("No user is logged in. Please log in first.");
                return;
            }

            try
            {
                if (amount < 0)
                {
                    throw new ArgumentException("Deposit amount must be greater than zero.");
                }

                user.Balance += amount;

                user.TransactionHistory.Add($"Deposited ₹{amount}. New balance: ₹{user.Balance}");
                await SaveChanges(user); // Save the updated user to JSON
                Console.WriteLine($"Deposit Successful! New balance: Rs.{user.Balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deposit: {ex.Message}");
            }
        }


        public async Task Withdraw(BankUser user, int amount)
        {
            if (user == null)
            {
                Console.WriteLine("No user is logged in. Please set a user first.");
                return;
            }

            try
            {
                if (amount < 0)
                {
                    throw new ArgumentException("Withdrawal amount must be greater than zero.");
                }
                if (user.Balance < amount)
                {
                    throw new InvalidOperationException("Insufficient balance!");
                }

                user.Balance -= amount;
                user.TransactionHistory.Add($"Withdrawn ₹{amount}. New balance: ₹{user.Balance}");
                await SaveChanges(user);
                Console.WriteLine($"Withdrawal Successful! New balance: Rs.{user.Balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during withdrawal: {ex.Message}");
            }
        }


        public void UpdateBalance(int amount)
        {
            if (_user == null) return;

            _user.Balance += amount;
            Console.WriteLine("Balance updated successfully!");
        }
        public void CheckBalance()
        {
            if (_user == null) return;

            Console.WriteLine($"Your current balance is: Rs.{_user.Balance}");
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

        public void RequestAdminRole(BankUser user)
        {
            if (user == null)
            {
                Console.WriteLine("User not logged in.");
                return;
            }

            if (user.Role == "Admin")
            {
                Console.WriteLine("You are already an admin.");
                return;
            }

            if (user.IsAdminRequestPending)
            {
                Console.WriteLine("Admin request is already pending.");
                return;
            }

            user.IsAdminRequestPending = true;
            Console.WriteLine("Admin role request submitted.");
            SaveChanges(user);
        }

        public void RequestDeleteAccount(BankUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Mark the user's account for deletion
            user.IsDeleteRequestPending = true;

            // Save changes to the database (if applicable)
            SaveChanges(user);
        }


        public void ApproveAdminRole(string username, bool approve)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            if (user.Role == "Admin")
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
                user.Role = "Admin";
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
    }
}
