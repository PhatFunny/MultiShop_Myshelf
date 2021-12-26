using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultiShop_Myself.Models;

namespace MultiShop_Myself.Controllers
{
    public class ProductController : Controller
    {
        MultiShopDbContext db = new MultiShopDbContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(int CategoryID = 0)
        {
            if (CategoryID != 0)
            {
                var model = db.Products.Where(p => p.CategoryId == CategoryID);
                return View(model);
            }
            return View();
        }

        public ActionResult Detail(int id)
        {
            var model = db.Products.Find(id);

            // Tăng số lần xem
            model.Views++;
            db.SaveChanges();

            // Lấy cookie cũ tên views
            var views = Request.Cookies["views"];
            // Nếu chưa có cookie cũ -> tạo mới
            if (views == null)
            {
                views = new HttpCookie("views");
            }
            // Bổ sung mặt hàng đã xem vào cookie
            views.Values[id.ToString()] = id.ToString();
            // Đặt thời hạn tồn tại của cookie
            views.Expires = DateTime.Now.AddHours(24);
            // Gửi cookie về client để lưu lại
            Response.Cookies.Add(views);

            // Lấy List<int> chứa mã hàng đã xem từ cookie
            var keys = views.Values
                .AllKeys.Select(k => int.Parse(k)).ToList();
            // Truy vấn háng đãn xem
            ViewBag.Views = db.Products
                .Where(p => keys.Contains(p.Id));

            //Lấy top 10 sản phẩm khách hàng hay xem
            ViewBag.Top = db.Products.Where(p => p.Id > 0).OrderByDescending(p => p.Views).Take(10).ToList();

            return View(model);
        }

        //Tìm kiếm sản phẩm
        public ActionResult Search(String keywords = "")
        {
            if (keywords != "")
            {
                var model = db.Products
                    .Where(p => p.Name.Contains(keywords));
                return View(model);
            }
            return View(db.Products);
        }
        
    }
}