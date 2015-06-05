//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.DatabaseHelper
{
    using ConsoleHost.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class DBHelper
    {
        private CartDBContext _CartDb = new CartDBContext();

        private ItemsDBContext _ItemDb = new ItemsDBContext();

        public IEnumerable<CartItem> GetCartItems()
        {
            return _CartDb.Cart;
        }

        public decimal GetCartCount() 
        {
            return _CartDb.Cart.Count();
        }

        public async Task<CartItem> GetCartItem(int id)
        {
            return await _CartDb.Cart.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CartItem> PostItemToCart(CartItem cartitem)
        {
            //var itemExists = await _CartDb.Cart.AnyAsync(c => c.Id == item.Id);
            var itemExists = await _CartDb.Cart.FirstOrDefaultAsync(c => (c.ItemId == cartitem.ItemId && c.Color == cartitem.Color && c.Size == cartitem.Size));
            //var itemExists = await _CartDb.Cart.FirstOrDefaultAsync(c => (c.Id == item.Id && c.Color==item.Color && c.Size==item.Size));
            if (itemExists != null)
            {
                // Item already in Cart
                Console.WriteLine("Item {0} already exist!", cartitem.Id);
                itemExists.Quantity = itemExists.Quantity + cartitem.Quantity;
                itemExists.TotalPrice = (itemExists.Price - itemExists.Discount) * itemExists.Quantity;
                await _CartDb.SaveChangesAsync();

                return itemExists;
            }
            else
            {
                Console.WriteLine("Item {0} not exist, add it into cart", cartitem.ItemId);   
                var item2 = await _ItemDb.Items.FirstOrDefaultAsync(c => c.Id == cartitem.ItemId);
                if (item2 == null)
                {
                    return null;
                }
                else
                {
                    cartitem.Name = item2.Name;
                    cartitem.Picture = item2.Picture;
                    cartitem.Price = item2.Price;
                    cartitem.Discount = item2.Discount;
                    cartitem.TotalPrice = (cartitem.Price - cartitem.Discount) * cartitem.Quantity;
                    _CartDb.Cart.Add(cartitem);
                    await _CartDb.SaveChangesAsync();

                    return cartitem;
                }
            }
        }

        public async Task<CartItem> UpdateItemToCart(CartItem cartitem)
        {
            var existing = await _CartDb.Cart.FirstOrDefaultAsync(c => c.Id == cartitem.Id);

            if (existing == null)
            {
                return null;
            }

            existing.Name = cartitem.Name;
            await _CartDb.SaveChangesAsync();
            return existing;
        }

        public async Task<CartItem> DeleteItemFromCart(int id)
        {
            var cartitem = await _CartDb.Cart.FirstOrDefaultAsync(c => c.Id == id);
            if (cartitem == null)
            {
                return null;
            }

            _CartDb.Cart.Remove(cartitem);
            await _CartDb.SaveChangesAsync();
            return cartitem;
        }

        public IEnumerable<Item> GetItems()
        {
            return _ItemDb.Items;
        }

        public async Task<Item> GetItem(int id)
        {
            Console.WriteLine("Get Item {0}", id);
            return await _ItemDb.Items.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Item> PostItemToItems(Item item)
        {
            _ItemDb.Items.Add(item);
            await _ItemDb.SaveChangesAsync();

            return item;
        }

        public async Task<Item> UpdateItemToItems(Item item)
        {
            var existing = await _ItemDb.Items.FirstOrDefaultAsync(c => c.Id == item.Id);

            if (existing == null)
            {
                return null;
            }

            existing.Name = item.Name;
            await _ItemDb.SaveChangesAsync();

            return existing;
        }

        public async Task<Item> DeleteItemFromItems(int id)
        {
            var item = await _ItemDb.Items.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                return null;
            }

            _ItemDb.Items.Remove(item);
            await _ItemDb.SaveChangesAsync();

            return item;
        }
    }
}
