//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.DatabaseHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ConsoleHost.Models;
    using System.Web.Http;
    using System.Data.Entity;
    using System.Net.Http;
    using System.Net;
    public class TransactionDBHelper : ApiController
    {
        //CartDBContext _CartDb = new CartDBContext();  
        //ItemsDBContext _ItemDb = new ItemsDBContext();
        TransactionDBContext _TsDB = new TransactionDBContext();
        public IEnumerable<Transaction> GetTransactions()
        {
            return _TsDB.Transactions;
        }

        public decimal GetTransactionCount() 
        {
            return _TsDB.Transactions.Count();
        }

        public async Task<Transaction> GetTransaction(int id)
        {
            var ts = await _TsDB.Transactions.FirstOrDefaultAsync(c => c.Id == id);
            if (ts == null)
            {
                throw new HttpResponseException(
                    System.Net.HttpStatusCode.NotFound);
                //can go to the 404 page
            }
            return ts;
        }

        public async Task<int> PostTransaction(Transaction ts)
        {
            if (ts == null)
            {
                return -1;
            }
     
            Console.WriteLine("Add Transaction into cart");        
            ts = _TsDB.Transactions.Add(ts);
            await _TsDB.SaveChangesAsync();
            return ts.Id;
        }

        public async Task<IHttpActionResult> UpdateTransaction(Transaction ts)
        {
            if (ts == null)
            {
                return BadRequest("Argument Null");
            }
            var existing = await _TsDB.Transactions.FirstOrDefaultAsync(c => c.Id == ts.Id);

            if (existing == null)
            {
                return NotFound();
            }

            //existing.Name = cartitem.Name;
            await _TsDB.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> DeleteTransaction(int id)
        {
            var ts = await _TsDB.Transactions.FirstOrDefaultAsync(c => c.Id == id);
            if (ts == null)
            {
                return NotFound();
            }
            _TsDB.Transactions.Remove(ts);
            await _TsDB.SaveChangesAsync();
            return Ok();
        }   
    }
}
