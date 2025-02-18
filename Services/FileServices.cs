using System.Text.Json;
using MVCApp1.Models;

namespace MVCApp1.Services
{
    public class FileServices
    {
        private readonly string _filePath = @"C:\Users\hiti.dudeja\source\repos\Test\users.json";

        public List<BankUser> LoadUsers()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("File not found. Creating a new one.");
                    return new List<BankUser>();
                }

                string json = File.ReadAllText(_filePath);
                //var userr = JsonSerializer.Deserialize<BankUser>(json);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Case-insensitive matching
                    IncludeFields = true // Include fields in deserialization
                };

                return JsonSerializer.Deserialize<List<BankUser>>(json, options) ?? new List<BankUser>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                return new List<BankUser>();
            }
        }

        public void SaveUsers(List<BankUser> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Users saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
    }
}


