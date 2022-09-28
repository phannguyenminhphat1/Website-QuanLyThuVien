using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebNangCao.Models;
using WebNangCao.SoMuchModel;

namespace WebNangCao.Controllers
{
    public class BorrowController : Controller
    {
        static int userId;          // Lưu userID.
        static string userName;     // Lưu Username.

        private UserEntity userDb = new UserEntity();
        private BookEntity bookDb = new BookEntity();
        private TransEntity transDb = new TransEntity();


        public ActionResult Index(int? userId, string userName)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser user = userDb.tblUsers.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            BorrowController.userId = (int)userId;
            BorrowController.userName = userName;
            return View(bookDb.tblBooks.ToList());
        }

        public ActionResult UserHome()
        {
            /*Sách kỹ năng sống*/
            var listBookKyNang = bookDb.tblBooks.Where(n => n.BookCategory == "Kỹ năng sống").OrderBy(n=>n.Copyright).ToList();
            ViewBag.ListBookKyNang = listBookKyNang;

            /*Sách cuộc sống*/
            var listBookCS = bookDb.tblBooks.Where(n => n.BookCategory == "Cuộc sống").OrderBy(n => n.Copyright).ToList();
            ViewBag.ListBookCS = listBookCS;

            /*Cách mạng công nghiệp*/
            var listBookCongNghiep = bookDb.tblBooks.Where(n => n.BookCategory == "Cách mạng công nghiệp").OrderBy(n => n.Copyright).ToList();
            ViewBag.ListBookCongNghiep = listBookCongNghiep;

            /*Giáo dục*/
            var listBookGiaoDuc = bookDb.tblBooks.Where(n => n.BookCategory == "Giáo Dục").OrderBy(n => n.Copyright).ToList();
            ViewBag.ListBookGiaoDuc = listBookGiaoDuc;

            /*Tiểu thuyết*/
            var listBookTieuThuyet = bookDb.tblBooks.Where(n => n.BookCategory == "Tiểu thuyết").OrderBy(n => n.Copyright).ToList();
            ViewBag.ListBookTieuThuyet = listBookTieuThuyet;


            /*Khoa học*/
            var listBookKH = bookDb.tblBooks.Where(n => n.BookCategory == "Khoa Học").OrderBy(n => n.Copyright).ToList();
            ViewBag.ListBookKH = listBookKH;

            /*Kinh doanh*/
            var listBookKinhDoanh = bookDb.tblBooks.Where(n => n.BookCategory == "Kinh Doanh").OrderBy(n => n.Copyright).ToList();
            ViewBag.ListBookKinhDoanh = listBookKinhDoanh;
            return View();
        }

        public ActionResult UserAbout()
        {
            return View();
        }

        public ActionResult UserContact()
        {
            return View();
        }

       
        public ActionResult MenuBorrow()
        {
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
        }

        public ActionResult MenuRequested()
        {
            return RedirectToAction("Requested", "UserTransaction", new { userId = userId });
        }

        public ActionResult MenuReceived()
        {
            Session.Remove("receivedBadge");
            return RedirectToAction("Received", "UserTransaction", new { userId = userId });
        }

        public ActionResult MenuRejected()
        {
            Session.Remove("rejectedBadge");
            return RedirectToAction("Rejected", "UserTransaction", new { userId = userId });
        }

        public ActionResult Borrow(int? bookId)
        {
            
            if (transDb.tblTransactions.Where(t => t.UserId == userId).Count() < 6)
            {
                if (bookId != null)
                {
                    tblBook book = bookDb.tblBooks.FirstOrDefault(b => b.BookId == bookId);
                    if (book == null)
                    {
                        return HttpNotFound();
                    }
                    if (book.BookCopies > 0)
                    {
                        book.BookCopies = book.BookCopies - 1;
                        tblTransaction trans = new tblTransaction()
                        {
                            BookId = book.BookId,
                            BookTitle = book.BookTitle,
                            TranDate = DateTime.Now.ToShortDateString(),
                            TranStatus = "Requested",
                            UserId = userId,
                            UserName = userName,
                        };
                        bookDb.SaveChanges();
                        transDb.tblTransactions.Add(trans);
                        transDb.SaveChanges();
                        Session["requestMsg"] = "Requested successfully";
                    }
                    else
                    {
                        Session["requestMsg"] = "Sorry you cant take, Book copy is zero";
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                Session["requestMsg"] = "Sorry you cant take more than six books";
            }
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
            
        }

        
        public ActionResult RequestAlert()
        {
            Session.Remove("requestMsg");
            return RedirectToAction("Index", "Borrow", new { userId = userId, userName = userName });
        }
        public ActionResult PartialSachUser()
        {
            return PartialView();
        }

        public ActionResult ReadALittleBookUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook book = bookDb.tblBooks.SingleOrDefault(n => n.BookId == id);
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
    }
}