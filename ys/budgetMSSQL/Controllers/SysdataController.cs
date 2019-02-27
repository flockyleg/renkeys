using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace applyvisa.Controllers
{
    public class SysdataController : Controller
    {
        //
        // GET: /Sysdata/系统数据

        public ActionResult Userdata()
        {
            return View();
        }
        public string ShowLoginName()
        {
            try
            {
                string surrentloginrealname = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
                if ((surrentloginrealname == string.Empty) | (surrentloginrealname == null)) surrentloginrealname = "";
                if (surrentloginrealname != "")
                {
                    ViewBag.dlzx = surrentloginrealname;
                }
                return surrentloginrealname;
            }
            catch
            {
                return "";
            }
        }


        public ActionResult Index()
        {
            ShowLoginName();

            return View();
        }

    }
}
