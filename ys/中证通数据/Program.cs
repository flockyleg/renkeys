using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Reflection; 

namespace 中证通数据
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process instance = RunningInstance();
            if (instance != null)   //已经有实例
            {
                MessageBox.Show("程序或进程已经在运行。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;

                //if (MessageBox.Show("程序或进程已经在运行，打开多个程序可能导致不可预见的错误，是否立即关闭？", "信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes) return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMainNew());
        }

        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表   
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程   
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径 
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回已经存在的进程 
                        return process;
                    }
                }
            }
            return null;
        } 



        public static string AppPath = "";
        //public static string ys_scnn = @"server=(local);uid=sa;pwd=;database=RK_BUDGET;pooling=false";  //预算数据 ms sql
        //public static string zzt_scnn = @"server=192.168.9.253;user id=root;password=123@qw;database=nampc";     //中证通数据库mysql
        //public static string zzt_scnn = @"server=localhost;user id=root;password=root;database=abc";     //中证通数据库mysql

        public static string ys_scnn = @"server=zhongzhengtong.mysql.rds.aliyuncs.com;user id=renosdata;password=Renosdata123;database=rk_budget";  //预算数据mysql
        public static string zzt_scnn = @"server=zhongzhengtong.mysql.rds.aliyuncs.com;user id=renosdata;password=Renosdata123;database=nampc";     //中证通数据库mysql


        public static DataTable 获取一般数据集sql(string SQLstring)
        {
            string cnnstring = ys_scnn;
            try
            {
                SqlConnection cnn = new SqlConnection(cnnstring);
                cnn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(SQLstring, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                cnn.Close();
                return ds.Tables[0];
            }
            catch { }
            return null;
        }
        public static DataTable 获取一般数据集sql(string 连接字符串, string SQLstring)
        {
            string cnnstring = 连接字符串;

            try
            {
                SqlConnection cnn = new SqlConnection(cnnstring);
                cnn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(SQLstring, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                cnn.Close();
                return ds.Tables[0];
            }
            catch { }
            return null;
        }

        public static int 执行SQL命令sql(string sSQLCommand)
        {
            int i = 0;

            try
            {
                SqlConnection cnn = new SqlConnection(ys_scnn);
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sSQLCommand, cnn);
                i = cmd.ExecuteNonQuery();
                cnn.Close();
                return i;
            }
            catch { return -1; }
            //catch(Exception ex)
            //{
            //    string str1 = ex.Message;
            //    return -1; 
            //}
        }




        public static DataTable 获取一般数据集mysql(string SQLstring)
        {
            string cnnstring = zzt_scnn;
            try
            {
                MySqlConnection cnn = new MySqlConnection(cnnstring);
                cnn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(SQLstring, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                cnn.Close();
                return ds.Tables[0];
            }
            catch { 
               //MessageBox.Show(cnnstring + System.Environment.NewLine + SQLstring); Clipboard.SetText(SQLstring); 
            }
            return null;
        }
        public static DataTable 获取一般数据集mysql(string 连接字符串, string SQLstring)
        {
            string cnnstring = 连接字符串;

            try
            {
                MySqlConnection cnn = new MySqlConnection(cnnstring);
                cnn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(SQLstring, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                cnn.Close();
                return ds.Tables[0];
            }
            catch { }
            return null;
        }

        public static int 执行SQL命令mysql(string sSQLCommand)
        {
            int i = 0;

            try
            {
                MySqlConnection cnn = new MySqlConnection(zzt_scnn);
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sSQLCommand, cnn);
                i = cmd.ExecuteNonQuery();
                cnn.Close();
                return i;
            }
            catch { return -1; }
            //catch(Exception ex)
            //{
            //    string str1 = ex.Message;
            //    return -1; 
            //}
        }

        public static int 执行SQL命令mysql(string 连接字符串, string sSQLCommand)
        {
            int i = 0;

            try
            {
                MySqlConnection cnn = new MySqlConnection(连接字符串);
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sSQLCommand, cnn);
                i = cmd.ExecuteNonQuery();
                cnn.Close();
                return i;
            }
            catch { return -1; }
            //catch(Exception ex)
            //{
            //    string str1 = ex.Message;
            //    return -1; 
            //}
        }
    }

    public class cls激励比例
    {
        #region 属性

        private string tmp_激励名称 = "";
        public string 激励名称
        {
            get
            {
                return tmp_激励名称;
            }
            set
            {
                tmp_激励名称 = value;
            }
        }

        private decimal tmp_激励比例 = 0.00m;
        public decimal 激励比例
        {
            get
            {
                return tmp_激励比例;
            }
            set
            {
                tmp_激励比例 = value;
                if (tmp_激励比例 < 0.00m) tmp_激励比例 = 0.00m;
                if (tmp_激励比例 > 100.00m) tmp_激励比例 = 100.00m;
            }
        }

        #endregion
    }
}
