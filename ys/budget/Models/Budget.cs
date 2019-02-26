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
    public class Budget
    {
        #region 预算申请

        /// <summary>
        /// 预算申请新记录的保存
        /// </summary>
        /// <param name="sb"></param>
        /// <returns>返回错误消息。正确时返回空字符串。</returns>
        public static string 保存新增预算(cls预算单 sb)
        {
            int i = 0;
            string sErr = "";

            if (sb == null) return "无效的申请。";

            if (sb.ID == null) sb.ID = Guid.NewGuid().ToString();
            if (sb.ID == "") sb.ID = Guid.NewGuid().ToString();

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;
                string sldh = "";
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                //获取预算编号
                DateTime dat1 = sb.申请日期;
                if (dat1 < DateTime.Parse("1950-01-01"))
                {
                    dat1 = DateTime.Now;
                    sb.申请日期 = dat1;
                }
                //string str1 = "select max(预算编号) from A预算 where 预算编号>" + dat1.Year.ToString() + "000000 and 预算编号<=" + dat1.Year.ToString() + "999999";
                //adp = new MySqlDataAdapter(str1, cnn);
                //ds = new DataSet();
                //adp.Fill(ds);
                //try
                //{
                //    sldh = CommonFunctions.ValInt(ds.Tables[0].Rows[0][0].ToString());
                //}
                //catch { }
                //if (sldh <= (dat1.Year * 1000000)) sldh = dat1.Year * 1000000;
                //sldh++;
                sldh = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();

                //保存预算收支
                if (sb.预算收支.Count > 0)
                {
                    adp = new MySqlDataAdapter("select * from A预算收支 where 1=2", cnn);
                    ds = new DataSet();
                    adp.Fill(ds);
                    tb = ds.Tables[0];
                    for (i = 0; i < sb.预算收支.Count; i++)
                    {
                        dr = tb.NewRow();
                        dr["ID"] = Guid.NewGuid().ToString();
                        dr["预算ID"] = sb.ID;
                        //dr["序号"] = i + 1;
                        dr["收支"] = sb.预算收支[i].收支;
                        dr["类型"] = sb.预算收支[i].类型;
                        dr["借贷方"] = sb.预算收支[i].借贷方;
                        dr["金额"] = sb.预算收支[i].金额;
                        dr["日期"] = sb.预算收支[i].日期;
                        dr["收付税额"] = sb.预算收支[i].收付税额;
                        dr["收付税率"] = sb.预算收支[i].收付税率;
                        dr["收付发票种类"] = sb.预算收支[i].收付发票种类;
                        dr["备注"] = sb.预算收支[i].备注;
                        dr["录入人"] = sb.预算收支[i].录入人;
                        if (sb.预算收支[i].录入人.Trim() == "") dr["录入人"] = sb.申请人;
                        dr["录入日期"] = sb.预算收支[i].录入日期;
                        if (sb.预算收支[i].录入日期 < DateTime.Parse("2000-01-01")) dr["录入日期"] = DateTime.Now;
                        tb.Rows.Add(dr);
                    }
                    cbu = new MySqlCommandBuilder(adp);
                    i = 0;
                    i = adp.Update(tb);
                    if (i < 1)
                    {
                        sErr = "保存失败（预算收支）。";
                        cnn.Close();
                        return sErr;
                    }
                }

                if (sb.类型 == 1)
                {

                    //保存内部转账
                    if (sb.内部转账.Count > 0)
                    {
                        adp = new MySqlDataAdapter("select * from A内部转账 where 1=2", cnn);
                        ds = new DataSet();
                        adp.Fill(ds);
                        tb = ds.Tables[0];
                        for (i = 0; i < sb.内部转账.Count; i++)
                        {
                            dr = tb.NewRow();
                            dr["ID"] = Guid.NewGuid().ToString();
                            dr["预算ID"] = sb.ID;
                            //dr["序号"] = i + 1;
                            dr["转账对象"] = sb.内部转账[i].转账对象;
                            dr["转账金额"] = sb.内部转账[i].转账金额;
                            dr["转账比例"] = sb.内部转账[i].转账比例;
                            dr["备注"] = sb.内部转账[i].备注;
                            dr["录入人"] = sb.内部转账[i].录入人;
                            if (sb.内部转账[i].录入人.Trim() == "") dr["录入人"] = sb.申请人;
                            dr["录入日期"] = sb.内部转账[i].录入日期;
                            if (sb.内部转账[i].录入日期 < DateTime.Parse("2000-01-01")) dr["录入日期"] = DateTime.Now;
                            tb.Rows.Add(dr);
                        }
                        cbu = new MySqlCommandBuilder(adp);
                        i = 0;
                        i = adp.Update(tb);
                        if (i < 1)
                        {
                            sErr = "保存失败（内部转账）。";
                            cnn.Close();
                            return sErr;
                        }
                    }

                    //保存预算发票
                    if (sb.预算发票.Count > 0)
                    {
                        adp = new MySqlDataAdapter("select * from A预算发票 where 1=2", cnn);
                        ds = new DataSet();
                        adp.Fill(ds);
                        tb = ds.Tables[0];
                        for (i = 0; i < sb.预算发票.Count; i++)
                        {
                            dr = tb.NewRow();
                            dr["ID"] = Guid.NewGuid().ToString();
                            dr["预算ID"] = sb.ID;
                            //dr["序号"] = i + 1;
                            dr["收票方"] = sb.预算发票[i].收票方;
                            dr["发票种类"] = sb.预算发票[i].发票种类;
                            dr["应收税额"] = sb.预算发票[i].应收税额;
                            dr["税点"] = sb.预算发票[i].税点;
                            dr["备注"] = sb.预算发票[i].备注;
                            dr["录入人"] = sb.预算发票[i].录入人;
                            if (sb.预算发票[i].录入人.Trim() == "") dr["录入人"] = sb.申请人;
                            dr["录入日期"] = sb.预算发票[i].录入日期;
                            if (sb.预算发票[i].录入日期 < DateTime.Parse("2000-01-01")) dr["录入日期"] = DateTime.Now;
                            tb.Rows.Add(dr);
                        }
                        cbu = new MySqlCommandBuilder(adp);
                        i = 0;
                        i = adp.Update(tb);
                        if (i < 1)
                        {
                            sErr = "保存失败（预算发票）。";
                            cnn.Close();
                            return sErr;
                        }
                    }
                }

                adp = new MySqlDataAdapter("select * from A预算 where 1=2", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                dr = tb.NewRow();
                dr["ID"] = sb.ID;
                sb.预算编号 = sldh;
                dr["预算编号"] = sb.预算编号;
                //sb.父ID = "";
                //dr["父ID"] = sb.父ID;
                //sb.作废 = 0;
                //dr["作废"] = sb.作废;                
                dr["预算名称"] = sb.预算名称;
                dr["成本中心编号"] = sb.成本中心编号;
                dr["业务类型"] = sb.业务类型;
                dr["销售"] = sb.销售;
                dr["预算说明"] = sb.预算说明;
                dr["申请人"] = sb.申请人;
                if (sb.申请日期 > DateTime.Parse("2000-01-01")) dr["申请日期"] = sb.申请日期;                

                dr["审批人"] = sb.审批人;
                dr["审批日期"] = sb.审批日期;
                dr["审批结果"] = sb.审批结果;
                dr["审批意见"] = sb.审批意见;
                dr["核定人"] = sb.核定人;
                dr["核定日期"] = sb.核定日期;
                dr["核定结果"] = sb.核定结果;
                dr["核定意见"] = sb.核定意见;
                
                dr["来源"] = sb.来源;
                dr["类型"] = sb.类型;

                if (sb.预算状态 != "") dr["预算状态"] = sb.预算状态;

                tb.Rows.Add(dr);
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（预算信息）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }
        public static string 保存已有预算(cls预算单 sb)
        {
            int i = 0;
            string sErr = "";

            if (sb == null) return "无效的预算。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                //预计收入支出等每次编辑按保存时已经保存，所以这里只需要保存预算单基本信息

                adp = new MySqlDataAdapter("select * from A预算 where ID='" + sb.ID.ToString() + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    sErr = "保存失败（无预算记录）。";
                    cnn.Close();
                    return sErr;
                }

                dr = tb.Rows[0];
                //dr["ID"] = sb.ID;
                //dr["作废"] = sb.作废;
                //dr["来源"] = sb.来源;
                //sb.申请日期 = dat1;
                //dr["申请日期"] = sb.申请日期;
                dr["预算名称"] = sb.预算名称;
                dr["成本中心编号"] = sb.成本中心编号;
                dr["业务类型"] = sb.业务类型;
                dr["销售"] = sb.销售;
                dr["预算说明"] = sb.预算说明;
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（预算信息）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }

        public static cls预算单 获取预算记录(string ID)
        {
            cls预算单 sb = null;
            string str1 = "where ID='" + ID + "'";
            try { sb = 获取预算记录s(str1)[0]; }
            catch { }
            return sb;
        }
        public static List<cls预算单> 获取预算记录s(string sWhere)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            List<cls预算单> shenbans = new List<cls预算单>();
            cls预算单 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                //str1 = "select * from A预算 " + sWhere;
                //str1 = "select top 50000 * from V预算统计 " + sWhere;
                str1 = "select * from V预算统计 " + sWhere;
                if (sWhere.IndexOf(" limit ") < 0) str1 = str1 + " limit 0,500";   //MySQL
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    cnn.Close();
                    return shenbans;
                }

                //sb = new cls预算单();
                //    sb.ID = "11";
                //    sb.预算编号 = str1;                    
                //    sb.来源 = tb.Rows.Count.ToString();
                //shenbans.Add(sb);

                for (k = 0; k < tb.Rows.Count; k++)
                {
                    sb = new cls预算单();
                    sb.ID = tb.Rows[k]["ID"].ToString();
                    sb.预算编号 = tb.Rows[k]["预算编号"].ToString();    //CommonFunctions.ValInt(tb.Rows[k]["预算编号"].ToString());
                    //sb.作废 = CommonFunctions.ValInt(tb.Rows[k]["作废"].ToString());
                    sb.来源 = tb.Rows[k]["来源"].ToString();
                    sb.预算名称 = tb.Rows[k]["预算名称"].ToString();
                    sb.成本中心编号 = tb.Rows[k]["成本中心编号"].ToString();
                    sb.业务类型 = tb.Rows[k]["业务类型"].ToString();
                    sb.销售 = tb.Rows[k]["销售"].ToString();
                    sb.预算说明 = tb.Rows[k]["预算说明"].ToString();
                    sb.申请人 = tb.Rows[k]["申请人"].ToString();
                    sb.申请日期 = DateTime.Parse(tb.Rows[k]["申请日期"].ToString());

                    sb.审批人 = tb.Rows[k]["审批人"].ToString();
                    sb.审批日期 = DateTime.Parse(tb.Rows[k]["审批日期"].ToString());
                    sb.审批结果 = tb.Rows[k]["审批结果"].ToString();
                    sb.审批意见 = tb.Rows[k]["审批意见"].ToString();
                    sb.核定人 = tb.Rows[k]["核定人"].ToString();
                    sb.核定日期 = DateTime.Parse(tb.Rows[k]["核定日期"].ToString());
                    sb.核定结果 = tb.Rows[k]["核定结果"].ToString();
                    sb.核定意见 = tb.Rows[k]["核定意见"].ToString();

                    sb.预算状态 = tb.Rows[k]["预算状态"].ToString();
                    sb.类型 = CommonFunctions.ValInt(tb.Rows[k]["类型"].ToString());
                    if (tb.Rows[k]["父ID"] != DBNull.Value) sb.父ID = tb.Rows[k]["父ID"].ToString();
                    sb.完成日期 = DateTime.Parse(tb.Rows[k]["完成日期"].ToString());

                    sb.是否中证通订单 = tb.Rows[k]["是否中证通订单"].ToString();
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
                    sb.线上线下 = tb.Rows[k]["线上线下"].ToString();
                    sb.导入日期 = DateTime.Parse(tb.Rows[k]["导入日期"].ToString());
                    sb.子订单号 = tb.Rows[k]["子订单号"].ToString();

                    sb.管理_激励金额 = CommonFunctions.ValDec(tb.Rows[k]["激励金额"].ToString());
                    sb.管理_转账金额 = CommonFunctions.ValDec(tb.Rows[k]["转账金额"].ToString());
                    sb.管理_预算收入金额 = CommonFunctions.ValDec(tb.Rows[k]["预算收入金额"].ToString());
                    sb.管理_预算支出金额 = CommonFunctions.ValDec(tb.Rows[k]["预算支出金额"].ToString());
                    sb.管理_实际收入金额 = CommonFunctions.ValDec(tb.Rows[k]["实际收入金额"].ToString());
                    sb.管理_实际支出金额 = CommonFunctions.ValDec(tb.Rows[k]["实际支出金额"].ToString());
                    sb.管理_预算销项税额 = CommonFunctions.ValDec(tb.Rows[k]["预算销项税额"].ToString());
                    sb.管理_预算进项税额 = CommonFunctions.ValDec(tb.Rows[k]["预算进项税额"].ToString());

                    if (tb.Rows.Count < 2)   //数量多会导致查询死掉，所以只有在查询单条记录时再搜索子数据
                    {
                        // 获取预算收支
                        str1 = "select * from A预算收支 where 预算ID='" + sb.ID.ToString() + "' order by 录入日期";
                        MySqlDataAdapter adpyssz = new MySqlDataAdapter(str1, cnn);
                        DataSet dsyssz = new DataSet();
                        adpyssz.Fill(dsyssz);
                        DataTable tbyssz = dsyssz.Tables[0];
                        for (i = 0; i < tbyssz.Rows.Count; i++)
                        {
                            cls预算收支 yssz = new cls预算收支();
                            yssz.ID = tbyssz.Rows[i]["ID"].ToString();
                            yssz.预算ID = tbyssz.Rows[i]["预算ID"].ToString();
                            yssz.收支 = tbyssz.Rows[i]["收支"].ToString();
                            yssz.类型 = tbyssz.Rows[i]["类型"].ToString();
                            yssz.借贷方 = tbyssz.Rows[i]["借贷方"].ToString();
                            yssz.金额 = CommonFunctions.ValDec(tbyssz.Rows[i]["金额"].ToString());
                            yssz.日期 = DateTime.Parse(tbyssz.Rows[i]["日期"].ToString());
                            yssz.收付税额 = CommonFunctions.ValDec(tbyssz.Rows[i]["收付税额"].ToString());
                            yssz.收付税率 = CommonFunctions.ValDec(tbyssz.Rows[i]["收付税率"].ToString());
                            yssz.收付发票种类 = tbyssz.Rows[i]["收付发票种类"].ToString();
                            yssz.备注 = tbyssz.Rows[i]["备注"].ToString();
                            yssz.录入人 = tbyssz.Rows[i]["录入人"].ToString();
                            yssz.录入日期 = DateTime.Parse(tbyssz.Rows[i]["录入日期"].ToString());
                            sb.预算收支.Add(yssz);
                        }

                        // 获取内部转账
                        str1 = "select * from A内部转账 where 预算ID='" + sb.ID.ToString() + "' order by 录入日期";
                        MySqlDataAdapter adpnbzz = new MySqlDataAdapter(str1, cnn);
                        DataSet dsnbzz = new DataSet();
                        adpnbzz.Fill(dsnbzz);
                        DataTable tbnbzz = dsnbzz.Tables[0];
                        for (i = 0; i < tbnbzz.Rows.Count; i++)
                        {
                            cls内部转账 nbzz = new cls内部转账();
                            nbzz.ID = tbnbzz.Rows[i]["ID"].ToString();
                            nbzz.预算ID = tbnbzz.Rows[i]["预算ID"].ToString();
                            nbzz.转账对象 = tbnbzz.Rows[i]["转账对象"].ToString();
                            nbzz.转账金额 = CommonFunctions.ValDec(tbnbzz.Rows[i]["转账金额"].ToString());
                            nbzz.转账比例 = CommonFunctions.ValDec(tbnbzz.Rows[i]["转账比例"].ToString());
                            nbzz.备注 = tbnbzz.Rows[i]["备注"].ToString();
                            nbzz.录入人 = tbnbzz.Rows[i]["录入人"].ToString();
                            nbzz.录入日期 = DateTime.Parse(tbnbzz.Rows[i]["录入日期"].ToString());
                            sb.内部转账.Add(nbzz);
                        }

                        // 获取预算发票
                        str1 = "select * from A预算发票 where 预算ID='" + sb.ID.ToString() + "' order by 录入日期";
                        MySqlDataAdapter adpysfp = new MySqlDataAdapter(str1, cnn);
                        DataSet dsysfp = new DataSet();
                        adpysfp.Fill(dsysfp);
                        DataTable tbysfp = dsysfp.Tables[0];
                        for (i = 0; i < tbysfp.Rows.Count; i++)
                        {
                            cls预算发票 ysfp = new cls预算发票();
                            ysfp.ID = tbysfp.Rows[i]["ID"].ToString();
                            ysfp.预算ID = tbysfp.Rows[i]["预算ID"].ToString();
                            ysfp.收票方 = tbysfp.Rows[i]["收票方"].ToString();
                            ysfp.发票种类 = tbysfp.Rows[i]["发票种类"].ToString();
                            ysfp.应收税额 = CommonFunctions.ValDec(tbysfp.Rows[i]["应收税额"].ToString());
                            ysfp.税点 = CommonFunctions.ValDec(tbysfp.Rows[i]["税点"].ToString());
                            ysfp.备注 = tbysfp.Rows[i]["备注"].ToString();
                            ysfp.录入人 = tbysfp.Rows[i]["录入人"].ToString();
                            ysfp.录入日期 = DateTime.Parse(tbysfp.Rows[i]["录入日期"].ToString());
                            sb.预算发票.Add(ysfp);
                        }

                        // 获取实际收支
                        str1 = "select * from A实际收支 where 预算ID='" + sb.ID.ToString() + "' order by 录入日期";
                        MySqlDataAdapter adpsjsz = new MySqlDataAdapter(str1, cnn);
                        DataSet dssjsz = new DataSet();
                        adpsjsz.Fill(dssjsz);
                        DataTable tbsjsz = dssjsz.Tables[0];
                        for (i = 0; i < tbsjsz.Rows.Count; i++)
                        {
                            cls实际收支 sjsz = new cls实际收支();
                            sjsz.ID = tbsjsz.Rows[i]["ID"].ToString();
                            sjsz.预算ID = tbsjsz.Rows[i]["预算ID"].ToString();
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

                        //// 获取激励
                        //str1 = "select * from A激励 where 预算ID='" + sb.ID.ToString() + "' order by 录入日期 desc";
                        //MySqlDataAdapter adpjl = new MySqlDataAdapter(str1, cnn);
                        //DataSet dsjl = new DataSet();
                        //adpjl.Fill(dsjl);
                        //DataTable tbjl = dsjl.Tables[0];                    
                        //if (tbjl.Rows.Count > 0)
                        //{
                        //    try
                        //    {
                        //        cls激励 jl = new cls激励();
                        //        jl.ID = tbjl.Rows[tbjl.Rows.Count - 1]["ID"].ToString();
                        //        jl.预算ID = tbjl.Rows[tbjl.Rows.Count - 1]["预算ID"].ToString();
                        //        jl.人员 = tbjl.Rows[tbjl.Rows.Count - 1]["人员"].ToString();
                        //        jl.金额 = CommonFunctions.ValDec(tbjl.Rows[tbjl.Rows.Count - 1]["金额"].ToString());
                        //        jl.比例 = CommonFunctions.ValDec(tbjl.Rows[tbjl.Rows.Count - 1]["比例"].ToString());
                        //        jl.备注 = tbjl.Rows[tbjl.Rows.Count - 1]["备注"].ToString();
                        //        jl.录入人 = tbjl.Rows[tbjl.Rows.Count - 1]["录入人"].ToString();
                        //        jl.录入日期 = DateTime.Parse(tbjl.Rows[tbjl.Rows.Count - 1]["录入日期"].ToString());
                        //        jl.审批人 = tbjl.Rows[tbjl.Rows.Count - 1]["审批人"].ToString();
                        //        jl.审批日期 = DateTime.Parse(tbjl.Rows[tbjl.Rows.Count - 1]["审批日期"].ToString());
                        //        jl.审批结果 = tbjl.Rows[tbjl.Rows.Count - 1]["审批结果"].ToString();
                        //        jl.审批意见 = tbjl.Rows[tbjl.Rows.Count - 1]["审批意见"].ToString();
                        //        sb.激励 = jl;
                        //    }
                        //    catch { }
                        //}
                        // 获取激励
                        str1 = "select * from A激励 where 预算ID='" + sb.ID.ToString() + "' order by 录入日期";
                        MySqlDataAdapter adpjl = new MySqlDataAdapter(str1, cnn);
                        DataSet dsjl = new DataSet();
                        adpjl.Fill(dsjl);
                        DataTable tbjl = dsjl.Tables[0];
                        for (i = 0; i < tbjl.Rows.Count; i++)
                        {
                            cls激励 jl = new cls激励();
                            jl.ID = tbjl.Rows[i]["ID"].ToString();
                            jl.预算ID = tbjl.Rows[i]["预算ID"].ToString();
                            jl.人员 = tbjl.Rows[i]["人员"].ToString();
                            jl.金额 = CommonFunctions.ValDec(tbjl.Rows[i]["金额"].ToString());
                            jl.比例 = CommonFunctions.ValDec(tbjl.Rows[i]["比例"].ToString());
                            jl.备注 = tbjl.Rows[i]["备注"].ToString();
                            jl.录入人 = tbjl.Rows[i]["录入人"].ToString();
                            jl.录入日期 = DateTime.Parse(tbjl.Rows[i]["录入日期"].ToString());
                            jl.审批人 = tbjl.Rows[i]["审批人"].ToString();
                            jl.审批日期 = DateTime.Parse(tbjl.Rows[i]["审批日期"].ToString());
                            jl.审批结果 = tbjl.Rows[i]["审批结果"].ToString();
                            jl.审批意见 = tbjl.Rows[i]["审批意见"].ToString();
                            sb.激励.Add(jl);
                        }

                        // 获取变更
                        str1 = "select * from A变更 where 预算ID='" + sb.ID.ToString() + "' order by 录入日期";
                        MySqlDataAdapter adpbg = new MySqlDataAdapter(str1, cnn);
                        DataSet dsbg = new DataSet();
                        adpbg.Fill(dsbg);
                        DataTable tbbg = dsbg.Tables[0];
                        for (i = 0; i < tbbg.Rows.Count; i++)
                        {
                            cls变更 bg = new cls变更();
                            bg.ID = tbbg.Rows[i]["ID"].ToString();
                            bg.预算ID = tbbg.Rows[i]["预算ID"].ToString();
                            bg.录入人 = tbbg.Rows[i]["录入人"].ToString();
                            bg.录入日期 = DateTime.Parse(tbbg.Rows[i]["录入日期"].ToString());
                            bg.变更原因 = tbbg.Rows[i]["变更原因"].ToString();
                            bg.审批人 = tbbg.Rows[i]["审批人"].ToString();
                            bg.审批日期 = DateTime.Parse(tbbg.Rows[i]["审批日期"].ToString());
                            bg.审批结果 = tbbg.Rows[i]["审批结果"].ToString();
                            bg.审批意见 = tbbg.Rows[i]["审批意见"].ToString();
                            sb.变更.Add(bg);
                        }

                        //// 获取附件
                        //str1 = "select * from A预算附件 where 预算ID='" + sb.ID.ToString() + "' order by 服务器文件名";
                        //MySqlDataAdapter adpfj = new MySqlDataAdapter(str1, cnn);
                        //DataSet dsfj = new DataSet();
                        //adpfj.Fill(dsfj);
                        //DataTable tbfj = dsfj.Tables[0];
                        //for (i = 0; i < tbfj.Rows.Count; i++)
                        //{
                        //    cls附件 fj = new cls附件();
                        //    fj.ID = tbfj.Rows[i]["ID"].ToString();
                        //    fj.预算ID = tbfj.Rows[i]["预算ID"].ToString(); 
                        //    fj.序号 = CommonFunctions.ValInt(tbfj.Rows[i]["序号"].ToString());
                        //    fj.服务器文件名 = tbfj.Rows[i]["服务器文件名"].ToString();
                        //    sb.附件.Add(fj);
                        //}
                    }

                    shenbans.Add(sb);
                }

                cnn.Close();
                return shenbans;
            }
            catch(Exception ex)
            {
                //sb = new cls预算单();
                //sb.ID = "22";
                //sb.预算编号 = ex.Message;
                //sb.预算名称 = "错误";
                //shenbans.Add(sb);

                return shenbans; 
            }
        }

        public static string 保存预计收支(string 预算ID, cls预算收支 yssz)
        {
            int i = 0;
            string sErr = "";
            int k = 0;

            if (预算ID == null) return "无效的记录。";
            if (预算ID == "") return "无效的记录。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr = null;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A预算收支 where 预算ID='" + 预算ID + "'", cnn);
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
                dr["预算ID"] = 预算ID;
                dr["收支"] = yssz.收支;
                dr["类型"] = yssz.类型;
                dr["借贷方"] = yssz.借贷方;
                dr["金额"] = yssz.金额;
                dr["日期"] = yssz.日期;
                dr["收付税额"] = yssz.收付税额;
                dr["收付税率"] = yssz.收付税率;
                dr["收付发票种类"] = yssz.收付发票种类;
                dr["备注"] = yssz.备注;
                dr["录入人"] = yssz.录入人;
                dr["录入日期"] = yssz.录入日期;

                if (isnew)
                {
                    tb.Rows.Add(dr);
                }

                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（预算收入/支出）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }
        public static string 保存内部转账(string 预算ID, cls内部转账 nbzz)
        {
            int i = 0;
            string sErr = "";
            int k = 0;

            if (预算ID == null) return "无效的记录。";
            if (预算ID == "") return "无效的记录。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr = null;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A内部转账 where 预算ID='" + 预算ID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                bool isnew = true;
                for (i = 0; i < tb.Rows.Count; i++)
                {
                    if (tb.Rows[i]["ID"].ToString() == nbzz.ID.ToString())
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

                dr["ID"] = nbzz.ID;
                dr["预算ID"] = 预算ID;
                dr["转账对象"] = nbzz.转账对象;
                dr["转账金额"] = nbzz.转账金额;
                dr["转账比例"] = nbzz.转账比例;
                dr["备注"] = nbzz.备注;
                dr["录入人"] = nbzz.录入人;
                dr["录入日期"] = nbzz.录入日期;

                if (isnew)
                {
                    tb.Rows.Add(dr);
                }

                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（内部转账）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }
        public static string 保存预算发票(string 预算ID, cls预算发票 ysfp)
        {
            int i = 0;
            string sErr = "";
            int k = 0;

            if (预算ID == null) return "无效的记录。";
            if (预算ID == "") return "无效的记录。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr = null;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A预算发票 where 预算ID='" + 预算ID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                bool isnew = true;
                for (i = 0; i < tb.Rows.Count; i++)
                {
                    if (tb.Rows[i]["ID"].ToString() == ysfp.ID.ToString())
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

                dr["ID"] = ysfp.ID;
                dr["预算ID"] = 预算ID;
                dr["收票方"] = ysfp.收票方;
                dr["发票种类"] = ysfp.发票种类;
                dr["应收税额"] = ysfp.应收税额;
                dr["税点"] = ysfp.税点;
                dr["备注"] = ysfp.备注;
                dr["录入人"] = ysfp.录入人;
                dr["录入日期"] = ysfp.录入日期;

                if (isnew)
                {
                    tb.Rows.Add(dr);
                }

                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（应收税额）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }
        public static string 保存实际收支(string 预算ID, cls实际收支 sjsz)
        {
            int i = 0;
            string sErr = "";
            int k = 0;

            if (预算ID == null) return "无效的记录。";
            if (预算ID == "") return "无效的记录。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr = null;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A实际收支 where 预算ID='" + 预算ID + "'", cnn);
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
                dr["预算ID"] = 预算ID;
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

                cbu = new MySqlCommandBuilder(adp);
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

        public static string 保存实际收支批量(List<cls预算单> 预算s, cls实际收支 sjsz, string 收入支出)
        {
            int i = 0;
            string str1 = "";
            string sErr = "";
            int k = 0;
            decimal dec1 = 0.00m;
            decimal dec2 = 0.00m;
            DataRow dr = null;

            if (预算s == null) return "无效的记录。";
            if (预算s.Count == 0) return "无效的记录。";

            for (i = 0; i < 预算s.Count; i++)
            {
                str1 = str1 + "'" + 预算s[i].ID.ToString() + "'";
                if (i < 预算s.Count - 1) str1 = str1 + ",";
            }
            str1 = "select ID,(预算收入金额-实际收入金额) as 收入金额,(预算支出金额-实际支出金额) as 支出金额 from V预算统计 where ID in (" + str1 + ")";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;                
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter(str1, cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                for (i = 0; i < tb.Rows.Count; i++)
                {
                    dec1 = dec1 + CommonFunctions.ValDec(tb.Rows[i][收入支出 + "金额"].ToString());
                }

                MySqlDataAdapter adp1 = new MySqlDataAdapter("select * from A实际收支 where 1=2", cnn);
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
                    dr["预算ID"] = tb.Rows[i]["ID"];
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

                    foreach(cls预算单 yd in 预算s)
                    {
                        if (yd.ID.ToString() == tb.Rows[i]["ID"].ToString())
                        {
                            yd.管理_1 = decimal.Round(dec2 * 1.00m, 2).ToString();
                            break;
                        }
                    }
                }

                cbu = new MySqlCommandBuilder(adp1);
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


        public static int 删除预计收支(string sqrID)
        {
            return main.执行SQL命令("delete from A预算收支 where ID='" + sqrID + "'");
        }
        public static int 删除内部转账(string sqrID)
        {
            return main.执行SQL命令("delete from A内部转账 where ID='" + sqrID + "'");
        }
        public static int 删除预算发票(string sqrID)
        {
            return main.执行SQL命令("delete from A预算发票 where ID='" + sqrID + "'");
        }
        public static int 删除实际收支(string sqrID)
        {
            return main.执行SQL命令("delete from A实际收支 where ID='" + sqrID + "'");
        }

        public static int 预算送审(string id)
        {
            //return main.执行SQL命令("update A预算 set 申请日期=getdate(),预算状态='待审批',审批结果='',核定结果='' where ID='" + id + "'");
            return main.执行SQL命令("update A预算 set 申请日期=now(),预算状态='待审批',审批结果='',核定结果='' where ID='" + id + "'");    //Mysql
        }
        public static int 预算审批(string id, string 处理人, string 处理结果, string 处理意见, string 申请人, string 预算类型)
        {
            int jj = 0;
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            string str3 = "";
            int m = 0;
            int n = 0;

            string[,] ids = new string[1, 4];
            if ((申请人 == "批量审批") && (预算类型 == "批量审批"))
            {
                str1 = id;
                str1 = str1.Replace("chk_", "");
                string[] sstmp1 = str1.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                i = sstmp1.Length;
                if (i < 1) return 0;
                ids = new string[i, 4];
                for (i = 0; i < sstmp1.Length; i++)
                {
                    str1 = "";
                    str2 = "";
                    DataTable tbx = main.获取一般数据集("select 申请人,类型,预算名称 from A预算 where ID='" + sstmp1[i] + "'");
                    try { str1 = tbx.Rows[0]["申请人"].ToString(); }
                    catch { }
                    try { str2 = tbx.Rows[0]["类型"].ToString(); }
                    catch { }
                    try { str3 = tbx.Rows[0]["预算名称"].ToString(); }
                    catch { }

                    ids[i, 0] = sstmp1[i];
                    ids[i, 1] = str1;
                    ids[i, 2] = str2;
                    ids[i, 3] = str3;
                }
            }
            else
            {
                ids[0, 0] = id;
                ids[0, 1] = 申请人;
                ids[0, 2] = 预算类型;
                ids[0, 3] = "";
                try { ids[0, 3] = main.获取一般数据集("select 预算名称 from A预算 where ID='" + id + "'").Rows[0][0].ToString(); }
                catch { }
            }

            string mrzzfqr = "";  //转账对象默认预算发起人
            try { mrzzfqr = main.获取一般数据集("select 参数值 from Z参数 where 参数名='转账对象默认预算发起人'").Rows[0][0].ToString(); }
            catch { }

            for (jj = 0; jj <= ids.GetUpperBound(0); jj++)
            {



                if (处理结果 == "同意")
                {
                    str2 = "待核定";
                }
                else
                {
                    str2 = "待申请";
                }
                //str1 = "update A预算 set 预算状态=N'" + str2 + "',审批人=N'" + 处理人 + "',审批结果=N'" + 处理结果 + "',审批日期=getdate(),审批意见=N'" + 处理意见.Replace("\"", "").Replace("'", "").Replace("\r", "").Replace("\n", "") + "' where ID='" + ids[jj, 0] + "'";
                str1 = "update A预算 set 预算状态='" + str2 + "',审批人='" + 处理人 + "',审批结果='" + 处理结果 + "',审批日期=now(),审批意见='" + 处理意见.Replace("\"", "").Replace("'", "").Replace("\r", "").Replace("\n", "") + "' where ID='" + ids[jj, 0] + "'";   //Mysql

                m = 0;
                m = main.执行SQL命令(str1);
                n = n + m;

                if ((m > 0) && ((预算类型 == "1") | (预算类型 == "批量审批")) && (处理结果 == "同意"))   //预算分解及激励转移
                {
                    try
                    {
                        string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();
                        MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                        cnn.Open();

                        #region 内部转账
                        str1 = "select * from A内部转账 where 预算ID='" + ids[jj, 0] + "' order by 录入日期";
                        MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        DataTable tb = ds.Tables[0];
                        if (tb.Rows.Count > 0)
                        {
                            string[] g = new string[tb.Rows.Count];
                            for (i = 0; i < tb.Rows.Count; i++)
                            {
                                g[i] = Guid.NewGuid().ToString();
                            }



                            str1 = "select * from A预算 where 1=2";
                            MySqlDataAdapter adp1 = new MySqlDataAdapter(str1, cnn);
                            DataSet ds1 = new DataSet();
                            adp1.Fill(ds1);
                            DataTable tb1 = ds1.Tables[0];
                            for (i = 0; i < tb.Rows.Count; i++)
                            {
                                DataRow dr1 = tb1.NewRow();
                                dr1["ID"] = g[i];
                                dr1["类型"] = 1;
                                dr1["父ID"] = ids[jj, 0];
                                dr1["预算编号"] = DateTime.Now.AddSeconds(i).ToString("yyyyMMddHHmmssfff").ToString();
                                dr1["预算名称"] = ids[jj, 3] + "+" + tb.Rows[i]["转账对象"].ToString();   //tb.Rows[i]["转账对象"].ToString() + "(转账对象)";
                                dr1["成本中心编号"] = "";
                                dr1["业务类型"] = "";
                                dr1["销售"] = "";
                                dr1["预算说明"] = "来自内部转账";
                                //dr1["申请人"] = tb.Rows[i]["录入人"].ToString();
                                dr1["申请人"] = CommonFunctions.ReadWordOnly(CommonFunctions.ReadWordOnly(mrzzfqr, tb.Rows[i]["转账对象"].ToString().Trim() + "[", false, true), "]", true, true);
                                //dr1["申请日期"] = DateTime.Now;
                                dr1["预算状态"] = "待申请";
                                tb1.Rows.Add(dr1);
                            }
                            MySqlCommandBuilder cbu1 = new MySqlCommandBuilder(adp1);
                            try
                            {
                                k = adp1.Update(tb1);
                            }
                            catch { }

                            str1 = "select * from A预算收支 where 1=2";
                            MySqlDataAdapter adp2 = new MySqlDataAdapter(str1, cnn);
                            DataSet ds2 = new DataSet();
                            adp2.Fill(ds2);
                            DataTable tb2 = ds2.Tables[0];
                            for (i = 0; i < tb.Rows.Count; i++)
                            {
                                DataRow dr2 = tb2.NewRow();
                                dr2["ID"] = Guid.NewGuid().ToString();
                                dr2["预算ID"] = g[i];
                                dr2["收支"] = "收入";
                                dr2["类型"] = "";
                                dr2["金额"] = decimal.Round(CommonFunctions.ValDec(tb.Rows[i]["转账金额"].ToString()), 2);
                                dr2["借贷方"] = tb.Rows[i]["转账对象"].ToString();
                                //dr2["日期"] = "";
                                //dr2["备注"] = "";
                                //dr2["收付税额"] = "";
                                //dr2["收付税率"] = "";
                                //dr2["收付发票种类"] = "";
                                //dr2["录入人"] = tb.Rows[i]["录入人"].ToString();
                                dr2["录入人"] = CommonFunctions.ReadWordOnly(CommonFunctions.ReadWordOnly(mrzzfqr, tb.Rows[i]["转账对象"].ToString().Trim() + "[", false, true), "]", true, true);
                                dr2["录入日期"] = DateTime.Parse(tb.Rows[i]["录入日期"].ToString());
                                tb2.Rows.Add(dr2);
                            }
                            MySqlCommandBuilder cbu2 = new MySqlCommandBuilder(adp2);
                            try
                            {
                                k = adp2.Update(tb2);
                            }
                            catch { }
                        }
                        #endregion


                        #region 激励转移
                        string xiaoshou = "";
                        decimal decsr = 0.00m;
                        string sqr = "";
                        str1 = "select 销售,预算收入金额,预算支出金额,申请人 from V预算统计 where ID='" + ids[jj, 0] + "'";
                        adp = new MySqlDataAdapter(str1, cnn);
                        ds = new DataSet();
                        adp.Fill(ds);
                        tb = ds.Tables[0];
                        if (tb.Rows.Count > 0)
                        {
                            xiaoshou = tb.Rows[0]["销售"].ToString();
                            xiaoshou = xiaoshou.Trim();
                            decsr = CommonFunctions.ValDec(tb.Rows[0]["预算收入金额"].ToString()) - CommonFunctions.ValDec(tb.Rows[0]["预算支出金额"].ToString());
                            sqr = tb.Rows[0]["申请人"].ToString();
                        }
                        if (xiaoshou != "")  //有销售人员
                        {
                            string[] xss = xiaoshou.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            decimal bl = 0.00m;
                            str1 = "select 参数值 from Z参数 where 参数名='项目销售激励比例'";
                            adp = new MySqlDataAdapter(str1, cnn);
                            ds = new DataSet();
                            adp.Fill(ds);
                            try
                            {
                                bl = CommonFunctions.ValDec(ds.Tables[0].Rows[0][0].ToString());
                            }
                            catch { }

                            MySqlDataAdapter adp1 = new MySqlDataAdapter("select * from A预算收支 where 1=2", cnn);
                            DataSet ds1 = new DataSet();
                            adp1.Fill(ds1);
                            DataTable tb1 = ds1.Tables[0];
                            for (i = 0; i < xss.Length; i++)
                            {
                                decimal decje = 0.00m;
                                try
                                {
                                    decje = decimal.Round(decsr * bl / 100 / xss.Length, 2);
                                }
                                catch { }

                                DataRow dr1 = tb1.NewRow();
                                dr1["类型"] = "激励";
                                dr1["ID"] = Guid.NewGuid().ToString();
                                dr1["预算ID"] = ids[jj, 0];
                                dr1["收支"] = "支出";
                                dr1["金额"] = decje;
                                dr1["借贷方"] = xss[i];
                                dr1["日期"] = DateTime.Parse("1900-01-01");
                                dr1["收付税额"] = 0;
                                dr1["收付税率"] = 0;
                                dr1["收付发票种类"] = "";
                                dr1["备注"] = "系统自动产生";
                                dr1["录入人"] = sqr;
                                dr1["录入日期"] = DateTime.Now;
                                tb1.Rows.Add(dr1);
                            }
                            MySqlCommandBuilder cbu1 = new MySqlCommandBuilder(adp1);
                            int v = 0;
                            v = adp1.Update(tb1);
                        }
                        #endregion

                        cnn.Close();
                    }
                    catch
                    { }
                }

                记录处理过程(ids[jj, 0], "预算审批", 处理人, 处理结果, 处理意见);

                //把有待审批的消息设置为已阅
                str1 = "update A消息 set 已阅='是' where 相关ID='" + ids[jj, 0] + "' and 接收人='" + 处理人 + "' and 相关业务='预算申请'";
                main.执行SQL命令(str1);

                str1 = "";
                try
                {
                    str1 = main.获取一般数据集("select 预算名称 from A预算 where ID='" + ids[jj, 0] + "'").Rows[0][0].ToString();
                }
                catch { }
                str1 = str1.Replace("'", "").Replace("\"", "");

                发送通知消息(ids[jj, 0], "预算审批", 处理人 + "审批" + 处理结果 + "了您的名为“" + str1 + "”的预算申请。", ids[jj, 1], 处理人);

                if (处理结果 == "同意")
                {
                    str2 = "";
                    try
                    {
                        str2 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='项目核定人'").Rows[0][0].ToString();
                    }
                    catch { }
                    发送通知消息(ids[jj, 0], "预算审批", "您有新的名为“" + str1 + "”的预算需要核定。", str2.Trim(), 处理人);
                }


            }
            return n;
        }
        //public static int 预算核定(string id, string 处理人, string 处理结果, string 处理意见, string 申请人)
        //{
        //    string str1 = "";
        //    string str2 = "";
        //    int m = 0;

        //    if (处理结果 == "同意")
        //    {
        //        str2 = "待完成";
        //    }
        //    else
        //    {
        //        str2 = "待申请";
        //    }
        //    //str1 = "update A预算 set 预算状态=N'" + str2 + "',核定人=N'" + 处理人 + "',核定结果=N'" + 处理结果 + "',核定日期=getdate(),核定意见=N'" + 处理意见.Replace("\"", "").Replace("'", "").Replace("\r", "").Replace("\n", "") + "' where ID='" + id + "'";
        //    str1 = "update A预算 set 预算状态='" + str2 + "',核定人='" + 处理人 + "',核定结果='" + 处理结果 + "',核定日期=now(),核定意见='" + 处理意见.Replace("\"", "").Replace("'", "").Replace("\r", "").Replace("\n", "") + "' where ID='" + id + "'";   //Mysql
            
        //    m = main.执行SQL命令(str1);

        //    记录处理过程(id, "预算核定", 处理人, 处理结果, 处理意见);

        //    //把有待核定的消息设置为已阅
        //    str1 = "update A消息 set 已阅='是' where 相关ID='" + id + "' and 接收人='" + 处理人 + "' and 相关业务='预算审批'";
        //    main.执行SQL命令(str1);

        //    str1 = "";
        //    try
        //    {
        //        str1 = main.获取一般数据集("select 预算名称 from A预算 where ID='" + id + "'").Rows[0][0].ToString();
        //    }
        //    catch { }
        //    str1 = str1.Replace("'", "").Replace("\"", "");

        //    发送通知消息(id, "预算核定", 处理人 + "核定" + 处理结果 + "了您的名为“" + str1 + "”的预算申请。", 申请人, 处理人);
            
        //    return m;
        //}
        public static int 预算核定(string id, string 处理人, string 处理结果, string 处理意见, string 申请人)
        {
            int jj = 0;
            int i = 0;
            int k = 0;
            string str1 = "";
            string str2 = "";
            int m = 0;
            int n = 0;

            string[,] ids = new string[1, 2];
            if (申请人 == "批量核定")
            {
                str1 = id;
                str1 = str1.Replace("chk_", "");
                string[] sstmp1 = str1.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                i = sstmp1.Length;
                if (i < 1) return 0;
                ids = new string[i, 2];
                for (i = 0; i < sstmp1.Length; i++)
                {
                    str1 = "";
                    str2 = "";
                    DataTable tbx = main.获取一般数据集("select 申请人 from A预算 where ID='" + sstmp1[i] + "'");
                    try { str1 = tbx.Rows[0]["申请人"].ToString(); }
                    catch { }

                    ids[i, 0] = sstmp1[i];
                    ids[i, 1] = str1;
                }
            }
            else
            {
                ids[0, 0] = id;
                ids[0, 1] = 申请人;
            }

            for (jj = 0; jj <= ids.GetUpperBound(0); jj++)
            {

                if (处理结果 == "同意")
                {
                    str2 = "待完成";
                }
                else
                {
                    str2 = "待申请";
                }
                //str1 = "update A预算 set 预算状态=N'" + str2 + "',核定人=N'" + 处理人 + "',核定结果=N'" + 处理结果 + "',核定日期=getdate(),核定意见=N'" + 处理意见.Replace("\"", "").Replace("'", "").Replace("\r", "").Replace("\n", "") + "' where ID='" + ids[jj, 0] + "'";
                str1 = "update A预算 set 预算状态='" + str2 + "',核定人='" + 处理人 + "',核定结果='" + 处理结果 + "',核定日期=now(),核定意见='" + 处理意见.Replace("\"", "").Replace("'", "").Replace("\r", "").Replace("\n", "") + "' where ID='" + ids[jj, 0] + "'";   //Mysql

                m = main.执行SQL命令(str1);
                n = n + m;

                记录处理过程(ids[jj, 0], "预算核定", 处理人, 处理结果, 处理意见);

                //把有待核定的消息设置为已阅
                str1 = "update A消息 set 已阅='是' where 相关ID='" + ids[jj, 0] + "' and 接收人='" + 处理人 + "' and 相关业务='预算审批'";
                main.执行SQL命令(str1);

                str1 = "";
                try
                {
                    str1 = main.获取一般数据集("select 预算名称 from A预算 where ID='" + ids[jj, 0] + "'").Rows[0][0].ToString();
                }
                catch { }
                str1 = str1.Replace("'", "").Replace("\"", "");

                发送通知消息(ids[jj, 0], "预算核定", 处理人 + "核定" + 处理结果 + "了您的名为“" + str1 + "”的预算申请。", ids[jj, 1], 处理人);

            }
            return n;
        }

        public static int 预算完成(string id, string 申请人)
        {
            int m = 0;
            //m = main.执行SQL命令("update A预算 set 完成日期=getdate(),预算状态='已完成' where ID='" + id + "'");
            m = main.执行SQL命令("update A预算 set 完成日期=now(),预算状态='已完成' where ID='" + id + "'");   //Mysql
            记录处理过程(id, "预算完成", 申请人, "完成", "设置项目为完成状态");
            return m;
        }
        public static bool 判断预算是否可以完成(string id)
        {
            bool b1 = false;

            //无实际收支时，不能设置为完成
            int m = 0;
            try
            {
                m = CommonFunctions.ValInt(main.获取一般数据集("select count(*) from A实际收支 where 预算ID='" + id + "'").Rows[0][0].ToString().Trim());
            }
            catch { }
            if (m > 0) return true;
            return false; ;
        }

        public static int 预算删除(string id, string 操作人)
        {
            int m = 0;
            int n = 0;
            m = main.执行SQL命令("delete from A预算 where ID='" + id + "'");   //Mysql
            n = main.执行SQL命令("delete from A预算收支 where 预算ID='" + id + "'");
            n = main.执行SQL命令("delete from A内部转账 where 预算ID='" + id + "'");
            n = main.执行SQL命令("delete from A预算发票 where 预算ID='" + id + "'");
            n = main.执行SQL命令("delete from A预算附件 where 预算ID='" + id + "'");
            记录处理过程(id, "预算删除", 操作人, "删除", "删除预算");
            return m;
        }



        /*
        public static string 保存激励申请(string ysID, string tjlry, string tjlje, string tjlbl, string tjlbz, string tlrr)
        {
            int i = 0;
            string sErr = "";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A激励 where 预算ID='" + ysID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    dr = tb.NewRow();
                    dr["ID"] = Guid.NewGuid().ToString();
                }
                else
                {
                    dr = tb.Rows[0];
                }
                dr["预算ID"] = ysID;
                dr["人员"] = tjlry;
                dr["金额"] = decimal.Round(CommonFunctions.ValDec(tjlje), 2);
                dr["比例"] = decimal.Round(CommonFunctions.ValDec(tjlbl), 2);
                dr["备注"] = tjlbz;
                dr["录入人"] = tlrr;
                dr["录入日期"] = DateTime.Now;
                if (tb.Rows.Count < 1) tb.Rows.Add(dr);
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（激励申请）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }
        */
        public static string 保存激励申请(string ysID, string tjlry, string tjlje, string tjlbl, string tjlbz, string tlrr)
        {
            int i = 0;
            string sErr = "";
            string str1 = "";
            string str2 = "";
            string jlid = Guid.NewGuid().ToString();

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A激励 where 1=2", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                dr = tb.NewRow();
                dr["ID"] = jlid;
                dr["预算ID"] = ysID;
                dr["人员"] = tjlry;
                dr["金额"] = decimal.Round(CommonFunctions.ValDec(tjlje), 2);
                dr["比例"] = decimal.Round(CommonFunctions.ValDec(tjlbl), 2);
                dr["备注"] = tjlbz;
                dr["录入人"] = tlrr;
                dr["录入日期"] = DateTime.Now;
                tb.Rows.Add(dr);
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（激励申请）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();


                string ln = "";
                ln = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
                if ((ln == string.Empty) | (ln == null)) ln = "";
                str1 = "";
                try
                {
                    str1 = main.获取一般数据集("select 预算名称 from A预算 where ID='" + ysID + "'").Rows[0][0].ToString();
                }
                catch { }
                str1 = str1.Replace("'", "").Replace("\"", "");
                str2 = Budget.获取对应的审批人(ln);
                Budget.发送通知消息(jlid.ToString(), "激励申请", ln + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "发起名为“" + str1 + "”的预算的激励请求。", str2, ln);


                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }

        /*
        public static int 保存激励审批(string ysID, string tspr, string tspjg, string tspyj)
        {
            int i = 0;
            string sErr = "";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A激励 where 预算ID='" + ysID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    sErr = "无效的激励记录。";
                    cnn.Close();
                    return 0;
                }
                else
                {
                    dr = tb.Rows[0];
                }
                dr["审批人"] = tspr;
                dr["审批结果"] = tspjg;
                dr["审批意见"] = tspyj;
                dr["审批日期"] = DateTime.Now;
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（激励审批）。";
                    cnn.Close();
                    return i;
                }

                //if (tspjg == "同意")
                //{
                //    MySqlDataAdapter adp1 = new MySqlDataAdapter("select * from A预算收支 where 1=2", cnn);
                //    DataSet ds1 = new DataSet();
                //    adp1.Fill(ds1);
                //    DataTable tb1 = ds1.Tables[0];
                //    DataRow dr1 = tb1.NewRow();
                //    dr1["类型"] = "激励";
                //    dr1["ID"] = Guid.NewGuid().ToString();
                //    dr1["预算ID"] = ysID;
                //    dr1["收支"] = "支出";
                //    dr1["金额"] = dr["金额"];
                //    dr1["借贷方"] = dr["人员"];
                //    dr1["日期"] = DateTime.Parse("1900-01-01");
                //    dr1["备注"] = dr["备注"];
                //    dr1["录入人"] = dr["录入人"];
                //    dr1["录入日期"] = dr["录入日期"];
                //    MySqlCommandBuilder cbu1 = new MySqlCommandBuilder(adp1);
                //    i = 0;
                //    i = adp1.Update(tb1);
                //}

                cnn.Close();
                return i;
            }
            catch (Exception ex) { return 0; }

        }
        */
        public static int 保存激励审批(string jlID, string ysID, string tspr, string tspjg, string tspyj)
        {
            int i = 0;
            string sErr = "";
            string shenqingren = "";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A激励 where ID='" + jlID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    sErr = "无效的激励记录。";
                    cnn.Close();
                    return 0;
                }
                else
                {
                    dr = tb.Rows[0];
                }
                shenqingren = dr["录入人"].ToString();
                dr["审批人"] = tspr;
                dr["审批结果"] = tspjg;
                dr["审批意见"] = tspyj;
                dr["审批日期"] = DateTime.Now;
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（激励审批）。";
                    cnn.Close();
                    return i;
                }

                if (tspjg == "同意")
                {
                    MySqlDataAdapter adp1 = new MySqlDataAdapter("select * from A预算收支 where 1=2", cnn);
                    DataSet ds1 = new DataSet();
                    adp1.Fill(ds1);
                    DataTable tb1 = ds1.Tables[0];
                    DataRow dr1 = tb1.NewRow();
                    dr1["类型"] = "激励";
                    dr1["ID"] = Guid.NewGuid().ToString();
                    dr1["预算ID"] = ysID;
                    dr1["收支"] = "支出";
                    dr1["金额"] = dr["金额"];
                    dr1["借贷方"] = dr["人员"];
                    dr1["日期"] = DateTime.Parse("1900-01-01");
                    dr1["收付税额"] = dr["收付税额"];
                    dr1["收付税率"] = dr["收付税率"];
                    dr1["收付发票种类"] = dr["收付发票种类"];                    
                    dr1["备注"] = dr["备注"];
                    dr1["录入人"] = dr["录入人"];
                    dr1["录入日期"] = dr["录入日期"];
                    tb1.Rows.Add(dr1);
                    MySqlCommandBuilder cbu1 = new MySqlCommandBuilder(adp1);
                    int k = 0;
                    k = adp1.Update(tb1);
                }
                cnn.Close();
                

                //把有待审批的消息设置为已阅
                string str1 = "update A消息 set 已阅='是' where 相关ID='" + jlID + "' and 接收人='" + tspr + "' and 相关业务='激励申请'";
                main.执行SQL命令(str1);
                str1 = "";
                try
                {
                    str1 = main.获取一般数据集("select 预算名称 from A预算 where ID='" + ysID + "'").Rows[0][0].ToString();
                }
                catch { }
                str1 = str1.Replace("'", "").Replace("\"", "");
                发送通知消息(jlID, "激励审批", tspr + "审批" + tspjg + "了您的名为“" + str1 + "”的预算的激励申请。", shenqingren, tspr);


                return i;
            }
            catch (Exception ex) { return 0; }

        }


        public static List<cls激励> 获取激励记录s(string sWhere)
        {
            int i = 0;
            int k = 0;
            string str1 = "";
            List<cls激励> shenbans = new List<cls激励>();
            cls激励 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                //str1 = "select top 50000 * from V激励和预算 " + sWhere;
                str1 = "select * from V激励和预算 " + sWhere + " limit 0,500";  //Mysql
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
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
                    sb = new cls激励();
                    sb.ID = tb.Rows[k]["ID"].ToString();
                    sb.预算ID = tb.Rows[k]["预算ID"].ToString();
                    sb.管理_1 = tb.Rows[k]["预算编号"].ToString();
                    sb.管理_2 = tb.Rows[k]["预算名称"].ToString();
                    sb.管理_3 = tb.Rows[k]["成本中心编号"].ToString();
                    sb.人员 = tb.Rows[k]["人员"].ToString();
                    sb.金额 = CommonFunctions.ValDec(tb.Rows[k]["金额"].ToString());
                    sb.比例 = CommonFunctions.ValDec(tb.Rows[k]["比例"].ToString());
                    sb.备注 = tb.Rows[k]["备注"].ToString();
                    sb.录入人 = tb.Rows[k]["录入人"].ToString();
                    sb.录入日期 = DateTime.Parse(tb.Rows[k]["录入日期"].ToString());
                    sb.审批人 = tb.Rows[k]["审批人"].ToString();
                    sb.审批日期 = DateTime.Parse(tb.Rows[k]["审批日期"].ToString());
                    sb.审批结果 = tb.Rows[k]["审批结果"].ToString();
                    sb.审批意见 = tb.Rows[k]["审批意见"].ToString();
                    shenbans.Add(sb);
                }

                cnn.Close();
                return shenbans;
            }
            catch
            { return shenbans; }
        }


        public static string 保存变更申请(string ysID, string tbgr, string tbgyy)
        {
            int i = 0;
            string sErr = "";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A变更 where 1=2", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                dr = tb.NewRow();
                dr["ID"] = Guid.NewGuid().ToString();
                dr["预算ID"] = ysID;
                dr["录入人"] = tbgr;
                dr["录入日期"] = DateTime.Now;
                dr["变更原因"] = tbgyy;
                tb.Rows.Add(dr);
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（变更申请）。";
                    cnn.Close();
                    return sErr;
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }
        public static int 保存变更审批(string bgID, string tspr, string tspjg, string tspyj)
        {
            int i = 0;
            string sErr = "";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A变更 where ID='" + bgID + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    sErr = "无效的变更记录。";
                    cnn.Close();
                    return 0;
                }
                else
                {
                    dr = tb.Rows[0];
                }
                dr["审批人"] = tspr;
                dr["审批结果"] = tspjg;
                dr["审批意见"] = tspyj;
                dr["审批日期"] = DateTime.Now;
                cbu = new MySqlCommandBuilder(adp);
                i = 0;
                i = adp.Update(tb);
                if (i < 1)
                {
                    sErr = "保存失败（变更审批）。";
                    cnn.Close();
                    return i;
                }

                cnn.Close();
                return i;
            }
            catch (Exception ex) { return 0; }

        }


        public static string 保存新增附件(string ysID, string 文件名)
        {
            int i = 0;
            string sErr = "";

            if (ysID == null) return "无效的记录。";
            if (ysID == "") return "无效的记录。";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlDataAdapter adp;
                DataSet ds;
                DataTable tb;
                DataRow dr;
                MySqlCommandBuilder cbu;

                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                adp = new MySqlDataAdapter("select * from A预算附件 where 预算ID='" + ysID + "' and 服务器文件名='" + 文件名 + "'", cnn);
                ds = new DataSet();
                adp.Fill(ds);
                tb = ds.Tables[0];
                if (tb.Rows.Count < 1)
                {
                    dr = tb.NewRow();
                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["预算ID"] = ysID;
                    dr["服务器文件名"] = 文件名;
                    tb.Rows.Add(dr);

                    cbu = new MySqlCommandBuilder(adp);
                    i = 0;
                    i = adp.Update(tb);
                    if (i < 1)
                    {
                        sErr = "保存失败（上传的材料）。";
                        cnn.Close();
                        return sErr;
                    }
                }

                cnn.Close();
                return "";
            }
            catch (Exception ex) { return ex.Message; }

        }

        public static List<cls附件> 获取附件s(string sWhere)
        {
            int k = 0;
            string str1 = "";
            List<cls附件> shenbans = new List<cls附件>();
            cls附件 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                //str1 = "select top 50000 * from A预算附件 " + sWhere;
                str1 = "select * from A预算附件 " + sWhere + " limit 0,500";  //Mysql
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
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
                    sb = new cls附件();
                    sb.ID = tb.Rows[k]["ID"].ToString();
                    sb.预算ID = tb.Rows[k]["预算ID"].ToString();
                    sb.序号 = CommonFunctions.ValInt(tb.Rows[k]["序号"].ToString());
                    sb.服务器文件名 = tb.Rows[k]["服务器文件名"].ToString();                   
                    shenbans.Add(sb);
                }

                cnn.Close();
                return shenbans;
            }
            catch
            { return shenbans; }
        }



        #endregion


        #region 处理过程
        public static int 记录处理过程(string 相关ID, string 相关业务, string 处理人, string 处理结果, string 处理意见)
        {
            int i = 0;
            string str1 = "";

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                str1 = "select * from A处理过程 where 1=2";
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                DataRow dr = tb.NewRow();
                dr["ID"] = Guid.NewGuid().ToString();
                dr["相关ID"] = 相关ID;
                dr["相关业务"] = 相关业务;
                dr["处理人"] = 处理人;
                dr["处理结果"] = 处理结果;
                dr["处理意见"] = 处理意见;
                tb.Rows.Add(dr);
                MySqlCommandBuilder cbu = new MySqlCommandBuilder(adp);
                try
                {
                    i = adp.Update(tb);
                }
                catch { }

                cnn.Close();
            }
            catch
            { }
            return i;
        }

        public static int 发送通知消息(string 相关ID, string 相关业务, string 消息内容, string 接收人, string 发送人)
        {
            int i = 0;
            string str1 = "";
            int m = 240;

            if (接收人 == "") return 0;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                str1 = "select 参数值 from Z参数 where 参数名='提醒消息有效期'";
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                try
                {
                    m = CommonFunctions.ValInt(ds.Tables[0].Rows[0][0].ToString());
                }
                catch { }
                if (m < 1) m = 1;

                str1 = "select * from A消息 where 1=2";
                adp = new MySqlDataAdapter(str1, cnn);
                ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                string[] ms = 接收人.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                i = 0;
                for (int x = 0; x < ms.Length; x++)
                {
                    DataRow dr = tb.NewRow();
                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["相关ID"] = 相关ID;
                    dr["相关业务"] = 相关业务;
                    dr["消息内容"] = 消息内容;
                    dr["接收人"] = ms[x];
                    dr["发送人"] = 发送人;
                    dr["失效时间"] = DateTime.Now.AddHours((double)m);
                    tb.Rows.Add(dr);
                }
                MySqlCommandBuilder cbu = new MySqlCommandBuilder(adp);
                try
                {
                    i = i + adp.Update(tb);
                }
                catch { }

                cnn.Close();
            }
            catch
            { }
            return i;
        }

        public static int 获取通知消息条数(string 接收人)
        {
            string str1 = "";
            int m = 0;

            if (接收人 == "") return 0;

            //str1 = "select count(*) from A消息 where 接收人='" + 接收人 + "' and 失效时间>getdate() and 已阅=''";
            str1 = "select count(*) from A消息 where 接收人='" + 接收人 + "' and 失效时间>now() and 已阅=''";   //Mysql
            try
            {
                m = CommonFunctions.ValInt(main.获取一般数据集(str1).Rows[0][0].ToString());
            }
            catch { }
            return m;
        }

        public static int 设置消息为已阅(string id)
        {
            return main.执行SQL命令("update A消息 set 已阅='是' where ID='" + id + "'");
        }

        public static int 设置全部消息为已阅(string 接收人)
        {
            //return main.执行SQL命令("update A消息 set 已阅='是' where 接收人='" + 接收人 + "' and 失效时间>getdate() and 已阅=''");
            return main.执行SQL命令("update A消息 set 已阅='是' where 接收人='" + 接收人 + "' and 失效时间>now() and 已阅=''");    //Mysql
        }

        public static List<cls消息> 获取消息记录s(string sWhere)
        {
            int k = 0;
            string str1 = "";
            List<cls消息> shenbans = new List<cls消息>();
            cls消息 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                //str1 = "select top 5000 * from A消息 " + sWhere;
                str1 = "select * from A消息 " + sWhere + " limit 0,500";  //Mysql
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
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
                    sb = new cls消息();
                    sb.ID = tb.Rows[k]["ID"].ToString();
                    sb.业务ID = tb.Rows[k]["相关ID"].ToString();
                    sb.业务类型 = tb.Rows[k]["相关业务"].ToString();
                    sb.接收人 = tb.Rows[k]["接收人"].ToString();
                    sb.消息内容 = tb.Rows[k]["消息内容"].ToString();                    
                    sb.发送人 = tb.Rows[k]["发送人"].ToString();
                    sb.发送时间 = DateTime.Parse(tb.Rows[k]["发送时间"].ToString());
                    sb.失效时间 = DateTime.Parse(tb.Rows[k]["失效时间"].ToString());
                    sb.已阅 = tb.Rows[k]["已阅"].ToString();

                    shenbans.Add(sb);
                }

                cnn.Close();
                return shenbans;
            }
            catch
            { return shenbans; }
        }

        public static List<cls办理过程> 获取办理过程s(string sWhere)
        {
            int k = 0;
            string str1 = "";
            List<cls办理过程> shenbans = new List<cls办理过程>();
            cls办理过程 sb = null;

            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();

                str1 = "select * from A处理过程 " + sWhere + " limit 0,500";  //Mysql
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
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
                    sb = new cls办理过程();
                    sb.ID = tb.Rows[k]["ID"].ToString();
                    sb.相关ID = tb.Rows[k]["相关ID"].ToString();
                    sb.相关业务 = tb.Rows[k]["相关业务"].ToString();
                    sb.处理人 = tb.Rows[k]["处理人"].ToString();
                    sb.处理日期 = DateTime.Parse(tb.Rows[k]["处理日期"].ToString());
                    sb.处理结果 = tb.Rows[k]["处理结果"].ToString();
                    sb.处理意见 = tb.Rows[k]["处理意见"].ToString();

                    shenbans.Add(sb);
                }

                cnn.Close();
                return shenbans;
            }
            catch
            { return shenbans; }
        }

        #endregion

        #region 系统

        public static List<cls部门> 获取部门(string strWhere)
        {
            List<cls部门> returnval = new List<cls部门>();

            DataTable tb = main.获取一般数据集("select * from Z部门 " + strWhere);
            if (tb != null)
            {
                returnval = (List<cls部门>)(main.ConvertTo<cls部门>(tb));
            }
            return returnval;
        }

        public static List<cls成本中心> 获取成本中心(string strWhere)
        {
            List<cls成本中心> returnval = new List<cls成本中心>();

            DataTable tb = main.获取一般数据集("select * from Z成本中心 " + strWhere);
            if (tb != null)
            {
                returnval = (List<cls成本中心>)(main.ConvertTo<cls成本中心>(tb));
            }
            return returnval;
        }


        public static List<cls用户> 获取用户(string strWhere)
        {
            List<cls用户> returnval = new List<cls用户>();

            DataTable tb = main.获取一般数据集("select * from Z用户 " + strWhere);
            if (tb != null)
            {
                returnval = (List<cls用户>)(main.ConvertTo<cls用户>(tb));
            }
            return returnval;
        }

        public static int 修改密码(string dlm, string xmm)
        {
            int i = 0;
            i = main.执行SQL命令("update Z用户 set 密码='" + xmm + "' where 登录名='" + dlm + "'");
            return i;
        }


        public static int 保存部门(bool 新增, cls部门 bm)
        {
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            int i = 0;
            string str1 = "select * from Z部门";
            if (新增)
                str1 = str1 + " where 1=2";
            else
                str1 = str1 + " where ID='" + bm.ID.ToString() + "'";

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                DataRow dr;
                if (新增)
                    dr = tb.NewRow();
                else
                    dr = tb.Rows[0];
                dr["ID"] = bm.ID;
                dr["编号"] = bm.编号;
                dr["名称"] = bm.名称;
                if (新增)
                    tb.Rows.Add(dr);

                MySqlCommandBuilder cbu = new MySqlCommandBuilder(adp);
                i = adp.Update(tb);

                cnn.Close();
            }
            catch { }
            return i;
        }

        public static int 保存成本中心(bool 新增, cls成本中心 cbzx)
        {
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            int i = 0;
            string str1 = "select * from Z成本中心";
            if (新增)
                str1 = str1 + " where 1=2";
            else
                str1 = str1 + " where ID='" + cbzx.ID.ToString() + "'";

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                DataRow dr;
                if (新增)
                    dr = tb.NewRow();
                else
                    dr = tb.Rows[0];
                dr["ID"] = cbzx.ID;
                dr["成本中心编号"] = cbzx.成本中心编号;
                dr["成本中心名称"] = cbzx.成本中心名称;
                dr["部门"] = cbzx.部门;
                if (新增)
                    tb.Rows.Add(dr);

                MySqlCommandBuilder cbu = new MySqlCommandBuilder(adp);
                i = adp.Update(tb);

                cnn.Close();
            }
            catch { }
            return i;
        }

        public static int 保存用户(bool 新增, cls用户 yh)
        {
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            int i = 0;
            string str1 = "select * from Z用户";
            if (新增)
                str1 = str1 + " where 1=2";
            else
                str1 = str1 + " where ID='" + yh.ID.ToString() + "'";

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                DataRow dr;
                if (新增)
                    dr = tb.NewRow();
                else
                    dr = tb.Rows[0];
                dr["ID"] = yh.ID;
                dr["序号"] = yh.序号;
                dr["登录名"] = yh.登录名;
                dr["姓名"] = yh.姓名;
                dr["密码"] = yh.密码;
                dr["权限"] = yh.权限;
                dr["备注"] = yh.备注;
                dr["部门"] = yh.部门;
                dr["角色"] = yh.角色;
                if (新增)
                    tb.Rows.Add(dr);

                MySqlCommandBuilder cbu = new MySqlCommandBuilder(adp);
                i = adp.Update(tb);

                cnn.Close();
            }
            catch { }
            return i;
        }


        public static List<cls业务类型> 获取业务类型()
        {
            List<cls业务类型> returnval = new List<cls业务类型>();

            string str1 = "";
            try
            {
                str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='项目业务类型'").Rows[0][0].ToString();
            }
            catch { }

            if (str1 != "")
            {
                string[] s = str1.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length > 0)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        cls业务类型 ywlx = new cls业务类型();
                        ywlx.业务类型 = s[i];
                        returnval.Add(ywlx);
                    }
                }
            }

            return returnval;
        }


        //审批人申请人对应，返回对应的申请人：'张三','李四'
        public static string 获取对应的申请人(string 审批人)
        {
            string str1 = "";
            string str2 = "";
            try
            {
                str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='审批人申请人对应'").Rows[0][0].ToString();
            }
            catch { }

            if (str1 != "")
            {
                string[] s = str1.Split(new string[1] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length > 0)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        str2 = s[i].Trim();
                        if (str2.IndexOf(审批人.Trim() + "[") == 0)
                        {
                            CommonFunctions.ReadWord(ref str2, "[");
                            str1 = CommonFunctions.ReadWord(ref str2, "]");
                            str1 = str1.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
                            if (str1 != "")
                            {
                                str1 = str1.Replace(" ", "','");
                                str1 = "'" + str1 + "'";
                                return str1;
                            }
                        }
                    }
                }
            }

            return "''";
        }
        //审批人申请人对应，返回对应审批人：王五
        public static string 获取对应的审批人(string 申请人)
        {
            string str1 = "";
            string str2 = "";
            string smr = "";
            try
            {
                str1 = main.获取一般数据集("select 参数值 from Z参数 where 参数名='审批人申请人对应'").Rows[0][0].ToString();
            }
            catch { }

            if (str1 != "")
            {
                string[] s = str1.Split(new string[1] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length > 0)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        str2 = s[i].Trim();
                        str1 = CommonFunctions.ReadWord(ref str2, "[");
                        if (smr == "") smr = str1;
                        str2 = " " + CommonFunctions.ReadWord(ref str2, "]") + " ";
                        if (str2.IndexOf(" " + 申请人 + " ") > -1)
                        {
                            return str1.Trim();
                        }
                    }
                }
            }

            return smr;
        }
        
        public static List<cls系统参数> 获取系统参数(string strWhere)
        {
            List<cls系统参数> returnval = new List<cls系统参数>();

            DataTable tb = main.获取一般数据集("select * from Z参数 " + strWhere + " order by 参数名");
            if (tb != null)
            {
                returnval = (List<cls系统参数>)(main.ConvertTo<cls系统参数>(tb));
            }
            return returnval;
        }
        public static int 保存系统参数(bool 新增, cls系统参数 yh)
        {
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            int i = 0;
            string str1 = "select * from Z参数";
            if (新增)
                str1 = str1 + " where 1=2";
            else
                str1 = str1 + " where ID='" + yh.ID.ToString() + "'";

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(str1, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable tb = ds.Tables[0];
                DataRow dr;
                if (新增)
                {
                    dr = tb.NewRow();
                    dr["ID"] = yh.ID;
                    dr["参数名"] = yh.参数名;
                    dr["说明"] = yh.说明;
                }
                else
                {
                    dr = tb.Rows[0];
                }
                dr["参数值"] = yh.参数值;

                if (新增)
                    tb.Rows.Add(dr);

                MySqlCommandBuilder cbu = new MySqlCommandBuilder(adp);
                i = adp.Update(tb);

                cnn.Close();
            }
            catch { }
            return i;
        }

        public static List<commData> 获取固定预算名称()
        {
            string str1 = "";
            try
            {
                str1 = 获取系统参数(" where 参数名='固定预算名称'")[0].参数值;
            }
            catch { }
            string[] s = str1.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
            List<commData> r = new List<commData>();
            if (s.Length > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    commData c = new commData();
                    c.Fld1 = s[i];
                    r.Add(c);
                }
            }
            return r;
        }

        public static List<commData> 获取转帐对象()
        {
            string str1 = "";
            try
            {
                str1 = 获取系统参数(" where 参数名='转账对象'")[0].参数值;
            }
            catch { }
            string[] s = str1.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
            List<commData> r = new List<commData>();
            if (s.Length > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    commData c = new commData();
                    c.Fld1 = s[i];
                    r.Add(c);
                }
            }
            return r;
        }

        #endregion
    }
}