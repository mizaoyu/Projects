//-----------------------------------------------------------------------
// <copyright company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConsoleHost.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }

        public string Name { get; set; }

        //public string Description { get; set; }

        public string Picture { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
