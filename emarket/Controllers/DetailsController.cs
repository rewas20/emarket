using emarket.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace emarket.Controllers
{

    public class DetailsController : Controller
    {
        private StoreEntities db = new StoreEntities();
        // GET: Details
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                ViewBag.Product = product;
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        //<------------------------Update Product---------------------->//
        //HttpGet for Update:-----------------
        [HttpGet]
        public ActionResult Update(int id)
        {
            var product = db.Products.SingleOrDefault(m => m.id == id);
            if (product != null)
            {

                ViewBag.Product = product;
                ViewBag.Category = db.Categories.ToList();
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }
        //HttpPost for Update:-------------
        [HttpPost]
        public object Update(Product product, HttpPostedFileBase up)
        {
            var update = db.Products.SingleOrDefault(p => p.id == product.id);
            if (ModelState.IsValid)
            {

                update.name = product.name;
                update.price = product.price;
                update.description = product.description;
                update.category_id = product.category_id;
                if (up != null)
                {
                    String path = Path.Combine(Server.MapPath("~/Uploads"), up.FileName);
                    up.SaveAs(path);
                    update.image = up.FileName;

                }
                
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
               
                return RedirectToAction("Details/" + product.id);
            }
            else
            {
                return HttpNotFound();
            }
            
            

        }



        //<----------------------Delect Product------------------>//
        //HttpGet for Delete:-----------
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            
           
            if (product == null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
               
                ViewBag.Product = product;
                return View();
            }
        }
        //HttpPost for Delete:--------------
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(Product product)
        {
            var remove = db.Products.Find(product.id);

            if (remove == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var category = db.Categories.SingleOrDefault(c => c.id == remove.category_id);
                    if (category.number_of_products > 0)
                    {
                        category.number_of_products--;

                        db.Entry(category).State = EntityState.Modified;
                    }


                    db.Products.Remove(remove);

                    db.SaveChanges();
                }
            }
            return RedirectToAction("Delete");
        }
        //=======================



    }
}