using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ExpenseTracker.Models;
using ExpenseTracker.Interfaces;

namespace ExpenseTracker.Services
{
    public class JsonDataService : IDataService
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "transactions.json");
        private readonly string _backupFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "transactions_backup.json");

        public void Save(List<Transaction> transactions)
        {
            if (transactions == null) return;

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(transactions, options);

                // 1. GHI AN TOÀN: Ghi ra một file tạm (temp) trước. 
                // Điều này giúp tránh việc đang ghi file chính mà mất điện thì hỏng luôn file.
                string tempFilePath = _filePath + ".tmp";
                File.WriteAllText(tempFilePath, jsonString);

                // 2. Chép từ file tạm đè lên file chính (thao tác này diễn ra cực nhanh và an toàn)
                File.Copy(tempFilePath, _filePath, overwrite: true);

                // 3. BACKUP: Tạo thêm một bản sao dự phòng
                File.Copy(_filePath, _backupFilePath, overwrite: true);

                // 4. Dọn dẹp file tạm
                File.Delete(tempFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public List<Transaction> Load()
        {
            // Nếu cả file chính và file backup đều không có (mở app lần đầu)
            if (!File.Exists(_filePath) && !File.Exists(_backupFilePath))
            {
                return new List<Transaction>();
            }

            try
            {
                // Thử đọc file chính trước
                if (File.Exists(_filePath))
                {
                    string jsonString = File.ReadAllText(_filePath);
                    var transactions = JsonSerializer.Deserialize<List<Transaction>>(jsonString);
                    if (transactions != null) return transactions;
                }
            }
            catch (JsonException)
            {
                // File chính bị hỏng định dạng (corrupted), chuyển sang bước cứu hộ bên dưới
                Console.WriteLine("File chính bị lỗi. Đang tiến hành khôi phục từ file backup...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
            }

            // BƯỚC CỨU HỘ: Nếu code chạy xuống đây nghĩa là file chính có vấn đề
            try
            {
                if (File.Exists(_backupFilePath))
                {
                    string backupJsonString = File.ReadAllText(_backupFilePath);
                    var backupTransactions = JsonSerializer.Deserialize<List<Transaction>>(backupJsonString);
                    return backupTransactions ?? new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File backup cũng bị lỗi: {ex.Message}");
            }

            // Nếu mọi cách đều thất bại, trả về list rỗng để app không bị văng
            return new List<Transaction>();
        }
    }
}