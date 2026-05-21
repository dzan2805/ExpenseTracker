using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class Expense : Transaction
    {
        public string Category { get; set; }

        public override string GetTransactionType()
        {
            return "Expense";
        }
    }
}
