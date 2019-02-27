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
    public class CheckController : Controller
    {
        [Authentication]   //[Authorization]//如果将此特性加在Controller上，那么访问这个Controller里面的方法都需要验证用户登录状态


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

        #region 批量对账申

        public ActionResult CheckZztQuery()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            if (!登录.登录权限验证1("中证通订单对账")) return RedirectToAction("Noauthorization", "Home");

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
        public ActionResult CheckZztQueryCx(string txtpaixu, string txtywlx, string txtsqrq1, string txtsqrq2, string txtysbh, string txtysmc, string txtsqr, string txtcbzx, string txtyssm, string txtxs, string txtclzt, string txtysfl, string txtzztdd, string txtxsxx, string txtqd, string txtgzc, string txtzddh, string txtsff)
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

            TempData["wheresql_CheckZztQueryCx"] = str1;
            TempData.Keep("wheresql_CheckZztQueryCx");

            string returnstr = "123456789";
            return Content(returnstr);
        }
        public ActionResult CheckZztQueryList()
        {
            string str1 = "where 1=2";

            try { str1 = TempData["wheresql_CheckZztQueryCx"].ToString(); }
            catch { }

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
