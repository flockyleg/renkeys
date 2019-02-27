using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using applyvisa.Models;
using System.IO;
using System.Diagnostics;
using System.Threading;

using NPOI;
using System.Data;
using System.Collections.ObjectModel;


namespace applyvisa.Controllers
{
    [Authentication]   //[Authorization]//如果将此特性加在Controller上，那么访问这个Controller里面的方法都需要验证用户登录状态
    public class MainController : Controller
    {

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

        #region 预算
        /// <summary>
        /// 固定预算申请
        /// </summary>
        /// <returns></returns>
        public ActionResult StaticApply()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            List<cls成本中心> cbzxs = Budget.获取成本中心("order by 成本中心编号");
            ViewBag.cbzxs = cbzxs;
            List<cls业务类型> ywlxs = Budget.获取业务类型();
            ViewBag.ywlxs = ywlxs;
            List<cls用户> yhs = Budget.获取用户(" order by 部门,序号");
            ViewBag.yhs = yhs;
            List<commData> ysmcs = Budget.获取固定预算名称();
            ViewBag.ysmcs = ysmcs;
            List<commData> zzdxs = Budget.获取转帐对象();
            ViewBag.zzdxs = zzdxs;
            string str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='增值税普通发票税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.zzsptfpsd = str1;
            str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='增值税专用发票税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.zzszyfpsd = str1;             
            str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='进项税税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.jxssd = str1;
            str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='销项税税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.xxssd = str1;


            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }
            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            return View();
        }
        public ActionResult FinanceApply()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            if (!登录.登录权限验证1("固定预算申请")) return RedirectToAction("Noauthorization", "Home");

            Apply(0);

            return RedirectToAction("StaticApply", "Main");
        }        
        public ActionResult ProjectApply()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("项目预算申请")) return RedirectToAction("Noauthorization", "Home");

            Apply(1);

            return RedirectToAction("StaticApply", "Main");
        }
        private void Apply(int 固定或项目)
        {
            TempData["acc_yusuan"] = null;
            cls预算单 acc_yusuan = null;

            if (acc_yusuan == null)
            {
                acc_yusuan = new cls预算单();
                acc_yusuan.ID = Guid.NewGuid().ToString();
                string ln = "";
                ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
                if ((ln == string.Empty) | (ln == null)) ln = "";
                acc_yusuan.申请人 = ln;
                acc_yusuan.来源 = "";
                acc_yusuan.类型 = 固定或项目;
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

        }


        //保存预算收支
        [HttpPost]
        public ActionResult SaveSz(bool savetodb, string tid, string tjzid, string szlx, string szf, string szje, string szrq, string szbz, string szlx1, string szse, string szsl, string szfpzl) 
        {
            string returnstr = "";
            int i = 0;

            #region
            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";
            string sjzid = tjzid;
            if ((sjzid == string.Empty) | (sjzid == null)) sjzid = "";
            string lx = szlx;
            if ((lx == string.Empty) | (lx == null)) lx = "";
            string s1 = szf;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = szje;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = szrq;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = szbz;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = szlx1;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            string s6 = szse;
            if ((s6 == string.Empty) | (s6 == null)) s6 = "";
            string s7 = szsl;
            if ((s7 == string.Empty) | (s7 == null)) s7 = "";
            string s8 = szfpzl;
            if ((s8 == string.Empty) | (s8 == null)) s8 = "";
            #endregion

            if ((lx.Trim() == "") | (s1.Trim() == "") | (s2.Trim() == "") | (s3.Trim() == ""))
            {
                returnstr = "300001";    //数据不合法
                return Content(returnstr);
            }

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效预算记录
                return Content(returnstr);
            }

            if (acc_yusuan.预算收支 == null)
            {
                acc_yusuan.预算收支 = new List<cls预算收支>();
            }

            cls预算收支 yssz = new cls预算收支();
            bool isnew = true;
            for (i = 0; i < acc_yusuan.预算收支.Count; i++)
            {
                if (acc_yusuan.预算收支[i].ID.ToString() == tid)
                {
                    yssz = acc_yusuan.预算收支[i];
                    isnew = false;
                    break;
                }
            }

            if (sid == "")
            {
                yssz.ID = Guid.NewGuid().ToString();
            }
            else
            {
                yssz.ID = sid;
            }
            yssz.预算ID = sjzid;
            yssz.收支 = lx;
            yssz.类型 = s5;
            yssz.借贷方 = s1;
            yssz.金额 = decimal.Round(CommonFunctions.ValDec(s2), 2);
            yssz.日期 = DateTime.Parse(s3);
            yssz.收付税额 = decimal.Round(CommonFunctions.ValDec(s6), 2);
            yssz.收付税率 = decimal.Round(CommonFunctions.ValDec(s7), 2);
            yssz.收付发票种类 = s8;
            yssz.备注 = s4;
            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";
            yssz.录入人 = ln;
            yssz.录入日期 = DateTime.Now;

            if (savetodb)
            {
                if (Budget.保存预计收支(sjzid, yssz) != "")
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (isnew)
            {
                acc_yusuan.预算收支.Add(yssz);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }
        //保存内部转账
        [HttpPost]
        public ActionResult SaveZz(bool savetodb, string tid, string tjzid, string zzdx, string zzje, string zzbl, string zzbz)
        {
            string returnstr = "";
            int i = 0;

            #region
            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";
            string sjzid = tjzid;
            if ((sjzid == string.Empty) | (sjzid == null)) sjzid = "";

            string s1 = zzdx;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = zzje;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "0.00";
            s2 = CommonFunctions.ValDec(s2).ToString();
            string s3 = zzbl;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "0.00";
            s3 = CommonFunctions.ValDec(s3).ToString();
            string s4 = zzbz;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            #endregion

            if ((s1.Trim() == "") | (s2.Trim() == ""))
            {
                returnstr = "300001";    //数据不合法
                return Content(returnstr);
            }

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效预算记录
                return Content(returnstr);
            }

            if (acc_yusuan.内部转账 == null)
            {
                acc_yusuan.内部转账 = new List<cls内部转账>();
            }

            cls内部转账 nbzz = new cls内部转账();
            bool isnew = true;
            for (i = 0; i < acc_yusuan.内部转账.Count; i++)
            {
                if (acc_yusuan.内部转账[i].ID.ToString() == tid)
                {
                    nbzz = acc_yusuan.内部转账[i];
                    isnew = false;
                    break;
                }
            }

            if (sid == "")
            {
                nbzz.ID = Guid.NewGuid().ToString();
            }
            else
            {
                nbzz.ID = sid;
            }
            nbzz.预算ID = sjzid;
            nbzz.转账对象 = s1;
            nbzz.转账金额 = decimal.Round(CommonFunctions.ValDec(s2), 2);
            nbzz.转账比例 = decimal.Round(CommonFunctions.ValDec(s3), 2);
            nbzz.备注 = s4;
            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";
            nbzz.录入人 = ln;
            nbzz.录入日期 = DateTime.Now;

            if (savetodb)
            {
                if (Budget.保存内部转账(sjzid, nbzz) != "")
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (isnew)
            {
                acc_yusuan.内部转账.Add(nbzz);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }
        //保存预算发票
        [HttpPost]
        public ActionResult SaveFp(bool savetodb, string tid, string tjzid, string spf, string fpzl, string ysse, string sd, string fpbz)
        {
            string returnstr = "";
            int i = 0;

            #region
            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";
            string sjzid = tjzid;
            if ((sjzid == string.Empty) | (sjzid == null)) sjzid = "";

            string s1 = spf;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = fpzl;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = ysse;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "0.00";
            string s4 = sd;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "0.00";
            string s5 = fpbz;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            #endregion

            if ((s1.Trim() == "") | (s2.Trim() == ""))
            {
                returnstr = "300001";    //数据不合法
                return Content(returnstr);
            }

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效预算记录
                return Content(returnstr);
            }

            if (acc_yusuan.预算收支 == null)
            {
                acc_yusuan.预算发票 = new List<cls预算发票>();
            }

            cls预算发票 ysfp = new cls预算发票();
            bool isnew = true;
            for (i = 0; i < acc_yusuan.预算发票.Count; i++)
            {
                if (acc_yusuan.预算发票[i].ID.ToString() == tid)
                {
                    ysfp = acc_yusuan.预算发票[i];
                    isnew = false;
                    break;
                }
            }

            if (sid == "")
            {
                ysfp.ID = Guid.NewGuid().ToString();
            }
            else
            {
                ysfp.ID = sid;
            }
            ysfp.预算ID = sjzid;
            ysfp.收票方 = s1;
            ysfp.发票种类 = s2;
            ysfp.应收税额 = decimal.Round(CommonFunctions.ValDec(s3), 2);
            ysfp.税点 = decimal.Round(CommonFunctions.ValDec(s4), 2);
            ysfp.备注 = s5;
            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";
            ysfp.录入人 = ln;
            ysfp.录入日期 = DateTime.Now;

            if (savetodb)
            {
                if (Budget.保存预算发票(sjzid, ysfp) != "")
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (isnew)
            {
                acc_yusuan.预算发票.Add(ysfp);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }
        //保存实际收支
        [HttpPost]
        public ActionResult SaveSjSz(bool savetodb, string tid, string tjzid, string szlx, string szf, string szje, string szrq, string szbz, string szlx1)
        {
            string returnstr = "";
            int i = 0;

            #region
            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";
            string sjzid = tjzid;
            if ((sjzid == string.Empty) | (sjzid == null)) sjzid = "";
            string lx = szlx;
            if ((lx == string.Empty) | (lx == null)) lx = "";
            string s1 = szf;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = szje;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = szrq;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = szbz;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = szlx1;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            #endregion

            if ((lx.Trim() == "") | (s1.Trim() == "") | (s2.Trim() == "") | (s3.Trim() == ""))
            {
                returnstr = "300001";    //数据不合法
                return Content(returnstr);
            }

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效预算记录
                return Content(returnstr);
            }

            if (acc_yusuan.实际收支 == null)
            {
                acc_yusuan.实际收支 = new List<cls实际收支>();
            }

            cls实际收支 sjsz = new cls实际收支();
            bool isnew = true;
            for (i = 0; i < acc_yusuan.实际收支.Count; i++)
            {
                if (acc_yusuan.实际收支[i].ID.ToString() == tid)
                {
                    sjsz = acc_yusuan.实际收支[i];
                    isnew = false;
                    break;
                }
            }

            if (sid == "")
            {
                sjsz.ID = Guid.NewGuid().ToString();
            }
            else
            {
                sjsz.ID = sid;
            }
            sjsz.预算ID = sjzid;
            sjsz.收支 = lx;
            sjsz.类型 = s5;
            sjsz.借贷方 = s1;
            sjsz.金额 = decimal.Round(CommonFunctions.ValDec(s2), 2);
            sjsz.日期 = DateTime.Parse(s3);
            sjsz.备注 = s4;
            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";
            sjsz.录入人 = ln;
            sjsz.录入日期 = DateTime.Now;

            if (savetodb)
            {
                if (Budget.保存实际收支(sjzid, sjsz) != "")
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (isnew)
            {
                acc_yusuan.实际收支.Add(sjsz);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }



        //删除预计收支
        [HttpPost]
        public ActionResult DelSz(bool savetodb, string tid) 
        {
            string returnstr = "";
            int i = 0;

            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效记录
                return Content(returnstr);
            }

            if (savetodb)
            {
                if (Budget.删除预计收支(tid) < 1)
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (acc_yusuan.预算收支 == null)
            {
                acc_yusuan.预算收支 = new List<cls预算收支>();
            }

            bool succ = false;
            for (i = 0; i < acc_yusuan.预算收支.Count; i++)
            {
                if (acc_yusuan.预算收支[i].ID.ToString() == tid)
                {
                    acc_yusuan.预算收支.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            if (!succ)
            {
                TempData["acc_yusuan"] = acc_yusuan;
                TempData.Keep("acc_yusuan");

                returnstr = "300002";    //无该预算收支
                return Content(returnstr);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }
        //删除内部转账
        [HttpPost]
        public ActionResult DelZz(bool savetodb, string tid)
        {
            string returnstr = "";
            int i = 0;

            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效记录
                return Content(returnstr);
            }

            if (savetodb)
            {
                if (Budget.删除内部转账(tid) < 1)
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (acc_yusuan.内部转账 == null)
            {
                acc_yusuan.内部转账 = new List<cls内部转账>();
            }

            bool succ = false;
            for (i = 0; i < acc_yusuan.内部转账.Count; i++)
            {
                if (acc_yusuan.内部转账[i].ID.ToString() == tid)
                {
                    acc_yusuan.内部转账.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            if (!succ)
            {
                TempData["acc_yusuan"] = acc_yusuan;
                TempData.Keep("acc_yusuan");

                returnstr = "300002";    //无该内部转账
                return Content(returnstr);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }
        //删除预算发票
        [HttpPost]
        public ActionResult DelFp(bool savetodb, string tid)
        {
            string returnstr = "";
            int i = 0;

            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效记录
                return Content(returnstr);
            }

            if (savetodb)
            {
                if (Budget.删除预算发票(tid) < 1)
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (acc_yusuan.预算发票 == null)
            {
                acc_yusuan.预算发票 = new List<cls预算发票>();
            }

            bool succ = false;
            for (i = 0; i < acc_yusuan.预算发票.Count; i++)
            {
                if (acc_yusuan.预算发票[i].ID.ToString() == tid)
                {
                    acc_yusuan.预算发票.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            if (!succ)
            {
                TempData["acc_yusuan"] = acc_yusuan;
                TempData.Keep("acc_yusuan");

                returnstr = "300002";    //无该预算发票
                return Content(returnstr);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }
        //删除实际收支
        [HttpPost]
        public ActionResult DelSjSz(bool savetodb, string tid)
        {
            string returnstr = "";
            int i = 0;

            string sid = tid;
            if ((sid == string.Empty) | (sid == null)) sid = "";

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "300001";    //无效记录
                return Content(returnstr);
            }

            if (savetodb)
            {
                if (Budget.删除实际收支(tid) < 1)
                {
                    TempData["acc_yusuan"] = acc_yusuan;
                    TempData.Keep("acc_yusuan");

                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (acc_yusuan.实际收支 == null)
            {
                acc_yusuan.实际收支 = new List<cls实际收支>();
            }

            bool succ = false;
            for (i = 0; i < acc_yusuan.实际收支.Count; i++)
            {
                if (acc_yusuan.实际收支[i].ID.ToString() == tid)
                {
                    acc_yusuan.实际收支.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            if (!succ)
            {
                TempData["acc_yusuan"] = acc_yusuan;
                TempData.Keep("acc_yusuan");

                returnstr = "300002";    //无该实际收支
                return Content(returnstr);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            returnstr = "123456789";
            return Content(returnstr);
        }

        [HttpPost]
        public ActionResult SaveNewStaticApply(string tysbh, string tysmc, string tcbzxbh, string tyssm, string tywlx, string txs)
        {
            string s1 = tysbh;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = tysmc;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = tcbzxbh;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = tyssm;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = tywlx;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            string s6 = txs;
            if ((s6 == string.Empty) | (s6 == null)) s6 = "";

            string returnstr = "";
            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "400001";    //无效签证记录
                return Content(returnstr);
            }

            acc_yusuan.预算编号 = s1;   // CommonFunctions.ValInt(s1);
            acc_yusuan.预算名称 = s2;
            acc_yusuan.成本中心编号 = s3;
            acc_yusuan.预算说明 = s4;
            acc_yusuan.业务类型 = s5;
            acc_yusuan.销售 = s6;

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";
            if (acc_yusuan.申请人 == "") acc_yusuan.申请人 = sdenglurenming;
            acc_yusuan.申请日期 = DateTime.Now;
            acc_yusuan.来源 = "";
            acc_yusuan.预算状态 = "待申请";

            string str1 = "";
            str1 = Budget.保存新增预算(acc_yusuan);
            if (str1 == "")
            {
                ViewBag.savesuccess = "";

                //从数据库重新获取
                acc_yusuan = Budget.获取预算记录(acc_yusuan.ID.ToString());
            }
            else
            {
                ViewBag.savesuccess = str1;
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");
            Session["acc_yusuan"] = acc_yusuan;   //Session再保存下，TempData在文件上传中好像很快失效

            returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }

        public ActionResult StaticApplyEdit()    //编辑、显示
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string blgc = "";

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                acc_yusuan = new cls预算单();
            }
            else
            {
                //List<cls办理过程> bls = Apply.获取预算办理过程s("where 预算ID='" + acc_yusuan.ID.ToString() + "'");
                //blgc = Apply.办理过程描述(bls);
            }
            ViewBag.blgc = blgc;

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            List<cls成本中心> cbzxs = Budget.获取成本中心("order by 成本中心编号");
            ViewBag.cbzxs = cbzxs;
            List<cls业务类型> ywlxs = Budget.获取业务类型();
            ViewBag.ywlxs = ywlxs;
            List<cls用户> yhs = Budget.获取用户(" order by 部门,序号");
            ViewBag.yhs = yhs;
            List<commData> ysmcs = Budget.获取固定预算名称();
            ViewBag.ysmcs = ysmcs;
            List<commData> zzdxs = Budget.获取转帐对象();
            ViewBag.zzdxs = zzdxs;
            string str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='增值税普通发票税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.zzsptfpsd = str1;
            str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='增值税专用发票税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.zzszyfpsd = str1;
            str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='进项税税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.jxssd = str1;
            str1 = "";
            try { str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='销项税税点'").Rows[0][0].ToString(); }
            catch { }
            ViewBag.xxssd = str1;

            return View();
        }
        [HttpPost]
        public ActionResult SaveExistStaticApply(string tysmc, string tcbzxbh, string tyssm, string tywlx, string txs)
        {
            string s1 = tysmc;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = tcbzxbh;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = tyssm;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = tywlx;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = txs;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";

            string returnstr = "";
            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }

            if (acc_yusuan == null)
            {
                returnstr = "400001";    //无效记录
                return Content(returnstr);
            }

            acc_yusuan.预算名称 = s1;
            acc_yusuan.成本中心编号 = s2;
            acc_yusuan.预算说明 = s3;
            acc_yusuan.业务类型 = s4;
            acc_yusuan.销售 = s5;

            string str1 = "";
            str1 = Budget.保存已有预算(acc_yusuan);
            if (str1 != "")
            {
                returnstr = "400002";    //保存到数据库出错
                return Content(returnstr);
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");
            Session["acc_yusuan"] = acc_yusuan;   //Session再保存下，TempData在文件上传中好像很快失效

            returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }


        [HttpPost]
        public ActionResult ApproveStaticApply(string id)    //送审
        {
            string returnstr = "";
            string str1 = "";
            string str2 = "";

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            if (s1 == "")
            {
                returnstr = "400001";    //无效的记录
                return Content(returnstr);
            }

            int i = 0;
            i = Budget.预算送审(s1);
            if (i < 1)
            {
                returnstr = "400002";    //保存到数据库出错
                return Content(returnstr);
            }

            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";

            i = Budget.记录处理过程(s1, "预算申请", ln, "待审批", "提交审批");

            str1 = "";
            try
            {
                str1 = main.获取一般数据集("select 预算名称 from A预算 where ID='" + s1 + "'").Rows[0][0].ToString();
            }
            catch { }
            str1 = str1.Replace("'", "").Replace("\"", "");
            str2 = Budget.获取对应的审批人(ln);
            i = Budget.发送通知消息(s1, "预算申请", ln + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "发起名为“" + str1 + "”的预算审批请求。", str2, ln);

            returnstr = "123456789";
            return Content(returnstr);
        }


        [HttpPost]
        public ActionResult DeleteStaticApply(string id)    //预算删除
        {
            string returnstr = "";

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            if (s1 == "")
            {
                returnstr = "400001";    //无效的记录
                return Content(returnstr);
            }

            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";

            int i = 0;
            i = Budget.预算删除(s1, ln);
            if (i < 1)
            {
                returnstr = "400002";    //删除数据库出错
                return Content(returnstr);
            }

            returnstr = "123456789";
            return Content(returnstr);
        }




        #region 预算查询

        /*
        public ActionResult StaticApplyList1(int p, int r)
        {
            if (!登录.登录权限验证1("固定预算查询")) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            yusuanchaxun(r, p, sdenglurenming, "固定");

            return RedirectToAction("StaticApplyList", "Main");
        }
        public ActionResult StaticApplyList2(int p, int r)
        {
            if (!登录.登录权限验证1("项目预算查询")) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            yusuanchaxun(r, p, sdenglurenming, "项目");

            return RedirectToAction("StaticApplyList", "Main");
        }
        public ActionResult StaticApplyListX(int p, int r)
        {
            if (!登录.登录权限验证1("预算查询(全)")) return RedirectToAction("Noauthorization", "Home");

            yusuanchaxun(r, p, "", "");
             
            return RedirectToAction("StaticApplyList", "Main");
        }
        public ActionResult StaticApplyList()
        {
            return View();
        }
        public void yusuanchaxun(int refresh, int p, string sdenglurenming, string sleixing)
        {
            if (refresh == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["dlzx"] = sdenglurenming;
            TempData["sleixing"] = sleixing;

            List<cls预算单> sbs = new List<cls预算单>();
            //ViewBag.pagenum = p;
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls预算单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls预算单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                //ViewBag.pagetotal = x1;
                TempData["pagetotal"] = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                //ViewBag.pagedata = sbs1;   //当前页的数据
                TempData["pagedata"] = sbs1;
            }
        }
        */
        
        public ActionResult StaticApplyList(int p, int r, int t)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string stp = "";
            if (t == 0) stp = "固定预算查询";
            if (t == 1) stp = "项目预算查询";
            if (t == 2) stp = "预算查询(全)";
            if (stp == "") return RedirectToAction("Noauthorization", "Home");
            if (!登录.登录权限验证1(stp)) return RedirectToAction("Noauthorization", "Home");

            List<cls成本中心> cbzxs = Budget.获取成本中心("order by 成本中心编号");
            ViewBag.cbzxs = cbzxs;
            
            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            string wcczr = "";
            string ysczcjqxr = "";
            string sleixing = "";
            if (t == 0) sleixing = "固定"; 
            if (t == 1) sleixing = "项目";
            if (t == 2) sleixing = "";
            try { wcczr = main.获取一般数据集("select 参数值 from Z参数 where 参数名='固定预算完成操作人'").Rows[0][0].ToString(); }
            catch { }
            wcczr = " " + wcczr + " ";
            try { ysczcjqxr = main.获取一般数据集("select 参数值 from Z参数 where 参数名='预算操作超级权限人'").Rows[0][0].ToString(); }
            catch { }
            ysczcjqxr = " " + ysczcjqxr + " ";
            ViewBag.wcczr = wcczr;
            ViewBag.ysczcjqxr = ysczcjqxr;

            if (r == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["dlzx"] = sdenglurenming;
            TempData["sleixing"] = sleixing;

            List<cls预算单> sbs = new List<cls预算单>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls预算单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls预算单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }

            return View();
        }


        #region 预算通用查询
        public ActionResult ApplyQuery()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            if (!登录.登录权限验证1("预算查询统计")) return RedirectToAction("Noauthorization", "Home");

            List<cls成本中心> cbzxs = Budget.获取成本中心("order by 成本中心编号");
            ViewBag.cbzxs = cbzxs;
            List<cls业务类型> ywlxs = Budget.获取业务类型();
            ViewBag.ywlxs = ywlxs;
            List<cls用户> yhs = Budget.获取用户(" order by 部门,序号");
            ViewBag.yhs = yhs;

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";


            return View();
        }
        [HttpPost]
        public ActionResult ApplyQueryCx(string txtpaixu, string txtywlx, string txtsqrq1, string txtsqrq2, string txtysbh, string txtysmc, string txtsqr, string txtcbzx, string txtyssm, string txtxs, string txtclzt, string txtysfl, string txtzztdd, string txtxsxx, string txtqd, string txtgzc, string txtzddh, string txtsff)
        {
            string paixu = txtpaixu;
            if ((paixu == string.Empty) | (paixu == null)) paixu = "";
            string ywlx = txtywlx;
            if ((ywlx == string.Empty) | (ywlx == null)) ywlx = "";
            string sqrq1 = txtsqrq1;
            if ((sqrq1 == string.Empty) | (sqrq1 == null)) sqrq1 = "";
            string sqrq2 = txtsqrq2;
            if ((sqrq2 == string.Empty) | (sqrq2 == null)) sqrq2 = "";
            string ysbh = txtysbh;
            if ((ysbh == string.Empty) | (ysbh == null)) ysbh = "";
            string ysmc = txtysmc;
            if ((ysmc == string.Empty) | (ysmc == null)) ysmc = "";
            string sqr = txtsqr;
            if ((sqr == string.Empty) | (sqr == null)) sqr = "";
            string cbzx = txtcbzx;
            if ((cbzx == string.Empty) | (cbzx == null)) cbzx = "";
            string yssm = txtyssm;
            if ((yssm == string.Empty) | (yssm == null)) yssm = "";
            string xs = txtxs;
            if ((xs == string.Empty) | (xs == null)) xs = "";
            string clzt = txtclzt;
            if ((clzt == string.Empty) | (clzt == null)) clzt = "";
            string ysfl = txtysfl;
            if ((ysfl == string.Empty) | (ysfl == null)) ysfl = "";
            string zztdd = txtzztdd;
            if ((zztdd == string.Empty) | (zztdd == null)) zztdd = "";
            string xsxx = txtxsxx;
            if ((xsxx == string.Empty) | (xsxx == null)) xsxx = "";
            string qd = txtqd;
            if ((qd == string.Empty) | (qd == null)) qd = "";
            string gzc = txtgzc;
            if ((gzc == string.Empty) | (gzc == null)) gzc = "";
            string zddh = txtzddh;
            if ((zddh == string.Empty) | (zddh == null)) zddh = "";
            string sff = txtsff;
            if ((sff == string.Empty) | (sff == null)) sff = "";


            string str1 = "";
            string str2 = "";
            string str3 = "";
            string[] sOpe = new string[16];
            string[] sFld = new string[16];
            string[] sVal = new string[16];

            sFld[0] = "业务类型";
            sFld[1] = "申请日期";
            sFld[2] = "申请日期";
            sFld[3] = "预算编号";
            sFld[4] = "预算名称";
            sFld[5] = "申请人";
            sFld[6] = "成本中心编号";
            sFld[7] = "预算说明";
            sFld[8] = "销售";
            sFld[9] = "预算状态";
            sFld[10] = "类型";
            sFld[11] = "是否中证通订单";
            sFld[12] = "线上线下";
            sFld[13] = "渠道";
            sFld[14] = "承办公证处";
            sFld[15] = "子订单号";

            sOpe[0] = "='";
            sOpe[1] = ">='";
            sOpe[2] = "<='";
            sOpe[3] = "like";
            sOpe[4] = "like";
            sOpe[5] = "like";
            sOpe[6] = "='";
            sOpe[7] = "like";
            sOpe[8] = "='";
            sOpe[9] = "='";
            sOpe[10] = "=";
            sOpe[11] = "='";
            sOpe[12] = "='";
            sOpe[13] = "like";
            sOpe[14] = "like";
            sOpe[15] = "like";

            sVal[0] = ywlx;
            sVal[0] = ywlx.Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
            if (sVal[0] != "")
            {
                try { sVal[0] = sVal[0].Replace(" ", "' or 业务类型='"); }
                catch { }
            }
            sVal[1] = sqrq1;
            if (sVal[1] != "")
            {
                try { sVal[1] = DateTime.Parse(sVal[1]).ToString("yyyy-MM-dd 00:00:00"); }
                catch { }
            }
            sVal[2] = sqrq2;
            if (sVal[2] != "")
            {
                try { sVal[2] = DateTime.Parse(sVal[2]).ToString("yyyy-MM-dd 23:59:59"); }
                catch { }
            }
            sVal[3] = ysbh;
            sVal[4] = ysmc;
            sVal[5] = sqr;
            sVal[6] = cbzx.Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
            if (sVal[6] != "")
            {
                try { sVal[6] = sVal[6].Replace(" ", "' or 成本中心编号='"); }
                catch { }
            }
            sVal[7] = yssm;
            sVal[8] = xs.Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
            if (sVal[8] != "")
            {
                try { sVal[8] = sVal[8].Replace(" ", "' or 销售='"); }
                catch { }
            }
            sVal[9] = clzt.Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
            if (sVal[9] != "")
            {
                try { sVal[9] = sVal[9].Replace(" ", "' or 预算状态='"); }
                catch { }
            }
            sVal[10] = ysfl;
            sVal[11] = zztdd;
            sVal[12] = xsxx;
            sVal[13] = qd;
            sVal[14] = gzc;
            sVal[15] = zddh;

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();
            if (str1 == "")
                str1 = "where";
            else
                str1 = str1 + " and";
            str1 = str1 + " (审批结果<>'驳回' and 核定结果<>'驳回')";


            //预算查询统计专人限制条件
            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";
            string xztj = "";
            try { xztj = main.获取一般数据集("select 参数值 from Z参数 where 参数名='预算查询统计专人限制条件'").Rows[0][0].ToString(); }
            catch { }
            if ((ln != "") && (xztj != ""))
            {
                str2 = "[" + ln + "]";
                if (xztj.IndexOf(str2) > -1)
                {
                    goto AAA;
                }
                else
                {
                    str2 = "[" + ln + " ";
                    if (xztj.IndexOf(str2) > -1)
                    {
                        goto AAA;
                    }
                    else
                    {
                        str2 = " " + ln + "]";
                        if (xztj.IndexOf(str2) > -1)
                        {
                            goto AAA;
                        }
                        else
                        {
                            goto BBB;
                        }
                    }
                }
            AAA:
                string stj1 = "";
                stj1 = CommonFunctions.ReadWordOnly(xztj, str2, false, true);
                stj1 = CommonFunctions.ReadWordOnly(stj1, "{", false, true);
                stj1 = CommonFunctions.ReadWordOnly(stj1, "}", true, true);
                stj1 = stj1.Trim();
                if (stj1 != "")
                {
                    str1 = str1 + " and (" + stj1 + ")";
                }
            }
        BBB:


            //预算计收支条件
            string[] sOpe1 = new string[1]; string[] sFld1 = new string[1]; string[] sVal1 = new string[1];
            sFld1[0] = "借贷方";
            sOpe1[0] = "like";
            sVal1[0] = txtsff;
            str3 = CommonFunctions.CreateWhereClause(sOpe1, sFld1, sVal1, "and").Trim();
            if (str3 != "")
            {
                str3 = "ID in (select 预算ID from A预算收支 " + str3 + ")";
                if (str1 == "")
                    str1 = str3;
                else
                    str1 = str1 + " and " + str3;
            }

            str1 = str1.Trim();
            if ((str1 != "") && (str1.IndexOf("where") != 0))
            {
                str1 = " where " + str1;
            }
            str1 = str1.Trim() + " order by ";
            switch (paixu)
            {
                case "申请日期升序":
                    str1 = str1 + " 申请日期";
                    break;
                case "成本中心编号":
                    str1 = str1 + " 成本中心编号";
                    break;
                case "业务类型":
                    str1 = str1 + " 业务类型";
                    break;
                case "处理状态":
                    str1 = str1 + " 处理状态";
                    break;
                case "申请人":
                    str1 = str1 + " 申请人";
                    break;
                case "销售":
                    str1 = str1 + " 销售";
                    break;
                default:
                    str1 = str1 + " 申请日期 desc";
                    break;
            }

            TempData["wheresql_ApplyQueryCx"] = str1;
            TempData.Keep("wheresql_ApplyQueryCx");

            string returnstr = "123456789";
            return Content(returnstr);
        }
        public ActionResult ApplyQueryList()
        {
            string str1 = "where 1=2";

            try { str1 = TempData["wheresql_ApplyQueryCx"].ToString(); }
            catch { }
            if (str1 != "")
            {
                //str1 = str1 + " limit 0,5000";
                TempData["wheresql_ApplyQueryCx"] = str1;
                TempData.Keep("wheresql_ApplyQueryCx");
            }

            List<cls预算单> rpts = new List<cls预算单>();
            try { rpts = Budget.获取预算记录s(str1); }
            catch { }
            if (rpts.Count < 1)
            {
                cls预算单 sb = new cls预算单();
                sb.ID = "";
                sb.预算编号 = "(无记录)";
                rpts.Add(sb);
            }
            cls预算单s rp = new cls预算单s();
            rp.记录集 = rpts;
            return View(rp);
        }
        public FileResult Export2Excel_ApplyQueryList(string w)
        {
            string str1 = "";
            int i = 0;

            str1 = "select (case 类型 WHEN 0 then '固定' ELSE '项目' end) as 分类,预算编号,预算名称,成本中心编号,业务类型,销售,渠道,预算状态,预算收入金额,预算销项税额,0.00 as 销项税率,预算支出金额,预算进项税额,0.00 as 进项税率,实际收入金额,实际支出金额,激励金额,转账金额,预算说明,申请人,申请日期,审批人,审批结果,核定人,核定结果 from V预算统计 ";
            str1 = str1 + " " + w;
            DataTable dt = main.获取一般数据集(str1);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    dt.Rows[i]["销项税率"] = decimal.Round(decimal.Divide(CommonFunctions.ValDec(dt.Rows[i]["预算销项税额"].ToString()), CommonFunctions.ValDec(dt.Rows[i]["预算收入金额"].ToString()) - CommonFunctions.ValDec(dt.Rows[i]["预算销项税额"].ToString())), 2);
                }
                catch { }
                try
                {
                    dt.Rows[i]["进项税率"] = decimal.Round(decimal.Divide(CommonFunctions.ValDec(dt.Rows[i]["预算进项税额"].ToString()), CommonFunctions.ValDec(dt.Rows[i]["预算支出金额"].ToString()) - CommonFunctions.ValDec(dt.Rows[i]["预算进项税额"].ToString())), 2);
                }
                catch { }

                //日期类型设置为“”会出错，导致速度慢
                //try { if (DateTime.Parse(dt.Rows[i]["审批日期"].ToString()) < DateTime.Parse("2000-01-01")) dt.Rows[i]["审批日期"] = ""; }
                //catch { }
                //try { if (DateTime.Parse(dt.Rows[i]["核定日期"].ToString()) < DateTime.Parse("2000-01-01")) dt.Rows[i]["核定日期"] = ""; }
                //catch { }
                //try { if (DateTime.Parse(dt.Rows[i]["完成日期"].ToString()) < DateTime.Parse("2000-01-01")) dt.Rows[i]["完成日期"] = ""; }
                //catch { }
            }

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";
            for (i = 0; i < dt.Columns.Count; i++)
            {
                row1.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }
            //将数据逐步写入sheet1各个行
            for (i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString().Trim());
                }
            }
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }
        #endregion

        #endregion

        #region 预算变更
        /*
        public ActionResult StaticApplyChange1(int p, int r)
        {
            if (!登录.登录权限验证1("固定预算变更申请")) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            yusuanbiangengchaxun(r, p, sdenglurenming, "固定");

            return RedirectToAction("StaticApplyChange", "Main");
        }
        public ActionResult StaticApplyChange2(int p, int r)
        {
            if (!登录.登录权限验证1("项目预算变更申请")) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            yusuanbiangengchaxun(r, p, sdenglurenming, "项目");

            return RedirectToAction("StaticApplyChange", "Main");
        }
        public ActionResult StaticApplyChange()
        {
            return View();
        }
        public void yusuanbiangengchaxun(int refresh, int p, string sdenglurenming, string sleixing)
        {
            if (refresh == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["dlzx"] = sdenglurenming;
            TempData["sleixing"] = sleixing;

            List<cls预算单> sbs = new List<cls预算单>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls预算单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls预算单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }
        }
        */

        public ActionResult StaticApplyChange(int p, int r, int t)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string stp = "";
            if (t == 0) stp = "固定预算变更申请";
            if (t == 1) stp = "项目预算变更申请";
            if (t == 2) stp = "预算变更查询(全)";
            if (stp == "") return RedirectToAction("Noauthorization", "Home");
            if (!登录.登录权限验证1(stp)) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            string sleixing = "";
            if (t == 0) sleixing = "固定";
            if (t == 1) sleixing = "项目";
            if (t == 2)
            {
                sleixing = "";
                sdenglurenming = "";
            }

            if (r == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["dlzx"] = sdenglurenming;
            TempData["sleixing"] = sleixing;

            List<cls预算单> sbs = new List<cls预算单>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls预算单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls预算单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }
            return View();
        }
        #endregion 预算变更




        //我的预算申请查询
        [HttpPost]
        public ActionResult StaticApplyCx(string t0, string t1, string t2, string t3, string t4, string t5, string t6, string t7, string t8)
        {
            string s0 = t0;
            if ((s0 == string.Empty) | (s0 == null)) s0 = "";
            string s1 = t1;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = t2;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = t3;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = t4;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = t5;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            string s6 = t6;
            if ((s6 == string.Empty) | (s6 == null)) s6 = "";
            string s7 = t7;
            if ((s7 == string.Empty) | (s7 == null)) s7 = "";
            string s8 = t8;
            if ((s8 == string.Empty) | (s8 == null)) s8 = "";

            string str1 = "";
            string str2 = "";
            string str3 = "";
            string[] sOpe = new string[8];
            string[] sFld = new string[8];
            string[] sVal = new string[8];

            sFld[0] = "类型";
            sFld[1] = "申请日期";
            sFld[2] = "申请日期";
            sFld[3] = "预算编号";
            sFld[4] = "预算名称";
            sFld[5] = "成本中心编号";
            sFld[6] = "预算状态";
            sFld[7] = "申请人";

            sOpe[0] = "=";
            sOpe[1] = ">='";
            sOpe[2] = "<='";
            sOpe[3] = "like";
            sOpe[4] = "like";
            sOpe[5] = "like";
            sOpe[6] = "='";
            sOpe[7] = "like";

            if (s0 == "固定")
            {
                s0 = "0";
            }
            if (s0 == "项目")
            {
                s0 = "1";
            }

            if ((s0 == "激励申请") | (s0 == "激励查询"))
            {
                s0 = "1";
            }
            sVal[0] = s0;
            sVal[1] = s1;
            sVal[2] = s2;
            sVal[3] = s3;
            sVal[4] = s4;
            sVal[5] = s5;
            sVal[6] = s6.Trim();
            //switch(sVal[6].Trim())
            //{
            //    case "待申请":
            //        sVal[6]="起草";
            //        break;
            //    case "待审批":
            //        sVal[6]="申请";
            //        break;
            //    case "待核定":
            //        sVal[6]="送核";
            //        break;
            //    case "待完成":
            //        sVal[6]="核定";
            //        break;
            //    case "已完成":
            //        sVal[6] = "完成";
            //        break;
            //    default:
            //        sVal[6] = sVal[6].Trim();
            //        break;
            //}
            sVal[7] = s7;

            if (sVal[6] == @"待审批/待核定/待完成")
            {
                sVal[6] = sVal[6].Replace(@"待审批/待核定/待完成", @"待审批' or 预算状态='待核定' or 预算状态='待完成");
            }
            if (sVal[6] == @"待核定/待完成")
            {
                sVal[6] = sVal[6].Replace(@"待核定/待完成", @"待核定' or 预算状态='待完成");
            }
            if (sVal[6] == @"已审核")
            {
                sVal[6] = sVal[6].Replace(@"已审核", @"待完成' or 预算状态='已完成");
            }

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();


            //预算查询附加条件



            //预算计收支条件
            string[] sOpe1 = new string[1]; string[] sFld1 = new string[1]; string[] sVal1 = new string[1];
            sFld1[0] = "借贷方";
            sOpe1[0] = "like";
            sVal1[0] = s8;
            str3 = CommonFunctions.CreateWhereClause(sOpe1, sFld1, sVal1, "and").Trim();
            if (str3 != "")
            {
                str3 = "ID in (select 预算ID from A预算收支 " + str3 + ")";
                if (str1 == "")
                    str1 = str3;
                else
                    str1 = str1 + " and " + str3;
            }

            str1 = str1.Trim();
            if ((str1 != "") && (str1.IndexOf("where") != 0))
            {
                str1 = " where " + str1;
            }
            str1 = str1.Trim() + " order by 预算编号 desc";

            List<cls预算单> sbs = Budget.获取预算记录s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");

            TempData["sbcxs"] = sbs;
            TempData.Keep("sbcxs");

            TempData["sbcx_t1"] = s1; TempData["sbcx_t2"] = s2; TempData["sbcx_t3"] = s3; TempData["sbcx_t4"] = s4; TempData["sbcx_t5"] = s5; TempData["sbcx_t6"] = s6; TempData["sbcx_t7"] = s7; TempData["sbcx_t8"] = s8;
            TempData.Keep("sbcx_t1"); TempData.Keep("sbcx_t2"); TempData.Keep("sbcx_t3"); TempData.Keep("sbcx_t4"); TempData.Keep("sbcx_t5"); TempData.Keep("sbcx_t6"); TempData.Keep("sbcx_t7"); TempData.Keep("sbcx_t8");

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }

        public ActionResult StaticApplyrecEdit(string id)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls预算单 acc_yusuan = null;
            try
            {
                acc_yusuan = Budget.获取预算记录s("where ID='" + s1 + "'")[0];
            }
            catch { }
            if (acc_yusuan == null)
            {
                acc_yusuan = new cls预算单();
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            return RedirectToAction("StaticApplyEdit", "Main");
        }

        /// <summary>
        /// 浏览预算
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StaticApplyView(string id)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls预算单 acc_yusuan = null;
            try
            {
                acc_yusuan = Budget.获取预算记录s("where ID='" + s1 + "'")[0];
            }
            catch { }
            if (acc_yusuan == null)
            {
                acc_yusuan = new cls预算单();
            }

            ViewBag.acc_yusuan = acc_yusuan;

            return View();
        }

        /// <summary>
        /// 预算审批
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ActionResult ApproveStaticApplyList(int p, string cxtj)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";
            ViewBag.dlzx = "（匿名）";
            if (sdenglurenming != "")
            {
                ViewBag.dlzx = sdenglurenming;
            }
            if (!登录.登录权限验证1("待审批预算")) return RedirectToAction("Noauthorization", "Home");

            string tjtj = "";
            if (cxtj != "")
            {
                tjtj = "'%" + cxtj + "%'";
                tjtj = " and (预算编号 like " + tjtj + " or 预算名称 like " + tjtj + " or 成本中心编号 like " + tjtj + " or 业务类型 like " + tjtj + " or 申请人 like " + tjtj + ") ";
            }

            string sqr = Budget.获取对应的申请人(sdenglurenming);
            string str1 = "where 预算状态='待审批' and 申请人 in (" + sqr + ") " + tjtj + " order by 申请日期 desc";
            List<cls预算单> sbs = Budget.获取预算记录s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");

            TempData["sbcxs"] = sbs;

            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }

        /// <summary>
        /// 预算核定
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ActionResult CheckStaticApplyList(int p, string cxtj)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";
            ViewBag.dlzx = "（匿名）";
            if (sdenglurenming != "")
            {
                ViewBag.dlzx = sdenglurenming;
            }
            if (!登录.登录权限验证1("待核定预算")) return RedirectToAction("Noauthorization", "Home");

            string tjtj = "";
            if (cxtj != "")
            {
                tjtj = "'%" + cxtj + "%'";
                tjtj = " and (预算编号 like " + tjtj + " or 预算名称 like " + tjtj + " or 成本中心编号 like " + tjtj + " or 业务类型 like " + tjtj + " or 申请人 like " + tjtj + ") ";
            }

            string str1 = "where 预算状态='待核定'" + tjtj + " order by 申请日期 desc";
            List<cls预算单> sbs = Budget.获取预算记录s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");

            TempData["sbcxs"] = sbs;

            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }

        
        [HttpPost]
        public ActionResult HandleStaticApply(string xgID, string xgyw, string clr, string cljg, string clyj, string shenqingren, string ysleixing)
        {
            string s1 = xgID;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = xgyw;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = clr;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = cljg;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = clyj;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            string ssqr = shenqingren;
            if ((ssqr == string.Empty) | (ssqr == null)) ssqr = "";
            string yslx = ysleixing;
            if ((yslx == string.Empty) | (yslx == null)) yslx = "";

            if ((s1 == "") | (s2 == "") | (s4 == ""))
            {
                return Content("900001");
            }
            int i = 0;

            if (s2 == "预算审批")
            {
                i = Budget.预算审批(s1, s3, s4, s5, ssqr, yslx);
                if (i < 1)
                {
                    return Content("900001");    //审批失败
                }
            }
            if (s2 == "预算核定")
            {
                i = Budget.预算核定(s1, s3, s4, s5, ssqr);
                if (i < 1)
                {
                    return Content("900001");    //核定失败
                }
            }
            if (s2 == "预算完成") 
            {
                if (!Budget.判断预算是否可以完成(s1))
                {
                    return Content("900004");    //未输入实际收支，不能设置为完成
                }
                i = Budget.预算完成(s1, ssqr);
                if (i < 1)
                {
                    return Content("900001");    //核定失败
                }
            }
            if (s2 == "激励审批")
            {
                string stmpjl1 = "";
                string stmpjl2 = s1;
                stmpjl1 = CommonFunctions.ReadWord(ref stmpjl2, " ");
                i = Budget.保存激励审批(stmpjl2, stmpjl1, s3, s4, s5);
                if (i < 1)
                {
                    return Content("900001");    //审批失败
                }
            }
            if (s2 == "变更审批")
            {
                i = Budget.保存变更审批(s1, s3, s4, s5);
                if (i < 1)
                {
                    return Content("900001");    //审批失败
                }
            }

            //处理过程记录、消息发送在Budget.预算审批()/Budget.预算核定()里写            

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }



        #region 激励
        public ActionResult StaticApplyMotivation(string id)    //激励编辑
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";
            ViewBag.dlzx = "（匿名）";
            if (sdenglurenming != "")
            {
                ViewBag.dlzx = sdenglurenming;
            }

            List<cls用户> yhs = Budget.获取用户(" order by 部门,序号");
            ViewBag.yhs = yhs;

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls预算单 acc_yusuan = null;
            try
            {
                acc_yusuan = Budget.获取预算记录s("where ID='" + s1 + "'")[0];
            }
            catch { }
            if (acc_yusuan == null)
            {
                acc_yusuan = new cls预算单();
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            List<cls成本中心> cbzxs = Budget.获取成本中心("order by 成本中心编号");
            ViewBag.cbzxs = cbzxs;

            return View();
        }


        /*
        public ActionResult StaticApplyMotivationList1(int p, int r)
        {
            if (!登录.登录权限验证1("激励申请")) return RedirectToAction("Noauthorization", "Home");

            jilichaxun(r, p, "激励申请");

            return RedirectToAction("StaticApplyMotivationList", "Main");
        }
        public ActionResult StaticApplyMotivationList2(int p, int r)
        {
            if (!登录.登录权限验证1("激励查询")) return RedirectToAction("Noauthorization", "Home");

            jilichaxun(r, p, "激励查询");

            return RedirectToAction("StaticApplyMotivationList", "Main");
        }
        public ActionResult StaticApplyMotivationList()
        {
            return View();
        }
        public void jilichaxun(int refresh, int p, string sleixing)
        {
            if (refresh == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["sleixing"] = sleixing;

            List<cls预算单> sbs = new List<cls预算单>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls预算单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls预算单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }
        }
        */

        /// <summary>
        /// 可申请激励的预算列表
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public ActionResult StaticApplyMotivationList(int p, int r)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            if (!登录.登录权限验证1("激励申请")) return RedirectToAction("Noauthorization", "Home");

            if (r == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["sleixing"] = "激励申请";

            List<cls预算单> sbs = new List<cls预算单>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls预算单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls预算单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }

            return View();
        }
        


        [HttpPost]
        public ActionResult SaveStaticApplyMotivation(string ysID, string tjlry, string tjlje, string tjlbl, string tjlbz)   //保存激励申请
        {
            string sysID = ysID;
            if ((sysID == string.Empty) | (sysID == null)) sysID = "";
            string s1 = tjlry;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = tjlje;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = tjlbl;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = tjlbz;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";

            string returnstr = "";
            
            if (sysID == "")
            {
                returnstr = "400001";    //无效记录
                return Content(returnstr);
            }


            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";

            string str1 = "";
            str1 = Budget.保存激励申请(ysID, tjlry, tjlje, tjlbl, tjlbz, sdenglurenming);
            if (str1 != "")
            {
                returnstr = "400002";    //保存到数据库出错
                return Content(returnstr);
            }

            returnstr = "123456789";
            return Content(returnstr);
        }

        //激励审批清单列表
        public ActionResult StaticApplyMotivationApproveList(int p)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";
            ViewBag.dlzx = "（匿名）";
            if (sdenglurenming != "")
            {
                ViewBag.dlzx = sdenglurenming;
            }
            if (!登录.登录权限验证1("待审批激励")) return RedirectToAction("Noauthorization", "Home");

            //string str1 = "where ID in (select 预算ID from A激励 where 审批结果='' or (审批结果='驳回' and 录入日期>审批日期)) order by 申请日期 desc";
            //List<cls预算单> sbs = Budget.获取预算记录s(str1);
            //string str1 = "select a.ID,b.预算名称 as Fld1,b.成本中心编号 as Fld2,a.人员 as Fld3,a.金额 as Fld4,a.录入人 as Fld5,convert(char(10),a.录入日期,120) as Fld6,a.比例 as Fld7,a.备注 as Fld8,b.ID as Fld9,'' as Fld10 from A激励 a left join A预算 b on a.预算ID=b.id where a.审批结果='' order by a.录入日期 desc";
            string str1 = "select a.ID,b.预算名称 as Fld1,b.成本中心编号 as Fld2,a.人员 as Fld3,a.金额 as Fld4,a.录入人 as Fld5,convert(a.录入日期,char(10)) as Fld6,a.比例 as Fld7,a.备注 as Fld8,b.ID as Fld9,'' as Fld10 from A激励 a left join A预算 b on a.预算ID=b.id where a.审批结果='' order by a.录入日期 desc";    //Mysql
            List<commData> coms = main.GetCommdataFromDb(str1);
            List<cls预算单> sbs = new List<cls预算单>();
            for (int i = 0; i < coms.Count; i++)
            {
                cls预算单 yd = new cls预算单();
                yd.管理_1 = coms[i].ID.ToString();  //激励ID
                yd.管理_2 = coms[i].Fld3;           //激励人员
                yd.管理_3 = coms[i].Fld4;           //激励金额
                yd.管理_4 = coms[i].Fld5;           //激励申请人
                yd.管理_5 = coms[i].Fld6;           //激励申请日期
                yd.管理_6 = coms[i].Fld7;           //激励比例
                yd.管理_7 = coms[i].Fld8;           //激励备注
                yd.ID = coms[i].Fld9;  //预算ID
                yd.预算名称 = coms[i].Fld1;
                yd.成本中心编号 = coms[i].Fld2;
                yd.类型 = 1;
                sbs.Add(yd);
            }

            TempData["sbcxs"] = sbs;
             
            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }

        //激励查询
        public ActionResult StaticApplyMotivationQuery(int p, int r, int t)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string stp = "";
            if (t == 0) stp = "激励查询";
            if (t == 2) stp = "激励查询(全)";
            if (stp == "") return RedirectToAction("Noauthorization", "Home");
            if (!登录.登录权限验证1(stp)) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            string sleixing = "项目";
            if (t == 2)
            {
                sdenglurenming = "";
            }


            if (r == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["dlzx"] = sdenglurenming;
            TempData["sleixing"] = sleixing;

            List<cls激励> sbs = new List<cls激励>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls激励>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls激励>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls激励> sbs1 = new List<cls激励>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }

            return View();
        }

        //激励查询
        [HttpPost]
        public ActionResult StaticApplyJlCx(string t1, string t2, string t3, string t4, string t5, string t6, string t7, string t8)
        {
            string s1 = t1;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = t2;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = t3;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = t4;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = t5;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            string s6 = t6;
            if ((s6 == string.Empty) | (s6 == null)) s6 = "";
            string s7 = t7;
            if ((s7 == string.Empty) | (s7 == null)) s7 = "";
            string s8 = t8;
            if ((s8 == string.Empty) | (s8 == null)) s8 = "";

            string str1 = "";
            string[] sOpe = new string[8];
            string[] sFld = new string[8];
            string[] sVal = new string[8];

            sFld[0] = "录入日期";
            sFld[1] = "录入日期";
            sFld[2] = "预算编号";
            sFld[3] = "预算名称";
            sFld[4] = "成本中心编号";
            sFld[5] = "审批结果";
            sFld[6] = "录入人";
            sFld[7] = "人员";
            
            sOpe[0] = ">='";
            sOpe[1] = "<='";
            sOpe[2] = "like";
            sOpe[3] = "like";
            sOpe[4] = "like";
            sOpe[5] = "='";
            sOpe[6] = "like";
            sOpe[7] = "like";
           
            sVal[0] = s1;
            sVal[1] = s2;
            sVal[2] = s3;
            sVal[3] = s4;
            sVal[4] = s5;
            sVal[5] = s6;
            sVal[6] = s7;
            sVal[7] = s8;

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim() + " order by 录入日期";

            List<cls激励> sbs = Budget.获取激励记录s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");

            TempData["sbcxs"] = sbs;
            TempData.Keep("sbcxs");

            TempData["sbcx_t1"] = s1; TempData["sbcx_t2"] = s2; TempData["sbcx_t3"] = s3; TempData["sbcx_t4"] = s4; TempData["sbcx_t5"] = s5; TempData["sbcx_t6"] = s6; TempData["sbcx_t7"] = s7; TempData["sbcx_t8"] = s8;
            TempData.Keep("sbcx_t1"); TempData.Keep("sbcx_t2"); TempData.Keep("sbcx_t3"); TempData.Keep("sbcx_t4"); TempData.Keep("sbcx_t5"); TempData.Keep("sbcx_t6"); TempData.Keep("sbcx_t7"); TempData.Keep("sbcx_t8");

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }
        #endregion

        public ActionResult StaticApplyActualList(int p, int r)  //预算实际收支输入
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            if (!登录.登录权限验证1("实际收支输入")) return RedirectToAction("Noauthorization", "Home");

            ViewBag.refesh = r;
            if (r == 1)
            {
                TempData["sbcxs"] = null;
            }

            List<cls预算单> sbs = new List<cls预算单>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls预算单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls预算单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }

            return View();
        }
        public ActionResult StaticApplyActual(string id)    //实际收支编辑
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls预算单 acc_yusuan = null;
            try
            {
                acc_yusuan = Budget.获取预算记录s("where ID='" + s1 + "'")[0];
            }
            catch { }
            if (acc_yusuan == null)
            {
                acc_yusuan = new cls预算单();
            }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            List<cls成本中心> cbzxs = Budget.获取成本中心("order by 成本中心编号");
            ViewBag.cbzxs = cbzxs;

            return View();
        }

        [HttpPost]
        public ActionResult piliangchuli(string ids)  //批量输入实际收支
        {
            int i = 0;
            string s1 = ids;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            string returnstr = "";
            string str1 = "";

            if (s1 == "")
            {
                returnstr = "400001";    //无效id
                return Content(returnstr);
            }

            string[] s = s1.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
            s1 = s1.Trim();
            s1 = s1.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
            
            List<cls预算单> ysds = new List<cls预算单>();
            s1 = "where ID in ('" + s1.Replace(" ", "','") + "')";
            ysds = Budget.获取预算记录s(s1);
            //RedirectToAction("BatchActual", "Main", new { ysds = ysds });
            this.TempData["plysd"] = ysds;

            i = s.Length;
            returnstr = i.ToString();
            return Content(returnstr);
        }

        public ActionResult BatchActual()
        {
            List<cls预算单> ysds = this.TempData["plysd"] as List<cls预算单>;
            ViewBag.ysds = ysds;

            this.TempData["plysd"] = ysds;
            return View();
        }

        //保存实际收支（批量）
        [HttpPost]
        public ActionResult SaveSjSzBatch(string szlx, string szf, string szje, string szrq, string szbz, string szlx1)
        {
            string returnstr = "";
            int i = 0;

            #region
            string lx = szlx;
            if ((lx == string.Empty) | (lx == null)) lx = "";
            string s1 = szf;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = szje;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = szrq;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = szbz;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = szlx1;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            #endregion

            if ((lx.Trim() == "") | (s1.Trim() == "") | (s2.Trim() == "") | (s3.Trim() == ""))
            {
                returnstr = "300001";    //数据不合法
                return Content(returnstr);
            }

            List<cls预算单> ysds = null;
            try
            {
                ysds = this.TempData["plysd"] as List<cls预算单>;
                this.TempData["plysd"] = ysds;
            }
            catch { }

            if (ysds == null)
            {
                returnstr = "300001";    //无效预算记录
                return Content(returnstr);
            }

            cls实际收支 sjsz = new cls实际收支();
            sjsz.收支 = lx;
            sjsz.类型 = s5;
            sjsz.借贷方 = s1;
            sjsz.金额 = decimal.Round(CommonFunctions.ValDec(s2), 2);
            sjsz.日期 = DateTime.Parse(s3);
            sjsz.备注 = s4;
            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";
            sjsz.录入人 = ln;
            sjsz.录入日期 = DateTime.Now;

            if (Budget.保存实际收支批量(ysds, sjsz, lx) != "")
            {
                returnstr = "300005";    //保存到数据库出错
                return Content(returnstr);
            }

            //string str1 = "";
            //foreach (cls预算单 yd in ysds)
            //{
            //    str1 = str1 + yd.ID.ToString() + "=" + yd.管理_1 + " ";
            //}

            this.TempData["plysd"] = ysds;

            returnstr = "123456789";
            return Content(returnstr);
        }


        [HttpPost]
        public ActionResult SaveStaticApplyChange(string ysID, string tbgr, string tbgyy)   //保存变更申请
        {
            string sysID = ysID;
            if ((sysID == string.Empty) | (sysID == null)) sysID = "";
            string s1 = tbgr;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = tbgyy;

            string returnstr = "";

            if (sysID == "")
            {
                returnstr = "400001";    //无效记录
                return Content(returnstr);
            }


            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";

            if (s1 == "") s1 = sdenglurenming;

            string str1 = "";
            str1 = Budget.保存变更申请(ysID, s1, s2);
            if (str1 != "")
            {
                returnstr = "400002";    //保存到数据库出错
                return Content(returnstr);
            }

            returnstr = "123456789";
            return Content(returnstr);
        }
        //变更审批清单列表
        public ActionResult StaticApplyChangeApproveList(int p)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";
            ViewBag.dlzx = "（匿名）";
            if (sdenglurenming != "")
            {
                ViewBag.dlzx = sdenglurenming;
            }
            if (!登录.登录权限验证1("待审批变更")) return RedirectToAction("Noauthorization", "Home");

            string str1 = "where ID in (select 预算ID from A变更 where 审批结果='' or (审批结果='驳回' and 录入日期>审批日期)) order by 申请日期 desc";
            List<cls预算单> sbs = Budget.获取预算记录s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");


            List<cls预算单> sbsopen = new List<cls预算单>();
            for (int i = 0; i < sbs.Count; i++)
            {
                for (int k = sbs[i].变更.Count-1; k >= 0; k--)
                {
                    if (sbs[i].变更[k].审批结果 == "")
                    {
                        cls预算单 ysd = sbs[i];
                        ysd.管理_1 = sbs[i].变更[k].录入人;
                        ysd.管理_2 = sbs[i].变更[k].录入日期.ToString("yyyy-MM-dd");
                        ysd.管理_3 = sbs[i].变更[k].变更原因;
                        ysd.管理_4 = sbs[i].变更[k].ID.ToString();
                        sbsopen.Add(ysd);
                    }
                }

            }

            TempData["sbcxs"] = sbsopen;

            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbsopen.Count / percount;
                x2 = sbsopen.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls预算单> sbs1 = new List<cls预算单>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbsopen.Count) break;
                    sbs1.Add(sbsopen[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }

        #endregion


        #region 其他
        private static DataTable ToDataTable(List<cls预算单> list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                System.Reflection.PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (System.Reflection.PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                foreach (object t in list)
                {
                    System.Collections.ArrayList tempList = new System.Collections.ArrayList();
                    foreach (System.Reflection.PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(t, null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        //private static string UnEscape(string str)
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    int len = str.Length;
        //    int i = 0;
        //    while (i != len)
        //    {
        //        if (Uri.IsHexEncoding(str, i))
        //            sb.Append(Uri.HexUnescape(str, ref i));
        //        else
        //            sb.Append(str[i++]);
        //    }
        //    return sb.ToString();
        //} 
        public FileResult Export2Excel(string w)   //List<cls预算单> list
        {
            //string ww = UnEscape(w);

            //DataTable dt = ToDataTable(list);
            DataTable dt = main.获取一般数据集(w);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                row1.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString().Trim());
                }
            }
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }





        public ActionResult StepUpfiles()
        {
            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }
            if (acc_yusuan == null)
            {
                try { acc_yusuan = (cls预算单)Session["qzacc_yusuan"]; }
                catch { }
            }
            if (acc_yusuan == null)
            { acc_yusuan = new cls预算单(); }

            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");
            Session["qzacc_yusuan"] = acc_yusuan;   //Session再保存下，TempData在文件上传中好像很快失效

            return View();
        }

        // 上传文件
        //需要把upload.js重新命名，否则可能执行不到这个UpLoadProcess
        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }
            if (acc_yusuan == null)
            {
                try { acc_yusuan = (cls预算单)Session["qzacc_yusuan"]; }
                catch { }
            }
            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");
            Session["qzacc_yusuan"] = acc_yusuan;   //Session再保存下，TempData在文件上传中好像很快失效


            //检测
            if (acc_yusuan == null)
            {
                return Json(new { jsonrpc = 2.0, error = "上传失败，时间过期或未知的记录", id = id, localfile = name, filePath = "" });
            }
            if (Request.Files.Count == 0)
            {
                //return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
                return Json(new { jsonrpc = 2.0, error = "保存失败", id = id, localfile = name, filePath = "" });
            }

            var fileName = Path.GetFileName(file.FileName).Replace("'", " ");   //可能有单引号
            DateTime dat1 = acc_yusuan.申请日期;
            if (dat1 < DateTime.Parse("1950-01-01"))
            {
                dat1 = DateTime.Now;
            }
            string str1 = "";
            str1 = @"Upfiles\" + dat1.Year.ToString() + @"\" + dat1.ToString("yyyyMMdd") + @"\" + acc_yusuan.ID.ToString();
            str1 = Path.Combine(HttpRuntime.AppDomainAppPath, str1);     //Server.MapPath("~/Upfiles");
            if (!str1.EndsWith(@"\")) str1 = str1 + @"\";

            if (!Directory.Exists(str1)) Directory.CreateDirectory(str1);
            str1 = Path.Combine(str1, fileName);
            if (System.IO.File.Exists(str1))
                System.IO.File.Delete(str1);
            file.SaveAs(str1);

            string filePathName = str1;

            //判断上传后的文件是否存在或大小为0（在安卓手机上上传时，如果采用压缩，则文件总是为0）
            System.IO.FileInfo fileInfo = null;
            try
            {
                fileInfo = new System.IO.FileInfo(filePathName);
            }
            catch { }
            bool uploadfail = false;
            if (fileInfo != null && fileInfo.Exists)
            {
                if (fileInfo.Length < 500) uploadfail = true;
            }
            else
            {
                uploadfail = true;
            }
            if (uploadfail)
            {
                return Json(new { jsonrpc = 2.0, error = "未成功上传", id = id, localfile = name, filePath = filePathName });
            }

            str1 = "";
            str1 = Budget.保存新增附件(acc_yusuan.ID.ToString(), fileName);

            //return Json(new
            //{
            //    jsonrpc = "2.0",
            //    id = id,
            //    filePath = urlPath + "/" + filePathName,
            //    error="0",
            //    localfile = name
            //});
            return Json(new { jsonrpc = 2.0, error = "0", id = id, localfile = name, filePath = filePathName });

        }




        //public ActionResult AddEvidence(string sID)   //增加附件
        //{
        //    string s1 = sID;
        //    if ((s1 == string.Empty) | (s1 == null)) s1 = "";

        //    if (s1 == "")
        //    {
        //        return RedirectToAction("Apprec", "Apply");
        //    }
        //    List<cls申办签证> sbs = Apply.获取申办记录s("where ID='" + s1 + "'");
        //    cls申办签证 shenban = null;
        //    try { shenban = sbs[0]; }
        //    catch { }
        //    if (shenban == null)
        //    {
        //        return RedirectToAction("Apprec", "Apply");
        //    }

        //    List<cls签证材料> xuyaocl = Apply.获取签证材料(shenban.地区, shenban.签证类型);
        //    ViewBag.xuyaocl = xuyaocl;

        //    shenban.编程用 = "1";
        //    TempData["shenban"] = shenban;
        //    TempData.Keep("shenban");
        //    Session["qzshenban"] = shenban;   //Session再保存下，TempData在文件上传中好像很快失效

        //    return RedirectToAction("StepUpfiles", "Apply");
        //}

        public ActionResult DelMaterial(string JZID, string CLID)   //删除预算的某个材料
        {
            string s1 = JZID;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = CLID;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";

            if ((s1 == "") | (s2 == ""))
            {
                return Content("100001");
            }

            string str1 = "delete A预算附件 where ID='" + s2 + "'";
            if (main.执行SQL命令(str1) < 1)
            {
                return Content("100001");
            }

            return Content("123456789");
        }

        public ActionResult Material(string sID, int viewonly)   //附件显示
        {
            string s1 = sID;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls预算单 acc_yusuan = null;
            try { acc_yusuan = (cls预算单)TempData["acc_yusuan"]; }
            catch { }
            TempData["acc_yusuan"] = acc_yusuan;
            TempData.Keep("acc_yusuan");

            List<cls附件> fjs = Budget.获取附件s("where 预算ID='" + s1 + "' order by 服务器文件名");
            ViewBag.JZID = s1;
            string rq = "";
            if (acc_yusuan == null)
            {
                try
                {
                    rq = DateTime.Parse(main.获取一般数据集("select 申请日期 from A预算 where ID='" + s1 + "'").Rows[0][0].ToString()).ToString("yyyyMMdd");
                }
                catch { }
            }
            else
            {
                rq = acc_yusuan.申请日期.ToString("yyyyMMdd");
            }
            ViewBag.sqrq = rq;
            ViewBag.viewonly = viewonly.ToString();

            return View(fjs);
        }

        public ActionResult Steps(string sID)   //处理过程显示
        {
            string s1 = sID;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            
            List<cls办理过程> fjs = Budget.获取办理过程s("where 相关ID='" + s1 + "' order by 处理日期");
            ViewBag.JZID = s1;            

            return View(fjs);
        }


        #endregion
    }
}
