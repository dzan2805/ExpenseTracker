using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Views;

namespace ExpenseTracker
{
    public partial class MainWindow : Window
    {
        private List<Transaction> transactions;
        private JsonDataService dataService;

        public MainWindow()
        {
            InitializeComponent();

            // Khởi tạo service
            dataService = new JsonDataService();

            // Load dữ liệu từ JSON
            transactions = dataService.Load() ?? new List<Transaction>();

            // Hiển thị dữ liệu
            RefreshDataGrid();
            UpdateBalance();
        }

        // =========================
        // REFRESH DATA GRID
        // =========================
        private void RefreshDataGrid()
{
    if (transactions == null)
        transactions = new List<Transaction>();

    if (transactionGrid == null)
        return;

    transactionGrid.ItemsSource = null;
    transactionGrid.ItemsSource = transactions;
}

        // =========================
        // UPDATE CURRENT BALANCE
        // =========================
        private void UpdateBalance()
        {
            double income = transactions
                .Where(t => t is Income)
                .Sum(t => t.Amount);

            double expense = transactions
                .Where(t => t is Expense)
                .Sum(t => t.Amount);

            double balance = income - expense;

            // tìm TextBlock Current Balance
            // đổi x:Name trong MainWindow.xaml thành:
            // x:Name="txtBalance"

            txtBalance.Text = $"{balance:N0} VND";
        }

        // =========================
        // ADD INCOME
        // =========================
        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            OpenAddTransactionWindow(true);
        }

        // =========================
        // ADD EXPENSE
        // =========================
        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            OpenAddTransactionWindow(false);
        }

        // =========================
        // OPEN ADD WINDOW
        // =========================
        private void OpenAddTransactionWindow(bool isIncome)
        {
            AddTransactionWindow window = new AddTransactionWindow(isIncome);

            bool? result = window.ShowDialog();

            if (result == true && window.NewTransaction != null)
            {
                transactions.Add(window.NewTransaction);

                // lưu JSON
                dataService.Save(transactions);

                // refresh UI
                RefreshDataGrid();
                UpdateBalance();
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (transactionGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn giao dịch cần xóa.");
                return;
            }

            Transaction selected = (Transaction)transactionGrid.SelectedItem;

            var result = MessageBox.Show(
                "Bạn có chắc muốn xóa giao dịch này không?",
                "Xác nhận",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                transactions.Remove(selected);

                dataService.Save(transactions);

                RefreshDataGrid();
                UpdateBalance();
            }
        }
        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (transactions == null)
                return;

            string keyword = txtSearch.Text?.ToLower() ?? "";

            var filtered = transactions
                .Where(t => (t.Title ?? "").ToLower().Contains(keyword))
                .ToList();

            transactionGrid.ItemsSource = null;
            transactionGrid.ItemsSource = filtered;
        }
        private void cbFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (transactions == null)
                return;

            if (cbFilter.SelectedItem == null)
                return;

            string category =
                ((System.Windows.Controls.ComboBoxItem)cbFilter.SelectedItem).Content.ToString();

            if (category == "Tất cả")
            {
                RefreshDataGrid();
                return;
            }

            var filtered = transactions
                .Where(t => (t.Category ?? "") == category)
                .ToList();

            transactionGrid.ItemsSource = null;
            transactionGrid.ItemsSource = filtered;
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (transactionGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn giao dịch cần chỉnh sửa.");
                return;
            }

            Transaction selected = (Transaction)transactionGrid.SelectedItem;

            int index = transactions.IndexOf(selected);

            AddTransactionWindow window = new AddTransactionWindow(selected);

            bool? result = window.ShowDialog();

            if (result == true && window.NewTransaction != null)
            {
                // thay transaction cũ bằng transaction mới
                transactions[index] = window.NewTransaction;

                // lưu JSON
                dataService.Save(transactions);

                // refresh UI
                RefreshDataGrid();
                UpdateBalance();
            }
        }
    }
}