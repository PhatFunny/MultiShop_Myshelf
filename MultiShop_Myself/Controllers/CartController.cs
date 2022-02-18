using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultiShop_Myself.Models;
using System.Net;
using System.Net.Mail;

namespace MultiShop_Myself.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var cart = ShoppingCart.Cart;
            return View(cart.Items);
        }

        public ActionResult Add(int id)
        {
            var cart = ShoppingCart.Cart;
            cart.Add(id);

            var info = new { cart.Count, cart.Total };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int id)
        {
            var cart = ShoppingCart.Cart;
            cart.Remove(id);

            var info = new { cart.Count, cart.Total };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int id, int quantity)
        {
            var cart = ShoppingCart.Cart;
            cart.Update(id, quantity);

            var p = cart.Items.Single(i => i.Id == id);
            var info = new
            {
                cart.Count,
                cart.Total,
                Amount = Math.Round(p.UnitPrice * p.Quantity * (1 - p.Discount), 1)
            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clear()
        {
            var cart = ShoppingCart.Cart;
            cart.Clear();
            return RedirectToAction("Index");
        }

        public List<Product> Items = new List<Product>();

        public RedirectToRouteResult DelCartItem(int id)
        {
            //Tìm cart muốn xóa
            var cart = ShoppingCart.Cart;
            cart.Remove(id);
            return RedirectToAction("Index");
        }

        public ActionResult mail(String Email)
        {
            var giohang = ShoppingCart.Cart;
            string sMsg = "<html>" +
                           "<body>" +
                            "<table border='1'>" +
                            "<caption>Thông tin đặt hàng</caption>" +
                                "<tr>" +
                                    "<th>STT</th>" +
                                    "<th>Tên hàng</th>" +
                                    "<th>Số lượng</th>" +
                                    "<th>Đơn giá</th>" +
                                    "<th>Thành tiền</th>" +
                                "</tr>";
            int i = 0;
            double tongtien = 0;
            foreach(Product p in giohang.Items)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + p.Name.ToString() + "</td>";
                sMsg += "<td>" + p.Quantity.ToString() + "</td>";
                sMsg += "<td>" + p.UnitPrice.ToString() + "</td>";
                sMsg += "<td>" + String.Format("{0:#,###}", p.Quantity * p.UnitPrice * (1 - p.Discount)) + "</td>";
                sMsg += "</tr>";
                tongtien += Math.Round(p.UnitPrice * p.Quantity * (1 - p.Discount), 1);
            }
            sMsg += "<tr><th colspan='5'>Tổng cộng: "
                + String.Format("{0:#,### $}", tongtien) + "</th></tr></table>";
            MailMessage mail = new MailMessage("1851010096phat@ou.edu.vn", Email, "Thông tin đơn hàng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("1851010096phat@ou.edu.vn", "272732580");
            mail.IsBodyHtml = true;
            client.Send(mail);
            return RedirectToAction("Index", "Home");
            //Gửi mail cho khách hàng
        }
    }
}