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

    public class CartController : ApiController
    {
        private DBHelper helper = new DBHelper();

        public IEnumerable<CartItem> Get()
        {
            return helper.GetCartItems();
        }

        [HttpGet]
        public decimal GetCount()
        {
            return helper.GetCartCount();
        }

        public async Task<CartItem> Get(int id)
        {
            var cartitem = await helper.GetCartItem(id);

            if (cartitem == null)
            {
                throw new HttpResponseException(
                    System.Net.HttpStatusCode.NotFound);
                //can go to the 404 page
            }

            return cartitem;
        }

        public async Task<IHttpActionResult> Post(CartItem cartitem)
        {
            if (cartitem == null)
            {
                return BadRequest("Argument Null");
            }

            var response = await helper.PostItemToCart(cartitem);
            if (response == null)
            {
                throw new HttpResponseException(
                    System.Net.HttpStatusCode.NotFound);
                //can go to the 404 page
            }

            return Ok();
        }

        public async Task<IHttpActionResult> Put(CartItem cartitem)
        {
            if (cartitem == null)
            {
                return BadRequest("Argument Null");
            }

            var response = await helper.UpdateItemToCart(cartitem);
            if (response == null)
            {
                return NotFound();
            }

            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var response = await helper.DeleteItemFromCart(id);
            if (response == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
