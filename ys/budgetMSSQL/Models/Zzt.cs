using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace applyvisa.Models
{
    public class Zzt
    {
        public static List<cls中证通订单> 获取中证通订单s(string sWhere)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            List<cls中证通订单> shenbans = new List<cls中证通订单>();
            cls中证通订单 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                SqlConnection cnn = new SqlConnection(数据库连接字符串);
                cnn.Open();

                str1 = "select top 50000 * from V中证通证书统计 " + sWhere;
                //str1 = "select * from V中证通证书统计 " + sWhere + " limit 0,50000";  //Mysql
                SqlDataAdapter adp = new SqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    cnn.Close();
                    return shenbans;
                }

                for (k = 0; k < tb.Rows.Count; k++)
                {
                    sb = new cls中证通订单();
                    sb.ID = tb.Rows[k]["ID"].ToString();
                    sb.管理_预算收入金额 = CommonFunctions.ValDec(tb.Rows[k]["预算收入金额"].ToString());
                    sb.管理_预算支出金额 = CommonFunctions.ValDec(tb.Rows[k]["预算支出金额"].ToString());
                    sb.管理_实际收入金额 = CommonFunctions.ValDec(tb.Rows[k]["实际收入金额"].ToString());
                    sb.管理_实际支出金额 = CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                    sb.订单号 = tb.Rows[k]["订单号"].ToString();
                    sb.订单ID = tb.Rows[k]["订单ID"].ToString();
                    sb.子订单号 = tb.Rows[k]["子订单号"].ToString();
                    sb.子订单ID = tb.Rows[k]["子订单ID"].ToString();
                    sb.当事人 = tb.Rows[k]["当事人"].ToString();
                    sb.当事人证件号 = tb.Rows[k]["当事人证件号"].ToString();
                    sb.订单日期 = DateTime.Parse(tb.Rows[k]["订单日期"].ToString());
                    sb.公证事项 = tb.Rows[k]["公证事项"].ToString();
                    sb.用地 = tb.Rows[k]["用地"].ToString();
                    sb.用途 = tb.Rows[k]["用途"].ToString();
                    sb.语种 = tb.Rows[k]["语种"].ToString();
                    sb.承办公证处 = tb.Rows[k]["承办公证处"].ToString();
                    sb.是否加急 = tb.Rows[k]["是否加急"].ToString();
                    sb.渠道 = tb.Rows[k]["渠道"].ToString();
                    sb.导入日期 = DateTime.Parse(tb.Rows[k]["导入日期"].ToString());
                    sb.备注 = tb.Rows[k]["备注"].ToString();

                    if (tb.Rows.Count < 2)   //数量多会导致查询死掉，所以只有在查询单条记录时再搜索子数据
                    {
                        // 获取预算收支
                        str1 = "select * from A中证通证书预算收支 where 证书ID='" + sb.ID.ToString() + "' order by 录入日期";
                        SqlDataAdapter adpyssz = new SqlDataAdapter(str1, cnn);
                        DataSet dsyssz = new DataSet();
                        adpyssz.Fill(dsyssz);
                        DataTable tbyssz = dsyssz.Tables[0];
                        for (i = 0; i < tbyssz.Rows.Count; i++)
                        {
                            cls中证通订单预算收支 yssz = new cls中证通订单预算收支();
                            yssz.ID = tbyssz.Rows[i]["ID"].ToString();
                            yssz.证书ID = tbyssz.Rows[i]["证书ID"].ToString();
                            yssz.收支 = tbyssz.Rows[i]["收支"].ToString();
                            yssz.类型 = tbyssz.Rows[i]["类型"].ToString();
                            yssz.借贷方 = tbyssz.Rows[i]["借贷方"].ToString();
                            yssz.金额 = CommonFunctions.ValDec(tbyssz.Rows[i]["金额"].ToString());
                            yssz.日期 = DateTime.Parse(tbyssz.Rows[i]["日期"].ToString());
                            yssz.备注 = tbyssz.Rows[i]["备注"].ToString();
                            yssz.录入人 = tbyssz.Rows[i]["录入人"].ToString();
                            yssz.录入日期 = DateTime.Parse(tbyssz.Rows[i]["录入日期"].ToString());
                            sb.预算收支.Add(yssz);
                        }
                        // 获取实际收支
                        str1 = "select * from A中证通证书实际收支 where 证书ID='" + sb.ID.ToString() + "' order by 录入日期";
                        SqlDataAdapter adpsjsz = new SqlDataAdapter(str1, cnn);
                        DataSet dssjsz = new DataSet();
                        adpsjsz.Fill(dssjsz);
                        DataTable tbsjsz = dssjsz.Tables[0];
                        for (i = 0; i < tbsjsz.Rows.Count; i++)
                        {
                            cls中证通订单实际收支 sjsz = new cls中证通订单实际收支();
                            sjsz.ID = tbsjsz.Rows[i]["ID"].ToString();
                            sjsz.证书ID = tbsjsz.Rows[i]["证书ID"].ToString();
                            sjsz.收支 = tbsjsz.Rows[i]["收支"].ToString();
                            sjsz.类型 = tbsjsz.Rows[i]["类型"].ToString();
                            sjsz.借贷方 = tbsjsz.Rows[i]["借贷方"].ToString();
                            sjsz.金额 = CommonFunctions.ValDec(tbsjsz.Rows[i]["金额"].ToString());
                            sjsz.日期 = DateTime.Parse(tbsjsz.Rows[i]["日期"].ToString());
                            sjsz.备注 = tbsjsz.Rows[i]["备注"].ToString();
                            sjsz.录入人 = tbsjsz.Rows[i]["录入人"].ToString();
                            sjsz.录入日期 = DateTime.Parse(tbsjsz.Rows[i]["录入日期"].ToString());
                            sb.实际收支.Add(sjsz);
                        }
                    }

                    shenbans.Add(sb);
                }

                cnn.Close();
                return shenbans;
            }
            catch
            { return shenbans; }
        }

        public static string 保存预算收支(string 证书ID, cls中证通订单预算收支 yssz)
        {
            int i = 0;
            string sErr = "";
            int k = 0;

            if (证书ID == null) return "无效的记录。";
            if (证书ID == "") return "无效的记录。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                SqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr = null;
                SqlCommandBuilder cbu;

                SqlConnection cnn = new SqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new SqlDataAdapter("select * from A中证通证书预算收支 where 证书ID='" + 证书ID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                bool isnew = true;
                for (i = 0; i < tb.Rows.Count; i++)
                {
                    if (tb.Rows[i]["ID"].ToString() == yssz.ID.ToString())
                    {
                        dr = tb.Rows[i];
                        isnew = false;
                        break;
                    }
                }
                if (isnew)
                {
                    dr = tb.NewRow();
                }

                dr["ID"] = yssz.ID;
                dr["证书ID"] = 证书ID; ;
                dr["收支"] = yssz.收支;
                dr["类型"] = yssz.类型;
                dr["借贷方"] = yssz.借贷方;
                dr["金额"] = yssz.金额;
                dr["日期"] = yssz.日期;
                dr["备注"] = yssz.备注;
                dr["录入人"] = yssz.录入人;
                dr["录入日期"] = yssz.录入日期;

                if (isnew)
                {
                    tb.Rows.Add(dr);
                }

                cbu = new SqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（收入/支出）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }


        public static int 删除实际收支(string sqrID)
        {
            return main.执行SQL命令("delete from A中证通证书实际收支 where ID='" + sqrID + "'");
        }

        public static string 保存实际收支(string 证书ID, cls中证通订单实际收支 sjsz)
        {
            int i = 0;
            string sErr = "";
            int k = 0;

            if (证书ID == null) return "无效的记录。";
            if (证书ID == "") return "无效的记录。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                SqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr = null;
                SqlCommandBuilder cbu;

                SqlConnection cnn = new SqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new SqlDataAdapter("select * from A中证通证书实际收支 where 证书ID='" + 证书ID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                bool isnew = true;
                for (i = 0; i < tb.Rows.Count; i++)
                {
                    if (tb.Rows[i]["ID"].ToString() == sjsz.ID.ToString())
                    {
                        dr = tb.Rows[i];
                        isnew = false;
                        break;
                    }
                }
                if (isnew)
                {
                    dr = tb.NewRow();
                }

                dr["ID"] = sjsz.ID;
                dr["证书ID"] = 证书ID; ;
                dr["收支"] = sjsz.收支;
                dr["类型"] = sjsz.类型;
                dr["借贷方"] = sjsz.借贷方;
                dr["金额"] = sjsz.金额;
                dr["日期"] = sjsz.日期;
                dr["备注"] = sjsz.备注;
                dr["录入人"] = sjsz.录入人;
                dr["录入日期"] = sjsz.录入日期;

                if (isnew)
                {
                    tb.Rows.Add(dr);
                }

                cbu = new SqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（收入/支出）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }

        public static string 保存实际收支批量(List<cls中证通订单> 订单s, cls中证通订单实际收支 sjsz, string 收入支出)
        {
            int i = 0;
            string str1 = "";
            string sErr = "";
            int k = 0;
            decimal dec1 = 0.00m;
            decimal dec2 = 0.00m;
            DataRow dr = null;

            if (订单s == null) return "无效的记录。";
            if (订单s.Count == 0) return "无效的记录。";

            for (i = 0; i < 订单s.Count; i++)
            {
                str1 = str1 + "'" + 订单s[i].ID.ToString() + "'";
                if (i < 订单s.Count - 1) str1 = str1 + ",";
            }
            str1 = "select ID,(预算收入金额-实际收入金额) as 收入金额,(预算支出金额-实际支出金额) as 支出金额 from V中证通证书统计 where ID in (" + str1 + ")";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                SqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                SqlCommandBuilder cbu;

                SqlConnection cnn = new SqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new SqlDataAdapter(str1, cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                for (i = 0; i < tb.Rows.Count; i++)
                {
                    dec1 = dec1 + CommonFunctions.ValDec(tb.Rows[i][收入支出 + "金额"].ToString());
                }

                SqlDataAdapter adp1 = new SqlDataAdapter("select * from A中证通证书实际收支 where 1=2", cnn);
                DataSet ds1 = new DataSet();
                adp1.Fill(ds1);
                DataTable tb1 = ds1.Tables[0];

                decimal dx = 0.00m;
                for (i = 0; i < tb.Rows.Count; i++)   //最后1条先保留
                {
                    dec2 = 0.00m;
                    dec2 = sjsz.金额 * CommonFunctions.ValDec(tb.Rows[i][收入支出 + "金额"].ToString()) / dec1;
                    dec2 = decimal.Floor(dec2);
                    dr = tb1.NewRow();
                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["证书ID"] = tb.Rows[i]["ID"];
                    dr["收支"] = sjsz.收支;
                    dr["类型"] = sjsz.类型;
                    dr["借贷方"] = sjsz.借贷方;
                    if (i < tb.Rows.Count - 1)
                    {
                        dx = dx + dec2;
                    }
                    else
                    {
                        dec2 = sjsz.金额 - dx;   //剩余的钱放到最后一个预算单上（即使是负数）
                    }
                    dr["金额"] = dec2;
                    dr["日期"] = sjsz.日期;
                    dr["备注"] = sjsz.备注;
                    dr["录入人"] = sjsz.录入人;
                    dr["录入日期"] = sjsz.录入日期;
                    tb1.Rows.Add(dr);

                    foreach (cls中证通订单 yd in 订单s)
                    {
                        if (yd.ID.ToString() == tb.Rows[i]["ID"].ToString())
                        {
                            yd.管理_1 = decimal.Round(dec2 * 1.00m, 2).ToString();
                            break;
                        }
                    }
                }

                cbu = new SqlCommandBuilder(adp1);
                i = 0;
                i = adp1.Update(tb1);
                if (i < 1)
                {
                    sErr = "保存失败（实际收入/支出）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }

    }
}