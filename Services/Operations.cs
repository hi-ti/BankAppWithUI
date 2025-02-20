using MVCApp1.Models;

namespace MVCApp1.Services
{
    public class Operations
    {
        public BankUser? _user; // Single user object
        private readonly FileServices _fileService;
        private List<BankUser> _users; // Full user list from JSON

        //_user = new Authentication.GetUser();
        public Operations(FileServices fs)
        {
            _fileService = fs;
            _users = _fileService.LoadUsers(); // Load all users from JSON

            //Find the specific user by Username
            //_user = _users.FirstOrDefault(u => u.Username == _user.Username);

            //if (_user == null)
            //{
            //    Console.WriteLine("User not found!");
            //}
        }
        public void SetUser(BankUser user)
        {
            _user = _users.FirstOrDefault(u => u.Username == user.Username);

            if (_user == null)
            {
                Console.WriteLine("User not found!");
                //_user = user; // Assign a new user if not found
                //_users.Add(user); // Optionally add the user to the list
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

    }
}
