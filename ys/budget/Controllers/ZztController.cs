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
    public class ZztController : Controller
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
        
        //中证通订单查询
        [HttpPost]
        public ActionResult OrderCx(string t1, string t2, string t3, string t4, string t5, string t6, string t7, string t8)
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

            sFld[0] = "订单日期";
            sFld[1] = "订单日期";
            sFld[2] = "订单号";
            sFld[3] = "当事人";
            sFld[4] = "承办公证处";
            sFld[5] = "是否加急";
            sFld[6] = "实际收入金额";
            sFld[7] = "实际收入金额";
            
            sOpe[0] = ">='";
            sOpe[1] = "<='";
            sOpe[2] = "like";
            sOpe[3] = "like";
            sOpe[4] = "like";
            sOpe[5] = "='";
            sOpe[6] = ">=";
            sOpe[7] = "<=";
           
            sVal[0] = s1;
            if (s1 != "")
            {
                try { sVal[0] = DateTime.Parse(s1).ToString("yyyy-MM-dd 00:00:00"); }
                catch { }
            }
            sVal[1] = s2;
            if (s2 != "")
            {
                try { sVal[1] = DateTime.Parse(s2).ToString("yyyy-MM-dd 23:59:59"); }
                catch { }
            }
            sVal[2] = s3;
            sVal[3] = s4;
            sVal[4] = s5;
            sVal[5] = s6;
            sVal[6] = s7;
            sVal[7] = s8;

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim() + " order by 订单日期 desc,订单号";

            List<cls中证通订单> sbs = Zzt.获取中证通订单s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");

            TempData["sbcxs"] = sbs;
            TempData.Keep("sbcxs");

            TempData["sbcx_t1"] = s1; TempData["sbcx_t2"] = s2; TempData["sbcx_t3"] = s3; TempData["sbcx_t4"] = s4; TempData["sbcx_t5"] = s5; TempData["sbcx_t6"] = s6; TempData["sbcx_t7"] = s7; TempData["sbcx_t8"] = s8;
            TempData.Keep("sbcx_t1"); TempData.Keep("sbcx_t2"); TempData.Keep("sbcx_t3"); TempData.Keep("sbcx_t4"); TempData.Keep("sbcx_t5"); TempData.Keep("sbcx_t6"); TempData.Keep("sbcx_t7"); TempData.Keep("sbcx_t8");

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }

        public ActionResult OrderList(int p, int r, int t)  //订单实际收支输入
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            
            string stp = "";
            if (t == 0) stp = "中证通订单实际收支";
            if (t == 2) stp = "中证通订单查询";
            if (stp == "") return RedirectToAction("Noauthorization", "Home");
            if (!登录.登录权限验证1(stp)) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            ViewBag.refesh = r;
            if (r == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["stp"] = t.ToString();

            List<cls中证通订单> sbs = new List<cls中证通订单>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls中证通订单>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls中证通订单>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 50;   //每页50行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls中证通订单> sbs1 = new List<cls中证通订单>();
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
        public ActionResult OrderActual(string id)    //订单实际收支编辑
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls中证通订单 acc_dingdan = null;
            try
            {
                acc_dingdan = Zzt.获取中证通订单s("where ID='" + s1 + "'")[0];
            }
            catch { }
            if (acc_dingdan == null)
            {
                acc_dingdan = new cls中证通订单();
            }

            TempData["acc_dingdan"] = acc_dingdan;
            TempData.Keep("acc_dingdan");

            return View();
        }

        public ActionResult OrderStatic(string id)    //订单预算收支编辑
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls中证通订单 acc_dingdan = null;
            try
            {
                acc_dingdan = Zzt.获取中证通订单s("where ID='" + s1 + "'")[0];
            }
            catch { }
            if (acc_dingdan == null)
            {
                acc_dingdan = new cls中证通订单();
            }

            TempData["acc_dingdan"] = acc_dingdan;
            TempData.Keep("acc_dingdan");

            return View();
        }

        //保存预算收支
        [HttpPost]
        public ActionResult SaveYsSz(bool savetodb, string tid, string tjzid, string szlx, string szf, string szje, string szrq, string szbz, string szlx1)
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

            cls中证通订单 acc_dingdan = null;
            try { acc_dingdan = (cls中证通订单)TempData["acc_dingdan"]; }
            catch { }

            if (acc_dingdan == null)
            {
                returnstr = "300001";    //无效预算记录
                return Content(returnstr);
            }

            if (acc_dingdan.预算收支 == null)
            {
                acc_dingdan.预算收支 = new List<cls中证通订单预算收支>();
            }

            cls中证通订单预算收支 yssz = new cls中证通订单预算收支();
            bool isnew = true;
            for (i = 0; i < acc_dingdan.预算收支.Count; i++)
            {
                if (acc_dingdan.预算收支[i].ID.ToString() == tid)
                {
                    yssz = acc_dingdan.预算收支[i];
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
            yssz.证书ID = sjzid;
            yssz.收支 = lx;
            yssz.类型 = s5;
            yssz.借贷方 = s1;
            yssz.金额 = decimal.Round(CommonFunctions.ValDec(s2), 2);
            yssz.日期 = DateTime.Parse(s3);
            yssz.备注 = s4;
            string ln = "";
            ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((ln == string.Empty) | (ln == null)) ln = "";
            yssz.录入人 = ln;
            yssz.录入日期 = DateTime.Now;

            if (savetodb)
            {
                if (Zzt.保存预算收支(sjzid, yssz) != "")
                {
                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (isnew)
            {
                acc_dingdan.预算收支.Add(yssz);
            }

            TempData["acc_dingdan"] = acc_dingdan;
            TempData.Keep("acc_dingdan");

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

            cls中证通订单 acc_dingdan = null;
            try { acc_dingdan = (cls中证通订单)TempData["acc_dingdan"]; }
            catch { }

            if (acc_dingdan == null)
            {
                returnstr = "300001";    //无效记录
                return Content(returnstr);
            }

            if (savetodb)
            {
                if (Zzt.删除实际收支(tid) < 1)
                {
                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (acc_dingdan.实际收支 == null)
            {
                acc_dingdan.实际收支 = new List<cls中证通订单实际收支>();
            }

            bool succ = false;
            for (i = 0; i < acc_dingdan.实际收支.Count; i++)
            {
                if (acc_dingdan.实际收支[i].ID.ToString() == tid)
                {
                    acc_dingdan.实际收支.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            if (!succ)
            {
                returnstr = "300002";    //无该实际收支
                return Content(returnstr);
            }

            TempData["acc_dingdan"] = acc_dingdan;
            TempData.Keep("acc_dingdan");

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

            cls中证通订单 acc_dingdan = null;
            try { acc_dingdan = (cls中证通订单)TempData["acc_dingdan"]; }
            catch { }

            if (acc_dingdan == null)
            {
                returnstr = "300001";    //无效预算记录
                return Content(returnstr);
            }

            if (acc_dingdan.实际收支 == null)
            {
                acc_dingdan.实际收支 = new List<cls中证通订单实际收支>();
            }

            cls中证通订单实际收支 sjsz = new cls中证通订单实际收支();
            bool isnew = true;
            for (i = 0; i < acc_dingdan.实际收支.Count; i++)
            {
                if (acc_dingdan.实际收支[i].ID.ToString() == tid)
                {
                    sjsz = acc_dingdan.实际收支[i];
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
            sjsz.证书ID = sjzid;
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
                if (Zzt.保存实际收支(sjzid, sjsz) != "")
                {
                    returnstr = "300005";    //保存到数据库出错
                    return Content(returnstr);
                }
            }

            if (isnew)
            {
                acc_dingdan.实际收支.Add(sjsz);
            }

            TempData["acc_dingdan"] = acc_dingdan;
            TempData.Keep("acc_dingdan");

            returnstr = "123456789";
            return Content(returnstr);
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
            
            List<cls中证通订单> ysds = new List<cls中证通订单>();
            s1 = "where ID in ('" + s1.Replace(" ", "','") + "')";
            ysds = Zzt.获取中证通订单s(s1);
            this.TempData["plzztdd"] = ysds;

            i = s.Length;
            returnstr = i.ToString();
            return Content(returnstr);
        }

        public ActionResult BatchActual()
        {
            List<cls中证通订单> ysds = this.TempData["plzztdd"] as List<cls中证通订单>;
            ViewBag.ysds = ysds;

            this.TempData["plzztdd"] = ysds;
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

            List<cls中证通订单> ysds = null;
            try
            {
                ysds = this.TempData["plzztdd"] as List<cls中证通订单>;
                this.TempData["plzztdd"] = ysds;
            }
            catch { }

            if (ysds == null)
            {
                returnstr = "300001";    //无效订单记录
                return Content(returnstr);
            }

            cls中证通订单实际收支 sjsz = new cls中证通订单实际收支();
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

            if (Zzt.保存实际收支批量(ysds, sjsz, lx) != "")
            {
                returnstr = "300005";    //保存到数据库出错
                return Content(returnstr);
            }

            //string str1 = "";
            //foreach (cls预算单 yd in ysds)
            //{
            //    str1 = str1 + yd.ID.ToString() + "=" + yd.管理_1 + " ";
            //}

            this.TempData["plzztdd"] = ysds;

            returnstr = "123456789";
            return Content(returnstr);
        }


        /// <summary>
        /// 浏览订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderView(string id)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            cls中证通订单 acc_dingdan = null;
            try
            {
                acc_dingdan = Zzt.获取中证通订单s("where ID='" + s1 + "'")[0];
            }
            catch { }
            if (acc_dingdan == null)
            {
                acc_dingdan = new cls中证通订单();
            }

            ViewBag.acc_dingdan = acc_dingdan;

            return View();
        }




        #region 预算通用查询
        public ActionResult ApplyQuery()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            if (!登录.登录权限验证1("中证通订单查询")) return RedirectToAction("Noauthorization", "Home");

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

            TempData["wheresql_ApplyQueryCx_zzt"] = str1;
            TempData.Keep("wheresql_ApplyQueryCx_zzt");

            string returnstr = "123456789";
            return Content(returnstr);
        }
        public ActionResult ApplyQueryList()
        {
            string str1 = "where 1=2";

            try { str1 = TempData["wheresql_ApplyQueryCx_zzt"].ToString(); }
            catch { }
            if (str1 != "")
            {
                //str1 = str1 + " limit 0,5000";
                TempData["wheresql_ApplyQueryCx_zzt"] = str1;
                TempData.Keep("wheresql_ApplyQueryCx_zzt");
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

    }
}
