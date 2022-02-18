using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MultiShop_Myself.Models;
using PagedList;
using System.IO;

namespace MultiShop_Myself.AdminControllers
{
    public class ProductsController : Controller
    {
        private MultiShop2Entities db = new MultiShop2Entities();

        // GET: Products
        public ActionResult Index(string sortOrder, int?page)
        {
            ViewBag.SortByName = String.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SortByPrice = (sortOrder == "dongia" ? "dongia_desc" : "dongia");
            ViewBag.CurrentSort = sortOrder;
            var products = db.Products.Include(p => p.Category).Include(p => p.Supplier);
            switch(sortOrder)
            {
                case "ten_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "dongia_desc":
                    products = products.OrderByDescending(s => s.UnitPrice);
                    break;
                case "dongia":
                    products = products.OrderBy(s => s.UnitPrice);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,UnitBrief,UnitPrice,Image,Available,Description,CategoryId,SupplierId,Quantity,Discount,Special,Latest,Views")] Product product,
            HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    string filename = Path.GetFileName(Image.FileName);
                    string path = Server.MapPath("~/Content/img/products/" + filename);
                    product.Image = filename;
                    Image.SaveAs(path);
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN", product.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN", product.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UnitBrief,UnitPrice,Image,Available,Description,CategoryId,SupplierId,Quantity,Discount,Special,Latest,Views")] Product product,
            HttpPostedFile HinhUpload, string Image)
        {
            if (ModelState.IsValid)
            {
                if (HinhUpload != null && HinhUpload.ContentLength > 0)
                {
                    string filename = Path.GetFileName(HinhUpload.FileName);
                    string path = Server.MapPath("~/Content/img/products/" + filename);
                    product.Image = filename;
                    HinhUpload.SaveAs(path);
                }
                else
                {
                    product.Image = Image;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "NameVN", product.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            //Xóa file hình
            System.IO.File.Delete(Server.MapPath("~/" + product.Image));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
