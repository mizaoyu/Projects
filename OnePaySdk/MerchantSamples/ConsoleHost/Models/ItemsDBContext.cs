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

    public class ItemsDBContext : DbContext
    {
        public ItemsDBContext() : base("ItemsDB")
        {
        }

        public IDbSet<Item> Items { get; set; }
    }

    public class ItemsDBInitializer : DropCreateDatabaseAlways<ItemsDBContext>
    {
        protected override void Seed(ItemsDBContext context)
        {
            base.Seed(context);
            context.Items.Add(new Item { Id = 1, Name = "Woman's T - Shirt A", Picture = "images/p1.jpg", Price = 129, Discount = 0 });
            context.Items.Add(new Item { Id = 2, Name = "Woman's T - Shirt B", Picture = "images/p2.jpg", Price = 89, Discount = 0 });
            context.Items.Add(new Item { Id = 3, Name = "Woman's T - Shirt C", Picture = "images/p3.jpg", Price = 149, Discount = 0 });
            context.Items.Add(new Item { Id = 4, Name = "Woman's T - Shirt D", Picture = "images/p4.jpg", Price = 40, Discount = 8 });
            context.Items.Add(new Item { Id = 5, Name = "Woman's T - Shirt E", Picture = "images/p5.jpg", Price = 86, Discount = 17.2M });
            context.Items.Add(new Item { Id = 6, Name = "Woman's T - Shirt F", Picture = "images/p6.jpg", Price = 149, Discount = 29.8M });
            context.Items.Add(new Item { Id = 7, Name = "Beautiful Shoes", Picture = "images/p7.jpg", Price = 36, Discount = 7.2M });
            context.Items.Add(new Item { Id = 8, Name = "Leather Boots", Picture = "images/p8.jpg", Price = 86, Discount = 17.2M });
            context.Items.Add(new Item { Id = 9, Name = "Canvas Shoes", Picture = "images/p9.jpg", Price = 60, Discount = 12 });
            context.Items.Add(new Item { Id = 10, Name = "Skateboard Shoes", Picture = "images/p10.jpg", Price = 129, Discount = 25.8M });
        }
    }
}
