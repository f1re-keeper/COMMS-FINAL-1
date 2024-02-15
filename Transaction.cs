using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Transaction
    {
        public string trasnactionDate {  get; set; }
        public string transactionType { get; set; }
        public int amount { get; set; }

        public Transaction(string trasnactionDate, string transactionType, int amount)
        {
            this.trasnactionDate = trasnactionDate;
            this.transactionType = transactionType;
            this.amount = amount;
        }
    }
}
