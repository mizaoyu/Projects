//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.Service.Controllers
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

    public class ItemsController : ApiController
    {
        private DBHelper helper = new DBHelper();

        public IEnumerable<Item> Get()
        {
            return helper.GetItems();
        }

        public async Task<Item> Get(int id)
        {
            var item = await helper.GetItem(id);

            if (item == null)
            {
                //can go to the 404 page
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            return item;
        }

        public async Task<IHttpActionResult> Post(Item item)
        {
            if (item == null)
            {
                return BadRequest("Argument Null");
            }

            var existing = await helper.GetItem(item.Id);

            if (existing != null)
            {
                return BadRequest("Exists");
            }

            await helper.PostItemToItems(item);

            return Ok();
        }

        public async Task<IHttpActionResult> Put(Item item)
        {
            if (item == null)
            {
                return BadRequest("Argument Null");
            }

            var existing = await helper.UpdateItemToItems(item);

            if (existing == null)
            {
                return NotFound();
            }

            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var existing = await helper.DeleteItemFromItems(id);

            if (existing == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
