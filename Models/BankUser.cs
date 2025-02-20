namespace MVCApp1.Models
{
    public class BankUser
    {
        public string Username { get; set; }
        public int Pin { get; set; }
        public int Balance { get; set; }
        public List<string> TransactionHistory { get; set; } = new List<string>();

        public BankUser()
        {
            // Default constructor for deserialization
        }
    }
}
