using MVCApp1.Models;

namespace MVCApp1.Services
{
    public class Operations
    {
        protected BankUser? _user; // Single user object
        private readonly FileServices _fileService;
        private List<BankUser> _users; // Full user list from JSON

        public Operations(BankUser user)
        {
            _fileService = new FileServices();
            _users = _fileService.LoadUsers(); // Load all users from JSON

            // Find the specific user by Username
            _user = _users.Find(u => u.Username == user.Username);

            if (_user == null)
            {
                Console.WriteLine("User not found!");
            }
        }

        public async Task Deposit(int amount)
        {
            if (_user == null) return;

            _user.Balance += amount;
            await SaveChanges();
            Console.WriteLine("Deposit successful!");
        }

        public async Task Withdraw(int amount)
        {
            if (_user == null) return;

            if (_user.Balance < amount)
            {
                Console.WriteLine("Insufficient balance!");
                return;
            }

            _user.Balance -= amount;
            await SaveChanges();
            Console.WriteLine("Withdrawal successful!");
        }

        protected void UpdateBalance(int amount)
        {
            if (_user == null) return;

            _user.Balance += amount;
            Console.WriteLine("Balance updated successfully!");
        }

        //protected void AddTransaction(string transaction)
        //{
        //    if (_user == null) return;

        //     _user.TransactionHist.Add(transaction);

        //    Console.WriteLine($"Transaction added: {transaction}");
        //}


        //public void ShowTransactionHistory()
        //{
        //    if (_user == null) return;

        //    Console.WriteLine("\nTransaction History:");
        //    foreach (string transaction in _user.TransactionHist)
        //    {
        //        Console.WriteLine(transaction);
        //    }
        //}

        public void CheckBalance()
        {
            if (_user == null) return;

            Console.WriteLine($"Your current balance is: Rs.{_user.Balance}");
        }

        private async Task SaveChanges()
        {
            var index = _users.FindIndex(u => u.Username == _user.Username);
            if (index != -1)
            {
                _users[index] = _user; // Update the user in the list
                _fileService.SaveUsers(_users); // Save the entire list
                Console.WriteLine("Changes saved");
            }
        }
    }
}
