namespace MVCApp1.Models
{
    public class BankUser
    {
        public string Username { get; set; }
        public int Pin { get; set; }
        public int Balance { get; set; }
        public string Role { get; set; } = "user"; // Default role is User
        public bool IsAdminRequestPending { get; set; } = false; // Default is no request pending

        public bool IsDeleteRequestPending { get; set; } = false;
        public List<string> TransactionHistory { get; set; } = new List<string>();
    }
}
