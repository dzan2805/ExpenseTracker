using System;
using System.Windows;
using ExpenseTracker.Models;

namespace ExpenseTracker.Views
{
    public partial class AddTransactionWindow : Window
    {
        public Transaction? NewTransaction { get; private set; }

        public AddTransactionWindow(bool isIncome = false)
        {
            InitializeComponent();

            // Chọn radio button mặc định
            if (isIncome)
            {
                rbIncome.IsChecked = true;
            }
            else
            {
                rbExpense.IsChecked = true;
            }

            InitializeCategories();

            dpDate.SelectedDate = DateTime.Now;
        }

        public AddTransactionWindow(Transaction existingTransaction)
        {
            InitializeComponent();

            // Đổ dữ liệu cũ vào form
            txtTitle.Text = existingTransaction.Title;
            txtAmount.Text = existingTransaction.Amount.ToString();
            dpDate.SelectedDate = existingTransaction.Date;

            if (existingTransaction is Income)
                rbIncome.IsChecked = true;
            else
                rbExpense.IsChecked = true;

            InitializeCategories();

            cbCategory.Text = existingTransaction.Category;
        }

        private void InitializeCategories()
        {
            if (cbCategory == null) return;

            cbCategory.Items.Clear();
            if (rbExpense?.IsChecked == true)
            {
                cbCategory.Items.Add("Ăn uống");
                cbCategory.Items.Add("Di chuyển");
                cbCategory.Items.Add("Mua sắm");
                cbCategory.Items.Add("Hóa đơn");
                cbCategory.Items.Add("Giải trí");
            }
            else
            {
                cbCategory.Items.Add("Lương");
                cbCategory.Items.Add("Thưởng");
                cbCategory.Items.Add("Thu nhập khác");
            }
            cbCategory.SelectedIndex = 0;
        }

        // Đảm bảo hàm này trùng khớp với Checked="rbType_Changed" bên XAML
        private void rbType_Changed(object sender, RoutedEventArgs e)
        {
            InitializeCategories();
        }

        // Đảm bảo hàm này trùng khớp với Click="btnSave_Click" bên XAML
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle?.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề giao dịch!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtAmount?.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Số tiền nhập vào phải là số dương hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpDate?.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày giao dịch!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string title = txtTitle.Text.Trim();
            DateTime date = dpDate.SelectedDate.Value;

            if (rbExpense?.IsChecked == true)
            {
                NewTransaction = new Expense
                {
                    Title = title,
                    Amount = amount,
                    Date = date,
                    Category = cbCategory.Text
                };
            }
            else
            {
                NewTransaction = new Income
                {
                    Title = title,
                    Amount = amount,
                    Date = date,
                    Category = cbCategory.Text
                };
            }

            this.DialogResult = true;
            this.Close();
        }

        // Đảm bảo hàm này trùng khớp với Click="btnCancel_Click" bên XAML
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}