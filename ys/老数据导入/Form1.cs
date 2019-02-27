using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using CBClassLibraryA;

namespace 老数据导入
{
    public partial class Form1 : Form
    {
        private DataTable tb公证处 = null;
        private DataTable tb人科用户 = null;
        private DataTable tb成本中心 = null;
        private DataTable tb业务类型 = null;


        public Form1()
        {
            InitializeComponent();

            grid.AutoGenerateColumns = false;
            
        }

        private void 获取数据(int ttt)
        {
            string str1 = "";
            grid.DataSource = null;

            //先获取对应表，每个周期都获取下
            tb公证处 = Program.获取一般数据集mysql(Program.old_scnn, "select PnoID,PnoName from pno"); tb公证处.PrimaryKey = new DataColumn[1] { tb公证处.Columns["PnoID"] };
            tb人科用户 = Program.获取一般数据集mysql(Program.old_scnn, "select renke_UserID,UserName from renke_user"); tb人科用户.PrimaryKey = new DataColumn[1] { tb人科用户.Columns["renke_UserID"] };
            tb成本中心 = Program.获取一般数据集mysql(Program.old_scnn, "select renke_CategoryID,ParentID,CategoryName from renke_category"); tb成本中心.PrimaryKey = new DataColumn[1] { tb成本中心.Columns["renke_CategoryID"] };    // where CategoryType=1
            tb业务类型 = Program.获取一般数据集mysql(Program.old_scnn, "select renke_CategoryID,ParentID,CategoryName from renke_category"); tb业务类型.PrimaryKey = new DataColumn[1] { tb业务类型.Columns["renke_CategoryID"] };    // where CategoryType=0
            

            str1 = "select 0 as 序号,";
            str1 = str1 + "renke_ItemID as ID,";
            str1 = str1 + "renke_UserID as 申请人,";
            str1 = str1 + "SellName as 销售,";
            str1 = str1 + "ItemNums as 预算编号,";
            str1 = str1 + "ItemName as 预算名称,";
            str1 = str1 + "ItemRemark as 预算说明,";
            str1 = str1 + "convert(BudgetType,char(5)) as 类型,";   //0=项目预算，1=固定预算,-10=历史数据,
            str1 = str1 + "renke_CategoryID_1 as 成本中心编号,";
            str1 = str1 + "renke_CategoryID_0 as 业务类型,";
            str1 = str1 + "ItemType as 是否中证通,";
            str1 = str1 + "OrderChildNums as 子订单号,";
            str1 = str1 + "convert(ItemForm,char(5)) as 线下线上,";     //0=未设置，1=线下，2=线上
            str1 = str1 + "convert(ItemCheck,char(5)) as 审批结果,";     //0=未审核，1=审核通过，-1=审核不通过
            str1 = str1 + "convert(Enable,char(5)) as 可用否,";     //0=可用，1=不可用
            str1 = str1 + "convert(UpdateTime,char(20)) as 更新时间,";
            str1 = str1 + "convert(ItemState,char(5)) as 预算状态,";   //0=未完成，1=已完成
            str1 = str1 + "convert(CreateTime,char(20)) as 申请日期,";
            str1 = str1 + "PnoID as 公证处,";
            str1 = str1 + "ItemNumsOld as 激励预算编号,";    //被激励的预算编号
            str1 = str1 + "ItemPercent as 激励预算值百分比,";
            str1 = str1 + "ItemBonus as 激励值,";
            str1 = str1 + "remark as 备用";
            str1 = str1 + " from renke_item ";
            str1 = str1 + " where ItemType=" + ttt + " order by CreateTime";      //0=其他项目，1=中证通


              //`ID` varchar(40) NOT NULL DEFAULT '',
              //`类型` int(11) NOT NULL DEFAULT '0',
              //`父ID` varchar(40) NOT NULL DEFAULT '',
              //`预算编号` varchar(20) NOT NULL DEFAULT '',
              //`预算名称` varchar(100) NOT NULL DEFAULT '',
              //`成本中心编号` varchar(50) NOT NULL DEFAULT '',
              //`业务类型` varchar(50) NOT NULL DEFAULT '',
              //`销售` varchar(50) NOT NULL DEFAULT '',
              //`预算说明` varchar(1000) NOT NULL DEFAULT '',
              //`申请人` varchar(50) NOT NULL DEFAULT '',
              //`申请日期` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
              //`审批人` varchar(50) NOT NULL DEFAULT '',
              //`审批日期` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
              //`审批结果` varchar(50) NOT NULL DEFAULT '',
              //`审批意见` varchar(500) NOT NULL DEFAULT '',
              //`核定人` varchar(50) NOT NULL DEFAULT '',
              //`核定日期` datetime NOT NULL DEFAULT '1900-01-01 00:00:00',
              //`核定结果` varchar(50) NOT NULL DEFAULT '',
              //`核定意见` varchar(500) NOT NULL DEFAULT '',
              //`预算状态` varchar(50) NOT NULL DEFAULT '',
              //`来源` varchar(20) NOT NULL DEFAULT '',
              //`完成日期` datetime NOT NULL DEFAULT '1900-01-01 00:00:00', 

            DataTable tb = Program.获取一般数据集mysql(Program.old_scnn, str1);
            grid.DataSource = tb;

            int i = 0;

            for (i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i].Cells["序号"].Value = i + 1;

                str1 = "";
                str1 = grid.Rows[i].Cells["申请人"].Value.ToString();
                grid.Rows[i].Cells["申请人"].Value = 获取人科用户名(str1);

                str1 = "";
                str1 = grid.Rows[i].Cells["类型"].Value.ToString().Trim();
                if (str1 == "0")
                    str1 = "1";
                else
                    str1 = "0";
                grid.Rows[i].Cells["类型"].Value = str1;

                str1 = "";
                str1 = grid.Rows[i].Cells["成本中心编号"].Value.ToString();
                grid.Rows[i].Cells["成本中心编号"].Value = 获取成本中心编号(str1);

                str1 = "";
                str1 = grid.Rows[i].Cells["业务类型"].Value.ToString();
                grid.Rows[i].Cells["业务类型"].Value = 获取业务类型名称(str1);

                str1 = "";
                str1 = grid.Rows[i].Cells["公证处"].Value.ToString();
                grid.Rows[i].Cells["公证处"].Value = 获取公证处名称(str1);

                str1 = "";
                str1 = grid.Rows[i].Cells["审批结果"].Value.ToString().Trim();
                if (str1 == "-1")
                    str1 = "驳回";
                else
                {
                    if (str1 == "1")
                        str1 = "同意";
                    else
                        str1 = "";
                }
                grid.Rows[i].Cells["审批结果"].Value = str1;

                str1 = "";
                str1 = grid.Rows[i].Cells["预算状态"].Value.ToString().Trim();
                if (str1 == "0")
                    str1 = "待完成";
                else
                    str1 = "已完成";
                grid.Rows[i].Cells["预算状态"].Value = str1;

            }
            for (i = 0; i < grid.Rows.Count; i++)
            {
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    if (grid.Rows[i].Cells[j].Value == null) grid.Rows[i].Cells[j].Value = "";
                }
            }


        }


        private void b开始导入_Click(object sender, EventArgs e)
        {
            string str1 = "";
            string err = "";
            int i = 0;

            for (i = 0; i < grid.Rows.Count; i++)
            {
                Application.DoEvents();

                str1 = "insert into B预算(ID,类型,父ID,预算编号,预算名称,成本中心编号,业务类型,销售,预算说明,申请人,申请日期,审批人,审批日期,审批结果,审批意见,核定人,核定日期,核定结果,核定意见,预算状态,来源,完成日期) values('";
                str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "',";
                str1 = str1 + grid.Rows[i].Cells["类型"].Value.ToString() + ",'";
                str1 = str1 + "" + "','";
                str1 = str1 + grid.Rows[i].Cells["预算编号"].Value.ToString() + "','";
                str1 = str1 + grid.Rows[i].Cells["预算名称"].Value.ToString() + "','";
                str1 = str1 + grid.Rows[i].Cells["成本中心编号"].Value.ToString() + "','";
                str1 = str1 + grid.Rows[i].Cells["业务类型"].Value.ToString() + "','";
                str1 = str1 + grid.Rows[i].Cells["销售"].Value.ToString() + "','";
                str1 = str1 + grid.Rows[i].Cells["预算说明"].Value.ToString() + "','";
                str1 = str1 + grid.Rows[i].Cells["申请人"].Value.ToString() + "','";
                str1 = str1 + DateTime.Parse(grid.Rows[i].Cells["申请日期"].Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','";
                str1 = str1 + "" + "','";
                str1 = str1 + "1900-01-01 00:00:00" + "','";
                str1 = str1 + grid.Rows[i].Cells["审批结果"].Value.ToString() + "','";
                str1 = str1 + "" + "','";
                str1 = str1 + "" + "','";
                str1 = str1 + "1900-01-01 00:00:00" + "','";
                str1 = str1 + "" + "','";
                str1 = str1 + "" + "','";
                str1 = str1 + grid.Rows[i].Cells["预算状态"].Value.ToString() + "','";
                str1 = str1 + "老系统" + "','";
                str1 = str1 + "1900-01-01 00:00:00" + "')";

                int k = 0;
                err = "";
                k = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                if (err != "")
                {
                    grid.Rows[i].Cells["导入错误原因"].Value = err;
                    grid.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    grid.Refresh();
                }
                if (k > 0)
                {
                    grid.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    grid.Refresh();

                    //激励
                    //str1 = "";
                    //str1 = grid.Rows[i].Cells["激励预算编号"].Value.ToString().Trim();
                    //if (str1 != "")
                    //{
                    //    str1 = "insert into B激励(ID,预算ID,人员,金额,比例,备注,录入人,录入日期,审批人,审批日期,审批结果,审批意见) values('";
                    //    str1 = str1 + Guid.NewGuid().ToString() + "','";
                    //    str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                    //    str1 = str1 + grid.Rows[i].Cells["销售"].Value.ToString() + "',";
                    //    str1 = str1 + CommonFunctions.ValDec(grid.Rows[i].Cells["激励值"].Value.ToString()).ToString() + ",";
                    //    str1 = str1 + CommonFunctions.ValDec(grid.Rows[i].Cells["激励预算值百分比"].Value.ToString()).ToString() + ",'";
                    //    str1 = str1 + grid.Rows[i].Cells["激励预算编号"].Value.ToString() + "','";
                    //    str1 = str1 + grid.Rows[i].Cells["申请人"].Value.ToString() + "','";
                    //    str1 = str1 + DateTime.Parse(grid.Rows[i].Cells["申请日期"].Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','";
                    //    str1 = str1 + "" + "','";
                    //    str1 = str1 + "1900-01-01 00:00:00" + "','";
                    //    str1 = str1 + grid.Rows[i].Cells["审批结果"].Value.ToString() + "','";
                    //    str1 = str1 + "" + "')";
                    //    int m = 0;
                    //    err = "";
                    //    m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                    //    if (err != "")
                    //    {
                    //        grid.Rows[i].Cells["激励错误"].Value = err;
                    //    }
                    //}
                    str1 = "";
                    str1 = grid.Rows[i].Cells["预算编号"].Value.ToString();
                    if (str1 != "")
                    {
                        str1 = "select * from renke_item where ItemNumsOld='" + str1 + "'";
                        DataTable tb0 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                        for (int j = 0; j < tb0.Rows.Count; j++)
                        {
                            str1 = "insert into B激励(ID,预算ID,人员,金额,比例,备注,录入人,录入日期,审批人,审批日期,审批结果,审批意见) values('";
                            str1 = str1 + tb0.Rows[j]["renke_ItemID"].ToString() + "','";
                            str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                            str1 = str1 + grid.Rows[i].Cells["销售"].Value.ToString() + "',";
                            str1 = str1 + CommonFunctions.ValDec(tb0.Rows[j]["ItemBonus"].ToString()).ToString() + ",";
                            str1 = str1 + CommonFunctions.ValDec(tb0.Rows[j]["ItemPercent"].ToString()).ToString() + ",'";
                            str1 = str1 + tb0.Rows[j]["remark"].ToString() + "','";
                            str1 = str1 + grid.Rows[i].Cells["申请人"].Value.ToString() + "','";
                            str1 = str1 + DateTime.Parse(grid.Rows[i].Cells["申请日期"].Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','";                            
                            str1 = str1 + "预算审批人" + "','";
                            str1 = str1 + DateTime.Parse(tb0.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','";
                            string spjg = tb0.Rows[j]["ItemCheck"].ToString().Trim();
                            if (spjg == "-1")
                            {
                                spjg = "拒绝";
                            }
                            else
                            {
                                if (spjg == "1")
                                {
                                    spjg = "同意";
                                }
                                else
                                {
                                    spjg = "";
                                }
                            }
                            str1 = str1 + spjg + "','";
                            str1 = str1 + "老系统" + "')";
                            int m = 0;
                            err = "";
                            m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                            if (err != "")
                            {
                                grid.Rows[i].Cells["激励错误"].Value = grid.Rows[i].Cells["激励错误"].Value.ToString() + "、" + err;
                            }
                        }
                    }


                    //预算收入
                    str1 = "select renke_ItemInID,renke_ItemID,renke_UserID,ItemInPower,ItemInFees,ItemInTime,ItemInRemark,ItemInState,Enable,UpdateTime,CreateTime from renke_itemin where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "'";
                    DataTable tb1 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                    for (int j = 0; j < tb1.Rows.Count; j++)
                    {
                        str1 = "insert into B预算收支(ID,预算ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                        str1 = str1 + tb1.Rows[j]["renke_ItemInID"].ToString() + "','";
                        str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                        str1 = str1 + "收入" + "',";
                        str1 = str1 + tb1.Rows[j]["ItemInFees"].ToString() + ",'";
                        str1 = str1 + tb1.Rows[j]["ItemInPower"].ToString() + "','";
                        try { str1 = str1 + DateTime.Parse(tb1.Rows[j]["ItemInTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                        catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                        str1 = str1 + tb1.Rows[j]["ItemInRemark"].ToString() + "','";
                        str1 = str1 + "" + "','";
                        str1 = str1 + DateTime.Parse(tb1.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        int m = 0;
                        err = "";
                        m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                        if (err != "")
                        {
                            grid.Rows[i].Cells["预算收入错误"].Value = grid.Rows[i].Cells["预算收入错误"].Value.ToString() + "、" + err;
                        }
                    }

                    //预算支出
                    str1 = "select renke_ItemOutID,renke_ItemID,renke_UserID,ItemOutPower,ItemOutFees,ItemOutTime,ItemOutRemark,ItemOutState,Enable,UpdateTime,CreateTime from renke_itemout where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "'";
                    DataTable tb2 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                    for (int j = 0; j < tb2.Rows.Count; j++)
                    {
                        str1 = "insert into B预算收支(ID,预算ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                        str1 = str1 + tb2.Rows[j]["renke_ItemOutID"].ToString() + "','";
                        str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                        str1 = str1 + "支出" + "',";
                        str1 = str1 + tb2.Rows[j]["ItemOutFees"].ToString() + ",'";
                        str1 = str1 + tb2.Rows[j]["ItemOutPower"].ToString() + "','";
                        try { str1 = str1 + DateTime.Parse(tb2.Rows[j]["ItemOutTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                        catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                        str1 = str1 + tb2.Rows[j]["ItemOutRemark"].ToString() + "','";
                        str1 = str1 + "" + "','";
                        str1 = str1 + DateTime.Parse(tb2.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        int m = 0;
                        err = "";
                        m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                        if (err != "")
                        {
                            grid.Rows[i].Cells["预算支出错误"].Value = grid.Rows[i].Cells["预算支出错误"].Value.ToString() + "、" + err;
                        }
                    }

                    //实际收入
                    str1 = "select renke_ItemInFinanceID,renke_ItemInID,ActualFees,ActualTime,Remark,Enable,UpdateTime,CreateTime from renke_iteminfinance where renke_ItemInID in (select renke_ItemInID from renke_itemin where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "')";
                    DataTable tb3 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                    for (int j = 0; j < tb3.Rows.Count; j++)
                    {
                        str1 = "insert into b实际收支(ID,预算ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                        str1 = str1 + tb3.Rows[j]["renke_ItemInFinanceID"].ToString() + "','";
                        str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                        str1 = str1 + "收入" + "',";
                        str1 = str1 + tb3.Rows[j]["ActualFees"].ToString() + ",'";
                        str1 = str1 + "" + "','";
                        try { str1 = str1 + DateTime.Parse(tb3.Rows[j]["ActualTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                        catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                        str1 = str1 + tb3.Rows[j]["Remark"].ToString() + "','";
                        str1 = str1 + "" + "','";
                        str1 = str1 + DateTime.Parse(tb3.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        int m = 0;
                        err = "";
                        m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                        if (err != "")
                        {
                            grid.Rows[i].Cells["实际收入错误"].Value = grid.Rows[i].Cells["实际收入错误"].Value.ToString() + "、" + err;
                        }
                    }

                    //实际支出
                    str1 = "select renke_ItemOutFinanceID,renke_ItemOutID,ActualFees,ActualTime,Remark,Enable,UpdateTime,CreateTime from renke_itemoutfinance where renke_ItemOutID in (select renke_ItemOutID from renke_itemout where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "')";
                    DataTable tb4 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                    for (int j = 0; j < tb4.Rows.Count; j++)
                    {
                        str1 = "insert into b实际收支(ID,预算ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                        str1 = str1 + tb4.Rows[j]["renke_ItemOutFinanceID"].ToString() + "','";
                        str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                        str1 = str1 + "支出" + "',";
                        str1 = str1 + tb4.Rows[j]["ActualFees"].ToString() + ",'";
                        str1 = str1 + "" + "','";
                        try { str1 = str1 + DateTime.Parse(tb4.Rows[j]["ActualTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                        catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                        str1 = str1 + tb4.Rows[j]["Remark"].ToString() + "','";
                        str1 = str1 + "" + "','";
                        str1 = str1 + DateTime.Parse(tb4.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        int m = 0;
                        err = "";
                        m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                        if (err != "")
                        {
                            grid.Rows[i].Cells["实际支出错误"].Value = grid.Rows[i].Cells["实际支出错误"].Value.ToString() + "、" + err;
                        }
                    }

                }

            }
        }

        
        private string 获取公证处名称(string ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb公证处 != null)
            {
                DataRow record;
                keys[0] = ID;   //ID
                record = tb公证处.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["PnoName"].ToString();
                }
            }
            return str1;
        }
        private string 获取人科用户名(string ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb人科用户 != null)
            {
                DataRow record;
                keys[0] = ID;   //ID
                record = tb人科用户.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["UserName"].ToString();
                }
            }
            if (str1.Trim() == "") str1 = "（无）";
            return str1;
        }
        private string 获取成本中心编号(string ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb成本中心 != null)
            {
                DataRow record;
                keys[0] = ID;   //ID
                record = tb成本中心.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["CategoryName"].ToString();
                }
            }
            if (str1.Trim() == "")
            {
                for (int i = 0; i < tb成本中心.Rows.Count; i++)
                {
                    if(tb成本中心.Rows[i]["ParentID"].ToString()==ID)
                    {
                        str1 = tb成本中心.Rows[i]["CategoryName"].ToString();
                        break;
                    }
                }
            }
            if (str1.Trim() == "") str1 = ID;
            if (str1.Trim() == "") str1 = "（无）";
            return str1;
        }
        private string 获取业务类型名称(string ID)
        {
            string[] keys = new string[1];
            string str1 = "";
            if (tb业务类型 != null)
            {
                DataRow record;
                keys[0] = ID;   //ID
                record = tb业务类型.Rows.Find(keys);
                if (record != null)
                {
                    str1 = record["CategoryName"].ToString();
                }
            }
            if (str1.Trim() == "")
            {
                for (int i = 0; i < tb成本中心.Rows.Count; i++)
                {
                    if (tb成本中心.Rows[i]["ParentID"].ToString() == ID)
                    {
                        str1 = tb成本中心.Rows[i]["CategoryName"].ToString();
                        break;
                    }
                }
            }
            if (str1.Trim() == "") str1 = ID;
            if (str1.Trim() == "") str1 = "（无）";
            return str1;
        }



        private void b获取数据_Click(object sender, EventArgs e)
        {
            获取数据(0);
        }

        private void b获取数据中证通_Click(object sender, EventArgs e)
        {
            获取数据(1);
        }

        private void b开始导入中证通_Click(object sender, EventArgs e)
        {
            string str1 = "";
            string strID = "";
            string err = "";
            int i = 0;

            for (i = 0; i < grid.Rows.Count; i++)
            {
                Application.DoEvents();

                //先找子订单号
                str1 = grid.Rows[i].Cells["子订单号"].Value.ToString().Trim();
                if (str1 == "")
                {
                    grid.Rows[i].Cells["导入错误原因"].Value = "无效的子订单号";
                    grid.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    grid.Refresh();
                    continue;
                }

                //子订单号在新预算系统已经存在
                DataTable tb = Program.获取一般数据集mysql(Program.new_scnn, "select ID,子订单号 from b中证通证书 where 子订单号='" + str1 + "'");
                strID = "";
                try { strID = tb.Rows[0]["ID"].ToString(); }
                catch { }
                if (strID == "")
                {
                    grid.Rows[i].Cells["导入错误原因"].Value = "子订单号在新预算系统不存在";
                    grid.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    grid.Refresh();
                    continue;
                }


                grid.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                grid.Refresh();

                //激励
                str1 = "";
                str1 = grid.Rows[i].Cells["预算编号"].Value.ToString();
                if (str1 != "")
                {
                    str1 = "select * from renke_item where ItemNumsOld='" + str1 + "'";
                    DataTable tb0 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                    for (int j = 0; j < tb0.Rows.Count; j++)
                    {
                        str1 = "insert into B激励(ID,预算ID,人员,金额,比例,备注,录入人,录入日期,审批人,审批日期,审批结果,审批意见) values('";
                        str1 = str1 + tb0.Rows[j]["renke_ItemID"].ToString() + "','";
                        str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                        str1 = str1 + grid.Rows[i].Cells["销售"].Value.ToString() + "',";
                        str1 = str1 + CommonFunctions.ValDec(tb0.Rows[j]["ItemBonus"].ToString()).ToString() + ",";
                        str1 = str1 + CommonFunctions.ValDec(tb0.Rows[j]["ItemPercent"].ToString()).ToString() + ",'";
                        str1 = str1 + tb0.Rows[j]["remark"].ToString() + "','";
                        str1 = str1 + grid.Rows[i].Cells["申请人"].Value.ToString() + "','";
                        str1 = str1 + DateTime.Parse(grid.Rows[i].Cells["申请日期"].Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','";
                        str1 = str1 + "预算审批人" + "','";
                        str1 = str1 + DateTime.Parse(tb0.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','";
                        string spjg = tb0.Rows[j]["ItemCheck"].ToString().Trim();
                        if (spjg == "-1")
                        {
                            spjg = "拒绝";
                        }
                        else
                        {
                            if (spjg == "1")
                            {
                                spjg = "同意";
                            }
                            else
                            {
                                spjg = "";
                            }
                        }
                        str1 = str1 + spjg + "','";
                        str1 = str1 + "老系统" + "')";
                        int m = 0;
                        err = "";
                        m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                        if (err != "")
                        {
                            grid.Rows[i].Cells["激励错误"].Value = grid.Rows[i].Cells["激励错误"].Value.ToString() + "、" + err;
                        }
                    }
                }


                //预算收入
                str1 = "select renke_ItemInID,renke_ItemID,renke_UserID,ItemInPower,ItemInFees,ItemInTime,ItemInRemark,ItemInState,Enable,UpdateTime,CreateTime from renke_itemin where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "'";
                DataTable tb1 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                for (int j = 0; j < tb1.Rows.Count; j++)
                {
                    str1 = "insert into b中证通证书预算收支(ID,证书ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                    str1 = str1 + tb1.Rows[j]["renke_ItemInID"].ToString() + "','";
                    str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                    str1 = str1 + "收入" + "',";
                    str1 = str1 + tb1.Rows[j]["ItemInFees"].ToString() + ",'";
                    str1 = str1 + tb1.Rows[j]["ItemInPower"].ToString() + "','";
                    try { str1 = str1 + DateTime.Parse(tb1.Rows[j]["ItemInTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                    catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                    str1 = str1 + tb1.Rows[j]["ItemInRemark"].ToString() + "','";
                    str1 = str1 + "" + "','";
                    str1 = str1 + DateTime.Parse(tb1.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    int m = 0;
                    err = "";
                    m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                    if (err != "")
                    {
                        grid.Rows[i].Cells["预算收入错误"].Value = grid.Rows[i].Cells["预算收入错误"].Value.ToString() + "、" + err;
                    }
                }

                //预算支出
                str1 = "select renke_ItemOutID,renke_ItemID,renke_UserID,ItemOutPower,ItemOutFees,ItemOutTime,ItemOutRemark,ItemOutState,Enable,UpdateTime,CreateTime from renke_itemout where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "'";
                DataTable tb2 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                for (int j = 0; j < tb2.Rows.Count; j++)
                {
                    str1 = "insert into b中证通证书预算收支(ID,证书ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                    str1 = str1 + tb2.Rows[j]["renke_ItemOutID"].ToString() + "','";
                    str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                    str1 = str1 + "支出" + "',";
                    str1 = str1 + tb2.Rows[j]["ItemOutFees"].ToString() + ",'";
                    str1 = str1 + tb2.Rows[j]["ItemOutPower"].ToString() + "','";
                    try { str1 = str1 + DateTime.Parse(tb2.Rows[j]["ItemOutTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                    catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                    str1 = str1 + tb2.Rows[j]["ItemOutRemark"].ToString() + "','";
                    str1 = str1 + "" + "','";
                    str1 = str1 + DateTime.Parse(tb2.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    int m = 0;
                    err = "";
                    m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                    if (err != "")
                    {
                        grid.Rows[i].Cells["预算支出错误"].Value = grid.Rows[i].Cells["预算支出错误"].Value.ToString() + "、" + err;
                    }
                }

                //实际收入
                str1 = "select renke_ItemInFinanceID,renke_ItemInID,ActualFees,ActualTime,Remark,Enable,UpdateTime,CreateTime from renke_iteminfinance where renke_ItemInID in (select renke_ItemInID from renke_itemin where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "')";
                DataTable tb3 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                for (int j = 0; j < tb3.Rows.Count; j++)
                {
                    str1 = "insert into b中证通证书实际收支(ID,证书ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                    str1 = str1 + tb3.Rows[j]["renke_ItemInFinanceID"].ToString() + "','";
                    str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                    str1 = str1 + "收入" + "',";
                    str1 = str1 + tb3.Rows[j]["ActualFees"].ToString() + ",'";
                    str1 = str1 + "" + "','";
                    try { str1 = str1 + DateTime.Parse(tb3.Rows[j]["ActualTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                    catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                    str1 = str1 + tb3.Rows[j]["Remark"].ToString() + "','";
                    str1 = str1 + "" + "','";
                    str1 = str1 + DateTime.Parse(tb3.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    int m = 0;
                    err = "";
                    m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                    if (err != "")
                    {
                        grid.Rows[i].Cells["实际收入错误"].Value = grid.Rows[i].Cells["实际收入错误"].Value.ToString() + "、" + err;
                    }
                }

                //实际支出
                str1 = "select renke_ItemOutFinanceID,renke_ItemOutID,ActualFees,ActualTime,Remark,Enable,UpdateTime,CreateTime from renke_itemoutfinance where renke_ItemOutID in (select renke_ItemOutID from renke_itemout where renke_ItemID='" + grid.Rows[i].Cells["ID"].Value.ToString() + "')";
                DataTable tb4 = Program.获取一般数据集mysql(Program.old_scnn, str1);
                for (int j = 0; j < tb4.Rows.Count; j++)
                {
                    str1 = "insert into b中证通证书实际收支(ID,证书ID,收支,金额,借贷方,日期,备注,录入人,录入日期) values('";
                    str1 = str1 + tb4.Rows[j]["renke_ItemOutFinanceID"].ToString() + "','";
                    str1 = str1 + grid.Rows[i].Cells["ID"].Value.ToString() + "','";
                    str1 = str1 + "支出" + "',";
                    str1 = str1 + tb4.Rows[j]["ActualFees"].ToString() + ",'";
                    str1 = str1 + "" + "','";
                    try { str1 = str1 + DateTime.Parse(tb4.Rows[j]["ActualTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','"; }
                    catch { str1 = str1 + "1900-01-01 00:00:00" + "','"; }
                    str1 = str1 + tb4.Rows[j]["Remark"].ToString() + "','";
                    str1 = str1 + "" + "','";
                    str1 = str1 + DateTime.Parse(tb4.Rows[j]["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    int m = 0;
                    err = "";
                    m = Program.执行SQL命令mysql(Program.new_scnn, str1, ref err);
                    if (err != "")
                    {
                        grid.Rows[i].Cells["实际支出错误"].Value = grid.Rows[i].Cells["实际支出错误"].Value.ToString() + "、" + err;
                    }
                }


            }
        }

        
    }
}
