using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Models;

namespace ExpenseTracker.Interfaces
{
    public interface IDataService
    {
        void Save(List<Transaction> transactions);
        List<Transaction> Load();
    }
}
