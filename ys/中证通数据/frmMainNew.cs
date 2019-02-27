using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CBClassLibraryA;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace 中证通数据
{
    public partial class frmMainNew : Form
    {
        private bool stop = false;

        private DataTable tb公证处 = null;
        private DataTable tb公证事项 = null;
        private DataTable tb渠道 = null;
        private DataTable tb销售 = null;
        private string tb成本中心编号 = "";

        private List<cls激励比例> JiLi = null;

        public frmMainNew()
        {
            InitializeComponent();

            grid.AutoGenerateColumns = false;
            grid.Dock = DockStyle.Fill;

            JiLi = new List<cls激励比例>();
            cls激励比例 jl = new cls激励比例();
            jl.激励名称 = "中证通预算_附件翻译激励比例";
            JiLi.Add(jl);
            jl = new cls激励比例();
            jl.激励名称 = "中证通预算_附件校对激励比例";
            JiLi.Add(jl);
            jl = new cls激励比例();
            jl.激励名称 = "中证通预算_证词翻译激励比例";
            JiLi.Add(jl);
            jl = new cls激励比例();
            jl.激励名称 = "中证通预算_证词校对激励比例";
            JiLi.Add(jl);
            jl = new cls激励比例();
            jl.激励名称 = "中证通预算_自助机翻译激励比例";
            JiLi.Add(jl);
            jl = new cls激励比例();
            jl.激励名称 = "中证通预算_校对激励比例";
            JiLi.Add(jl);
            jl = new cls激励比例();
            jl.激励名称 = "中证通预算_加急激励比例";
            JiLi.Add(jl);
        }

        private void frmMainNew_Load(object sender, EventArgs e)
        {
            Program.AppPath = Application.StartupPath;
            if (Program.AppPath.Substring(Program.AppPath.Length - 1, 1) != "\\") Program.AppPath = Program.AppPath + "\\";
            string str1 = Program.AppPath + "系统配置.ini";

            string s1 = "";
            string s2 = "";
            string s3 = "";
            string s4 = "";
            try
            {
                s1 = ReadWriteIniFile.ReadString("预算数据库", "server", "", str1);
                s2 = ReadWriteIniFile.ReadString("预算数据库", "uid", "sa", str1);
                s3 = ReadWriteIniFile.ReadString("预算数据库", "pwd", "", str1);
                s4 = ReadWriteIniFile.ReadString("预算数据库", "database", "", str1);
                //Program.ys_scnn = @"server=" + s1 + ";uid=" + s2 + ";pwd=" + s3 + ";database=" + s4 + ";pooling=false";
                Program.ys_scnn = @"server=" + s1 + ";user id=" + s2 + ";password=" + s3 + ";database=" + s4;
            }
            catch { }

            s1 = "";
            s2 = "";
            s3 = "";
            s4 = "";
            try
            {
                s1 = ReadWriteIniFile.ReadString("中证通数据库", "server", "", str1);
                s2 = ReadWriteIniFile.ReadString("中证通数据库", "uid", "sa", str1);
                s3 = ReadWriteIniFile.ReadString("中证通数据库", "pwd", "", str1);
                s4 = ReadWriteIniFile.ReadString("中证通数据库", "database", "", str1);
                Program.zzt_scnn = @"server=" + s1 + ";user id=" + s2 + ";password=" + s3 + ";database=" + s4;
            }
            catch { }

            t1_TextChanged(t1, e);
            timer1.Enabled = true;
        }

        private void b仅获取_Click(object sender, EventArgs e)
        {
            string str1 = "";
            string str2 = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            grid.DataSource = null;

            //b`.`MonitoringUpdateTime` > `b`.`SyncTime`

            //先获取对应表，每个周期都获取下
            tb公证处 = Program.获取一般数据集mysql("select * from pno"); tb公证处.PrimaryKey = new DataColumn[1] { tb公证处.Columns["PnoID"] };
            tb公证事项 = Program.获取一般数据集mysql("select * from matter"); tb公证事项.PrimaryKey = new DataColumn[1] { tb公证事项.Columns["MatterID"] };
            tb渠道 = Program.获取一般数据集mysql("select * from channel"); tb渠道.PrimaryKey = new DataColumn[1] { tb渠道.Columns["ChannelID"] };
            tb销售 = Program.获取一般数据集mysql("select * from renke_user"); tb销售.PrimaryKey = new DataColumn[1] { tb销售.Columns["renke_UserID"] };
            try
            {
                tb成本中心编号 = Program.获取一般数据集mysql(Program.ys_scnn, "select 参数值 from Z参数 where 参数名='中证通订单成本中心编号'").Rows[0][0].ToString();
            }
            catch { }
            获取激励比例();

            str1 = "select '' as 导入失败原因,OrderType as 业务类型,OrderChildType as 子订单类型,renke_OrdersID as 订单ID,renke_OrderChildID as 子订单ID,OrdersNums as 订单号,OrderChildNums as 子订单号,";
            str1 = str1 + "MatterID as 公证事项,PnoID as 承办公证处,PnoID as 承办公证处ID,matterPeoper as 当事人,matterPeoperCard as 当事人证件号,CreateTime as 订单日期,";
            str1 = str1 + "CountryArea_Name as 用地,Aim as 用途,Language_Name as 语种,IsUrgent as 是否加急,renke_UserID as 销售,NotaryFees as 公证费,";
            str1 = str1 + "NotaryFees_Urgent as 加急费,NotaryTranslateFees_old as 证词翻译费,NotaryTranslateFees as 证词翻译费_支出,";
            str1 = str1 + "NotaryTranslateFees_power as 证词翻译费_支出方,NotaryTranslateFees_power as 证词翻译费_支出方ID,AffixTranslateFees_old as 附件翻译费,AffixTranslateFees as 附件翻译费_支出,";
            str1 = str1 + "AffixTranslateFees_power as 附件翻译费_支出方,AffixTranslateFees_power as 附件翻译费_支出方ID,OfficialAuthFees_0 as 官方认证费,OfficialAuthFees_1 as 外办认证费,";
            str1=str1+"ServiceAuthFees as 服务认证费,CopyCost as 副本费,ServiceFees as 渠道服务费,ServiceFeesRenke as 人科服务费,ResearchFees as 调查费,";
            str1 = str1 + "TranslateServerFees as 翻译服务费,DiscountFees as 优惠金额,DiscountFees as 优惠金额_支出,SetFees as 退回费用,SetFees as 退回费用_支出,TotalFees as 实际收入,AuthCostFees as 认证费成本,";
            str1 = str1 + "ChannelID as 渠道,ChannelID as 渠道ID,FinanceState as 线上线下,SingleAuthCounts as 单认证数量,DoubleAuthCounts as 双认证数量,Counts as 副本数量, CONVERT(IsInland,SIGNED) as IsInland, CONVERT(State,SIGNED) as State ,CONVERT(s_Enable,SIGNED) as s_Enable ,CONVERT(f_Enable,SIGNED) as f_Enable ,CONVERT(IsAuth,SIGNED) as IsAuth ";
            str1 = str1 + " from V预算获取新 limit 0,2000";   //order by CreateTime    order by加上会死掉   limit太多也会死
            DataTable tb = Program.获取一般数据集mysql(str1);
            //grid.AutoGenerateColumns = true;
            grid.DataSource = tb;

            if (tb == null) b仅获取.Text = "仅显示要导入数据(出错)";

            //数据外部表处理
            for (i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i].Cells["序号"].Value = i + 1;

                str1 = "";
                try { str1 = grid.Rows[i].Cells["承办公证处"].Value.ToString(); }
                catch { }
                if (str1 != "")
                {
                    str2 = "";
                    str2 = 获取公证处名称(str1);
                    if (str2 != "")
                    {
                        grid.Rows[i].Cells["承办公证处"].Value = str2;
                    }
                }
                str1 = "";
                try { str1 = grid.Rows[i].Cells["公证事项"].Value.ToString(); }
                catch { }
                if (str1 != "")
                {
                    str2 = "";
                    str2 = 获取公证事项(str1);
                    if (str2 != "")
                    {
                        grid.Rows[i].Cells["公证事项"].Value = str2;
                    }
                }
                str1 = "";
                try { str1 = grid.Rows[i].Cells["渠道"].Value.ToString(); }
                catch { }
                if (str1 != "")
                {
                    str2 = "";
                    str2 = 获取渠道名称(str1);
                    if (str2 != "")
                    {
                        grid.Rows[i].Cells["渠道"].Value = str2;
                    }
                }
                str1 = "";
                try { str1 = grid.Rows[i].Cells["销售"].Value.ToString(); }
                catch { }
                if (str1 != "")
                {
                    str2 = "";
                    str2 = 获取销售名称(str1);
                    if (str2 != "")
                    {
                        grid.Rows[i].Cells["销售"].Value = str2;
                    }
                }

                str1 = "";
                try { str1 = grid.Rows[i].Cells["证词翻译费_支出方"].Value.ToString(); }
                catch { }
                if (str1 != "")
                {
                    str2 = "";
                    str2 = 获取公证处名称(str1);
                    if (str2 != "")
                    {
                        grid.Rows[i].Cells["证词翻译费_支出方"].Value = str2;
                    }
                }
                str1 = "";
                try { str1 = grid.Rows[i].Cells["附件翻译费_支出方"].Value.ToString(); }
                catch { }
                if (str1 != "")
                {
                    str2 = "";
                    str2 = 获取公证处名称(str1);
                    if (str2 != "")
                    {
                        grid.Rows[i].Cells["附件翻译费_支出方"].Value = str2;
                    }
                }

                //try
                //{
                //    grid.Rows[i].Cells["IsInland"].Value = CommonFunctions.ValInt(grid.Rows[i].Cells["IsInland"].Value.ToString()).ToString();
                //}
                //catch { }
                //try
                //{
                //    grid.Rows[i].Cells["State"].Value = CommonFunctions.ValInt(grid.Rows[i].Cells["State"].Value.ToString()).ToString();
                //}
                //catch { }
                //try
                //{ 
                //    grid.Rows[i].Cells["s_Enable"].Value = CommonFunctions.ValInt(grid.Rows[i].Cells["s_Enable"].Value.ToString()).ToString(); 
                //}
                //catch { }
                //try
                //{ 
                //    grid.Rows[i].Cells["f_Enable"].Value = CommonFunctions.ValInt(grid.Rows[i].Cells["f_Enable"].Value.ToString()).ToString();
                //}
                //catch { }
                //try
                //{ 
                //    grid.Rows[i].Cells["IsAuth"].Value = CommonFunctions.ValInt(grid.Rows[i].Cells["IsAuth"].Value.ToString()).ToString(); 
                //}
                //catch { }
            }

            Cursor.Current = Cursors.Default;
        }
        private void b立即获取_Click(object sender, EventArgs e)
        {
            string str1 = "";
            string sErr = "";
            int i = 0;

            b仅获取_Click(b仅获取, e);

            b立即获取.Enabled = false;
            b停止.Enabled = true;
            stop = false;
            timer1.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            //导入数据
            MySqlConnection cnn = new MySqlConnection(Program.ys_scnn);
            cnn.Open();

            for (i = 0; i < grid.Rows.Count; i++)
            {
                //if (i > 0) break;

                Application.DoEvents();
                if (stop)
                {
                    try { cnn.Close(); }
                    catch { }
                    b立即获取.Enabled = true;
                    b停止.Enabled = false;
                    stop = false;
                    timer1.Enabled = true;
                    Cursor.Current = Cursors.WaitCursor;
                    return;
                }

                string 证书ID = Guid.NewGuid().ToString();
                string s订单号 = "";
                string s订单ID = "";
                string s子订单号 = "";
                string s子订单ID = "";
                string s当事人 = "";
                string s当事人证件号 = "";
                DateTime s订单日期 = DateTime.Parse("1900-01-01");
                string s公证事项 = "";
                string s用地 = "";
                string s用途 = "";
                string s语种 = "";
                string s承办公证处 = "";
                string s是否加急 = "";
                string s销售 = "";
                decimal s公证费 = 0.00m;
                decimal s加急费 = 0.00m;
                decimal s证词翻译费 = 0.00m;
                decimal s证词翻译费_支出 = 0.00m;
                decimal s附件翻译费 = 0.00m;
                decimal s附件翻译费_支出 = 0.00m;
                decimal s官方认证费 = 0.00m;
                decimal s外办认证费 = 0.00m;
                decimal s服务认证费 = 0.00m;
                decimal s副本费 = 0.00m;
                decimal s渠道服务费 = 0.00m;
                decimal s人科服务费 = 0.00m;
                decimal s调查费 = 0.00m;
                decimal s翻译服务费 = 0.00m;
                decimal s优惠金额_支出 = 0.00m;
                decimal s退回费用_支出 = 0.00m;
                decimal s认证费成本 = 0.00m;
                decimal s实际收入 = 0.00m;
                string s渠道 = "";
                string s渠道ID = "";
                string s证词翻译费_支出方 = "";
                string s证词翻译费_支出方ID = "";
                string s附件翻译费_支出方 = "";
                string s附件翻译费_支出方ID = "";
                DateTime s导入日期 = DateTime.Now;
                string s备注 = "";
                string s业务类型 = "";
                string s线上线下 = "";
                string s成本中心编号 = "";
                string s子订单类型 = "";
                string s承办公证处ID = "";
                int s单认证数量 = 0;
                int s双认证数量 = 0;
                int s副本数量 = 0;

                try { 证书ID = grid.Rows[i].Cells["子订单ID"].Value.ToString(); }
                catch { }
                try { s订单号 = grid.Rows[i].Cells["订单号"].Value.ToString(); }
                catch { }
                try { s订单ID = grid.Rows[i].Cells["订单ID"].Value.ToString(); }
                catch { }
                try { s子订单号 = grid.Rows[i].Cells["子订单号"].Value.ToString(); }
                catch { }
                try { s子订单ID = grid.Rows[i].Cells["子订单ID"].Value.ToString(); }
                catch { }
                try { s当事人 = grid.Rows[i].Cells["当事人"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s当事人证件号 = grid.Rows[i].Cells["当事人证件号"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s订单日期 = DateTime.Parse(grid.Rows[i].Cells["订单日期"].Value.ToString()); }
                catch { }
                try { s公证事项 = grid.Rows[i].Cells["公证事项"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s用地 = grid.Rows[i].Cells["用地"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s用途 = grid.Rows[i].Cells["用途"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s语种 = grid.Rows[i].Cells["语种"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s承办公证处 = grid.Rows[i].Cells["承办公证处"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s是否加急 = grid.Rows[i].Cells["是否加急"].Value.ToString(); }
                catch { }
                if ((s是否加急 == "") | (s是否加急 == "0"))
                    s是否加急 = "否";
                else
                    s是否加急 = "是";
                try { s销售 = grid.Rows[i].Cells["销售"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s公证费 = CommonFunctions.ValDec(grid.Rows[i].Cells["公证费"].Value.ToString()); }
                catch { }
                try { s加急费 = CommonFunctions.ValDec(grid.Rows[i].Cells["加急费"].Value.ToString()); }
                catch { }
                try { s证词翻译费 = CommonFunctions.ValDec(grid.Rows[i].Cells["证词翻译费"].Value.ToString()); }
                catch { }
                try { s证词翻译费_支出 = CommonFunctions.ValDec(grid.Rows[i].Cells["证词翻译费_支出"].Value.ToString()); }
                catch { }
                try { s附件翻译费 = CommonFunctions.ValDec(grid.Rows[i].Cells["附件翻译费"].Value.ToString()); }
                catch { }
                try { s附件翻译费_支出 = CommonFunctions.ValDec(grid.Rows[i].Cells["附件翻译费_支出"].Value.ToString()); }
                catch { }
                try { s官方认证费 = CommonFunctions.ValDec(grid.Rows[i].Cells["官方认证费"].Value.ToString()); }
                catch { }
                try { s外办认证费 = CommonFunctions.ValDec(grid.Rows[i].Cells["外办认证费"].Value.ToString()); }
                catch { }
                try { s服务认证费 = CommonFunctions.ValDec(grid.Rows[i].Cells["服务认证费"].Value.ToString()); }
                catch { }
                try { s副本费 = CommonFunctions.ValDec(grid.Rows[i].Cells["副本费"].Value.ToString()); }
                catch { }
                try { s渠道服务费 = CommonFunctions.ValDec(grid.Rows[i].Cells["渠道服务费"].Value.ToString()); }
                catch { }
                try { s人科服务费 = CommonFunctions.ValDec(grid.Rows[i].Cells["人科服务费"].Value.ToString()); }
                catch { }
                try { s调查费 = CommonFunctions.ValDec(grid.Rows[i].Cells["调查费"].Value.ToString()); }
                catch { }
                try { s翻译服务费 = CommonFunctions.ValDec(grid.Rows[i].Cells["翻译服务费"].Value.ToString()); }
                catch { }
                try { s优惠金额_支出 = CommonFunctions.ValDec(grid.Rows[i].Cells["优惠金额_支出"].Value.ToString()); }
                catch { }
                try { s退回费用_支出 = CommonFunctions.ValDec(grid.Rows[i].Cells["退回费用_支出"].Value.ToString()); }
                catch { }
                try { s渠道 = grid.Rows[i].Cells["渠道"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s渠道ID = grid.Rows[i].Cells["渠道ID"].Value.ToString(); }
                catch { }
                try { s证词翻译费_支出方 = grid.Rows[i].Cells["证词翻译费_支出方"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s证词翻译费_支出方ID = grid.Rows[i].Cells["证词翻译费_支出方ID"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s附件翻译费_支出方 = grid.Rows[i].Cells["附件翻译费_支出方"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s附件翻译费_支出方ID = grid.Rows[i].Cells["附件翻译费_支出方ID"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s认证费成本 = CommonFunctions.ValDec(grid.Rows[i].Cells["认证费成本"].Value.ToString()); }
                catch { }
                try { s实际收入 = CommonFunctions.ValDec(grid.Rows[i].Cells["实际收入"].Value.ToString()); }
                catch { }
                try { s业务类型 = 获取业务类型(grid.Rows[i].Cells["业务类型"].Value.ToString()); }
                catch { }
                try { s线上线下 = 获取线上线下(grid.Rows[i].Cells["线上线下"].Value.ToString()); }
                catch { }
                try { s成本中心编号 = 获取成本中心编号(s订单日期.ToString("yyyy-MM-dd")); }
                catch { }
                try { s子订单类型 = grid.Rows[i].Cells["子订单类型"].Value.ToString().Trim(); }
                catch { }
                try { s承办公证处ID = grid.Rows[i].Cells["承办公证处ID"].Value.ToString().Trim(); }
                catch { }
                try { s单认证数量 = CommonFunctions.ValInt(grid.Rows[i].Cells["单认证数量"].Value.ToString()); }
                catch { }
                try { s双认证数量 = CommonFunctions.ValInt(grid.Rows[i].Cells["双认证数量"].Value.ToString()); }
                catch { }
                try { s副本数量 = CommonFunctions.ValInt(grid.Rows[i].Cells["副本数量"].Value.ToString()); }
                catch { }


                string s预算编号 = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();


                //先判断记录是否已经导入
                int iczjl = 0;
                string 原状态 = "";
                string 原财务状态 = "";
                try
                {
                    str1 = "";
                    DataTable tbold = Program.获取一般数据集mysql(Program.ys_scnn, "select count(*) as 数量,预算编号,预算状态,财务状态 from A预算 where ID='" + 证书ID + "'");
                    try { str1 = tbold.Rows[0]["预算编号"].ToString(); }
                    catch { }
                    try { iczjl = CommonFunctions.ValInt(tbold.Rows[0]["数量"].ToString()); }
                    catch { }
                    str1 = str1.Trim();
                    if (str1 != "")
                    {
                        s预算编号 = str1;   //使用原先的预算编号
                    }
                    try { 原状态 = tbold.Rows[0]["预算状态"].ToString(); }
                    catch { }
                    try { 原财务状态 = tbold.Rows[0]["财务状态"].ToString(); }
                    catch { }
                }
                catch { }
                if (iczjl > 0)
                {
                    //删除已经存在的预算系统中的、属于上次导入的数据
                    int zx = 0;
                    zx = Program.执行SQL命令mysql(Program.ys_scnn, "delete from A预算 where ID='" + 证书ID + "'");
                    grid.Rows[i].Cells["导入失败原因"].Value = "清除原有记录...";
                    if (zx > 0)
                    {
                        Program.执行SQL命令mysql(Program.ys_scnn, "delete from A预算收支 where 预算ID='" + 证书ID + "' and 录入人='导入程序'");
                        Program.执行SQL命令mysql(Program.ys_scnn, "delete from A实际收支 where 预算ID='" + 证书ID + "' and 录入人='导入程序'");
                        grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "成功。";
                    }
                    else
                    {
                        grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "失败。";
                    }
                }

                //预约记录不导入
                if ((grid.Rows[i].Cells["IsInland"].Value.ToString().Trim() != "-1") && grid.Rows[i].Cells["业务类型"].Value.ToString().Trim() == "3")
                {
                    grid.Rows[i].DefaultCellStyle.BackColor = lbl2.BackColor;
                    grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "预约订单不用导入";
                    int nd = 0;
                    str1 = "update renke_orderchild set SyncTime='" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "' where renke_OrderChildID='" + 证书ID + "'";
                    nd = Program.执行SQL命令mysql(str1);
                    continue;
                }
                ////关闭的订单不导入   改导入  
                //if (grid.Rows[i].Cells["State"].Value.ToString().Trim() == "-1")
                //{
                //    grid.Rows[i].DefaultCellStyle.BackColor = lbl2.BackColor;
                //    grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "关闭的订单不用导入";
                //    int nd = 0;
                //    str1 = "update renke_orderchild set SyncTime='" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "' where renke_OrderChildID='" + 证书ID + "'";
                //    nd = Program.执行SQL命令mysql(str1);
                //    continue;
                //}
                //未支付的订单不导入                
                if (grid.Rows[i].Cells["State"].Value.ToString().Trim() == "0")
                {
                    grid.Rows[i].DefaultCellStyle.BackColor = lbl2.BackColor;
                    grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "未支付的订单不用导入";
                    int nd = 0;
                    str1 = "update renke_orderchild set SyncTime='" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "' where renke_OrderChildID='" + 证书ID + "'";
                    nd = Program.执行SQL命令mysql(str1);
                    continue;
                }
                //订单不可用的不用导入
                if ((grid.Rows[i].Cells["s_Enable"].Value.ToString().Trim() != "0") | (grid.Rows[i].Cells["f_Enable"].Value.ToString().Trim() != "0"))
                {
                    grid.Rows[i].DefaultCellStyle.BackColor = lbl2.BackColor;
                    grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "订单不可用的不用导入";
                    int nd = 0;
                    str1 = "update renke_orderchild set SyncTime='" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "' where renke_OrderChildID='" + 证书ID + "'";
                    nd = Program.执行SQL命令mysql(str1);
                    continue;
                }


                if (原状态 == "") 原状态 = "待完成";

                str1 = "insert into A预算(ID,类型,父ID,预算编号,预算名称,成本中心编号,业务类型,销售,预算说明,申请人,申请日期,来源,是否中证通订单,子订单号,当事人,当事人证件号,订单日期,公证事项,用地,用途,语种,承办公证处,是否加急,渠道,线上线下,导入日期,审批结果,审批日期,核定结果,核定日期,预算状态,财务状态) values(";
                str1 = str1 + "'" + 证书ID + "',";
                str1 = str1 + "" + "1" + ",";
                str1 = str1 + "'" + "" + "',";
                str1 = str1 + "'" + s预算编号 + "',";
                str1 = str1 + "'" + s当事人 + "[" + s公证事项 + "]" + "',";
                str1 = str1 + "'" + s成本中心编号 + "',";
                str1 = str1 + "'" + s业务类型 + "',";
                str1 = str1 + "'" + s销售 + "',";
                str1 = str1 + "'" + s备注 + "',";
                str1 = str1 + "'" + s当事人 + "',";
                str1 = str1 + "'" + s订单日期 + "',";
                str1 = str1 + "'" + "中证通系统" + "',";
                str1 = str1 + "'" + "是" + "',";
                str1 = str1 + "'" + s子订单号 + "',";
                str1 = str1 + "'" + s当事人 + "',";
                str1 = str1 + "'" + s当事人证件号 + "',";
                str1 = str1 + "'" + s订单日期 + "',";
                str1 = str1 + "'" + s公证事项 + "',";
                str1 = str1 + "'" + s用地 + "',";
                str1 = str1 + "'" + s用途 + "',";
                str1 = str1 + "'" + s语种 + "',";
                str1 = str1 + "'" + s承办公证处 + "',";
                str1 = str1 + "'" + s是否加急 + "',";
                str1 = str1 + "'" + s渠道 + "',";
                str1 = str1 + "'" + s线上线下 + "',";
                str1 = str1 + "'" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                str1 = str1 + "'" + "同意" + "',";
                str1 = str1 + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                str1 = str1 + "'" + "同意" + "',";
                str1 = str1 + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                str1 = str1 + "'" + 原状态 + "',";
                str1 = str1 + "'" + 原财务状态 + "')";

                int m = 0;
                MySqlCommand cmd = new MySqlCommand(str1, cnn);
                sErr = "";
                try
                {
                    m = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    sErr = ex.Message;
                }
                if (m > 0)
                {
                    int n = 0;

                    #region 作废
                    ////预算收支
                    //n = insertyssz(cnn, 证书ID, "公证费", "收入", s当事人, s公证费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "公证费", "支出", s承办公证处, s公证费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "加急费", "收入", s当事人, s加急费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "证词翻译费", "收入", s当事人, s证词翻译费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "证词翻译费", "支出", s证词翻译费_支出方, s证词翻译费_支出, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "附件翻译费", "收入", s当事人, s附件翻译费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "附件翻译费", "支出", s附件翻译费_支出方, s附件翻译费_支出, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "官方认证费", "收入", s当事人, s官方认证费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "官方认证费", "支出", "领事馆", s官方认证费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "外办认证费", "收入", s当事人, s外办认证费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "外办认证费", "支出", "外事办", s外办认证费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "服务认证费", "收入", s当事人, s服务认证费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "副本费", "收入", s当事人, s副本费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "副本费", "支出", s承办公证处, s副本费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "渠道服务费", "收入", s当事人, s渠道服务费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "渠道服务费", "支出", s渠道, s渠道服务费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "人科服务费", "收入", s当事人, s人科服务费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "调查费", "收入", s当事人, s调查费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "调查费", "支出", "不确定", s调查费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "翻译服务费", "收入", s当事人, s翻译服务费, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "优惠金额", "支出", "中证通", s优惠金额_支出, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "退回费用", "支出", "中证通", s退回费用_支出, s导入日期, "");
                    //n = insertyssz(cnn, 证书ID, "认证费成本", "支出", "不确定", s认证费成本, s导入日期, "");


                    ////实际收入
                    //n = insertsjsz(cnn, 证书ID, "实际收入", "收入", "中证通", s实际收入, s导入日期, "");
                    ////实际支出
                    //n = insertsjsz(cnn, 证书ID, "公证费", "支出", s承办公证处, s公证费, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "证词翻译费", "支出", s证词翻译费_支出方, s证词翻译费_支出, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "附件翻译费", "支出", s附件翻译费_支出方, s附件翻译费_支出, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "官方认证费", "支出", "领事馆", s官方认证费, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "外办认证费", "支出", "外事办", s外办认证费, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "副本费", "支出", s承办公证处, s副本费, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "渠道服务费", "支出", s渠道, s渠道服务费, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "调查费", "支出", "不确定", s调查费, s导入日期, "");                    
                    ////n = insertsjsz(cnn, 证书ID, "优惠金额", "支出", s当事人, s优惠金额_支出, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "退回费用", "支出", s当事人, s退回费用_支出, s导入日期, "");
                    //n = insertsjsz(cnn, 证书ID, "认证费成本", "支出", "不确定", s认证费成本, s导入日期, "");

                    #endregion

                    //1、查询补款
                    DataTable tbbk = Program.获取一般数据集mysql(Program.zzt_scnn, "select FMState,FMFees,RefundFlag,SetFees from renke_fmorderchild where renke_OrderChildID='" + s子订单ID + "'");
                    if (tbbk != null)
                    {
                        for (int bk = 0; bk < tbbk.Rows.Count; bk++)
                        {
                            decimal decbk1 = 0.00m;
                            decimal decbk2 = 0.00m;
                            try { decbk1 = CommonFunctions.ValDec(tbbk.Rows[bk]["FMFees"].ToString()); }
                            catch { }
                            try { decbk2 = CommonFunctions.ValDec(tbbk.Rows[bk]["SetFees"].ToString()); }
                            catch { }
                            str1 = "";
                            try { str1 = tbbk.Rows[bk]["FMState"].ToString().Trim(); }
                            catch { }
                            if (str1 == "2")
                            {
                                //补款FMState=2时，FMFees金额作为预算收入、实际收入
                                n = insertyssz(cnn, 证书ID, "补款", "收入", s当事人, decbk1, s导入日期, "", s线上线下, s订单日期);
                                n = insertsjsz(cnn, 证书ID, "补款", "收入", s当事人, decbk1, s导入日期, "", s线上线下, s订单日期);
                            }
                            str1 = "";
                            try { str1 = tbbk.Rows[bk]["RefundFlag"].ToString().Trim(); }
                            catch { }
                            if (str1 == "2")
                            {
                                //补款RefundFlag=2时，SetFees金额作为预算支出、实际支出
                                n = insertyssz(cnn, 证书ID, "补款退款", "支出", s当事人, decbk2, s导入日期, "", s线上线下, s订单日期);
                                n = insertsjsz(cnn, 证书ID, "补款退款", "支出", s当事人, decbk2, s导入日期, "", s线上线下, s订单日期);
                            }
                        }
                    }

                    //2、总金额：不用考虑订单是单纯的翻译、认证什么的，肯定要计算该金额。 线上作为预算收入、实际收入；线下的作为预算收入。
                    if (s线上线下 == "线上")
                    {
                        n = insertyssz(cnn, 证书ID, "总收入", "收入", s当事人, s实际收入, s导入日期, "", s线上线下, s订单日期);
                        n = insertsjsz(cnn, 证书ID, "总收入", "收入", s当事人, s实际收入, s导入日期, "", s线上线下, s订单日期);
                    }
                    if (s线上线下 == "线下")
                    {
                        n = insertyssz(cnn, 证书ID, "总收入", "收入", s当事人, s实际收入, s导入日期, "", s线上线下, s订单日期);
                    }

                    //已经关闭的订单
                    if (grid.Rows[i].Cells["State"].Value.ToString().Trim() == "-1")
                    {
                        //只计算退回费用
                        n = insertyssz(cnn, 证书ID, "退回费用", "支出", s当事人, s退回费用_支出, s导入日期, "", s线上线下, s订单日期);
                        n = insertsjsz(cnn, 证书ID, "退回费用", "支出", s当事人, s退回费用_支出, s导入日期, "", s线上线下, s订单日期);
                        goto DDD;
                    }

                    //子订单类型
                    //0：公证、翻译、认证：认证肯定为单认证。费用全部考虑，但是不考虑副本费
                    //1：公证、翻译：不考虑认证费，其他全要
                    //2：翻译：只考虑证书翻译费、附件翻译费
                    //3：认证：只考虑认证费。
                    //           单认证：SingleAuthCounts>0为单认证，不用考虑外办认证费
                    //           双认证：DoubleAuthCounts>0为双认证，只考虑外办认证费

                    //3、公证费
                    if ((s子订单类型 == "0") | (s子订单类型 == "1"))
                    {
                        n = insertyssz(cnn, 证书ID, "公证费", "支出", s承办公证处, s公证费, s导入日期, "", s线上线下, s订单日期);
                    }

                                       
                    //4、证词翻译费、附件翻译费
                    //MessageBox.Show("|"+grid.Rows[i].Cells["证词翻译费_支出"].Value.ToString()+"|");
                    //MessageBox.Show("子订单号：" + s子订单类型 + "  s证词翻译费_支出:" + s证词翻译费_支出.ToString() + " s附件翻译费_支出:" + s附件翻译费_支出.ToString() + " s证词翻译费_支出方ID:" + s证词翻译费_支出方ID + "   s附件翻译费_支出方ID:" + s附件翻译费_支出方ID);
                    if ((s子订单类型 == "0") | (s子订单类型 == "1") | (s子订单类型 == "2"))
                    {
                        #region
                        ////先查找对应的公证处是否做翻译
                        //DataTable tbgzc = Program.获取一般数据集mysql(Program.zzt_scnn, "select En_Affix,NoEn_Affix,En_Evidence,NoEn_Evidence from pno where PnoID='" + s承办公证处ID + "'");
                        //bool b做英文证词翻译 = false; 
                        //bool b做非英文证词翻译 = false; 
                        //bool b做英文附件翻译 = false; 
                        //bool b做非英文附件翻译 = false;
                        //if (tbgzc != null)
                        //{
                        //    if (tbgzc.Rows.Count > 0)
                        //    {
                        //        if (tbgzc.Rows[0]["En_Evidence"].ToString().Trim() == "0") b做英文证词翻译 = true;
                        //        if (tbgzc.Rows[0]["NoEn_Evidence"].ToString().Trim() == "0") b做非英文证词翻译 = true;
                        //        if (tbgzc.Rows[0]["En_Affix"].ToString().Trim() == "0") b做英文附件翻译 = true;
                        //        if (tbgzc.Rows[0]["NoEn_Affix"].ToString().Trim() == "0") b做非英文附件翻译 = true;
                        //    }
                        //}
                        //if ((s语种 == "英语") | (s语种 == "英文"))
                        //{
                        //    if (b做英文证词翻译)
                        //    {
                        //        n = insertyssz(cnn, 证书ID, "证词翻译费", "支出", s承办公证处, s证词翻译费, s导入日期, "");
                        //    }
                        //    else
                        //    {
                        //        if (s证词翻译费_支出方.IndexOf("上海人科") < 0)
                        //        {
                        //            n = insertyssz(cnn, 证书ID, "证词翻译费", "支出", s证词翻译费_支出方, s证词翻译费, s导入日期, "");
                        //        }
                        //    }
                        //    if (b做英文附件翻译)
                        //    {
                        //        n = insertyssz(cnn, 证书ID, "附件翻译费", "支出", s承办公证处, s附件翻译费, s导入日期, "");
                        //    }
                        //    else
                        //    {
                        //        if (s附件翻译费_支出方.IndexOf("上海人科") < 0)
                        //        {
                        //            n = insertyssz(cnn, 证书ID, "附件翻译费", "支出", s附件翻译费_支出方, s附件翻译费, s导入日期, "");
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (b做非英文证词翻译)
                        //    {
                        //        n = insertyssz(cnn, 证书ID, "证词翻译费", "支出", s承办公证处, s证词翻译费, s导入日期, "");
                        //    }
                        //    else
                        //    {
                        //        if (s证词翻译费_支出方.IndexOf("上海人科") < 0)
                        //        {
                        //            n = insertyssz(cnn, 证书ID, "证词翻译费", "支出", s证词翻译费_支出方, s证词翻译费, s导入日期, "");
                        //        }
                        //    }
                        //    if (b做非英文附件翻译)
                        //    {
                        //        n = insertyssz(cnn, 证书ID, "附件翻译费", "支出", s承办公证处, s附件翻译费, s导入日期, "");
                        //    }
                        //    else
                        //    {
                        //        if (s附件翻译费_支出方.IndexOf("上海人科") < 0)
                        //        {
                        //            n = insertyssz(cnn, 证书ID, "附件翻译费", "支出", s附件翻译费_支出方, s附件翻译费, s导入日期, "");
                        //        }
                        //    }
                        //}
                        #endregion

                        //先判断是否人科翻译
                        if (s证词翻译费_支出方ID != "10013")    //if (!s证词翻译费_支出方.Contains("上海人科"))
                        {
                            n = insertyssz(cnn, 证书ID, "证词翻译费", "支出", s证词翻译费_支出方, s证词翻译费_支出, s导入日期, "", s线上线下, s订单日期);
                        }
                        if (s附件翻译费_支出方ID != "10013")    //if (!s附件翻译费_支出方.Contains("上海人科"))
                        {
                            n = insertyssz(cnn, 证书ID, "附件翻译费", "支出", s附件翻译费_支出方, s附件翻译费_支出, s导入日期, "", s线上线下, s订单日期);
                        }
                    }

                    /*
                    //5、官方认证费
                    if (s子订单类型 == "0")
                    {
                        n = insertyssz(cnn, 证书ID, "官方认证费", "支出", "领事馆", s官方认证费, s导入日期, "");
                    }
                    if (s子订单类型 == "3")
                    {
                        if (s单认证数量 > 0)
                        {
                            n = insertyssz(cnn, 证书ID, "官方认证费", "支出", "领事馆", s官方认证费, s导入日期, "");
                        }
                    }

                    //6、外办认证费
                    if (s子订单类型 == "0")
                    {
                        //不考虑
                    }
                    if (s子订单类型 == "3")
                    {
                        if (s双认证数量 > 0)
                        {
                            n = insertyssz(cnn, 证书ID, "外办认证费", "支出", "外事办", s外办认证费, s导入日期, "");
                        }
                    }
                     * */
                    //认证费
                    if (grid.Rows[i].Cells["IsAuth"].Value.ToString().Trim() != "0")
                    {
                        n = insertyssz(cnn, 证书ID, "认证费", "支出", "认证方", s认证费成本, s导入日期, "", s线上线下, s订单日期);
                    }



                    //7、副本费
                    if (s副本数量 > 1)
                    {
                        //decimal dfb = 0.00m;
                        //dfb = s副本费 * (s副本数量 - 1);
                        //if (dfb < 0.00m) dfb = 0.00m;
                        n = insertyssz(cnn, 证书ID, "副本费", "支出", s承办公证处, s副本费, s导入日期, "", s线上线下, s订单日期);
                    }

                    //8、渠道服务费
                    //n = insertyssz(cnn, 证书ID, "渠道服务费", "支出", s渠道, s渠道服务费, s导入日期, "");   //s渠道服务费其实是服务费
                    if ((s线上线下 == "线上") && (s子订单类型 != "3"))   //线上且非纯认证
                    {
                        //查找表Channel中对应的Channelcost
                        decimal dChannelcost = 0.00m;
                        try
                        {
                            dChannelcost = CommonFunctions.ValDec(Program.获取一般数据集mysql(Program.zzt_scnn, "select ChannelCost from Channel where ChannelID='" + s渠道ID + "'").Rows[0][0].ToString());
                        }
                        catch { }
                        n = insertyssz(cnn, 证书ID, "渠道服务费", "支出", s渠道, dChannelcost, s导入日期, "", s线上线下, s订单日期);
                    }
                    if ((s线上线下 == "线下") || ((s线上线下 == "线上") && (s子订单类型 == "3")))   //线下 或 线上的纯认证
                    {
                        n = insertyssz(cnn, 证书ID, "渠道服务费", "支出", s渠道, s渠道服务费, s导入日期, "", s线上线下, s订单日期);
                    }


                    //9、退回费用
                    n = insertyssz(cnn, 证书ID, "退回费用", "支出", s当事人, s退回费用_支出, s导入日期, "", s线上线下, s订单日期);
                    n = insertsjsz(cnn, 证书ID, "退回费用", "支出", s当事人, s退回费用_支出, s导入日期, "", s线上线下, s订单日期);

                    //10、认证费成本
                    //n = insertyssz(cnn, 证书ID, "认证费成本", "支出", "领事馆", s认证费成本, s导入日期, "");   //前面已计算

                    //11、加急费、服务认证费、人科服务费、调查费、翻译服务费、优惠金额：不考虑

                    //*********返点
                    decimal fandian = 0.00m;
                    //携程
                    if ((grid.Rows[i].Cells["渠道ID"].Value.ToString().Trim() == "10002") | (grid.Rows[i].Cells["渠道ID"].Value.ToString().Trim() == "10078"))
                    {
                        if (s人科服务费 > 0.00m)
                        {
                            fandian = 30.00m;
                            n = insertyssz(cnn, 证书ID, "商务费", "支出", s渠道, fandian, s导入日期, "", s线上线下, s订单日期);
                        }
                    }
                    //天津市北方公证处
                    fandian = 0.00m;
                    if (s承办公证处.Contains("天津") && s承办公证处.Contains("北方"))
                    {
                        if (s渠道.ToUpper().Contains("NAM机"))
                        {
                            if (s附件翻译费_支出方ID == "10013")
                            {
                                fandian = s附件翻译费_支出 * 0.40m;
                                n = insertyssz(cnn, 证书ID, "商务费", "支出", "天津市北方公证处", fandian, s导入日期, "", s线上线下, s订单日期);
                            }
                        }
                    }
                    //上海市闵行公证处
                    fandian = 0.00m;
                    if (s承办公证处.Contains("上海") && s承办公证处.Contains("闵行"))
                    {
                        if (s子订单类型 == "2")   //纯翻译的
                        {
                            if (s证词翻译费_支出方ID == "10013")
                            {
                                fandian = s证词翻译费_支出 * 0.40m;
                                n = insertyssz(cnn, 证书ID, "商务费", "支出", "上海市闵行公证处", fandian, s导入日期, "", s线上线下, s订单日期);
                            }
                            if (s附件翻译费_支出方ID == "10013")
                            {
                                fandian = s附件翻译费_支出 * 0.40m;
                                n = insertyssz(cnn, 证书ID, "商务费", "支出", "上海市闵行公证处", fandian, s导入日期, "", s线上线下, s订单日期);
                            }
                        }
                    }
                    ////上海市奉贤公证处
                    //fandian = 0.00m;
                    //if (s承办公证处.Contains("上海") && s承办公证处.Contains("奉贤"))
                    //{
                    //    if (s证词翻译费_支出方ID == "10013")
                    //    {
                    //        fandian = s证词翻译费_支出 * 0.20m;
                    //        n = insertyssz(cnn, 证书ID, "商务费", "支出", "上海市奉贤公证处", fandian, s导入日期, "", s线上线下, s订单日期);
                    //    }
                    //}
                    //*********返点



                    //激励的计算
                    //中证通预算_附件翻译激励比例
                    //中证通预算_附件校对激励比例
                    //中证通预算_证词翻译激励比例
                    //中证通预算_证词校对激励比例
                    //中证通预算_自助机翻译激励比例
                    //中证通预算_校对激励比例
                    //中证通预算_加急激励比例
                    /*
                    if (s附件翻译费_支出 < 0.01m)
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_附件翻译激励比例", "附件翻译人", s附件翻译费, s导入日期, s线上线下, s订单日期);
                        n = insertjili(cnn, 证书ID, "中证通预算_附件校对激励比例", "附件校对人", s附件翻译费, s导入日期, s线上线下, s订单日期);
                    }
                    if (s证词翻译费_支出 < 0.01m)
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_证词翻译激励比例", "证词翻译人", s证词翻译费, s导入日期, s线上线下, s订单日期);
                        n = insertjili(cnn, 证书ID, "中证通预算_证词校对激励比例", "证词校对人", s证词翻译费, s导入日期, s线上线下, s订单日期);
                    }
                    if (s渠道.ToUpper().Contains("NAM机"))
                    {
                        if (s附件翻译费_支出 < 0.01m)
                        {
                            n = insertjili(cnn, 证书ID, "中证通预算_自助机翻译激励比例", s销售, s附件翻译费, s导入日期, s线上线下, s订单日期);
                        }
                    }
                    if ((s证词翻译费_支出 + s附件翻译费_支出) > 0.00m)
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_校对激励比例", "外翻校对人", s证词翻译费_支出 + s附件翻译费_支出, s导入日期, s线上线下, s订单日期);
                    }
                    if (s是否加急 == "是")
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_加急激励比例", "加急办理人", s加急费, s导入日期, s线上线下, s订单日期);
                    }
                     * */


                DDD:

                    n = 0;
                    //str1 = "update renke_orders set IsSynchronous=1,SynchronousDate='" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "' where renke_OrdersID='" + s订单ID + "'";
                    str1 = "update renke_orderchild set SyncTime='" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "' where renke_OrderChildID='" + 证书ID + "'";
                    n = Program.执行SQL命令mysql(str1);

                    grid.Rows[i].DefaultCellStyle.BackColor = lbl1.BackColor;
                }
                else
                {
                    grid.Rows[i].DefaultCellStyle.BackColor = lbl2.BackColor;
                    grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + sErr;
                }
            }

            cnn.Close();

            b立即获取.Enabled = true;
            b停止.Enabled = false;
            stop = false;
            timer1.Enabled = true;
            Cursor.Current = Cursors.WaitCursor;
        }
        private int insertyssz(MySqlConnection cnn, string zsid, string lx, string shouzhi, string jdf, decimal je, DateTime daoruriqi, string bz, string 线上线下, DateTime 收支日期)
        {
            if (je != 0.00m)
            {
                string dat1 = DateTime.Parse("1900-01-01").ToString();
                if (线上线下 == "线上")
                {
                    try
                    {
                        dat1 = 收支日期.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch { }
                }

                string str1 = "";
                str1 = "insert into A预算收支(ID,预算ID,类型,收支,借贷方,金额,日期,备注,录入人,录入日期) values(";
                str1 = str1 + "'" + Guid.NewGuid().ToString() + "',";
                str1 = str1 + "'" + zsid.ToString() + "',";
                str1 = str1 + "'" + lx + "',";
                str1 = str1 + "'" + shouzhi + "',";
                str1 = str1 + "'" + jdf + "',";
                str1 = str1 + "'" + je.ToString("f2") + "',";
                str1 = str1 + "'" + dat1 + "',";
                str1 = str1 + "'" + bz + "',";
                str1 = str1 + "'" + "导入程序" + "',";
                str1 = str1 + "'" + daoruriqi.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(str1, cnn);
                    return cmd.ExecuteNonQuery();
                }
                catch { }
                return -1;
            }
            else
                return -1;
        }
        private int insertsjsz(MySqlConnection cnn, string zsid, string lx, string shouzhi, string jdf, decimal je, DateTime daoruriqi, string bz, string 线上线下, DateTime 收支日期)
        {
            if (je != 0.00m)
            {
                string dat1 = DateTime.Parse("1900-01-01").ToString();
                if ((线上线下 == "线上") && (shouzhi == "收入"))
                {
                    try
                    {
                        dat1 = 收支日期.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch { }
                }

                string str1 = "";
                str1 = "insert into A实际收支(ID,预算ID,类型,收支,借贷方,金额,日期,备注,录入人,录入日期) values(";
                str1 = str1 + "'" + Guid.NewGuid().ToString() + "',";
                str1 = str1 + "'" + zsid.ToString() + "',";
                str1 = str1 + "'" + lx + "',";
                str1 = str1 + "'" + shouzhi + "',";
                str1 = str1 + "'" + jdf + "',";
                str1 = str1 + "'" + je.ToString("f2") + "',";
                str1 = str1 + "'" + dat1 + "',";
                str1 = str1 + "'" + bz + "',";
                str1 = str1 + "'" + "导入程序" + "',";
                str1 = str1 + "'" + daoruriqi.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(str1, cnn);
                    return cmd.ExecuteNonQuery();
                }
                catch { }
                return -1;
            }
            else
                return -1;
        }
        private int insertjili(MySqlConnection cnn, string zsid, string lx, string jdf, decimal je, DateTime daoruriqi, string 线上线下, DateTime 收支日期)
        {
            int i = 0;
            if (je == 0.00m) return 0;
            if (lx == "") return 0;

            string dat1 = DateTime.Parse("1900-01-01").ToString();
            //if (线上线下 == "线上")
            //{
                try
                {
                    dat1 = 收支日期.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch { }
            //}

            for (i = 0; i < JiLi.Count; i++)
            {
                if (lx == JiLi[i].激励名称)
                {
                    decimal dec1 = 0.00m;
                    dec1 = je * JiLi[i].激励比例 / 100;
                    string bz = dec1.ToString("f2") + "=" + je.ToString("f2") + "*" + JiLi[i].激励比例.ToString() + "%";

                    if (dec1 != 0.00m)
                    {
                        string str1 = "";
                        str1 = "insert into A预算收支(ID,预算ID,类型,收支,借贷方,金额,日期,备注,录入人,录入日期) values(";
                        str1 = str1 + "'" + Guid.NewGuid().ToString() + "',";
                        str1 = str1 + "'" + zsid.ToString() + "',";
                        str1 = str1 + "'" + lx.Replace("中证通预算_", "").Replace("比例", "") + "',";
                        str1 = str1 + "'" + "支出" + "',";
                        str1 = str1 + "'" + jdf + "',";
                        str1 = str1 + "'" + dec1.ToString("f2") + "',";
                        str1 = str1 + "'" + dat1 + "',";
                        str1 = str1 + "'" + bz + "',";
                        str1 = str1 + "'" + "导入程序" + "',";
                        str1 = str1 + "'" + daoruriqi.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(str1, cnn);
                            return cmd.ExecuteNonQuery();
                        }
                        catch { }
                        return -1;
                    }
                    else
                        return -1;                    
                }
            }
            return 0;
        }

        
        private string 获取公证处名称(string 公证处ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb公证处 != null)
            {
                DataRow record;
                //tb公证处.Rows.Find()
                keys[0] = 公证处ID;   //ID
                record = tb公证处.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["PnoName"].ToString();
                }
            }
            return str1;
        }
        private string 获取公证事项(string 事项ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb公证事项 != null)
            {
                DataRow record;
                keys[0] = 事项ID;
                record = tb公证事项.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["MatterName_cn"].ToString();
                }
            }
            return str1;
        }
        private string 获取渠道名称(string 渠道ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb渠道 != null)
            {
                DataRow record;
                keys[0] = 渠道ID;
                record = tb渠道.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["ChannelFullName"].ToString();
                }
            }
            return str1;
        }
        private string 获取销售名称(string 销售ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb销售 != null)
            {
                DataRow record;
                keys[0] = 销售ID;
                record = tb销售.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["UserName"].ToString();
                }
            }
            return str1;
        }
        private void 获取激励比例()
        {
            try
            {
                DataTable tb = Program.获取一般数据集mysql(Program.ys_scnn, "select 参数名,参数值 from Z参数");
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    for (int k = 0; k < JiLi.Count; k++)
                    {
                        if (JiLi[k].激励名称 == tb.Rows[i]["参数名"].ToString())
                        {
                            JiLi[k].激励比例 = CommonFunctions.ValDec(tb.Rows[i]["参数值"].ToString());
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private string 获取成本中心编号(string 申请日期)
        {
            string str1 = "";
            if (tb成本中心编号 != "")
            {
                try
                {
                    str1 = CommonFunctions.ReadWordOnly(CommonFunctions.ReadWordOnly(tb成本中心编号, "[" + DateTime.Parse(申请日期).Year.ToString() + "]", false, true), @"/", true, false);
                }
                catch { }
            }
            return str1;
        }
        private string 获取业务类型(string ywlx)
        {
            //业务类型：公证录音 RDP（自助取证） MPS（电子印章） ANM（自助公证） 财务 中证通 中证通（微信）  中证通（12348天津法网） 其他
            switch(ywlx.Trim())
            {
                case "1":
                    return " ANM（自助公证）";
                case "2":
                    return " 中证通（微信）";
                case "3":
                    return " 中证通（12348天津法网）";
                default:
                    return "中证通";
            }
        }
        private string 获取线上线下(string lx)
        {
            switch (lx.Trim())
            {
                case "0":
                    return "线上";
                default:
                    return "线下";
            }
        }
        

        private void b停止_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            b立即获取_Click(b立即获取, e);
            timer1.Enabled = true;
        }

        private void t1_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            i = CommonFunctions.ValInt(t1.Text);
            if (i < 5) i = 5;
            if (i > 1440) i = 1440;
            timer1.Interval = i * 60 * 1000;

            //timer1.Interval = 10000;
        }

        private void b设为未导入_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要设置中证通的全部数据为未导入状态？如果这样设置，系统会尝试所有数据重新导入预算系统（预算系统已存在的数据将覆盖）。", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            
            string str1 = "";
            int i = 0;
            Cursor.Current = Cursors.WaitCursor;
            //str1 = "update renke_orders set IsSynchronous=0,SynchronousDate='1900-01-01'";
            str1 = "update renke_orderchild set SyncTime='1900-01-01'";
            i = Program.执行SQL命令mysql(str1);            
            Cursor.Current = Cursors.Default;
        }

        

        //private string 更新中证通数据库()
        //{
        //    //renke_orders的IsSynchronous从0设置为1，SynchronousDate设置日期

        //}
    }
}
