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
    [Authentication]
    public class ReportController : Controller
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

        #region 预算盈亏表

        //预算盈亏表
        public ActionResult rpt1()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("预算盈亏表")) return RedirectToAction("Noauthorization", "Home");

            List<string> cbzxs = Report.获取公司名称();
            ViewBag.cbzxs = cbzxs;

            return View();
        }
        public ActionResult rpt1a()   //汇总表
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";

            try { str1 = TempData["wheresql_rpt1_gsm"].ToString(); }
            catch { }
            ViewBag.wheresql_rpt1_gsm = str1;

            try { str2 = TempData["wheresql_rpt1_rq1"].ToString(); }
            catch { }
            ViewBag.wheresql_rpt1_rq1 = str2;

            try { str3 = TempData["wheresql_rpt1_rq2"].ToString(); }
            catch { }
            ViewBag.wheresql_rpt1_rq2 = str3;

            List<cls预算盈亏汇总表> rpts = new List<cls预算盈亏汇总表>();
            try { rpts = (List<cls预算盈亏汇总表>)TempData["rpts"]; }
            catch { }
            cls预算盈亏汇总表s rp = new cls预算盈亏汇总表s();
            rp.记录集 = rpts;
            return View(rp);
        }

        public ActionResult rpt1b(string gsm, string rq1, string rq2)   //分类明细表:1
        {
            ViewBag.gsm = gsm;
            ViewBag.wheresql_rpt1_gsm = gsm;
            ViewBag.wheresql_rpt1_rq1 = rq1;
            ViewBag.wheresql_rpt1_rq2 = rq2;

            List<cls预算盈亏明细表> rpts = new List<cls预算盈亏明细表>();
            try { rpts = Report.获取预算盈亏明细表b(gsm, rq1, rq2); }
            catch { }
            cls预算盈亏明细表s rp = new cls预算盈亏明细表s();
            rp.记录集 = rpts;
            return View(rp);
        }
        public ActionResult rpt1c(string gsm, string rq1, string rq2)   //分类明细表:2
        {
            ViewBag.gsm = gsm;
            ViewBag.wheresql_rpt1_gsm = gsm;
            ViewBag.wheresql_rpt1_rq1 = rq1;
            ViewBag.wheresql_rpt1_rq2 = rq2;

            List<cls预算盈亏明细表> rpts = new List<cls预算盈亏明细表>();
            try { rpts = Report.获取预算盈亏明细表c(gsm, rq1, rq2); }
            catch { }
            cls预算盈亏明细表s rp = new cls预算盈亏明细表s();
            rp.记录集 = rpts;
            return View(rp);
        }

        //预算盈亏表查询
        [HttpPost]
        public ActionResult rptrst1(string t0, string t1, string t2)
        {
            string s0 = t0;
            if ((s0 == string.Empty) | (s0 == null)) s0 = "";
            string s1 = t1;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = t2;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";

            string str1 = "";
            string str2 = "";
            int i = 0;
            string[] sOpe = new string[3];
            string[] sFld = new string[3];
            string[] sVal = new string[3];

            sFld[0] = "成本中心编号";
            sFld[1] = "申请日期";
            sFld[2] = "申请日期";

            sOpe[0] = "in";
            sOpe[1] = ">='";
            sOpe[2] = "<='";

            sVal[0] = s0;
            try
            {
                sVal[1] = DateTime.Parse(s1).ToString("yyyy-MM-dd 00:00:00");
            }
            catch { }
            try
            {
                sVal[2] = DateTime.Parse(s2).ToString("yyyy-MM-dd 23:59:59");
            }
            catch { }

            if (sVal[0] != "")
            {
                List<string> cbzxbhs = Report.获取某公司所有成本中心编号(sVal[0]);
                str2 = "";
                for (i = 0; i < cbzxbhs.Count; i++)
                {
                    if (str2 != "") str2 = str2 + ",";
                    str2 = str2 + "'" + cbzxbhs[i] + "'";
                }
                str2 = "(" + str2 + ")";
                sVal[0] = sVal[0].Replace(sVal[0], str2);
            }

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();

            str1 = str1 + " and 类型=1";   //只统计项目预算

            List<cls预算盈亏汇总表> rpts = Report.获取预算盈亏汇总表(str1);

            //把查询条件分开存放起来备用
            TempData["wheresql_rpt1_gsm"] = sVal[0];
            TempData.Keep("wheresql_rpt1_gsm");
            TempData["wheresql_rpt1_rq1"] = sVal[1];
            TempData.Keep("wheresql_rpt1_rq1");
            TempData["wheresql_rpt1_rq2"] = sVal[2];
            TempData.Keep("wheresql_rpt1_rq2");

            TempData["rpts"] = rpts;
            TempData.Keep("rpts");
            
            string returnstr = "123456789";
            return Content(returnstr);
        }

        public FileResult Export2Excel_rpt1a(string gsm, string rq1, string rq2)
        {
            string str1 = "";
            int i = 0;

            string[] sOpe = new string[3];
            string[] sFld = new string[3];
            string[] sVal = new string[3];

            sFld[0] = "成本中心编号";
            sFld[1] = "申请日期";
            sFld[2] = "申请日期";
            sOpe[0] = "in";
            sOpe[1] = ">='";
            sOpe[2] = "<='";
            sVal[0] = gsm;
            sVal[1] = rq1;
            sVal[2] = rq2;
            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();

            List<cls预算盈亏汇总表> rpts = Report.获取预算盈亏汇总表(str1);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";

            row1.CreateCell(0).SetCellValue("公司名称");
            row1.CreateCell(1).SetCellValue("项目预算收入(A)");
            row1.CreateCell(2).SetCellValue("销项税额(B)");
            row1.CreateCell(3).SetCellValue("项目预算支出(C)");
            row1.CreateCell(4).SetCellValue("进项税额(D)");
            row1.CreateCell(5).SetCellValue("应交税额(E=B-D)");
            row1.CreateCell(6).SetCellValue("GP(F=A-C-E)");
            row1.CreateCell(7).SetCellValue("固定费用支出(G)");
            row1.CreateCell(8).SetCellValue("盈亏(H=F-G)");
            
            //将数据逐步写入sheet1各个行
            for (i = 0; i < rpts.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(rpts[i].公司名称.Trim());
                rowtemp.CreateCell(1).SetCellValue(rpts[i].项目预算收入.ToString());
                rowtemp.CreateCell(2).SetCellValue(rpts[i].销项税额.ToString());
                rowtemp.CreateCell(3).SetCellValue(rpts[i].项目预算支出.ToString());
                rowtemp.CreateCell(4).SetCellValue(rpts[i].进项税额.ToString());
                rowtemp.CreateCell(5).SetCellValue(rpts[i].应交税额.ToString());
                rowtemp.CreateCell(6).SetCellValue(rpts[i].GP.ToString());
                rowtemp.CreateCell(7).SetCellValue(rpts[i].固定费用支出.ToString());
                rowtemp.CreateCell(8).SetCellValue(rpts[i].盈亏.ToString());
            }
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }

        public FileResult Export2Excel_rpt1b(string shouzhi, string gsm, string rq1, string rq2)
        {
            int i = 0;

            List<cls预算盈亏明细表> rpts = Report.获取预算盈亏明细表b(gsm, rq1, rq2);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";

            if (shouzhi == "收入")
            {
                row1.CreateCell(0).SetCellValue("序号");
                row1.CreateCell(1).SetCellValue("预算编号");
                row1.CreateCell(2).SetCellValue("子订单号");
                row1.CreateCell(3).SetCellValue("项目名称");
                row1.CreateCell(4).SetCellValue("付款方");
                row1.CreateCell(5).SetCellValue("项目状态");
                row1.CreateCell(6).SetCellValue("销售");
                row1.CreateCell(7).SetCellValue("收支类型");
                row1.CreateCell(8).SetCellValue("预算收入");
                row1.CreateCell(9).SetCellValue("销项税额");
                row1.CreateCell(10).SetCellValue("提交日期");

                //将数据逐步写入sheet1各个行
                int k = 0;
                for (i = 0; i < rpts.Count; i++)
                {
                    if (rpts[i].收支 == "收入")
                    {
                        k++;
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(k);
                        rowtemp.CreateCell(0).SetCellValue(k.ToString());
                        rowtemp.CreateCell(1).SetCellValue(rpts[i].预算编号.Trim());
                        rowtemp.CreateCell(2).SetCellValue(rpts[i].子订单号.Trim());
                        rowtemp.CreateCell(3).SetCellValue(rpts[i].项目名称.Trim());
                        rowtemp.CreateCell(4).SetCellValue(rpts[i].付款方.Trim());
                        rowtemp.CreateCell(5).SetCellValue(rpts[i].项目状态.Trim());
                        rowtemp.CreateCell(6).SetCellValue(rpts[i].销售.Trim());
                        rowtemp.CreateCell(7).SetCellValue(rpts[i].收支类型.Trim());
                        rowtemp.CreateCell(8).SetCellValue(rpts[i].预算收入.ToString());
                        rowtemp.CreateCell(9).SetCellValue(rpts[i].销项税额.ToString());
                        rowtemp.CreateCell(10).SetCellValue(rpts[i].提交日期.Trim());
                    }
                }
            }
            if (shouzhi == "支出")
            {
                row1.CreateCell(0).SetCellValue("序号");
                row1.CreateCell(1).SetCellValue("预算编号");
                row1.CreateCell(2).SetCellValue("子订单号");
                row1.CreateCell(3).SetCellValue("项目名称");
                row1.CreateCell(4).SetCellValue("付款方");
                row1.CreateCell(5).SetCellValue("项目状态");
                row1.CreateCell(6).SetCellValue("销售");
                row1.CreateCell(7).SetCellValue("收支类型");
                row1.CreateCell(8).SetCellValue("预算支出");
                row1.CreateCell(9).SetCellValue("进项税额");
                row1.CreateCell(10).SetCellValue("提交日期");

                //将数据逐步写入sheet1各个行
                int k = 0;
                for (i = 0; i < rpts.Count; i++)
                {
                    if (rpts[i].收支 == "支出")
                    {
                        k++;
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(k);
                        rowtemp.CreateCell(0).SetCellValue(k.ToString());
                        rowtemp.CreateCell(1).SetCellValue(rpts[i].预算编号.Trim());
                        rowtemp.CreateCell(2).SetCellValue(rpts[i].子订单号.Trim());
                        rowtemp.CreateCell(3).SetCellValue(rpts[i].项目名称.Trim());
                        rowtemp.CreateCell(4).SetCellValue(rpts[i].付款方.Trim());
                        rowtemp.CreateCell(5).SetCellValue(rpts[i].项目状态.Trim());
                        rowtemp.CreateCell(6).SetCellValue(rpts[i].销售.Trim());
                        rowtemp.CreateCell(7).SetCellValue(rpts[i].收支类型.Trim());
                        rowtemp.CreateCell(8).SetCellValue(rpts[i].预算支出.ToString());
                        rowtemp.CreateCell(9).SetCellValue(rpts[i].进项税额.ToString());
                        rowtemp.CreateCell(10).SetCellValue(rpts[i].提交日期.Trim());
                    }
                }
            }

            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }

        public FileResult Export2Excel_rpt1c(string gsm, string rq1, string rq2)
        {
            int i = 0;

            List<cls预算盈亏明细表> rpts = Report.获取预算盈亏明细表c(gsm, rq1, rq2);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("预算编号");
            row1.CreateCell(2).SetCellValue("预算名称");
            row1.CreateCell(3).SetCellValue("实际支出金额");
            row1.CreateCell(4).SetCellValue("实际付款日期");
            row1.CreateCell(5).SetCellValue("预算状态");
            row1.CreateCell(6).SetCellValue("备注");

            //将数据逐步写入sheet1各个行
            int k = 0;
            for (i = 0; i < rpts.Count; i++)
            {
                k++;
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(k);
                rowtemp.CreateCell(0).SetCellValue(k.ToString());
                rowtemp.CreateCell(1).SetCellValue(rpts[i].预算编号.Trim());
                rowtemp.CreateCell(2).SetCellValue(rpts[i].项目名称.Trim());
                rowtemp.CreateCell(3).SetCellValue(rpts[i].实际支出.ToString());
                rowtemp.CreateCell(4).SetCellValue(rpts[i].提交日期.Trim());
                rowtemp.CreateCell(5).SetCellValue(rpts[i].项目状态.Trim());
                rowtemp.CreateCell(6).SetCellValue("");
            }

            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }

        #endregion

        #region 实际盈亏表
        //实际盈亏表
        public ActionResult rpt2()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("实际盈亏表")) return RedirectToAction("Noauthorization", "Home");

            List<string> cbzxs = Report.获取公司名称();
            ViewBag.cbzxs = cbzxs;

            return View();
        }
        public ActionResult rpt2a()   //汇总表
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";

            try { str1 = TempData["wheresql_rpt2_gsm"].ToString(); }
            catch { }
            ViewBag.wheresql_rpt2_gsm = str1;

            try { str2 = TempData["wheresql_rpt2_rq1"].ToString(); }
            catch { }
            ViewBag.wheresql_rpt2_rq1 = str2;

            try { str3 = TempData["wheresql_rpt2_rq2"].ToString(); }
            catch { }
            ViewBag.wheresql_rpt2_rq2 = str3;

            List<cls实际盈亏汇总表> rpts = new List<cls实际盈亏汇总表>();
            try { rpts = (List<cls实际盈亏汇总表>)TempData["rpts"]; }
            catch { }
            cls实际盈亏汇总表s rp = new cls实际盈亏汇总表s();
            rp.记录集 = rpts;
            return View(rp);
        }

        public ActionResult rpt2b(string gsm, string rq1, string rq2)   //分类明细表:1
        {
            ViewBag.gsm = gsm;
            ViewBag.wheresql_rpt2_gsm = gsm;
            ViewBag.wheresql_rpt2_rq1 = rq1;
            ViewBag.wheresql_rpt2_rq2 = rq2;

            List<cls实际盈亏明细表> rpts = new List<cls实际盈亏明细表>();
            try { rpts = Report.获取实际盈亏明细表b(gsm, rq1, rq2); }
            catch { }
            cls实际盈亏明细表s rp = new cls实际盈亏明细表s();
            rp.记录集 = rpts;
            return View(rp);
        }

        public ActionResult rpt2c(string gsm, string rq1, string rq2)   //分类明细表:2
        {
            ViewBag.gsm = gsm;
            ViewBag.wheresql_rpt2_gsm = gsm;
            ViewBag.wheresql_rpt2_rq1 = rq1;
            ViewBag.wheresql_rpt2_rq2 = rq2;

            List<cls实际盈亏明细表> rpts = new List<cls实际盈亏明细表>();
            try { rpts = Report.获取实际盈亏明细表c(gsm, rq1, rq2); }
            catch { }
            cls实际盈亏明细表s rp = new cls实际盈亏明细表s();
            rp.记录集 = rpts;
            return View(rp);
        }

        //实际盈亏表查询
        [HttpPost]
        public ActionResult rptrst2(string t0, string t1, string t2)
        {
            string s0 = t0;
            if ((s0 == string.Empty) | (s0 == null)) s0 = "";
            string s1 = t1;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = t2;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";

            string str1 = "";
            string str2 = "";
            int i = 0;
            string[] sOpe = new string[3];
            string[] sFld = new string[3];
            string[] sVal = new string[3];

            sFld[0] = "成本中心编号";
            sFld[1] = "收支日期";
            sFld[2] = "收支日期";

            sOpe[0] = "in";
            sOpe[1] = ">='";
            sOpe[2] = "<='";

            sVal[0] = s0;
            try
            {
                sVal[1] = DateTime.Parse(s1).ToString("yyyy-MM-dd 00:00:00");
            }
            catch { }
            try
            {
                sVal[2] = DateTime.Parse(s2).ToString("yyyy-MM-dd 23:59:59");
            }
            catch { }

            if (sVal[0] != "")
            {
                List<string> cbzxbhs = Report.获取某公司所有成本中心编号(sVal[0]);
                str2 = "";
                for (i = 0; i < cbzxbhs.Count; i++)
                {
                    if (str2 != "") str2 = str2 + ",";
                    str2 = str2 + "'" + cbzxbhs[i] + "'";
                }
                str2 = "(" + str2 + ")";
                sVal[0] = sVal[0].Replace(sVal[0], str2);
            }

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();

            str1 = str1 + " and 类型=1";   //只统计项目

            List<cls实际盈亏汇总表> rpts = Report.获取实际盈亏汇总表(str1);

            //把查询条件分开存放起来备用
            TempData["wheresql_rpt2_gsm"] = sVal[0];
            TempData.Keep("wheresql_rpt2_gsm");
            TempData["wheresql_rpt2_rq1"] = sVal[1];
            TempData.Keep("wheresql_rpt2_rq1");
            TempData["wheresql_rpt2_rq2"] = sVal[2];
            TempData.Keep("wheresql_rpt2_rq2");

            TempData["rpts"] = rpts;
            TempData.Keep("rpts");

            string returnstr = "123456789";
            return Content(returnstr);
        }


        public FileResult Export2Excel_rpt2a(string gsm, string rq1, string rq2)
        {
            string str1 = "";
            int i = 0;

            string[] sOpe = new string[3];
            string[] sFld = new string[3];
            string[] sVal = new string[3];

            sFld[0] = "成本中心编号";
            sFld[1] = "申请日期";
            sFld[2] = "申请日期";
            sOpe[0] = "in";
            sOpe[1] = ">='";
            sOpe[2] = "<='";
            sVal[0] = gsm;
            sVal[1] = rq1;
            sVal[2] = rq2;
            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();

            List<cls实际盈亏汇总表> rpts = Report.获取实际盈亏汇总表(str1);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";

            row1.CreateCell(0).SetCellValue("公司名称");
            row1.CreateCell(1).SetCellValue("项目实际收入(A)");
            row1.CreateCell(2).SetCellValue("销项税额(B)");
            row1.CreateCell(3).SetCellValue("项目实际支出(C)");
            row1.CreateCell(4).SetCellValue("进项税额(D)");
            row1.CreateCell(5).SetCellValue("应交税额(E=B-D)");
            row1.CreateCell(6).SetCellValue("GP(F=A-C-E)");
            row1.CreateCell(7).SetCellValue("固定费用支出(G)");
            row1.CreateCell(8).SetCellValue("盈亏(H=F-G)");

            //将数据逐步写入sheet1各个行
            for (i = 0; i < rpts.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(rpts[i].公司名称.Trim());
                rowtemp.CreateCell(1).SetCellValue(rpts[i].项目实际收入.ToString());
                rowtemp.CreateCell(2).SetCellValue(rpts[i].销项税额.ToString());
                rowtemp.CreateCell(3).SetCellValue(rpts[i].项目实际支出.ToString());
                rowtemp.CreateCell(4).SetCellValue(rpts[i].进项税额.ToString());
                rowtemp.CreateCell(5).SetCellValue(rpts[i].应交税额.ToString());
                rowtemp.CreateCell(6).SetCellValue(rpts[i].GP.ToString());
                rowtemp.CreateCell(7).SetCellValue(rpts[i].固定费用支出.ToString());
                rowtemp.CreateCell(8).SetCellValue(rpts[i].盈亏.ToString());
            }
            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }

        public FileResult Export2Excel_rpt2b(string shouzhi, string gsm, string rq1, string rq2)
        {
            int i = 0;

            List<cls实际盈亏明细表> rpts = Report.获取实际盈亏明细表b(gsm, rq1, rq2);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";

            if (shouzhi == "收入")
            {
                row1.CreateCell(0).SetCellValue("序号");
                row1.CreateCell(1).SetCellValue("预算编号");
                row1.CreateCell(2).SetCellValue("子订单号");
                row1.CreateCell(3).SetCellValue("项目名称");
                row1.CreateCell(4).SetCellValue("付款方");
                row1.CreateCell(5).SetCellValue("项目状态");
                row1.CreateCell(6).SetCellValue("销售");
                row1.CreateCell(7).SetCellValue("收支类型");
                row1.CreateCell(8).SetCellValue(" ");   //预算收入
                row1.CreateCell(9).SetCellValue("实际收入");
                row1.CreateCell(10).SetCellValue("销项税额");
                row1.CreateCell(11).SetCellValue("提交日期");

                //将数据逐步写入sheet1各个行
                int k = 0;
                for (i = 0; i < rpts.Count; i++)
                {
                    if (rpts[i].收支 == "收入")
                    {
                        k++;
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(k);
                        rowtemp.CreateCell(0).SetCellValue(k.ToString());
                        rowtemp.CreateCell(1).SetCellValue(rpts[i].预算编号.Trim());
                        rowtemp.CreateCell(2).SetCellValue(rpts[i].子订单号.Trim());
                        rowtemp.CreateCell(3).SetCellValue(rpts[i].项目名称.Trim());
                        rowtemp.CreateCell(4).SetCellValue(rpts[i].付款方.Trim());
                        rowtemp.CreateCell(5).SetCellValue(rpts[i].项目状态.Trim());
                        rowtemp.CreateCell(6).SetCellValue(rpts[i].销售.Trim());
                        rowtemp.CreateCell(7).SetCellValue(rpts[i].收支类型.Trim());
                        rowtemp.CreateCell(8).SetCellValue(rpts[i].预算收入.ToString());
                        rowtemp.CreateCell(9).SetCellValue(rpts[i].实际收入.ToString());
                        rowtemp.CreateCell(10).SetCellValue(rpts[i].销项税额.ToString());
                        rowtemp.CreateCell(11).SetCellValue(rpts[i].提交日期.Trim());
                    }
                }
            }
            if (shouzhi == "支出")
            {
                row1.CreateCell(0).SetCellValue("序号");
                row1.CreateCell(1).SetCellValue("预算编号");
                row1.CreateCell(2).SetCellValue("子订单号");
                row1.CreateCell(3).SetCellValue("项目名称");
                row1.CreateCell(4).SetCellValue("付款方");
                row1.CreateCell(5).SetCellValue("项目状态");
                row1.CreateCell(6).SetCellValue("销售");
                row1.CreateCell(7).SetCellValue("收支类型");
                row1.CreateCell(8).SetCellValue(" ");   //预算支出
                row1.CreateCell(9).SetCellValue("实际支出");
                row1.CreateCell(10).SetCellValue("进项税额");
                row1.CreateCell(11).SetCellValue("提交日期");

                //将数据逐步写入sheet1各个行
                int k = 0;
                for (i = 0; i < rpts.Count; i++)
                {
                    if (rpts[i].收支 == "支出")
                    {
                        k++;
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(k);
                        rowtemp.CreateCell(0).SetCellValue(k.ToString());
                        rowtemp.CreateCell(1).SetCellValue(rpts[i].预算编号.Trim());
                        rowtemp.CreateCell(2).SetCellValue(rpts[i].子订单号.Trim());
                        rowtemp.CreateCell(3).SetCellValue(rpts[i].项目名称.Trim());
                        rowtemp.CreateCell(4).SetCellValue(rpts[i].付款方.Trim());
                        rowtemp.CreateCell(5).SetCellValue(rpts[i].项目状态.Trim());
                        rowtemp.CreateCell(6).SetCellValue(rpts[i].销售.Trim());
                        rowtemp.CreateCell(7).SetCellValue(rpts[i].收支类型.Trim());
                        rowtemp.CreateCell(8).SetCellValue(rpts[i].预算支出.ToString());
                        rowtemp.CreateCell(9).SetCellValue(rpts[i].实际支出.ToString());
                        rowtemp.CreateCell(10).SetCellValue(rpts[i].进项税额.ToString());
                        rowtemp.CreateCell(11).SetCellValue(rpts[i].提交日期.Trim());
                    }
                }
            }

            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }

        public FileResult Export2Excel_rpt2c(string gsm, string rq1, string rq2)
        {
            int i = 0;

            List<cls实际盈亏明细表> rpts = Report.获取实际盈亏明细表c(gsm, rq1, rq2);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.RowStyle.FillBackgroundColor = "";

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("预算编号");
            row1.CreateCell(2).SetCellValue("预算名称");
            row1.CreateCell(3).SetCellValue("实际支出金额");
            row1.CreateCell(4).SetCellValue("实际付款日期");
            row1.CreateCell(5).SetCellValue("预算状态");
            row1.CreateCell(6).SetCellValue("备注");

            //将数据逐步写入sheet1各个行
            int k = 0;
            for (i = 0; i < rpts.Count; i++)
            {
                k++;
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(k);
                rowtemp.CreateCell(0).SetCellValue(k.ToString());
                rowtemp.CreateCell(1).SetCellValue(rpts[i].预算编号.Trim());
                rowtemp.CreateCell(2).SetCellValue(rpts[i].项目名称.Trim());
                rowtemp.CreateCell(3).SetCellValue(rpts[i].实际支出.ToString());
                rowtemp.CreateCell(4).SetCellValue(rpts[i].提交日期.Trim());
                rowtemp.CreateCell(5).SetCellValue(rpts[i].项目状态.Trim());
                rowtemp.CreateCell(6).SetCellValue("");
            }

            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }

        #endregion

        #region 应收应付表
        public ActionResult rpt3()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("应收应付表")) return RedirectToAction("Noauthorization", "Home");

            List<string> cbzxs = Report.获取公司名称();
            ViewBag.cbzxs = cbzxs;

            return View();
        }
        
        public ActionResult rpt3b(string gsm, string rq1, string rq2)
        {
            string s0 = gsm;
            if ((s0 == string.Empty) | (s0 == null)) s0 = "";
            string s1 = rq1;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = rq2;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";

            string str1 = "";
            string str2 = "";
            int i = 0;
            string[] sOpe = new string[3];
            string[] sFld = new string[3];
            string[] sVal = new string[3];

            sFld[0] = "成本中心编号";
            sFld[1] = "日期";
            sFld[2] = "日期";

            sOpe[0] = "in";
            sOpe[1] = ">='";
            sOpe[2] = "<='";

            sVal[0] = s0;
            try
            {
                sVal[1] = DateTime.Parse(s1).ToString("yyyy-MM-dd 00:00:00");
            }
            catch { }
            try
            {
                sVal[2] = DateTime.Parse(s2).ToString("yyyy-MM-dd 23:59:59");
            }
            catch { }

            if (sVal[0] != "")
            {
                List<string> cbzxbhs = Report.获取某公司所有成本中心编号(sVal[0]);
                str2 = "";
                for (i = 0; i < cbzxbhs.Count; i++)
                {
                    if (str2 != "") str2 = str2 + ",";
                    str2 = str2 + "'" + cbzxbhs[i] + "'";
                }
                str2 = "(" + str2 + ")";
                sVal[0] = sVal[0].Replace(sVal[0], str2);
            }

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();

            List<cls应收应付明细表> rpts = Report.获取应收应付明细表(str1, s0);
            TempData["wheresql_rpt3"] = str1;
            TempData.Keep("wheresql_rpt3");

            ViewBag.gsm = s0;
            
            cls应收应付明细表s rp = new cls应收应付明细表s();
            rp.记录集 = rpts;
            return View(rp);
        }
        #endregion

        #region 销售业绩表
        public ActionResult rpt4()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("业绩跟踪表")) return RedirectToAction("Noauthorization", "Home");

            List<string> cbzxs = Report.获取公司名称();
            ViewBag.cbzxs = cbzxs;

            return View();
        }

        public ActionResult rpt4b(string gsm, string rq1, string rq2)
        {
            string s0 = gsm;
            if ((s0 == string.Empty) | (s0 == null)) s0 = "";
            string s1 = rq1;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = rq2;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";

            string str1 = "";
            string str2 = "";
            int i = 0;
            string[] sOpe = new string[3];
            string[] sFld = new string[3];
            string[] sVal = new string[3];

            sFld[0] = "a.成本中心编号";
            sFld[1] = "a.申请日期";
            sFld[2] = "a.申请日期";

            sOpe[0] = "in";
            sOpe[1] = ">='";
            sOpe[2] = "<='";

            sVal[0] = s0;
            try
            {
                sVal[1] = DateTime.Parse(s1).ToString("yyyy-MM-dd 00:00:00");
            }
            catch { }
            try
            {
                sVal[2] = DateTime.Parse(s2).ToString("yyyy-MM-dd 23:59:59");
            }
            catch { }

            if (sVal[0] != "")
            {
                List<string> cbzxbhs = Report.获取某公司所有成本中心编号(sVal[0]);
                str2 = "";
                for (i = 0; i < cbzxbhs.Count; i++)
                {
                    if (str2 != "") str2 = str2 + ",";
                    str2 = str2 + "'" + cbzxbhs[i] + "'";
                }
                str2 = "(" + str2 + ")";
                sVal[0] = sVal[0].Replace(sVal[0], str2);
            }

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();

            List<cls销售业绩明细表> rpts = Report.获取销售业绩明细表(str1, s0);
            TempData["wheresql_rpt4"] = str1;
            TempData.Keep("wheresql_rpt4");

            ViewBag.gsm = s0;

            cls销售业绩明细表s rp = new cls销售业绩明细表s();
            rp.记录集 = rpts;
            return View(rp);
        }
        #endregion
        
        #region 中证通订单应收表
        public ActionResult rpt5()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("中证通订单应收表")) return RedirectToAction("Noauthorization", "Home");

            return View();
        }

        public ActionResult rpt5b(string zddh, string dsr, string gzc, string rq1, string rq2)
        {
            string s0 = zddh;
            if ((s0 == string.Empty) | (s0 == null)) s0 = "";
            string s1 = dsr;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = gzc;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = rq1;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = rq2;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";

            string str1 = "";
            string[] sOpe = new string[5];
            string[] sFld = new string[5];
            string[] sVal = new string[5];

            sFld[0] = "子订单号";
            sFld[1] = "当事人";
            sFld[2] = "承办公证处";
            sFld[3] = "申请日期";
            sFld[4] = "申请日期";

            sOpe[0] = "like";
            sOpe[1] = "like";
            sOpe[2] = "like";
            sOpe[3] = ">='";
            sOpe[4] = "<='";

            sVal[0] = s0;
            sVal[1] = s1;
            sVal[2] = s2;
            sVal[3] = s3;
            sVal[4] = s4;
            if (sVal[3] != "")
            {
                try
                {
                    sVal[3] = DateTime.Parse(sVal[3]).ToString("yyyy-MM-dd 00:00:00");
                }
                catch { }
            }
            if (sVal[4] != "")
            {
                try
                {
                    sVal[4] = DateTime.Parse(sVal[4]).ToString("yyyy-MM-dd 23:59:59");
                }
                catch { }
            }

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Trim();

            if (str1 == "")
                str1 = " where ";
            else
                str1 = str1 + " and ";
            str1 = str1 + " (预算收入金额>实际收入金额) ";

            string sql = "";
            List<cls中证通订单应收明细表> rpts = Report.获取中证通订单应收明细表(str1, ref sql);
            ViewBag.wheresql_rpt5 = sql;
            
            cls中证通订单应收明细表s rp = new cls中证通订单应收明细表s();
            rp.记录集 = rpts;
            return View(rp);
        }

        public FileResult Export2Excel_rpt5(string w)
        {
            string str1 = "";
            int i = 0;

            str1 = w;
            DataTable dt = main.获取一般数据集(str1);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    dt.Rows[i]["应收额"] = CommonFunctions.ValDec(dt.Rows[i]["预算收入金额"].ToString()) - CommonFunctions.ValDec(dt.Rows[i]["实际收入金额"].ToString());
                }
                catch { }
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
