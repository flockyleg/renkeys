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
            获取激励比例();

            str1 = "select '' as 导入失败原因,renke_OrdersID as 订单ID,renke_OrderChildID as 子订单ID,OrdersNums as 订单号,OrderChildNums as 子订单号,";
            str1=str1+"MatterID as 公证事项,PnoID as 承办公证处,matterPeoper as 当事人,matterPeoperCard as 当事人证件号,CreateTime as 订单日期,";
            str1 = str1 + "CountryArea_Name as 用地,Language_Name as 语种,IsUrgent as 是否加急,renke_UserID as 销售,NotaryFees as 公证费,";
            str1 = str1 + "NotaryFees_Urgent as 加急费,NotaryTranslateFees_old as 证词翻译费,NotaryTranslateFees as 证词翻译费_支出,";
            str1 = str1 + "NotaryTranslateFees_power as 证词翻译费_支出方,AffixTranslateFees_old as 附件翻译费,AffixTranslateFees as 附件翻译费_支出,";
            str1 = str1 + "AffixTranslateFees_power as 附件翻译费_支出方,OfficialAuthFees_0 as 官方认证费,OfficialAuthFees_1 as 外办认证费,"
            str1=str1+"ServiceAuthFees as 服务认证费,CopyCost as 副本费,ServiceFees as 渠道服务费,ServiceFeesRenke as 人科服务费,ResearchFees as 调查费,";
            str1=str1+"TranslateServerFees as 翻译服务费,DiscountFees as 优惠金额,SetFees as 退回费用,TotalFees as 实际收入,AuthCostFees as 认证费成本,";
            str1=str1+"ChannelID as 渠道,OrderType as 业务类型,FinanceState as 线上线下 ";
            str1 = str1 + " from V预算获取新 limit 0,1000";   //order by CreateTime    order by加上会死掉   limit太多也会死
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
                Application.DoEvents();
                if(stop)
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
                string s证词翻译费_支出方 = "";
                string s附件翻译费_支出方 = "";
                DateTime s导入日期 = DateTime.Now;
                string s备注 = "";

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
                //try { s用途 = grid.Rows[i].Cells["用途"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                //catch { }
                try { s语种 = grid.Rows[i].Cells["语种"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s承办公证处 = grid.Rows[i].Cells["承办公证处"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s是否加急 = grid.Rows[i].Cells["是否加急"].Value.ToString(); }
                catch { }
                if ((s是否加急 == "") | (s是否加急 == "0"))
                    s是否加急 = "";
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
                try { s证词翻译费_支出方 = grid.Rows[i].Cells["证词翻译费_支出方"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s附件翻译费_支出方 = grid.Rows[i].Cells["附件翻译费_支出方"].Value.ToString().Replace("'", "").Replace("\"", ""); }
                catch { }
                try { s认证费成本 = CommonFunctions.ValDec(grid.Rows[i].Cells["认证费成本"].Value.ToString()); }
                catch { }
                try { s实际收入 = CommonFunctions.ValDec(grid.Rows[i].Cells["实际收入"].Value.ToString()); }
                catch { }

                //先判断记录是否已经导入
                int iczjl = 0;
                try { iczjl = CommonFunctions.ValInt(Program.获取一般数据集mysql(Program.ys_scnn, "select count(*) from A中证通证书 where ID='" + 证书ID + "'").Rows[0][0].ToString()); }
                catch { }
                if (iczjl > 0)
                {
                    //删除已经存在的预算系统中的、属于上次导入的数据
                    int zx = 0;
                    zx = Program.执行SQL命令mysql(Program.ys_scnn, "delete from A中证通证书 where ID='" + 证书ID + "'");
                    grid.Rows[i].Cells["导入失败原因"].Value = "清除原有记录...";
                    if (zx > 0)
                    {
                        Program.执行SQL命令mysql(Program.ys_scnn, "delete from A中证通证书预算收支 where 证书ID='" + 证书ID + "' and 录入人='导入程序'");
                        Program.执行SQL命令mysql(Program.ys_scnn, "delete from A中证通证书实际收支 where 证书ID='" + 证书ID + "' and 录入人='导入程序'");
                        grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "成功。";
                    }
                    else
                    {
                        grid.Rows[i].Cells["导入失败原因"].Value = grid.Rows[i].Cells["导入失败原因"].Value.ToString() + "失败。";
                    }
                }

                //业务类型：公证录音 RDP（自助取证） MPS（电子印章） ANM（自助公证） 财务 中证通 其他

                str1 = "insert into A中证通证书(ID,订单号,订单ID,子订单号,子订单ID,当事人,当事人证件号,订单日期,公证事项,用地,用途,语种,承办公证处,是否加急,销售,渠道,导入日期,备注) values(";
                str1 = str1 + "'" + 证书ID + "',";
                str1 = str1 + "'" + s订单号 + "',";
                str1 = str1 + "'" + s订单ID + "',";
                str1 = str1 + "'" + s子订单号 + "',";
                str1 = str1 + "'" + s子订单ID + "',";
                str1 = str1 + "'" + s当事人 + "',";
                str1 = str1 + "'" + s当事人证件号 + "',";
                str1 = str1 + "'" + s订单日期 + "',";
                str1 = str1 + "'" + s公证事项 + "',";
                str1 = str1 + "'" + s用地 + "',";
                str1 = str1 + "'" + s用途 + "',";
                str1 = str1 + "'" + s语种 + "',";
                str1 = str1 + "'" + s承办公证处 + "',";
                str1 = str1 + "'" + s是否加急 + "',";
                str1 = str1 + "'" + s销售 + "',";
                str1 = str1 + "'" + s渠道 + "',";
                str1 = str1 + "'" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "',";
                str1 = str1 + "'" + s备注 + "')";

                int m = 0;
                MySqlCommand cmd = new MySqlCommand(str1, cnn);
                sErr = "";
                try
                {
                    m = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    sErr = ex.Message;
                }
                if (m > 0)
                {
                    int n = 0;

                    //预算收支
                    n = insertyssz(cnn, 证书ID, "公证费", "收入", s当事人, s公证费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "公证费", "支出", s承办公证处, s公证费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "加急费", "收入", s当事人, s加急费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "证词翻译费", "收入", s当事人, s证词翻译费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "证词翻译费", "支出", s证词翻译费_支出方, s证词翻译费_支出, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "附件翻译费", "收入", s当事人, s附件翻译费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "附件翻译费", "支出", s附件翻译费_支出方, s附件翻译费_支出, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "官方认证费", "收入", s当事人, s官方认证费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "官方认证费", "支出", "领事馆", s官方认证费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "外办认证费", "收入", s当事人, s外办认证费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "外办认证费", "支出", "外事办", s外办认证费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "服务认证费", "收入", s当事人, s服务认证费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "副本费", "收入", s当事人, s副本费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "副本费", "支出", s承办公证处, s副本费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "渠道服务费", "收入", s当事人, s渠道服务费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "渠道服务费", "支出", s渠道, s渠道服务费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "人科服务费", "收入", s当事人, s人科服务费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "调查费", "收入", s当事人, s调查费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "调查费", "支出", "不确定", s调查费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "翻译服务费", "收入", s当事人, s翻译服务费, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "优惠金额", "支出", "中证通", s优惠金额_支出, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "退回费用", "支出", "中证通", s退回费用_支出, s导入日期, "");
                    n = insertyssz(cnn, 证书ID, "认证费成本", "支出", "不确定", s认证费成本, s导入日期, "");


                    //实际收入
                    n = insertsjsz(cnn, 证书ID, "实际收入", "收入", "中证通", s实际收入, s导入日期, "");
                    //实际支出
                    n = insertsjsz(cnn, 证书ID, "公证费", "支出", s承办公证处, s公证费, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "证词翻译费", "支出", s证词翻译费_支出方, s证词翻译费_支出, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "附件翻译费", "支出", s附件翻译费_支出方, s附件翻译费_支出, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "官方认证费", "支出", "领事馆", s官方认证费, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "外办认证费", "支出", "外事办", s外办认证费, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "副本费", "支出", s承办公证处, s副本费, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "渠道服务费", "支出", s渠道, s渠道服务费, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "调查费", "支出", "不确定", s调查费, s导入日期, "");                    
                    //n = insertsjsz(cnn, 证书ID, "优惠金额", "支出", s当事人, s优惠金额_支出, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "退回费用", "支出", s当事人, s退回费用_支出, s导入日期, "");
                    n = insertsjsz(cnn, 证书ID, "认证费成本", "支出", "不确定", s认证费成本, s导入日期, "");

                    //激励的计算
                    //中证通预算_附件翻译激励比例
                    //中证通预算_附件校对激励比例
                    //中证通预算_证词翻译激励比例
                    //中证通预算_证词校对激励比例
                    //中证通预算_自助机翻译激励比例
                    //中证通预算_校对激励比例
                    //中证通预算_加急激励比例
                    if (s附件翻译费_支出 < 0.01m)
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_附件翻译激励比例", "附件翻译人", s附件翻译费, s导入日期);
                        n = insertjili(cnn, 证书ID, "中证通预算_附件校对激励比例", "附件校对人", s附件翻译费, s导入日期);
                    }
                    if (s证词翻译费_支出 < 0.01m)
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_证词翻译激励比例", "证词翻译人", s证词翻译费, s导入日期);
                        n = insertjili(cnn, 证书ID, "中证通预算_证词校对激励比例", "证词校对人", s证词翻译费, s导入日期);
                    }
                    if (s渠道.ToUpper().IndexOf("NAM机") > -1)
                    {
                        if (s附件翻译费_支出 < 0.01m)
                        {
                            n = insertjili(cnn, 证书ID, "中证通预算_自助机翻译激励比例", s销售, s附件翻译费, s导入日期);
                        }
                    }
                    if ((s证词翻译费_支出 + s附件翻译费_支出) > 0.00m)
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_校对激励比例", "外翻校对人", s证词翻译费_支出 + s附件翻译费_支出, s导入日期);
                    }
                    if (s是否加急 == "是")
                    {
                        n = insertjili(cnn, 证书ID, "中证通预算_加急激励比例", "加急办理人", s加急费, s导入日期);
                    }


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
                    ////考虑到一个订单多个子订单的情况，如果其中一个子订单导入错误，应该把整个订单设置为未导入状态，以便下次再导
                    //if (sErr.IndexOf("插入重复键") < 0)
                    //{
                    //    int n = 0;
                    //    str1 = "update renke_orders set IsSynchronous=0,SynchronousDate='1900-01-01' where renke_OrdersID='" + s订单ID + "'";
                    //    n = Program.执行SQL命令mysql(str1);
                    //}
                    //else
                    //{
                    //    int n1 = CommonFunctions.ValInt(Program.获取一般数据集mysql(Program.ys_scnn,"select count(*) from A中证通证书 where 订单ID='" + s订单ID + "'").Rows[0][0].ToString());
                    //    int n2 = CommonFunctions.ValInt(Program.获取一般数据集mysql("select count(*) from V预算获取新 where renke_OrdersID='" + s订单ID + "'").Rows[0][0].ToString());
                    //    if ((n1 > 0) && (n1 == n2))     ///如果全部子订单已经在预算系统里
                    //    {
                    //        int n = 0;
                    //        str1 = "update renke_orders set IsSynchronous=1,SynchronousDate='" + s导入日期.ToString("yyyy-MM-dd HH:mm:ss") + "' where renke_OrdersID='" + s订单ID + "'";
                    //        n = Program.执行SQL命令mysql(str1);
                    //    }
                    //}
                }
            }

            cnn.Close();

            b立即获取.Enabled = true;
            b停止.Enabled = false;
            stop = false;
            timer1.Enabled = true;
            Cursor.Current = Cursors.WaitCursor;
        }
        private int insertyssz(MySqlConnection cnn, string zsid, string lx, string shouzhi, string jdf, decimal je, DateTime daoruriqi, string bz)
        {
            if (je != 0.00m)
            {
                string str1 = "";
                str1 = "insert into A中证通证书预算收支(ID,证书ID,类型,收支,借贷方,金额,日期,备注,录入人,录入日期) values(";
                str1 = str1 + "'" + Guid.NewGuid().ToString() + "',";
                str1 = str1 + "'" + zsid.ToString() + "',";
                str1 = str1 + "'" + lx + "',";
                str1 = str1 + "'" + shouzhi + "',";
                str1 = str1 + "'" + jdf + "',";
                str1 = str1 + "'" + je.ToString("f2") + "',";
                str1 = str1 + "'" + "1900-01-01" + "',";
                str1 = str1 + "'" + bz + "',";
                str1 = str1 + "'" + "导入程序" + "',";
                str1 = str1 + "'" + daoruriqi.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                MySqlCommand cmd = new MySqlCommand(str1, cnn);
                return cmd.ExecuteNonQuery();
            }
            else
                return -1;
        }
        private int insertsjsz(MySqlConnection cnn, string zsid, string lx, string shouzhi, string jdf, decimal je, DateTime daoruriqi, string bz)
        {
            if (je != 0.00m)
            {
                string str1 = "";
                str1 = "insert into A中证通证书实际收支(ID,证书ID,类型,收支,借贷方,金额,日期,备注,录入人,录入日期) values(";
                str1 = str1 + "'" + Guid.NewGuid().ToString() + "',";
                str1 = str1 + "'" + zsid.ToString() + "',";
                str1 = str1 + "'" + lx + "',";
                str1 = str1 + "'" + shouzhi + "',";
                str1 = str1 + "'" + jdf + "',";
                str1 = str1 + "'" + je.ToString("f2") + "',";
                str1 = str1 + "'" + "1900-01-01" + "',";
                str1 = str1 + "'" + bz + "',";
                str1 = str1 + "'" + "导入程序" + "',";
                str1 = str1 + "'" + daoruriqi.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                MySqlCommand cmd = new MySqlCommand(str1, cnn);
                return cmd.ExecuteNonQuery();
            }
            else
                return -1;
        }
        private int insertjili(MySqlConnection cnn, string zsid, string lx, string jdf, decimal je, DateTime daoruriqi)
        {
            int i = 0;
            if (je == 0.00m) return 0;
            if (lx == "") return 0;
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
                        str1 = "insert into A中证通证书预算收支(ID,证书ID,类型,收支,借贷方,金额,日期,备注,录入人,录入日期) values(";
                        str1 = str1 + "'" + Guid.NewGuid().ToString() + "',";
                        str1 = str1 + "'" + zsid.ToString() + "',";
                        str1 = str1 + "'" + lx.Replace("中证通预算_", "").Replace("比例", "") + "',";
                        str1 = str1 + "'" + "支出" + "',";
                        str1 = str1 + "'" + jdf + "',";
                        str1 = str1 + "'" + dec1.ToString("f2") + "',";
                        str1 = str1 + "'" + "1900-01-01" + "',";
                        str1 = str1 + "'" + bz + "',";
                        str1 = str1 + "'" + "导入程序" + "',";
                        str1 = str1 + "'" + daoruriqi.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        MySqlCommand cmd = new MySqlCommand(str1, cnn);
                        return cmd.ExecuteNonQuery();
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
