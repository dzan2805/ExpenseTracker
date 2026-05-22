using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Thêm các thư viện cần thiết cho File IO và JSON
using System.IO;
using System.Text.Json;
using ExpenseTracker.Models;
using ExpenseTracker.Interfaces;

namespace ExpenseTracker.Services
{
    // Đổi 'internal' thành 'public' và kế thừa 'IDataService'
    public class JsonDataService : IDataService
    {
        // Đường dẫn tới file transactions.json trong thư mục chạy của app
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "transactions.json");

        public void Save(List<Transaction> transactions)
        {
            try
            {
                // Format JSON dễ nhìn
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(transactions, options);

                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public List<Transaction> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Transaction>();
            }

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                var transactions = JsonSerializer.Deserialize<List<Transaction>>(jsonString);

                return transactions ?? new List<Transaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<Transaction>();
            }
        }
    }
}