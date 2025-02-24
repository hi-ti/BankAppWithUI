using System.Globalization;

namespace MVCApp1.Models
{
    public class TransactionModel
    {
        public string Username {get; set;}
        public int Amount { get; set; }
        public int UpdatedBalance { get; set; }
        public string TimeOfTransaction { get; set; } = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }

        public TransactionModel() { }

        public TransactionModel(BankUser user, int amount, int updatedBalance, TransactionType type, TransactionStatus status)
        {
            Username = user.Username;
            Amount = amount;
            UpdatedBalance = updatedBalance;
            Type = type;
            Status = status;
        }

    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }

    public enum TransactionStatus
    {
        Success,
        Failed
    }
}
