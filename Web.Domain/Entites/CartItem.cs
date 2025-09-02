﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class CartItem
    {
        public int CartId { get; set; }
        public int ProductId {  get; set; }
        public int Quantity {  get; set; }

        public Cart Cart { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
