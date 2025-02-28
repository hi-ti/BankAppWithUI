using MVCApp1.Models;

namespace MVCApp1.Services
{
    public class Authentication
    {
        private readonly FileServices _fs;
        private List<BankUser> _users;

        public Authentication()
        {
            _fs = new FileServices();
            _users = _fs.LoadUsers();
        }

        public BankUser? GetUser(string username)
        {
            return _users.FirstOrDefault(x => x.Username == username);
        }
        public BankUser Register(string name, string firstname, string lastname, int pin, int balance)
        {
            bool isFirstUser = !_users.Any();

            if (_users.Any(u => u.Username == name))
            {
                throw new Exception("Username exists, kindly login...");
            }

            BankUser user = new BankUser()
            { Username = name,
              FirstName = firstname,
              LastName = lastname,
              Pin = pin,
              Balance = balance,
              Role = isFirstUser ? "admin" : "user"
            };

            TransactionModel transaction = new TransactionModel(user, balance, balance, TransactionType.Deposit, TransactionStatus.Success);

            user.Transactions.Add(transaction);
            _users.Add(user);
            _fs.SaveUsers(_users);

            //Console.WriteLine(isFirstUser ? $"{name} is the default admin." : $"{name} registered as a user.");
            return user;
        }

        public BankUser? Login(string name, int pin)
         {
            BankUser foundUser = null;
            foreach (var u in _users)
            {
                Console.WriteLine($"Checking: {u.Username} vs {name}, {u.Pin} vs {pin}");
                if (u.Username.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) && u.Pin == pin)
                {
                    foundUser = u;
                    //return foundUser;
                }
            }

            if (foundUser == null)
            {
                Console.WriteLine("User not found.");
            }
            return foundUser;

            //var user = _users.Find(u => u.Username == name && u.Pin == pin);
            //Console.Write("------------" + user + "---------------" + "\n LOGIN HO GYAAAA \n");
            //if (user == null)
            //{
            //    Console.WriteLine("\nInvalid username or PIN.");
            //    return null;
            //}
            //return user;
        }


    }
}