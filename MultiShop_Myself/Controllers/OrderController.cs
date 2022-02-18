using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultiShop_Myself.Models;
using System.Net;
using System.Net.Mail;

namespace MultiShop.Controllers
{
    public class OrderController : Controller
    {
        MultiShop2Entities db = new MultiShop2Entities();
        //
        // GET: /Order/
        public ActionResult Checkout()
        {
            var model = new Order();
            model.CustomerId = User.Identity.Name;
            model.OrderDate = DateTime.Now.Date;
            model.Amount = ShoppingCart.Cart.Total;

            return View(model);
        }

        public ActionResult Purchase(Order model)
        {
            try
            {
                db.Orders.Add(model);

                var cart = ShoppingCart.Cart;
                foreach (var p in cart.Items)
                {
                    var d = new OrderDetail
                    {
                        Order = model,
                        ProductId = p.Id,
                        UnitPrice = p.UnitPrice,
                        Discount = p.Discount,
                        Quantity = p.Quantity
                    };

                    db.OrderDetails.Add(d);
                }
                db.SaveChanges();
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            

            return RedirectToAction("Detail", new { id = model.Id });
        }

        public ActionResult Detail(int id)
        {
            var order = db.Orders.Find(id);
            return View(order);
        }

        public ActionResult List()
        {
            var orders = db.Orders
                .Where(o => o.CustomerId == User.Identity.Name);
            return View(orders);
        }
    }
}