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
        public BankUser Register(string name, int pin, int balance)
        {
            //bool isFirstUser = !_users.Any();

            if (_users.Any(u => u.Username == name))
            {
                throw new Exception("Username exists, kindly login...");
            }

            BankUser user = new BankUser()
            { Username = name, 
              Pin = pin,
              Balance = balance,
              //Role = isFirstUser ? "admin" : "user" 
            };

            user.TransactionHistory.Add($"Created a new account with balance: ₹{balance}");
            _users.Add(user);
            _fs.SaveUsers(_users);

            //Console.WriteLine(isFirstUser ? $"{name} is the default admin." : $"{name} registered as a user.");
            return user;
        }

        public BankUser? Login(string name, int pin)
         {
            var user = _users.Find(u => u.Username == name && u.Pin == pin);
            Console.Write("------------" + user + "---------------" + "\n LOGIN HO GYAAAA \n");
            if (user == null)
            {
                Console.WriteLine("\nInvalid username or PIN.");
                return null;
            }
            return user;
        }
    }
}