using FetchRewards.Models;
using System.Collections.Generic;

namespace FetchRewards.Repository
{
    // Interface to keep Repository until program ends
    public interface ITransactionRepo
    {
        void AddTransaction(Transaction transaction);
        IEnumerable<Balance> GetBalance();
        IEnumerable<Transaction> GetTransactions();
        IEnumerable<Balance> SpendPoints(Points points);
        public bool TotalSpend(Points p, string payer = "");
    }
}