namespace MVCApp1.Models
{
    public class BankUser
    {
        public string? Username { get; set; }
        public int Pin { get; set; }
        public int Balance { get; set; }

        public List<string> TransactionHist { get; set; }

        public BankUser()
        {
            TransactionHist = new List<string> { $"Account created with balance {Balance}" };
        }
    }
}
