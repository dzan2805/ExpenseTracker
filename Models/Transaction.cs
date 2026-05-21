using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public abstract class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public abstract string GetTransactionType();
    }
}