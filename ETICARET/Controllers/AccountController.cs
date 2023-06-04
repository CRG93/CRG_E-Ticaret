using ETICARET.Context;
using ETICARET.Identity;
using ETICARET.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETICARET.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;
        private DataContext db = new DataContext();
        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            userManager = new UserManager<ApplicationUser>(userStore);

            var roleStore = new RoleStore<ApplicationRole>(new IdentityDataContext());
            roleManager = new RoleManager<ApplicationRole>(roleStore);
        }
        public ActionResult _FooterCtg()
        {
            return PartialView(db.Categories.ToList());
        }
        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;

            var orders = db.Orders
                .Where(i => i.Username == username)
                .Select(i => new UserOrderModel()
                {
                    Id = i.Id,
                    OrderNumber = i.OrderNumber,
                    OrderDate = i.OrderDate,
                    OrderState = i.OrderState,
                    Total = i.Total
                }).OrderByDescending(i => i.OrderDate).ToList();

            return View(orders);
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            var entity = db.Orders
               .Where(i => i.Id == id)
               .Select(i => new OrderDetaisModel()
               {
                   OrderId = i.Id,
                   OrderNumber = i.OrderNumber,
                   Total = i.Total,
                   OrderDate = i.OrderDate,
                   OrderState = i.OrderState,
                   AdresBasligi = i.AdresBasligi,
                   Adres = i.Adres,
                   Sehir = i.Sehir,
                   Semt = i.Semt,
                   Mahalle = i.Mahalle,
                   PostaKodu = i.PostaKodu,
                   orderItems = i.orderItems.Select(a => new OrderItemModel()
                   {
                       ProductId = a.ProductId,
                       ProductName = a.Product.Name.Length > 50 ? a.Product.Name.Substring(0, 47)+"..." : a.Product.Name,
                       Image = a.Product.Image,
                       Quantity = a.Quantity,
                       Price = a.Price
                   }).ToList()
               }).FirstOrDefault();
            return View(entity);
        }

        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                //Kayıt İşlemleri
                var user = new ApplicationUser();
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.UserName = model.Username;
                user.Email = model.Email;

                var result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    //Kullanıcı oluştu ve kullanıcıyı bir role atayabiliriz.
                    if (roleManager.RoleExists("user"))
                    {
                        userManager.AddToRole(user.Id, "user");
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                   ModelState.AddModelError("RegisterUserError", "Kullanıcı oluşturulamadı.") ;
                }

            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model,string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                // Login işlemleri
                var user = userManager.Find(model.Username, model.Password);

                if (user != null)
                {
                    // varolan kullanıcıyı sisteme dahil et.
                    // ApplicationCookie oluşturup sisteme bırak.

                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identityclaims = userManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = model.RememberMe; // beni hatırlanın cevabına göre tarayıcıda tutulacak.
                    authManager.SignIn(authProperties, identityclaims);

                    if (!string.IsNullOrEmpty(ReturnUrl)) // string ad = NULL, string ad="";
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("LoginUserError", "Böyle bir kullanıcı yok");
                }

            }
            return View(model);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}