using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNangCao.Models;

namespace WebNangCao.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private AdminEntities adminDb = new AdminEntities();

        // Returns admin login view, here admin can login.
        [HttpGet]
        [HandleError]
        public ActionResult Login()
        {
            return View();
        }

        // Checks admin credentials, redirecting to admin section (index, tblBooks). 
        [HttpPost]
        [HandleError]
        public ActionResult Login(tblAdmin admin)
        {
            var adm = adminDb.tblAdmins.SingleOrDefault(a => a.AdminEmail == admin.AdminEmail && a.AdminPass == admin.AdminPass);
            if (adm != null)
            {
                int id = adm.AdminId;
                Session["adminId"] = adm.AdminId;
                return RedirectToAction("Index", "TblBooks", new { id = id });
            }
            else if (admin.AdminEmail == null && admin.AdminPass == null)
            {
                return View();
            }
            ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }
        /* Admin credentials validation two
        public ActionResult Validate(tblAdmin admin)
        {
            var adm = db.tblAdmins.SingleOrDefault(a => a.AdminEmail == admin.AdminEmail && a.AdminPass == admin.AdminPass);
            if (adm != null)
            {
                int id = adm.AdminId;
                return RedirectToAction("Index", "tblBooks", new { @id = id });
            }
            else if (admin.AdminEmail == null && admin.AdminPass == null)
            {
                return View("Login");
            }
            ViewBag.Message = "User name and password are not matching";
            return View("Login");
        }*/

        // Admin logout, redirect to main. 
        [HandleError]
        public ActionResult Logout()
        {
            Session.Remove("adminId");
            return RedirectToAction("Home", "Main");
        }
    }
}