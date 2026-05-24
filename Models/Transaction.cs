using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Income), "income")]
    [JsonDerivedType(typeof(Expense), "expense")]
    public abstract class Transaction
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string Category { get; set; } = "";

        public string Type => GetTransactionType();

        public abstract string GetTransactionType();
    }
}