using SB_INFOTECH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.MappingViews;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace SB_INFOTECH.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        protected SB_InfotechEntities db = new SB_InfotechEntities();
        public ActionResult Login()
        {
            var model = new Admin_mst();
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(Admin_mst admin)
        {
            if (admin.Gmail == null && admin.Mobile == null)
            {
                TempData["Msg"] = "<script>alert('Please Enter One Of Identification')</script>";
            }
            else if (admin.Password == null)
            {
                TempData["Msg"] = "<script>alert('Please Enter Your Password')</script>";
            }
            else
            {
                if (admin.Gmail != null)
                {
                    var adm = db.Admin_mst.Where(m => m.Gmail == admin.Gmail).FirstOrDefault();
                    if (adm != null)
                    {
                        if (adm.Password == admin.Password)
                        {
                            FormsAuthentication.SetAuthCookie(adm.Gmail, false);
                            TempData["Msg"] = "<script>alert('Login Success using 1st Identification')</script>";
                        }
                        else
                        {
                            TempData["Msg"] = "<script>alert('Invalid Password...!')</script>";
                        }
                    }
                    else
                    {
                        TempData["Msg"] = "<script>alert('Invalid Identification...!')</script>";
                    }
                }
                else
                {
                    var adm = db.Admin_mst.Where(m => m.Mobile == admin.Mobile).FirstOrDefault();
                    if (adm != null)
                    {
                        if (adm.Password == admin.Password)
                        {
                            FormsAuthentication.SetAuthCookie(adm.Gmail, false);
                            TempData["Msg"] = "<script>alert('Login Success using 2nd Identification')</script>";
                        }
                        else
                        {
                            TempData["Msg"] = "<script>alert('Invalid Password...!')</script>";
                        }
                    }
                    else
                    {
                        TempData["Msg"] = "<script>alert('Invalid Identification...!')</script>";
                    }
                    //
                }
            }

            return View();
        }
        //[Authorize]
        public ActionResult Dashboard()
        {
            //if (Session["AdminId"] != null)
            {
               return View();
            }
            //else
            //{
            //    TempData["Msg"] = "<script>alert('Please Login First')</script>";
            //    return RedirectToAction("Login");
            //}
        }
        public ActionResult NewFunction()
        {
            var model = new Function();
            return View();
        }
        [HttpPost]
        public ActionResult NewFunction(Function fn)
        {
            db.Functions.Add(fn);
            db.SaveChanges();
            return View();
        }
        public ActionResult UploadImage()
        {
            ViewBag.Function = new SelectList(db.Functions, "FunctionID", "Title");
            var model = new FunctionImage();
            return View(model);
        }
        [HttpPost]
        public ActionResult UploadImage(IEnumerable<HttpPostedFileBase> files,FunctionImage fn)
        {
            if (files != null)
            {
                
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var folderPath = Server.MapPath("~/Assets");
                        var fullPath = Path.Combine(folderPath, fileName);

                        // Save file to server
                        file.SaveAs(fullPath);

                        // Save info to database
                        var image = new FunctionImage
                        {
                            FunctionID=fn.FunctionID,
                            Image = "/Assets/" + fileName, // For URL use
                            UploadedDate = DateTime.Now
                        };

                        db.FunctionImages.Add(image);
                    }
                }
            }
            ViewBag.Function = new SelectList(db.Functions, "FunctionID", "Title");
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            TempData["Msg"] = "<script>alert('Logout Success')</script>";
            return RedirectToAction("Index","Home");
        }
    }
}