using MVCApp1.Models;
using TransactionStatus = MVCApp1.Models.TransactionStatus;

namespace MVCApp1.Services
{
    public class Operations
    {
        public BankUser? _user; // Single user object
        private readonly FileServices _fileService;
        private List<BankUser> _users;
        private TransactionModel _transactions;
        public Operations(FileServices fs)
        {
            _fileService = fs;
            _users = _fileService.LoadUsers();
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

                int updatedBalance = user.Balance;
                TransactionModel transaction = new TransactionModel(user, amount, updatedBalance, TransactionType.Deposit, TransactionStatus.Success);

                user.Transactions.Add(transaction);
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
                //    int updatedBalance = user.Balance;
                //    TransactionModel transaction = new TransactionModel(user, amount, updatedBalance, TransactionType.Withdrawal, TransactionStatus.Success);

                if (amount < 0)
                {
                    throw new ArgumentException("Withdrawal amount must be greater than zero.");
                }
                if (user.Balance < amount)
                {
                int updatedBalance = user.Balance;
                TransactionModel transaction = new TransactionModel(user, amount, updatedBalance, TransactionType.Withdrawal, TransactionStatus.Failed);
                user.Transactions.Add(transaction);
                }
                else
                {
                    user.Balance -= amount;
                    int updatedBalance = user.Balance;
                    TransactionModel transaction = new TransactionModel(user, amount, updatedBalance, TransactionType.Withdrawal, TransactionStatus.Success);
                    user.Transactions.Add(transaction);
                }
                //user.Balance -= amount;
                //user.Transactions.Add(transaction);
                //user.TransactionHistory.Add($"Withdrawn ₹{amount}. New balance: ₹{user.Balance}");
                await SaveChanges(user);
                Console.WriteLine($"Withdrawal Successful! New balance: Rs.{user.Balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during withdrawal: {ex.Message}");
            }
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
    }
}
