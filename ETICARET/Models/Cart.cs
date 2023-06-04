using ETICARET.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETICARET.Models
{
    public class Cart
    {
        public List<CartItem> CartLines { get { return _cartLines; } }

        private List<CartItem> _cartLines = new List<CartItem>();     
        public void AddProduct(Product product, int quantity)
        {
            var line = _cartLines.FirstOrDefault(i => i.Product.Id == product.Id);

            if (line == null)
            {
                _cartLines.Add(new CartItem() { Product = product,Quantity=quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void DeleteProduct(Product product)
        {
            _cartLines.RemoveAll(i => i.Product.Id == product.Id);
        }

        public double Total()
        {
            return _cartLines.Sum(i => i.Product.Price * i.Quantity);
        }

        public void Clear()
        {
            _cartLines.Clear();
        }
    }


    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}