using ETICARET.Context;
using ETICARET.Entity;
using ETICARET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETICARET.Controllers
{
    public class CartController : Controller
    {
        private DataContext db = new DataContext();
        public ActionResult Index()
        {
            return View(GetCart());
        }
        public ActionResult _FooterCtg()
        {
            return PartialView(db.Categories.ToList());
        }
        public ActionResult AddToCart(int Id,int? quantity)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);

            if (product != null)
            {
                if (quantity == null)
                {
                    GetCart().AddProduct(product, 1);
                }
                else
                {
                    GetCart().AddProduct(product, quantity.Value);
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);

            if (product != null)
            {
                GetCart().DeleteProduct(product);
            }

            return RedirectToAction("Index");
        }

        public Cart GetCart()
        {
            var cart = (Cart)Session["Cart"];

            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public PartialViewResult Summary()
        {
            return PartialView(GetCart());
        }
        public ActionResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ActionResult Checkout(ShippingDetails entity)
        {
            var cart = GetCart();

            if (cart.CartLines.Count == 0)
            {
                ModelState.AddModelError("UunYokError", "Sepetinizde ürün bulunmamaktadır.");
            }

            if (ModelState.IsValid)
            {
                SaveOrder(cart,entity);
                cart.Clear();
                return View("Completed");
            }

            return View(entity);
        }

        public void SaveOrder(Cart cart,ShippingDetails entity)
        {
            var order = new Order();

            order.OrderDate = DateTime.Now;
            order.OrderNumber = "A" + (new Random()).Next(11111, 99999).ToString();
            order.Total = cart.Total();
            order.OrderState = EnumOrderState.Waiting;
            order.Username = User.Identity.Name;

            order.AdresBasligi = entity.AdresBasligi;
            order.Adres = entity.Adres;
            order.Sehir = entity.Sehir;
            order.Semt = entity.Semt;
            order.Mahalle = entity.Mahalle;
            order.PostaKodu = entity.PostaKodu;

            order.orderItems = new List<OrderItem>();

            foreach (var pr in cart.CartLines)
            {
                var orderItem = new OrderItem();
                orderItem.Quantity = pr.Quantity;
                orderItem.Price = pr.Product.Price*pr.Quantity;
                orderItem.ProductId = pr.Product.Id;

                order.orderItems.Add(orderItem);
            }
            db.Orders.Add(order);
            db.SaveChanges();

        }
    }
}