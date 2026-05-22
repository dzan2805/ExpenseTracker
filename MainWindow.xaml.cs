using System;
using System.Collections.Generic;
using System.Windows;

namespace ExpenseTracker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadFakeData();
        }

        private void LoadFakeData()
        {
            var transactions = new List<dynamic>
            {
                new
                {
                    Title = "Coffee",
                    Amount = "45,000 VND",
                    Type = "Expense",
                    Category = "Food",
                    Date = DateTime.Now.ToString("dd/MM/yyyy")
                },

                new
                {
                    Title = "Salary",
                    Amount = "15,000,000 VND",
                    Type = "Income",
                    Category = "Bills",
                    Date = DateTime.Now.ToString("dd/MM/yyyy")
                },

                new
                {
                    Title = "Movie Ticket",
                    Amount = "120,000 VND",
                    Type = "Expense",
                    Category = "Entertainment",
                    Date = DateTime.Now.ToString("dd/MM/yyyy")
                },

                new
                {
                    Title = "Bus Ticket",
                    Amount = "10,000 VND",
                    Type = "Expense",
                    Category = "Transport",
                    Date = DateTime.Now.ToString("dd/MM/yyyy")
                },

                new
                {
                    Title = "Freelance Work",
                    Amount = "3,000,000 VND",
                    Type = "Income",
                    Category = "Shopping",
                    Date = DateTime.Now.ToString("dd/MM/yyyy")
                }
            };

            transactionGrid.ItemsSource = transactions;
        }
    }
}