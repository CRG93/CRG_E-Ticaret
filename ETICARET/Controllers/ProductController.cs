using ETICARET.Context;
using ETICARET.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETICARET.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private DataContext db = new DataContext();
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }
        public ActionResult _FooterCtg()
        {
            return PartialView(db.Categories.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                //var product = db.Products.Where(i=> i.Id==id).Include("Category").FirstOrDefault();
                var product = db.Products.Find(id);

                if (product == null)
                {
                    return HttpNotFound();
                }

                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
                return View(product);
            }
            else
            {
                return View("ErrosPage");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                //var model = db.Products.Find(product.Id);
                //model.Image = product.Image;
                //model.IsHome = product.IsHome;
                //model.Stock = product.Stock;
                //model.Price = product.Price;
                //model.Name = product.Name;
                //model.CategoryId = product.CategoryId;
                //model.Description = product.Description;
                //model.IsApproved = product.IsApproved;

                //db.SaveChanges();

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View(product);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("ErrorPage");
            }

            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var product = db.Products.Find(Id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}