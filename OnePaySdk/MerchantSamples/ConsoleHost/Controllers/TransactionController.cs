//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ConsoleHost.Models;
    using ConsoleHost.DatabaseHelper;
    using System.Web.Http;
    using System.Data.Entity;
    public class TransactionController : ApiController
    {
        TransactionDBHelper helper = new TransactionDBHelper();
        public IEnumerable<Transaction> Get()
        {
            return helper.GetTransactions();
        }

        [HttpGet]
        public decimal GetCount()
        {
            return helper.GetTransactionCount();
        }

        public Task<Transaction> Get(int id)
        {
            return helper.GetTransaction(id);
        }


        public async Task<IHttpActionResult> Post(Transaction Ts)
        {
            var response = await helper.PostTransaction(Ts);
            if (response == -1) return BadRequest("Argument Null");
            else return Ok(response);
        }


        public Task<IHttpActionResult> Put(Transaction Ts)
        {
            return helper.UpdateTransaction(Ts);
        }


        public Task<IHttpActionResult> Delete(int id)
        {
            return helper.DeleteTransaction(id);
        }
    }
}
