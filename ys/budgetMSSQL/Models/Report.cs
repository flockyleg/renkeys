using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IO;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace applyvisa.Models
{
    public class Report
    {
        public static List<string> 获取公司名称()
        {
            List<string> returnval = new List<string>();

            DataTable tb = main.获取一般数据集("select distinct 部门 from Z成本中心 where 停用=''");
            if (tb != null)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    returnval.Add(tb.Rows[i][0].ToString());
                }
            }
            return returnval;
        }
        public static List<string> 获取某公司所有成本中心编号(string 公司名)
        {
            List<string> returnval = new List<string>();

            DataTable tb = main.获取一般数据集("select 成本中心编号 from Z成本中心 where 停用='' and 部门='" + 公司名 + "'");
            if (tb != null)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    returnval.Add(tb.Rows[i][0].ToString());
                }
            }
            return returnval;
        }


        public static List<cls预算盈亏汇总表> 获取预算盈亏汇总表(string sWhere)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            List<cls预算盈亏汇总表> shenbans = new List<cls预算盈亏汇总表>();
            cls预算盈亏汇总表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            DataTable tbcbzx = main.获取一般数据集(数据库连接字符串, "select 成本中心编号,部门 from Z成本中心");

            str1 = "select 类型,成本中心编号,sum(预算收入金额) as 预算收入金额,sum(预算销项税额) as 预算销项税额,sum(预算支出金额) as 预算支出金额,sum(预算进项税额) as 预算进项税额,sum(实际收入金额) as 实际收入金额,sum(实际支出金额) as 实际支出金额 from V预算统计 " + sWhere + " and (预算状态='待完成' or 预算状态='已完成') group by 类型,成本中心编号";
            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                str1 = "";
                str1 = tb.Rows[k]["成本中心编号"].ToString();
                for (i = 0; i < tbcbzx.Rows.Count; i++)
                {
                    if (tbcbzx.Rows[i]["成本中心编号"].ToString() == str1)
                    {
                        str1 = tbcbzx.Rows[i]["部门"].ToString();
                        break;
                    }
                }
                if (str1 == "") str1 = tb.Rows[k]["成本中心编号"].ToString();

                sb = new cls预算盈亏汇总表();
                sb.ID = Guid.NewGuid().ToString();
                sb.公司名称 = str1;
                if (tb.Rows[k]["类型"].ToString() == "0")
                {
                    sb.项目预算收入 = 0.00m;
                    sb.销项税额 = 0.00m;
                    sb.项目预算支出 = 0.00m;
                    sb.进项税额 = 0.00m;
                    sb.应交税额 = 0.00m;
                    sb.GP = 0.00m;
                    sb.固定费用支出 = 0.00m;   //CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                    sb.盈亏 = 0.00m;
                }
                else
                {
                    sb.项目预算收入 = CommonFunctions.ValDec(tb.Rows[k]["预算收入金额"].ToString());
                    sb.销项税额 = CommonFunctions.ValDec(tb.Rows[k]["预算销项税额"].ToString());
                    sb.项目预算支出 = CommonFunctions.ValDec(tb.Rows[k]["预算支出金额"].ToString());
                    sb.进项税额 = CommonFunctions.ValDec(tb.Rows[k]["预算进项税额"].ToString());
                    sb.应交税额 = 0.00m;    //sb.销项税额 - sb.进项税额;
                    sb.GP = 0.00m;    //sb.项目预算收入 - sb.项目预算支出 - sb.应交税额;
                    sb.固定费用支出 = 0.00m;
                    sb.盈亏 = 0.00m;
                }
                shenbans.Add(sb);
            }

            List<cls预算盈亏汇总表> returns = new List<cls预算盈亏汇总表>();
            for (i = 0; i < shenbans.Count; i++)
            {
                bool b1 = true;
                for (k = 0; k < returns.Count; k++)
                {
                    if (shenbans[i].公司名称 == returns[k].公司名称)
                    {
                        returns[k].项目预算收入 = returns[k].项目预算收入 + shenbans[i].项目预算收入;
                        returns[k].销项税额 = returns[k].销项税额 + shenbans[i].销项税额;
                        returns[k].项目预算支出 = returns[k].项目预算支出 + shenbans[i].项目预算支出;
                        returns[k].进项税额 = returns[k].进项税额 + shenbans[i].进项税额;
                        returns[k].固定费用支出 = returns[k].固定费用支出 + shenbans[i].固定费用支出;
                        b1 = false;
                        break;
                    }
                }
                if (b1)
                {
                    returns.Add(shenbans[i]);
                }
            }

            for (k = 0; k < returns.Count; k++)
            {
                string 日期1 = CommonFunctions.ReadWordOnly(sWhere, "日期", false, true);
                日期1 = CommonFunctions.ReadWordOnly(日期1, "'", false, true);
                日期1 = CommonFunctions.ReadWordOnly(日期1, "'", true, true);
                string 日期2 = CommonFunctions.ReadLastWordOnly(sWhere, "日期", false, true);
                日期2 = CommonFunctions.ReadWordOnly(日期2, "'", false, true);
                日期2 = CommonFunctions.ReadWordOnly(日期2, "'", true, true);
                List<cls实际盈亏明细表> mx = 获取实际盈亏明细表c(returns[k].公司名称, 日期1, 日期2);
                decimal decmx = 0.00m;
                for (int m = 0; m < mx.Count; m++)
                {
                    decmx = decmx + mx[m].实际支出;
                }
                returns[k].固定费用支出 = decmx;
            }

            for (k = 0; k < returns.Count; k++)
            {
                returns[k].应交税额 = returns[k].销项税额 - returns[k].进项税额;
                returns[k].GP = returns[k].项目预算收入 - returns[k].项目预算支出 - returns[k].应交税额;
                returns[k].盈亏 = returns[k].GP - returns[k].固定费用支出;
            }
            return returns;
        }
        public static List<cls预算盈亏明细表> 获取预算盈亏明细表(string sWhere, string 公司名)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls预算盈亏明细表> shenbans = new List<cls预算盈亏明细表>();
            cls预算盈亏明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str2 = "";
            if (sWhere.IndexOf("成本中心编号") < 0)
            {
                List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
                str2 = "";
                for (i = 0; i < cbzxbhs.Count; i++)
                {
                    if (str2 != "") str2 = str2 + ",";
                    str2 = str2 + "'" + cbzxbhs[i] + "'";
                }
                if (str2 == "") str2 = "'" + 公司名 + "'";
                str2 = " and 成本中心编号 in (" + str2 + ")";
            }

            str1 = "select 类型,申请日期,预算编号,预算名称,业务类型,预算状态,销售,预算收入金额,预算销项税额,预算支出金额,预算进项税额,实际收入金额,实际支出金额 from V预算统计 " + sWhere + str2 + " and 预算状态<>'待申请' and 审批结果<>'驳回' and 核定结果<>'驳回' order by 申请日期";
            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls预算盈亏明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.付款方 = tb.Rows[k]["业务类型"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                sb.提交日期 = DateTime.Parse(tb.Rows[k]["申请日期"].ToString()).ToString("yyyy-MM-dd");
                sb.预算收入 = CommonFunctions.ValDec(tb.Rows[k]["预算收入金额"].ToString());
                sb.预算支出 = CommonFunctions.ValDec(tb.Rows[k]["预算支出金额"].ToString());
                sb.实际收入 = CommonFunctions.ValDec(tb.Rows[k]["实际收入金额"].ToString());
                sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                sb.进项税额 = CommonFunctions.ValDec(tb.Rows[k]["预算进项税额"].ToString());
                sb.销项税额 = CommonFunctions.ValDec(tb.Rows[k]["预算销项税额"].ToString());

                if (tb.Rows[k]["类型"].ToString() == "0")
                {
                    sb.类型 = "0";
                    sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                }
                else
                {
                    sb.类型 = "1";
                    sb.实际支出 = 0.00m;
                }

                shenbans.Add(sb);
            }
            return shenbans;
        }
        public static List<cls预算盈亏明细表> 获取预算盈亏明细表b(string 公司名, string 日期1, string 日期2)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls预算盈亏明细表> shenbans = new List<cls预算盈亏明细表>();
            cls预算盈亏明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str1 = "where 申请日期>='" + 日期1 + "' and 申请日期<='" + 日期2 + "' and 类型=1 ";

            str2 = "";
            List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
            str2 = "";
            for (i = 0; i < cbzxbhs.Count; i++)
            {
                if (str2 != "") str2 = str2 + ",";
                str2 = str2 + "'" + cbzxbhs[i] + "'";
            }
            if (str2 == "") str2 = "'" + 公司名 + "'";
            str2 = " and 成本中心编号 in (" + str2 + ")";

            str1 = str1 + str2;

            str1 = "select 类型,申请日期,预算编号,预算名称,业务类型,预算状态,销售,借贷方,金额,收付税额,收支,收支日期,子订单号,收支类型 from V预算预算收支 " + str1 + " and (预算状态='待完成' or 预算状态='已完成') order by 预算编号";

            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls预算盈亏明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.付款方 = tb.Rows[k]["业务类型"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                sb.提交日期 = DateTime.Parse(tb.Rows[k]["申请日期"].ToString()).ToString("yyyy-MM-dd");    //DateTime.Parse(tb.Rows[k]["收支日期"].ToString()).ToString("yyyy-MM-dd");
                if (sb.提交日期 == "1900-01-01") sb.提交日期 = "";
                sb.实际收入 = 0.00m;
                sb.实际支出 = 0.00m;
                sb.类型 = tb.Rows[k]["类型"].ToString();
                sb.收支 = tb.Rows[k]["收支"].ToString();
                if (tb.Rows[k]["收支"].ToString() == "收入")
                {
                    sb.预算收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.销项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                if (tb.Rows[k]["收支"].ToString() == "支出")
                {
                    sb.预算支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.进项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                sb.子订单号 = tb.Rows[k]["子订单号"].ToString();
                sb.收支类型 = tb.Rows[k]["收支类型"].ToString();

                shenbans.Add(sb);
            }
            return shenbans;
        }
        public static List<cls预算盈亏明细表> 获取预算盈亏明细表c(string 公司名, string 日期1, string 日期2)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls预算盈亏明细表> shenbans = new List<cls预算盈亏明细表>();
            cls预算盈亏明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str1 = "where 收支日期>='" + 日期1 + "' and 收支日期<='" + 日期2 + "' and 类型=0 ";

            str2 = "";
            List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
            str2 = "";
            for (i = 0; i < cbzxbhs.Count; i++)
            {
                if (str2 != "") str2 = str2 + ",";
                str2 = str2 + "'" + cbzxbhs[i] + "'";
            }
            if (str2 == "") str2 = "'" + 公司名 + "'";
            str2 = " and 成本中心编号 in (" + str2 + ")";

            str1 = str1 + str2;

            str1 = "select 类型,申请日期,预算编号,预算名称,业务类型,预算状态,销售,借贷方,金额,0.00 as 收付税额,收支,收支日期,子订单号,收支类型 from V预算实际收支 " + str1 + " and 收支='支出' and (预算状态='待完成' or 预算状态='已完成') order by 预算编号";

            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls预算盈亏明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.付款方 = tb.Rows[k]["业务类型"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                sb.提交日期 = DateTime.Parse(tb.Rows[k]["申请日期"].ToString()).ToString("yyyy-MM-dd");   // DateTime.Parse(tb.Rows[k]["收支日期"].ToString()).ToString("yyyy-MM-dd");
                sb.预算收入 = 0.00m;
                sb.预算支出 = 0.00m;
                sb.类型 = tb.Rows[k]["类型"].ToString();
                sb.收支 = tb.Rows[k]["收支"].ToString();
                if (tb.Rows[k]["收支"].ToString() == "收入")
                {
                    sb.实际收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.销项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                if (tb.Rows[k]["收支"].ToString() == "支出")
                {
                    sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.进项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                sb.子订单号 = tb.Rows[k]["子订单号"].ToString();
                sb.收支类型 = tb.Rows[k]["收支类型"].ToString();

                shenbans.Add(sb);
            }
            return shenbans;
        }


        public static List<cls实际盈亏汇总表> 获取实际盈亏汇总表(string sWhere)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            List<cls实际盈亏汇总表> shenbans = new List<cls实际盈亏汇总表>();
            cls实际盈亏汇总表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            DataTable tbcbzx = main.获取一般数据集(数据库连接字符串, "select 成本中心编号,部门 from Z成本中心");

            //str1 = "select 类型,成本中心编号,sum(预算收入金额) as 预算收入金额,sum(预算销项税额) as 预算销项税额,sum(预算支出金额) as 预算支出金额,sum(预算进项税额) as 预算进项税额,sum(实际收入金额) as 实际收入金额,0.00 as 实际销项税额,sum(实际支出金额) as 实际支出金额,0.00 as 实际进项税额 from V预算统计 " + sWhere + " and (预算状态='待完成' or 预算状态='已完成') group by 类型,成本中心编号";
            str1 = "select 收支,类型,成本中心编号,sum(金额) as 金额,0.00 as 预算收入金额,0.00 as 预算销项税额,0.00 as 预算支出金额,0.00 as 预算进项税额,0.00 as 实际收入金额,0.00 as 实际销项税额,0.00 as 实际支出金额,0.00 as 实际进项税额 from V预算实际收支 " + sWhere + " and (预算状态='待完成' or 预算状态='已完成') group by 类型,成本中心编号,收支";
            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                str1 = "";
                str1 = tb.Rows[k]["成本中心编号"].ToString();
                for (i = 0; i < tbcbzx.Rows.Count; i++)
                {
                    if (tbcbzx.Rows[i]["成本中心编号"].ToString() == str1)
                    {
                        str1 = tbcbzx.Rows[i]["部门"].ToString();
                        break;
                    }
                }
                if (str1 == "") str1 = tb.Rows[k]["成本中心编号"].ToString();

                sb = new cls实际盈亏汇总表();
                sb.ID = Guid.NewGuid().ToString();
                sb.公司名称 = str1;
                if (tb.Rows[k]["类型"].ToString() == "0")
                {
                    sb.项目实际收入 = 0.00m;
                    sb.销项税额 = 0.00m;
                    sb.项目实际支出 = 0.00m;
                    sb.进项税额 = 0.00m;
                    sb.应交税额 = 0.00m;
                    sb.GP = 0.00m;
                    sb.固定费用支出 = 0.00m;    //CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                    sb.盈亏 = 0.00m;
                }
                else
                {
                    if (tb.Rows[k]["收支"].ToString() == "收入")
                        sb.项目实际收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    else
                        sb.项目实际支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.应交税额 = 0.00m;    //sb.销项税额 - sb.进项税额;
                    sb.GP = 0.00m;    //sb.项目实际收入 - sb.项目实际支出 - sb.应交税额;
                    sb.固定费用支出 = 0.00m;
                    sb.盈亏 = 0.00m;
                }
                shenbans.Add(sb);
            }

            List<cls实际盈亏汇总表> returns = new List<cls实际盈亏汇总表>();
            for (i = 0; i < shenbans.Count; i++)
            {
                bool b1 = true;
                for (k = 0; k < returns.Count; k++)
                {
                    if (shenbans[i].公司名称 == returns[k].公司名称)
                    {
                        returns[k].项目实际收入 = returns[k].项目实际收入 + shenbans[i].项目实际收入;
                        returns[k].销项税额 = returns[k].销项税额 + shenbans[i].销项税额;
                        returns[k].项目实际支出 = returns[k].项目实际支出 + shenbans[i].项目实际支出;
                        returns[k].进项税额 = returns[k].进项税额 + shenbans[i].进项税额;
                        returns[k].固定费用支出 = returns[k].固定费用支出 + shenbans[i].固定费用支出;
                        b1 = false;
                        break;
                    }
                }
                if (b1)
                {
                    returns.Add(shenbans[i]);
                }
            }

            for (k = 0; k < returns.Count; k++)
            {
                string 日期1 = CommonFunctions.ReadWordOnly(sWhere, "日期", false, true);
                日期1 = CommonFunctions.ReadWordOnly(日期1, "'", false, true);
                日期1 = CommonFunctions.ReadWordOnly(日期1, "'", true, true);
                string 日期2 = CommonFunctions.ReadLastWordOnly(sWhere, "日期", false, true);
                日期2 = CommonFunctions.ReadWordOnly(日期2, "'", false, true);
                日期2 = CommonFunctions.ReadWordOnly(日期2, "'", true, true);
                List<cls实际盈亏明细表> mx = 获取实际盈亏明细表c(returns[k].公司名称, 日期1, 日期2);
                decimal decmx = 0.00m;
                for (int m = 0; m < mx.Count; m++)
                {
                    decmx = decmx + mx[m].实际支出;
                }
                returns[k].固定费用支出 = decmx;
            }


            for (k = 0; k < returns.Count; k++)
            {
                returns[k].应交税额 = returns[k].销项税额 - returns[k].进项税额;
                returns[k].GP = returns[k].项目实际收入 - returns[k].项目实际支出 - returns[k].应交税额;
                returns[k].盈亏 = returns[k].GP - returns[k].固定费用支出;
            }

            return returns;
        }
        public static List<cls实际盈亏明细表> 获取实际盈亏明细表(string sWhere, string 公司名)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls实际盈亏明细表> shenbans = new List<cls实际盈亏明细表>();
            cls实际盈亏明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str2 = "";
            if (sWhere.IndexOf("成本中心编号") < 0)
            {
                List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
                str2 = "";
                for (i = 0; i < cbzxbhs.Count; i++)
                {
                    if (str2 != "") str2 = str2 + ",";
                    str2 = str2 + "'" + cbzxbhs[i] + "'";
                }
                if (str2 == "") str2 = "'" + 公司名 + "'";
                str2 = " and 成本中心编号 in (" + str2 + ")";
            }

            str1 = "select 类型,申请日期,预算编号,预算名称,业务类型,预算状态,销售,预算收入金额,预算销项税额,预算支出金额,预算进项税额,实际收入金额,0.00 as 实际销项税额,实际支出金额,0.00 as 实际进项税额 from V预算统计 " + sWhere + str2 + " and 预算状态<>'待申请' and 审批结果<>'驳回' and 核定结果<>'驳回' order by 申请日期";
            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls实际盈亏明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.付款方 = tb.Rows[k]["业务类型"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                sb.提交日期 = DateTime.Parse(tb.Rows[k]["申请日期"].ToString()).ToString("yyyy-MM-dd");
                sb.预算收入 = CommonFunctions.ValDec(tb.Rows[k]["预算收入金额"].ToString());
                sb.预算支出 = CommonFunctions.ValDec(tb.Rows[k]["预算支出金额"].ToString());
                sb.实际收入 = CommonFunctions.ValDec(tb.Rows[k]["实际收入金额"].ToString());
                sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                sb.进项税额 = CommonFunctions.ValDec(tb.Rows[k]["实际进项税额"].ToString());
                sb.销项税额 = CommonFunctions.ValDec(tb.Rows[k]["实际销项税额"].ToString());

                if (tb.Rows[k]["类型"].ToString() == "0")
                {
                    sb.类型 = "0";
                    sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                }
                else
                {
                    sb.类型 = "1";
                    sb.实际支出 = 0.00m;
                }

                shenbans.Add(sb);
            }
            return shenbans;
        }
        public static List<cls实际盈亏明细表> 获取实际盈亏明细表b(string 公司名, string 日期1, string 日期2)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls实际盈亏明细表> shenbans = new List<cls实际盈亏明细表>();
            cls实际盈亏明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str1 = "where 收支日期>='" + 日期1 + "' and 收支日期<='" + 日期2 + "' and 类型=1 ";

            str2 = "";
            List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
            str2 = "";
            for (i = 0; i < cbzxbhs.Count; i++)
            {
                if (str2 != "") str2 = str2 + ",";
                str2 = str2 + "'" + cbzxbhs[i] + "'";
            }
            if (str2 == "") str2 = "'" + 公司名 + "'";
            str2 = " and 成本中心编号 in (" + str2 + ")";

            str1 = str1 + str2;

            str1 = "select 类型,申请日期,预算编号,预算名称,业务类型,预算状态,销售,借贷方,金额,0.00 as 收付税额,收支,收支日期,子订单号,收支类型 from V预算实际收支 " + str1 + " and (预算状态='待完成' or 预算状态='已完成') order by 预算编号";

            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls实际盈亏明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.付款方 = tb.Rows[k]["业务类型"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                sb.提交日期 = DateTime.Parse(tb.Rows[k]["收支日期"].ToString()).ToString("yyyy-MM-dd");    //DateTime.Parse(tb.Rows[k]["申请日期"].ToString()).ToString("yyyy-MM-dd");
                sb.预算收入 = 0.00m;
                sb.预算支出 = 0.00m;
                sb.类型 = tb.Rows[k]["类型"].ToString();
                sb.收支 = tb.Rows[k]["收支"].ToString();
                if (tb.Rows[k]["收支"].ToString() == "收入")
                {
                    sb.实际收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.销项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                if (tb.Rows[k]["收支"].ToString() == "支出")
                {
                    sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.进项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                sb.子订单号 = tb.Rows[k]["子订单号"].ToString();
                sb.收支类型 = tb.Rows[k]["收支类型"].ToString();

                shenbans.Add(sb);
            }
            return shenbans;
        }
        public static List<cls实际盈亏明细表> 获取实际盈亏明细表c(string 公司名, string 日期1, string 日期2)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls实际盈亏明细表> shenbans = new List<cls实际盈亏明细表>();
            cls实际盈亏明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str1 = "where 收支日期>='" + 日期1 + "' and 收支日期<='" + 日期2 + "' and 类型=0 ";

            str2 = "";
            List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
            str2 = "";
            for (i = 0; i < cbzxbhs.Count; i++)
            {
                if (str2 != "") str2 = str2 + ",";
                str2 = str2 + "'" + cbzxbhs[i] + "'";
            }
            if (str2 == "") str2 = "'" + 公司名 + "'";
            str2 = " and 成本中心编号 in (" + str2 + ")";

            str1 = str1 + str2;

            str1 = "select 类型,申请日期,预算编号,预算名称,业务类型,预算状态,销售,借贷方,金额,0.00 as 收付税额,收支,收支日期,子订单号,收支类型 from V预算实际收支 " + str1 + " and 收支='支出' and (预算状态='待完成' or 预算状态='已完成') order by 预算编号";

            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls实际盈亏明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.付款方 = tb.Rows[k]["业务类型"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                sb.提交日期 = DateTime.Parse(tb.Rows[k]["收支日期"].ToString()).ToString("yyyy-MM-dd");    //DateTime.Parse(tb.Rows[k]["申请日期"].ToString()).ToString("yyyy-MM-dd");
                sb.预算收入 = 0.00m;
                sb.预算支出 = 0.00m;
                sb.类型 = tb.Rows[k]["类型"].ToString();
                sb.收支 = tb.Rows[k]["收支"].ToString();
                if (tb.Rows[k]["收支"].ToString() == "收入")
                {
                    sb.实际收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.销项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                if (tb.Rows[k]["收支"].ToString() == "支出")
                {
                    sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.进项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                sb.子订单号 = tb.Rows[k]["子订单号"].ToString();
                sb.收支类型 = tb.Rows[k]["收支类型"].ToString();

                shenbans.Add(sb);
            }
            return shenbans;
        }



        public static List<cls应收应付明细表> 获取应收应付明细表(string sWhere, string 公司名)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls应收应付明细表> shenbans = new List<cls应收应付明细表>();
            cls应收应付明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str2 = "";
            if (sWhere.IndexOf("成本中心编号") < 0)
            {
                if (公司名 != "")
                {
                    List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
                    str2 = "";
                    for (i = 0; i < cbzxbhs.Count; i++)
                    {
                        if (str2 != "") str2 = str2 + ",";
                        str2 = str2 + "'" + cbzxbhs[i] + "'";
                    }
                    if (str2 == "") str2 = "'" + 公司名 + "'";
                    str2 = " and 成本中心编号 in (" + str2 + ")";
                }
            }

            //str1 = "select '预算' as 预算or实际,a.类型,a.申请日期,a.预算编号,a.预算名称,a.业务类型,a.预算状态,a.销售,b.收支,b.金额,b.借贷方,b.日期,b.收付税额 from A预算 a inner join A预算收支 b on a.ID=b.预算ID " + sWhere + str2 + " and a.预算状态<>'待申请' and a.审批结果<>'驳回' and a.核定结果<>'驳回'";
            str1 = "select 预算or实际,类型,申请日期,预算编号,预算名称,业务类型,预算状态,销售,收支,金额,借贷方,日期,收付税额 from V预算收付 " + sWhere + str2 + " and 预算状态<>'待申请' and 审批结果<>'驳回' and 核定结果<>'驳回' order by 日期";
            
            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls应收应付明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.收付款方 = tb.Rows[k]["借贷方"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                if (tb.Rows[k]["预算or实际"].ToString() == "预算")
                {
                    if (tb.Rows[k]["收支"].ToString() == "收入")
                    {
                        sb.收支 = "收入";
                        sb.预算收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                        sb.预算支出 = 0.00m;
                        sb.实际收入 = 0.00m;
                        sb.实际支出 = 0.00m;
                        sb.进项税额 = 0.00m;
                        sb.销项税额 = 0.00m;
                    }
                    else
                    {
                        sb.收支 = "支出";
                        sb.预算收入 = 0.00m;
                        sb.预算支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                        sb.实际收入 = 0.00m;
                        sb.实际支出 = 0.00m;
                        sb.进项税额 = 0.00m;
                        sb.销项税额 = 0.00m;
                    }
                }
                else
                {
                    if (tb.Rows[k]["收支"].ToString() == "收入")
                    {
                        sb.收支 = "收入";
                        sb.预算收入 = 0.00m;
                        sb.预算支出 = 0.00m;
                        sb.实际收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                        sb.实际支出 = 0.00m;
                        sb.进项税额 = 0.00m;
                        sb.销项税额 = 0.00m;
                    }
                    else
                    {
                        sb.收支 = "支出";
                        sb.预算收入 = 0.00m;
                        sb.预算支出 = 0.00m;
                        sb.实际收入 = 0.00m;
                        sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                        sb.进项税额 = 0.00m;
                        sb.销项税额 = 0.00m;
                    }
                }
                sb.预计收付日期 = DateTime.Parse(tb.Rows[k]["日期"].ToString()).ToString("yyyy-MM-dd");
                
                shenbans.Add(sb);
            }
            return shenbans;
        }


        public static List<cls销售业绩明细表> 获取销售业绩明细表(string sWhere, string 公司名)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            List<cls销售业绩明细表> shenbans = new List<cls销售业绩明细表>();
            cls销售业绩明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str2 = "";
            if (sWhere.IndexOf("成本中心编号") < 0)
            {
                if (公司名 != "")
                {
                    List<string> cbzxbhs = 获取某公司所有成本中心编号(公司名);
                    str2 = "";
                    for (i = 0; i < cbzxbhs.Count; i++)
                    {
                        if (str2 != "") str2 = str2 + ",";
                        str2 = str2 + "'" + cbzxbhs[i] + "'";
                    }
                    if (str2 == "") str2 = "'" + 公司名 + "'";
                    str2 = " and 成本中心编号 in (" + str2 + ")";
                }
            }

            str1 = "select a.类型,a.申请日期,a.预算编号,a.预算名称,a.业务类型,a.预算状态,a.销售,b.收支,b.金额,0.00 as 收付税额 from A预算 a inner join A实际收支 b on a.ID=b.预算ID " + sWhere + str2 + " and a.预算状态='已完成' and a.审批结果<>'驳回' and a.核定结果<>'驳回'";
            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls销售业绩明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.销售 = tb.Rows[k]["销售"].ToString();
                if (tb.Rows[k]["收支"].ToString() == "收入")
                {
                    sb.收支 = "收入";
                    sb.实际收入 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.实际支出 = 0.00m;
                    sb.实际进项税额 = 0.00m;
                    sb.实际销项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                }
                else
                {
                    sb.收支 = "支出";
                    sb.实际收入 = 0.00m;
                    sb.实际支出 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.实际进项税额 = CommonFunctions.ValDec(tb.Rows[k]["收付税额"].ToString());
                    sb.实际销项税额 = 0.00m;
                }

                shenbans.Add(sb);
            }
            return shenbans;
        }




        public static List<cls中证通订单应收明细表> 获取中证通订单应收明细表(string sWhere, ref string sql)
        {
            int k = 0;
            string str1 = "";
            List<cls中证通订单应收明细表> shenbans = new List<cls中证通订单应收明细表>();
            cls中证通订单应收明细表 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            str1 = "select 预算编号,子订单号,申请日期,预算名称,业务类型,成本中心编号,预算状态,销售,当事人,公证事项,承办公证处,线上线下,预算收入金额,实际收入金额,0.00 as 应收额 from V预算统计 " + sWhere + " and 是否中证通订单='是' order by 申请日期";

            sql = str1;
            DataTable tb = main.获取一般数据集(数据库连接字符串, str1);
            if (tb.Rows.Count < 1)
            {
                return shenbans;
            }
            for (k = 0; k < tb.Rows.Count; k++)
            {
                sb = new cls中证通订单应收明细表();
                sb.ID = Guid.NewGuid().ToString();
                sb.子订单号 = tb.Rows[k]["子订单号"].ToString();
                sb.申请日期 = DateTime.Parse(tb.Rows[k]["申请日期"].ToString()).ToString("yyyy-MM-dd");
                sb.预算编号 = tb.Rows[k]["预算编号"].ToString();
                sb.项目名称 = tb.Rows[k]["预算名称"].ToString();
                sb.业务类型 = tb.Rows[k]["业务类型"].ToString();
                sb.成本中心编号 = tb.Rows[k]["成本中心编号"].ToString();
                sb.项目状态 = tb.Rows[k]["预算状态"].ToString();
                sb.当事人 = tb.Rows[k]["当事人"].ToString();
                sb.公证事项 = tb.Rows[k]["公证事项"].ToString();
                sb.承办公证处 = tb.Rows[k]["承办公证处"].ToString();
                sb.线上线下 = tb.Rows[k]["线上线下"].ToString();
                sb.预算收入金额 = CommonFunctions.ValDec(tb.Rows[k]["预算收入金额"].ToString());
                sb.实际收入金额 = CommonFunctions.ValDec(tb.Rows[k]["实际收入金额"].ToString());

                shenbans.Add(sb);
            }
            return shenbans;
        }

    }
    public class cls预算盈亏汇总表
    {
        #region 属性
        public string ID { get; set; }

        public string 公司名称 { get; set; }
        public decimal 项目预算收入 { get; set; }
        public decimal 销项税额 { get; set; }
        public decimal 项目预算支出 { get; set; }
        public decimal 进项税额 { get; set; }
        public decimal 应交税额 { get; set; }
        public decimal GP { get; set; }
        public decimal 固定费用支出 { get; set; }
        public decimal 盈亏 { get; set; }

        #endregion
    }
    public class cls预算盈亏汇总表s
    {
        public List<cls预算盈亏汇总表> 记录集 { get; set; }
    }

    public class cls预算盈亏明细表
    {
        #region 属性
        public string ID { get; set; }

        public string 类型 { get; set; }
        public string 预算编号 { get; set; }
        public string 项目名称 { get; set; }
        public string 付款方 { get; set; }
        public string 项目状态 { get; set; }
        public string 销售 { get; set; }
        public decimal 预算收入 { get; set; }
        public decimal 预算支出 { get; set; }
        public decimal 实际收入 { get; set; }
        public decimal 实际支出 { get; set; }
        public decimal 进项税额 { get; set; }
        public decimal 销项税额 { get; set; }
        public string 提交日期 { get; set; }
        public string 收支 { get; set; }

        public string 收支类型 { get; set; }
        public string 子订单号 { get; set; }


        #endregion
    }
    public class cls预算盈亏明细表s
    {
        public List<cls预算盈亏明细表> 记录集 { get; set; }
    }



    public class cls实际盈亏汇总表
    {
        #region 属性
        public string ID { get; set; }

        public string 公司名称 { get; set; }
        public decimal 项目实际收入 { get; set; }
        public decimal 销项税额 { get; set; }
        public decimal 项目实际支出 { get; set; }
        public decimal 进项税额 { get; set; }
        public decimal 应交税额 { get; set; }
        public decimal GP { get; set; }
        public decimal 固定费用支出 { get; set; }
        public decimal 盈亏 { get; set; }

        #endregion
    }
    public class cls实际盈亏汇总表s
    {
        public List<cls实际盈亏汇总表> 记录集 { get; set; }
    }

    public class cls实际盈亏明细表
    {
        #region 属性
        public string ID { get; set; }

        public string 类型 { get; set; }
        public string 预算编号 { get; set; }
        public string 项目名称 { get; set; }
        public string 付款方 { get; set; }
        public string 项目状态 { get; set; }
        public string 销售 { get; set; }
        public decimal 预算收入 { get; set; }
        public decimal 预算支出 { get; set; }
        public decimal 实际收入 { get; set; }
        public decimal 实际支出 { get; set; }
        public decimal 进项税额 { get; set; }
        public decimal 销项税额 { get; set; }
        public string 提交日期 { get; set; }
        public string 收支 { get; set; }

        public string 收支类型 { get; set; }
        public string 子订单号 { get; set; }


        #endregion
    }
    public class cls实际盈亏明细表s
    {
        public List<cls实际盈亏明细表> 记录集 { get; set; }
    }

    public class cls应收应付明细表
    {
        #region 属性
        public string ID { get; set; }

        public string 类型 { get; set; }
        public string 预算编号 { get; set; }
        public string 项目名称 { get; set; }
        public string 收付款方 { get; set; }
        public string 项目状态 { get; set; }
        public string 销售 { get; set; }
        public string 收支 { get; set; }
        public decimal 预算收入 { get; set; }
        public decimal 预算支出 { get; set; }
        public decimal 实际收入 { get; set; }
        public decimal 实际支出 { get; set; }
        public decimal 进项税额 { get; set; }
        public decimal 销项税额 { get; set; }
        public string 预计收付日期 { get; set; }
        public decimal 应收应付账款 { get; set; }

        #endregion
    }
    public class cls应收应付明细表s
    {
        public List<cls应收应付明细表> 记录集 { get; set; }
    }

    public class cls销售业绩明细表
    {
        #region 属性
        public string ID { get; set; }

        public string 预算编号 { get; set; }
        public string 项目名称 { get; set; }
        public string 项目状态 { get; set; }
        public string 销售 { get; set; }
        public string 收支 { get; set; }
        public decimal 实际收入 { get; set; }
        public decimal 实际支出 { get; set; }
        public decimal 实际进项税额 { get; set; }
        public decimal 实际销项税额 { get; set; }
        public decimal GP { get; set; }

        #endregion
    }
    public class cls销售业绩明细表s
    {
        public List<cls销售业绩明细表> 记录集 { get; set; }
    }



    public class cls中证通订单应收明细表
    {
        #region 属性
        public string ID { get; set; }

        public string 子订单号 { get; set; }
        public string 预算编号 { get; set; }
        public string 项目名称 { get; set; }
        public string 申请日期 { get; set; }
        public string 业务类型 { get; set; }
        public string 成本中心编号 { get; set; }
        public string 当事人 { get; set; }
        public string 公证事项 { get; set; }
        public string 承办公证处 { get; set; }
        public string 线上线下 { get; set; }
        public string 收付款方 { get; set; }
        public string 项目状态 { get; set; }
        public string 销售 { get; set; }
        public decimal 预算收入金额 { get; set; }
        public decimal 实际收入金额 { get; set; }

        #endregion
    }
    public class cls中证通订单应收明细表s
    {
        public List<cls中证通订单应收明细表> 记录集 { get; set; }
    }
}