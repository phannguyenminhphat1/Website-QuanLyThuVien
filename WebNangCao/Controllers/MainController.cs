using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNangCao.Models;


namespace WebNangCao.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        BookEntity bookDb = new BookEntity();
        public ActionResult Home()
        {
            /*Sách kỹ năng sống*/
            var listBookKyNang = bookDb.tblBooks.Where(n => n.BookCategory == "Kỹ năng sống").ToList();
            ViewBag.ListBookKyNang = listBookKyNang;

            /*Sách cuộc sống*/
            var listBookCS = bookDb.tblBooks.Where(n => n.BookCategory == "Cuộc sống").ToList();
            ViewBag.ListBookCS = listBookCS;

            /*Cách mạng công nghiệp*/
            var listBookCongNghiep = bookDb.tblBooks.Where(n => n.BookCategory == "Cách mạng công nghiệp").ToList();
            ViewBag.ListBookCongNghiep = listBookCongNghiep;

            /*Giáo dục*/
            var listBookGiaoDuc = bookDb.tblBooks.Where(n => n.BookCategory == "Giáo Dục").ToList();
            ViewBag.ListBookGiaoDuc = listBookGiaoDuc;

            /*Tiểu thuyết*/
            var listBookTieuThuyet = bookDb.tblBooks.Where(n => n.BookCategory == "Tiểu thuyết").ToList();
            ViewBag.ListBookTieuThuyet = listBookTieuThuyet;


            /*Khoa học*/
            var listBookKH = bookDb.tblBooks.Where(n => n.BookCategory == "Khoa Học").ToList();
            ViewBag.ListBookKH = listBookKH;

            /*Kinh doanh*/
            var listBookKinhDoanh = bookDb.tblBooks.Where(n => n.BookCategory == "Kinh Doanh").ToList();
            ViewBag.ListBookKinhDoanh = listBookKinhDoanh;
            return View();
        }

        // Returns about view.
        public ActionResult About()
        {
            return View();
        }

        // Returns contact view.
        public ActionResult Contact()
        {
            return View();
        }

        // Returns login view.
        public ActionResult Login()
        {
            return View();
        }
    }
}