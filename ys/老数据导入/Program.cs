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

namespace 老数据导入
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        
        public static string AppPath = "";
        public static string new_scnn = @"server=zhongzhengtong.mysql.rds.aliyuncs.com;user id=renosdata;password=Renosdata123;database=rk_budget";  //新预算数据mysql
        public static string old_scnn = @"server=zhongzhengtong.mysql.rds.aliyuncs.com;user id=renosdata;password=Renosdata123;database=renkefinance";     //老系统数据库mysql
        //public static string new_scnn = @"server=192.168.9.253;user id=root;password=123@qw;database=rk_budget";  //新预算数据mysql
        //public static string old_scnn = @"server=192.168.9.253;user id=root;password=123@qw;database=renkefinance";     //老系统数据库mysql



        public static DataTable 获取一般数据集mysql(string 连接字符串, string SQLstring)
        {
            //MessageBox.Show(SQLstring);
            string cnnstring = 连接字符串;

            //try
            //{
                MySqlConnection cnn = new MySqlConnection(cnnstring);
                cnn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(SQLstring, cnn);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                cnn.Close();
                return ds.Tables[0];
            //}
            //catch { }
            return null;
        }

        public static int 执行SQL命令mysql(string 连接字符串, string sSQLCommand, ref string err)
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
            catch(Exception ex)
            {
                err = ex.Message;
                return -1; 
            }
            //catch(Exception ex)
            //{
            //    string str1 = ex.Message;
            //    return -1; 
            //}
        }
    }


    public class CommonFunctions
    {
        public static string 英文字符 = " `1234567890-=~!@#$%^&*()_+|qwertyuiop[]asdfghjkl;'zxcvbnm,./QWERTYUIOP{}ASDFGHJKL:ZXCVBNM<>?\\";

        public CommonFunctions()
        {
            //
        }

        public static string Tab键 = ((char)(9)).ToString();   //Tab键

        //分隔符，下面4个分隔符号能隐藏显示
        public static string 文件分隔符 = ((char)(28)).ToString();   //文件分隔符
        public static string 组分隔符 = ((char)(29)).ToString();     //组分隔符
        public static string 记录分隔符 = ((char)(30)).ToString();   //记录分隔符
        public static string 单元分隔符 = ((char)(31)).ToString();   //单元分隔符

        public static string 图片文件后缀 = "*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tif";
        public static string 办公文件后缀 = "*.doc;*.xls;*.ppt;*.dot;*.rtf;*.wps;*.docx;*.xlsx;*.pptx;*.xps";
        public static string 网页文件后缀 = "*.htm;*.html;*.mht;*.txt;*.pdf";


        /// <summary>
        /// 如果字符串strFather中含有chrCompart，则将strFather以第一个chrCompart为界限分割为前后两部分前部分在ReadWord中，后部分在strFather中，前后部分都不包括chrCompart
        /// </summary>
        /// <param name="strFather">父字符串</param>
        /// <param name="chrCompart">子字符串</param>
        /// <returns></returns>
        public static string ReadWord(ref string strFather, string chrCompart)
        {
            int i;
            string s = "";

            try
            {
                s = strFather;
                if (strFather == "" | chrCompart == "")
                {
                    strFather = "";
                    return s;
                }
                i = strFather.IndexOf(chrCompart);
                if (i < 0)
                {
                    strFather = "";
                    return s;
                }
                else
                {
                    s = strFather.Substring(0, i);
                    strFather = strFather.Substring(i + chrCompart.Length);
                    return s;
                }
            }
            catch
            {
                throw new Exception("函数发生错误。");
            }
        }

        /// <summary>
        /// 如果字符串strFather中含有chrCompart，则将strFather以最后一个chrCompart为界限分割为前后两部分前部分在ReadWord中，后部分在strFather中，前后部分都不包括chrCompart
        /// </summary>
        /// <param name="strFather">父字符串</param>
        /// <param name="chrCompart">子字符串</param>
        /// <returns></returns>
        public static string ReadLastWord(ref string strFather, string chrCompart)
        {
            int i;
            string s = "";

            try
            {
                s = strFather;
                if (strFather == "" | chrCompart == "")
                {
                    strFather = "";
                    return s;
                }
                i = strFather.LastIndexOf(chrCompart);
                if (i < 0)
                {
                    strFather = "";
                    return s;
                }
                else
                {
                    s = strFather.Substring(0, i);
                    strFather = strFather.Substring(i + chrCompart.Length);
                    return s;
                }
            }
            catch
            {
                throw new Exception("函数发生错误。");
            }
        }

        /// <summary>
        /// Right(String,Length)
        /// </summary>
        /// <param name="strFather"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string Right(string strFather, int Length)
        {
            string s = "";

            try
            {
                if (Length < 1) return "";
                if (Length >= strFather.Length) return strFather;
                s = strFather.Substring(strFather.Length - Length, Length);
                return s;
            }
            catch
            {
                throw new Exception("函数发生错误。");
            }
        }

        /// <summary>
        /// Left(String,Length)
        /// </summary>
        /// <param name="strFather"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string Left(string strFather, int Length)
        {
            string s = "";

            try
            {
                if (Length < 1) return "";
                if (Length >= strFather.Length) return strFather;
                s = strFather.Substring(0, Length);
                return s;
            }
            catch
            {
                throw new Exception("函数发生错误。");
            }
        }

        /// <summary>
        /// 对一个二维矩阵的行，按照指定的列进行指定的排序
        /// </summary>
        /// <param name="Arr">二维矩阵（一个二维数组）</param>
        /// <param name="Col">指定列集合，以这些列排序，列集合中下标越小优先级越高</param>
        /// <param name="Asc">升序还是降序</param>
        /// /// <param name="CurrentCulture">比较类型：0按数值排序（不是数值就排序失败），1按哈希码排序，2(或其他)按字符串（汉字拼音）排序</param>
        /// <returns></returns>
        public static void ArraySort(ref string[,] Arr, int[] Col, bool Asc, int Culture)
        {
            int i = 0, j = 0, k = 0, m = 0;
            int cRow = 0;
            string str1 = "", str2 = "";
            int fistRow = 0, lastRow = 0;

            try
            {
                fistRow = Arr.GetLowerBound(0);
                lastRow = Arr.GetUpperBound(0);
                if ((lastRow - fistRow) < 1) return;   //如果只有一行			
                for (i = fistRow; i < lastRow; i++)
                {
                    cRow = i;
                    for (m = Col.GetLowerBound(0); m <= Col.GetUpperBound(0); m++)
                    {
                        j = Col[m];
                        str1 = Arr[i, j];
                        if (str1 == null) str1 = "";
                        for (k = i + 1; k <= lastRow; k++)
                        {
                            str2 = Arr[k, j];
                            if (str2 == null) str2 = "";
                            switch (Culture)
                            {
                                case 0:
                                    if (str1 == "") str1 = "0";
                                    if (str2 == "") str2 = "0";
                                    if ((decimal.Parse(str2) < decimal.Parse(str1)) == Asc) cRow = k;
                                    break;
                                case 1:
                                    if ((str2.GetHashCode() < str1.GetHashCode()) == Asc) cRow = k;
                                    break;
                                case 2:
                                    //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                                    if ((string.Compare(str2, str1) < 0) == Asc) cRow = k;
                                    break;
                            }
                        }
                        if (cRow != i)
                        {
                            break;
                        }
                    }

                    //两行交换
                    if (cRow != i)
                    {
                        for (j = Arr.GetLowerBound(1); j <= Arr.GetUpperBound(1); j++)
                        {
                            str2 = Arr[i, j];
                            Arr[i, j] = Arr[cRow, j];
                            Arr[cRow, j] = str2;
                        }
                    }
                }
            }
            catch
            {
                ///
            }
        }

        /// <summary>
        /// 返回子字符串在父字符串中的数目
        /// </summary>
        /// <param name="SFather">父字符串</param>
        /// <param name="sSon">子字符串</param>
        /// <returns>共有几个</returns>
        public static int HowManyInThisString(string SFather, string sSon)
        {
            int i = 0, j = 0, k = 0;
            j = SFather.IndexOf(sSon, 0);
            while (j >= 0)
            {
                i++;
                k = j + sSon.Length;
                j = SFather.IndexOf(sSon, k);
            }
            return i;
        }


        /// <summary>
        /// 将strF强制转化为decimal，类似vb的val()
        /// </summary>
        /// <param name="strF"></param>
        /// <returns></returns>
        public static decimal ValDec(string strF)
        {
            string str1 = "";
            bool bool1 = false;
            Decimal DecReturn = 0.00m;
            int i = 0;
            const string tempS = "1234567890.";
            string strIn;
            bool bool2 = false;

            strIn = strF.Trim();
            if (strIn.Length > 0)
            {
                if (strIn.Substring(0, 1) == "-")
                {
                    bool2 = true;
                    ReadWord(ref strIn, "-");
                }
            }

            if (strIn.Length > 0)
            {
                for (i = 0; i < strIn.Length; i++)
                {
                    if (tempS.IndexOf(strIn.Substring(i, 1)) >= 0)
                    {
                        if (strIn.Substring(i, 1) == ".")
                        {
                            if (bool1)
                                break;
                            else
                                bool1 = true;
                        }
                        str1 = str1 + strIn.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                str1 = str1.Trim();
                if (str1 == ".") str1 = "";
                if (str1 != "") DecReturn = Decimal.Parse(str1);
            }

            if (bool2) DecReturn = -DecReturn;
            return DecReturn;
        }

        /// <summary>
        /// 将strF强制转化为long，类似vb的val()
        /// </summary>
        /// <param name="strF"></param>
        /// <returns></returns>
        public static long ValLng(string strF)
        {
            string str1 = "";
            long DecReturn = 0;
            int i = 0;
            const string tempS = "1234567890";
            string strIn;
            bool bool2 = false;

            strIn = strF.Trim();
            if (strIn.Length > 0)
            {
                if (strIn.Substring(0, 1) == "-")
                {
                    bool2 = true;
                    ReadWord(ref strIn, "-");
                }
            }

            if (strIn.Length > 0)
            {
                for (i = 0; i < strIn.Length; i++)
                {
                    if (tempS.IndexOf(strIn.Substring(i, 1)) >= 0)
                    {
                        str1 = str1 + strIn.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                if (str1 != "") DecReturn = long.Parse(str1);
            }

            if (bool2) DecReturn = -DecReturn;
            return DecReturn;
        }
        /// <summary>
        /// 将strF强制转化为int，类似vb的val()
        /// </summary>
        /// <param name="strF"></param>
        /// <returns></returns>
        public static int ValInt(string strF)
        {
            try { return ((int)(ValLng(strF))); }
            catch { return 0; }
        }



        /// <summary>
        /// 读取一个字符串（strFather）中第几个（Index）指定字符（chrCompart）后紧跟的、并且是下一个chrCompart前的内容。Index从0开始。
        /// </summary>
        /// <param name="strFather"></param>
        /// <param name="chrCompart"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string ReadIndexWord(string strFather, string chrCompart, int Index)
        {
            int i = 0;
            string str1 = "";
            string str2 = "";

            str1 = strFather;
            if (Index < 0) return str1;
            for (i = 0; i <= Index; i++)
            {
                str2 = ReadWord(ref str1, chrCompart);
                if (str1 == "") break;
            }
            str2 = ReadWord(ref str1, chrCompart);
            return str2;
        }

        /// <summary>
        /// 生成SQL的where子句
        /// </summary>
        /// <param name="sOperator">操作符</param>
        /// <param name="sFieldName">字段名</param>
        /// <param name="sValue">查询值</param>
        /// <returns></returns>
        public static string CreateWhereClause(string[] sOperator, string[] sFieldName, string[] sValue, string or_and)
        {
            int i = 0;
            string str1 = "", str2 = "", str3 = "";

            if ((sOperator.Length != sFieldName.Length) | (sOperator.Length != sValue.Length)) return "";

            for (i = 0; i < sOperator.Length; i++)
            {
                if (sValue[i] != "")
                {
                    if (str1 == "")
                        str1 = str1 + " where ";
                    else
                        str1 = str1 + " " + or_and + " ";
                    str2 = "";
                    str3 = "";
                    switch (sOperator[i].Trim().ToLower())
                    {
                        case ">":
                        case ">=":
                        case "<":
                        case "<=":
                        case "=":
                        case "is":
                        case "":
                        case "<>":
                            str2 = " ";
                            str3 = " ";
                            break;
                        case ">'":
                        case ">='":
                        case "<'":
                        case "<='":
                        case "='":
                            str2 = "";
                            str3 = "' ";
                            break;
                        //case "like":
                        //	str2="'%";
                        //	str3="%'";
                        //	break;
                        default:
                            str2 = " '%";
                            str3 = "%' ";
                            break;
                    }
                    str1 = str1 + " (" + sFieldName[i] + " " + sOperator[i] + str2 + sValue[i] + str3 + ")";
                }
            }
            return str1;
        }

        public static string ReadWordOnly(string strFather, string strSpart, Boolean forwards, Boolean returnnothing)
        {
            //returnnothing指明当找不到strSpart时是返回空还是整个字符串
            string str1 = "";
            int i = 0;

            if (strSpart == "") str1 = "";
            i = strFather.IndexOf(strSpart, 0);
            if (i < 0)      //如果returnnothing=true ，那么返回空白字符串，否则返回整个字符串
            {
                if (returnnothing)
                    str1 = "";
                else
                    str1 = strFather;
            }
            else
            {
                if (forwards)
                    str1 = Left(strFather, i);
                else
                    str1 = Right(strFather, strFather.Length - i - strSpart.Length);
            }
            return str1;
        }

        public static string ReadLastWordOnly(string strFather, string strSpart, Boolean forwards, Boolean returnnothing)
        {
            //returnnothing指明当找不到strSpart时是返回空还是整个字符串
            string str1 = "";
            int i = 0;

            if (strSpart == "") str1 = "";
            i = strFather.LastIndexOf(strSpart);
            if (i < 0)      //如果returnnothing=true ，那么返回空白字符串，否则返回整个字符串
            {
                if (returnnothing)
                    str1 = "";
                else
                    str1 = strFather;
            }
            else
            {
                if (forwards)
                    str1 = Left(strFather, i);
                else
                    str1 = Right(strFather, strFather.Length - i - strSpart.Length);
            }
            return str1;
        }

    }
}
