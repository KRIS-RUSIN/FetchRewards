using System;
using System.Collections.Generic;
using System.Linq;
using FetchRewards.Models;

namespace FetchRewards.Repository
{
    public class TransactionRepo : ITransactionRepo
    {
        // Memory
        private List<Transaction> transactions { get; set; }

        // initialize
        public TransactionRepo()
        {
            transactions = new List<Transaction>();
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return transactions;
        }

        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
        }

        public IEnumerable<Balance> GetBalance()
        {
            return transactions
                .GroupBy(_ => _.Payer, _ => new { Points = _.Points })
                .Select(p => new Balance
                {
                    Payer = p.Key,
                    Points = p.Sum(f => f.Points)
                });
        }

        // Used to see if there are enough points to spend/keep payers from going negative
        public bool TotalSpend(Points p, string payer = "")
        {
            if(payer.Length > 0)
            {
                if(p.points < 0)
                {
                    int total = transactions.Where(x => x.Payer == payer).Sum(p => p.Points);
                    return total + p.points >= 0;
                }
                return true;
            }
            else
            {
                int total = transactions.Sum(p => p.Points);
                return total >= p.points;
            }
        }

        public IEnumerable<Balance> SpendPoints(Points p)
        {
            int points = p.points;
            var sortedTransactions = (from transaction in transactions
                                     group transaction by (transaction.Payer, transaction.Timestamp.Date) into t
                                     select new Transaction
                                     {
                                         Payer = t.Key.Payer,
                                         Timestamp = t.Key.Date,
                                         Points = t.Sum(f => f.Points)
                                     }).OrderBy(x => x.Timestamp).ToList();

            List<Balance> pointsDeducted = new List<Balance>();
            int i = 0;
            while (points != 0 && i < sortedTransactions.Count())
            {
                if(sortedTransactions[i].Points >= points)
                {
                    pointsDeducted.Add(new Balance { Payer = sortedTransactions[i].Payer, Points = -points });
                    AddTransaction(new Transaction { Payer = sortedTransactions[i].Payer, Points = -points, Timestamp = DateTime.UtcNow });
                    points = 0;
                    sortedTransactions[i].Points = sortedTransactions[i].Points - points;
                }
                else if(sortedTransactions[i].Points < points && sortedTransactions[i].Points > 0)
                {
                    pointsDeducted.Add(new Balance { Payer = sortedTransactions[i].Payer, Points = -sortedTransactions[i].Points });
                    AddTransaction(new Transaction { Payer = sortedTransactions[i].Payer, Points = -sortedTransactions[i].Points, Timestamp = DateTime.UtcNow });
                    points = points - sortedTransactions[i].Points;
                    sortedTransactions[i].Points = 0;
                }
                i++;
            }
            return pointsDeducted;
        }
    }
}
