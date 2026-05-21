using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Models
{
    public class Income : Transaction
    {
        public override string GetTransactionType()
        {
            return "Income";
        }
    }
}
