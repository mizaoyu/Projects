//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using ConsoleHost.Models;
    public class TransactionDBContext : DbContext
    {
        public TransactionDBContext()
            : base("TransactionDB")
        {
        }

        public IDbSet<Transaction> Transactions { get; set; }
    }

    public class TransactionDBInitializer : DropCreateDatabaseAlways<TransactionDBContext>
    {
        protected override void Seed(TransactionDBContext context)
        {
            base.Seed(context);
            //context.Cart.Add(new CartItem { Id = 1, ItemId=1, Name = "Woman's T - Shirt A", Picture = "images/p1.jpg", Price = 129, Discount = 0, Quantity = 1});
            //context.Cart.Add(new CartItem { Id = 2, ItemId = 1, Name = "Woman's T - Shirt A", Picture = "images/p1.jpg", Price = 129, Discount = 2, Quantity = 1 });
            //context.Cart.Add(new CartItem { Id = 3, ItemId = 1, Name = "Woman's T - Shirt A", Picture = "images/p1.jpg", Price = 129, Discount = 5, Quantity = 1 });
        }
    }
}
