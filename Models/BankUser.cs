using System.Globalization;

namespace MVCApp1.Models
{
    public class BankUser
    {
        private string? _username;

        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public int Pin { get; set; }
        public int Balance { get; set; }
        public string Role { get; set; } = "user";

        public string CreatedOn { get; set; } = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        public bool IsAdminRequestPending { get; set; } = false;

        public bool IsDeleteRequestPending { get; set; } = false;
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();

    }
}
