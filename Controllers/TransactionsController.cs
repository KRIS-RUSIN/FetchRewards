using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using FetchRewards.Repository;
using FetchRewards.Models;

namespace FetchRewards.Controllers
{
    [Route("transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private ITransactionRepo repository;

        // initialize repository
        public TransactionsController(ITransactionRepo repository)
        {
            this.repository = repository;
        }

        // GET /transactions: returns all transactions
        [HttpGet]
        public IEnumerable<Transaction> GetTransactions()
        {
            return repository.GetTransactions();
        }

        // GET /transactions/balance: returns summed up balances of each payer
        [HttpGet("balance")]
        public IEnumerable<Balance> GetBalance()
        {
            return repository.GetBalance();
        }

        // POST /transactions: Add transaction by passing Transaction into request body 
        [HttpPost]
        public ActionResult<IEnumerable<Transaction>> AddTransaction(Transaction transaction)
        {
            if (!repository.TotalSpend(new Points { points = transaction.Points}, transaction.Payer))
            {
                return Content("Transaction not possible. Balance cannot be below 0.");
            }
            Transaction t = new Transaction
            {
                Payer = transaction.Payer,
                Points = transaction.Points,
                Timestamp = transaction.Timestamp
            };
            repository.AddTransaction(t);
            return repository.GetTransactions().ToList();
        }
        
        // POST /transactions/spend: Spends points from oldest transaction first
        [HttpPost("spend")]
        public ActionResult<IEnumerable<Balance>> SpendPoints(Points points)
        {
            if(!repository.TotalSpend(points))
            {
                return Content("Trying to spend too many points. Balance cannot be below 0.");
            }
            return repository.SpendPoints(points).ToList();
        }
    }
}
