﻿using ETICARET.Context;
using ETICARET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ETICARET.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult _FooterCtg()
        {
            return PartialView(db.Categories.ToList());
        }

        private DataContext db = new DataContext();
        // GET: Home
        public ActionResult Index()
        {
            var urunler = db.Products
                .Where(i => i.IsApproved && i.IsHome)
                .Select(i => new ProductModel()
                {
                    Id = i.Id,
                    Name = i.Name.Length>50 ? i.Name.Substring(0,50)+"...":i.Name,
                    Description = i.Description.Length > 80 ? i.Description.Substring(0, 80) + "..." : i.Description,
                    Price=i.Price,
                    Stock=i.Stock,
                    Image=i.Image,
                    CategoryId=i.CategoryId
                }).OrderByDescending(i=> i.Id).ToList();

            return View(urunler);
        }

        public ActionResult Details(int id)
        {
            return View(db.Products.Find(id));
        }

        public ActionResult List(int? id) // ? nullable
        {
            var urunler = db.Products
               .Where(i => i.IsApproved)
               .Select(i => new ProductModel()
               {
                   Id = i.Id,
                   Name = i.Name.Length > 50 ? i.Name.Substring(0, 50) + "..." : i.Name,
                   Description = i.Description.Length > 80 ? i.Description.Substring(0, 80) + "..." : i.Description,
                   Price = i.Price,
                   Stock = i.Stock,
                   Image = i.Image,
                   CategoryId = i.CategoryId
               }).AsQueryable();

            if (id != null)
            {
                urunler = urunler.Where(i => i.CategoryId == id);
            }

            return View(urunler.ToList());
             

        }

        public PartialViewResult GetCategories()
        {
            return PartialView(db.Categories.ToList());
        }
    }
}