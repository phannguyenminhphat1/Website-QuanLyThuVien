using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebNangCao.Models;
using WebNangCao.SoMuchModel;

namespace WebNangCao.Controllers
{
    public class TblBooksController : Controller
    {
        private BookEntity bookDb = new BookEntity();

        // GET: tblBooks
        public ActionResult Index()
        {
            return View(bookDb.tblBooks.ToList());
        }
        public ActionResult GetAll()
        {
            var booklist = bookDb.tblBooks.ToList();
            return Json(new { data = booklist }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: tblBooks/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "BookId,BookTitle,BookCategory,BookAuthor,BookCopies,Copyright,DateAdded,Statuss,Descrip,Detail,Images1,Images2,Images3,Images")] tblBook tblBook, HttpPostedFileBase Images)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileName(Images.FileName);
                string _filename = DateTime.Now.ToString("yymmssff") + filename;
                string extension = Path.GetExtension(Images.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Images/"), _filename);
                tblBook.Images =_filename;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                {
                    if (Images.ContentLength <= 100000000)
                    {
                        Session["operationMsg"] = "Book added successfully";
                        bookDb.tblBooks.Add(tblBook);
                        if (bookDb.SaveChanges() > 0)
                        {
                            Images.SaveAs(path);
                        }

                    }
                    else
                    {
                        ViewBag.Mess = "Không đúng định dạng";
                        return View();
                    }
                    return RedirectToAction("Index");

                }
            }
            
            return View(tblBook);


            //if (ModelState.IsValid)
            //{
            //    if (Images.ContentLength > 0)
            //    {
            //        var fileName = Path.GetFileName(Images.FileName);
            //        var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
            //        if (System.IO.File.Exists(path))
            //        {
            //            ViewBag.upload = "Hình ảnh này đã tồn tại";
            //            return View();

            //        }
            //        else
            //        {
            //            Images.SaveAs(path);
            //            tblBook.Images = fileName;
            //        }
            //    }
            //    Session["operationMsg"] = "Book added successfully";
            //    bookDb.tblBooks.Add(tblBook);
            //    bookDb.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(tblBook);
            

            //if (ModelState.IsValid)
            //{

            //    Session["operationMsg"] = "Book added successfully";
            //    bookDb.tblBooks.Add(tblBook);
            //    bookDb.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(tblBook);
        }

        public ActionResult OperationAlert()
        {
            Session.Remove("operationMsg");
            return RedirectToAction("Index");

        }

        // GET: tblBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            Session["imgPath"] = tblBook.Images;
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "BookId,BookTitle,BookCategory,BookAuthor,BookCopies,Copyright,DateAdded,Statuss,Descrip,Detail,Images1,Images2,Images3,Images")] tblBook tblBook, HttpPostedFileBase Images)
        {
            if (ModelState.IsValid)
            {
                if (Images != null)
                {
                    string filename = Path.GetFileName(Images.FileName);
                    string _filename = DateTime.Now.ToString("yymmssff") + filename;
                    string extension = Path.GetExtension(Images.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), _filename);
                    tblBook.Images = _filename;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                    {
                        if (Images.ContentLength <= 100000000)
                        {
                            bookDb.Entry(tblBook).State = EntityState.Modified;
                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                            //bookDb.tblBooks.Add(tblBook);
                            if (bookDb.SaveChanges() > 0)
                            {
                                Images.SaveAs(path);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }
                                Session["operationMsg"] = "Book updated successfully";

                            }
                        }
                        else
                        {
                            ViewBag.Mess = "Không đúng định dạng";
                            return View();
                        }
                        return RedirectToAction("Index");

                    }
                }
                //Session["operationMsg"] = "Book updated successfully";
                //bookDb.Entry(tblBook).State = EntityState.Modified;
                //bookDb.SaveChanges();
                //return RedirectToAction("Index");
            }
            else
            {
                tblBook.Images = Session["imgPath"].ToString();
                bookDb.Entry(tblBook).State = EntityState.Modified;
                if (bookDb.SaveChanges() > 0)
                {
                    Session["operationMsg"] = "Book updated successfully";
                    return RedirectToAction("Index");
                }
            }
            return View(tblBook);
        }

        // GET: tblBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = bookDb.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        // POST: tblBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblBook tblBook = bookDb.tblBooks.Find(id);
            bookDb.tblBooks.Remove(tblBook);
            bookDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bookDb.Dispose();
            }
            base.Dispose(disposing);
        }
        [ChildActionOnly]
        public ActionResult PartialSanPham()
        {
            return PartialView();
        }

        public ActionResult ReadALittleDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook book = bookDb.tblBooks.FirstOrDefault(n => n.BookId == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            var listLQ = bookDb.tblBooks.Where(n => n.BookCategory == book.BookCategory).ToList();

            objBookAndListRelated objBookAndListRelated = new objBookAndListRelated();
            objBookAndListRelated.objBook = book;
            objBookAndListRelated.listBookRelated = listLQ;


            return View(objBookAndListRelated);
        }
        public ActionResult Home()
        {
            return RedirectToAction("Home", "Main");
        }
        public ActionResult About()
        {
            return RedirectToAction("About", "Main");
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Contact", "Main");
        }

        public ActionResult Login()
        {
            return RedirectToAction("Login", "Main");
        }
    }
}
