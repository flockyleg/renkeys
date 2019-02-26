using System;
using System.Collections.Generic;
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
    public class main
    {
        #region 通用函数
        public static DataTable 获取一般数据集(string SQLstring)
        {
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
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

        public static int 执行SQL命令(string sMySqlCommand)
        {
            int i = 0;

            try
            {
                string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();
                MySqlConnection cnn = new MySqlConnection(数据库连接字符串);
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sMySqlCommand, cnn);
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

        public static DataTable 获取一般数据集(string cnnStr, string SQLstring)
        {
            try
            {
                MySqlConnection cnn = new MySqlConnection(cnnStr);
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
        public static int 执行SQL命令(string cnnStr, string sMySqlCommand)
        {
            int i = 0;

            try
            {
                MySqlConnection cnn = new MySqlConnection(cnnStr);
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand(sMySqlCommand, cnn);
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



        public static DataTable 获取一般数据集MSSQL(string SQLstring)
        {
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            try
            {
                SqlConnection cnn = new SqlConnection(数据库连接字符串);
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

        public static int 执行SQL命令MSSQL(string sMySqlCommand)
        {
            int i = 0;

            try
            {
                string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();
                SqlConnection cnn = new SqlConnection(数据库连接字符串);
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sMySqlCommand, cnn);
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

        public static DataTable 获取一般数据集MSSQL(string cnnStr, string SQLstring)
        {
            try
            {
                SqlConnection cnn = new SqlConnection(cnnStr);
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
        public static int 执行SQL命令MSSQL(string cnnStr, string sMySqlCommand)
        {
            int i = 0;

            try
            {
                SqlConnection cnn = new SqlConnection(cnnStr);
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sMySqlCommand, cnn);
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





        public enum 是否
        {
            是,
            否
        }
        public enum 性别
        {
            男,
            女
        }

        #endregion


        #region DataTable和List的转换
        public static DataTable ConvertTo<T>(IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {
                        // You can log something here  
                        throw;
                    }
                }
            }

            return obj;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
        #endregion

        /// <summary>
        /// 生成指定位数的验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateVerificationCode(int length)
        {
            string str1 = "";

            Random ran = new Random();
            //return ran.Next(1, 999999).ToString().PadLeft(length, '0');

            for (int i = 1; i <= length; i++)
            {
                str1 = str1 + ran.Next(0, 9).ToString();
            }
            return str1.PadRight(length, '0').Substring(0, length);
        }


        /// <summary>
        /// 从数据库获取数据，并以commData类集合形式返回
        /// </summary>
        /// <param name="sql">必须包含ID、Fld1等字段，例如select 用地 as Fld1 from Z用地 order by 频率 desc</param>
        /// <returns></returns>
        public static List<commData> GetCommdataFromDb(string sql)
        {
            int i = 0;
            List<commData> lst = new List<commData>();

            DataTable tb = 获取一般数据集(sql);
            for (i = 0; i < tb.Rows.Count; i++)
            {
                commData item = new commData();
                //try { item.ID = new Guid(tb.Rows[i]["ID"].ToString()); }
                //catch { }
                item.ID = tb.Rows[i]["ID"].ToString();
                item.Fld1 = tb.Rows[i]["Fld1"].ToString();
                item.Fld2 = tb.Rows[i]["Fld2"].ToString();
                item.Fld3 = tb.Rows[i]["Fld3"].ToString();
                item.Fld4 = tb.Rows[i]["Fld4"].ToString();
                item.Fld5 = tb.Rows[i]["Fld5"].ToString();
                item.Fld6 = tb.Rows[i]["Fld6"].ToString();
                item.Fld7 = tb.Rows[i]["Fld7"].ToString();
                item.Fld8 = tb.Rows[i]["Fld8"].ToString();
                item.Fld9 = tb.Rows[i]["Fld9"].ToString();
                item.Fld10 = tb.Rows[i]["Fld10"].ToString();

                lst.Add(item);
            }
            return lst;
        }


        public static void 申请人保存Cookie(string xinming, string zhengjianhao, string dizhi, string dianhua)
        {
            HttpCookie cookie = new HttpCookie("shenbanrenxinxi");
            cookie.Values["xinming"] = HttpUtility.UrlEncode(xinming, Encoding.GetEncoding("UTF-8")); //直接写中文可能乱码
            cookie.Values["zhengjianhao"] = HttpUtility.UrlEncode(zhengjianhao, Encoding.GetEncoding("UTF-8")); //直接写中文可能乱码      //(new 加解密()).EncryptString(UserPwd);
            cookie.Values["dizhi"] = HttpUtility.UrlEncode(dizhi, Encoding.GetEncoding("UTF-8"));
            cookie.Values["dianhua"] = HttpUtility.UrlEncode(dianhua, Encoding.GetEncoding("UTF-8"));
            cookie.Expires = DateTime.Now.AddDays(100);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string 申请人Cookie获取(string cookieKey)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["shenbanrenxinxi"];
            if (cookie.HasKeys)
            {
                try { return cookie.Values[cookieKey].ToString(); }
                catch { }
            }
            return "";
        }
        public static void 申请人删除Cookie()
        {
            HttpCookie cookie = null;
            cookie = HttpContext.Current.Request.Cookies["shenbanrenxinxi"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-10);
            }
        }


        public static bool ExamPhoneNumber(string PhoNum)   //判断手机号是否合法
        {
            string str1 = PhoNum;
            if (str1 == null) return false;
            str1 = str1.Replace(" ", "");
            if (str1.Length != 11) return false;

            return true;
        }
        public static string 短信格式(int 格式号, string 地区, string 签证类型,string 验证码)
        {
            string con1 = "【浙江长诚出入境】";
            string sdat = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            switch (格式号)
            {
                case 1:
                    return con1 + "您" + sdat + "申办" + 地区 + 签证类型 + "，验证码" + 验证码 + "，20分钟内有效，更多服务访问官网或公众号";
                case 2:
                    return con1 + "您" + sdat + "申办" + 地区 + 签证类型 + "已成功接受，更多服务访问官网或公众号";
            }
            //return con1 + "感谢您的访问，请访问我们的官网或公众号获取更多服务。";
            return "";
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
        /// 从文件中获取指定项目的值
        /// </summary>
        /// <param name="sFileName">文件</param>
        /// <param name="sCollectionName">分类</param>
        /// <param name="ItemName">分类中的项目</param>
        /// <param name="DefaultValue">找不到时的返回值</param>
        /// <returns></returns>
        public static string GetItemFromFile(string sFileName, string sCollectionName, string sItemName, string sDefaultValue)
        {
            string str1 = "";
            char[] char1 = new char[1] { '=' };

            if ((sFileName == "") || (sCollectionName == "") || (sItemName == ""))
            {
                throw new Exception("错误的参数。");
                //return sDefaultValue;
            }
            try
            {
                using (StreamReader sr = new StreamReader(sFileName, System.Text.Encoding.Default))
                {
                    string sline;
                    while ((sline = sr.ReadLine()) != null)
                    {
                        if (sline == "") continue;
                        if ((sline.Substring(0, 1) == "[") && (sline.Substring(sline.Length - 1, 1) == "]"))
                        {
                            str1 = sline;
                        }
                        else
                        {
                            if (str1 == ("[" + sCollectionName + "]"))
                            {
                                string[] ss = null;
                                ss = sline.Split(char1, 2);
                                if (ss[0] == sItemName)
                                {
                                    return ss[1];
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                str1 = e.Message;
                return sDefaultValue;
            }
            return sDefaultValue;
        }


        /// <summary>
        /// 从文件中获取指定分类的所有项目及项目对应的值。
        /// 注意返回的是个二维数组
        /// </summary>
        /// <param name="sFileName">文件</param>
        /// <param name="sCollectionName">分类</param>
        /// <returns></returns>
        public static string[,] GetItemFromFile(string sFileName, string sCollectionName)
        {
            string str1 = "";
            char[] char1 = new char[1] { '=' };
            ArrayList arrReturn1 = new ArrayList();
            ArrayList arrReturn2 = new ArrayList();
            int i = 0;

            if ((sFileName == "") | (sCollectionName == ""))
            {
                throw new Exception("错误的参数。");
                //return sDefaultValue;
            }
            try
            {
                using (StreamReader sr = new StreamReader(sFileName, System.Text.Encoding.Default))
                {
                    string sline;
                    while ((sline = sr.ReadLine()) != null)
                    {
                        if (sline == "") continue;
                        if ((sline.Substring(0, 1) == "[") && (sline.Substring(sline.Length - 1, 1) == "]"))
                        {
                            str1 = sline;
                        }
                        else
                        {
                            if (str1 == ("[" + sCollectionName + "]"))
                            {
                                string[] ss = null;
                                ss = sline.Split(char1, 2);
                                if (ss[0] != "")
                                {
                                    arrReturn1.Add(ss[0]);
                                    arrReturn2.Add(ss[1]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                str1 = e.Message;
                return null;
            }
            if (arrReturn1.Count > 0)
            {
                string[,] sreturn = new string[2, arrReturn1.Count];
                for (i = 0; i < arrReturn1.Count; i++)
                {
                    sreturn[0, i] = (string)arrReturn1[i];
                    sreturn[1, i] = (string)arrReturn2[i];
                }
                return sreturn;
            }
            else
                return null;
        }


        /// <summary>
        /// 设置文件sFileName中sCollectionName类sItemName项目的值为sNewValue
        /// 当没有该项目时，如果bCreatIfNotExist为真，则创建该项目并赋值为sNewValue
        /// </summary>
        /// <param name="sFileName">文件名</param>
        /// <param name="sCollectionName">类别</param>
        /// <param name="sItemName">项目</param>
        /// <param name="sNewValue">新的值</param>
        /// <param name="bCreatIfNotExist">没有找到该项目时是否创建</param>
        /// <returns>返回值为0则正确，非0有错误</returns>
        public static int SetItemToFile(string sFileName, string sCollectionName, string sItemName, string sNewValue, bool bCreatIfNotExist)
        {
            string str1 = "", str2 = "", str3 = "", str4 = "";
            char[] char1 = new char[1] { '=' };
            bool bol1 = false, bol2 = false;
            string sCrlf = "\r\n";
            char[] chrs = new char[3] { '\\', 'n', 'r' };
            int i = 0;

            if ((sFileName == "") || (sCollectionName == "") || (sItemName == ""))
            {
                throw new Exception("错误的参数。");
                //return -1;
            }
            try
            {
                using (StreamReader sr = new StreamReader(sFileName, System.Text.Encoding.Default))
                {
                    string sline;
                    while ((sline = sr.ReadLine()) != null)
                    {
                        if (str2 != "") str2 = str2 + sCrlf;
                        if (sline == "")
                        {
                            str2 = str2 + sline;
                            continue;
                        }
                        if ((sline.Substring(0, 1) == "[") && (sline.Substring(sline.Length - 1, 1) == "]"))
                        {
                            if ((bol1) && (!bol2))
                            {
                                if (bCreatIfNotExist)
                                {
                                    while ((str2.Length > sCrlf.Length) && (str2.Substring(str2.Length - sCrlf.Length, sCrlf.Length) == sCrlf))
                                    {
                                        str2 = str2.Substring(0, str2.Length - sCrlf.Length);
                                        i++;
                                        if (i > 1) str4 = str4 + sCrlf;
                                    }
                                    str2 = str2 + sCrlf + sItemName + "=" + sNewValue + str4 + sCrlf;
                                }
                            }
                            str1 = sline;
                            str2 = str2 + sline;
                        }
                        else
                        {
                            str3 = sline;
                            if (str1 == ("[" + sCollectionName + "]"))
                            {
                                bol1 = true;
                                string[] ss = null;
                                ss = sline.Split(char1, 2);
                                if (ss[0] == sItemName)
                                {
                                    str3 = ss[0] + "=" + sNewValue;
                                    bol2 = true;
                                }
                            }
                            str2 = str2 + str3;
                        }
                    }
                }
                if ((!bol1) && (!bol2))
                {
                    if (bCreatIfNotExist)
                    {
                        str2 = str2 + sCrlf + sCrlf + "[" + sCollectionName + "]";
                        str2 = str2 + sCrlf + sItemName + "=" + sNewValue;
                    }
                }

                using (StreamWriter sw = new StreamWriter(sFileName))
                {
                    sw.Write(str2);
                }
            }
            catch (Exception e)
            {
                str1 = e.Message;
                return -1;
            }
            return 0;
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
        /// 例如 In="|1你|2我|3他|"  拆分符="|"  组合符="、"  那么返回的是"1你、2我、3他"  MaxLength是LeftB(返回值)  如果 排序=-1 则返回"3他、2我、1你"
        /// </summary>
        /// <param name="In"></param>
        /// <param name="拆分符"></param>
        /// <param name="MaxLength">返回字符串的最大字节数</param>
        /// <param name="排序">-1倒序  0不排序 1顺序</param>
        /// /// <param name="format">格式化，format="～"表示格式化为"2007000001～2007000004、2007000006"</param>
        /// <returns></returns>
        public static string 拆分和组合A(string In, string 拆分符, string 组合符, int MaxLength, int 排序, string format)
        {
            if (In == "") return In;
            if (组合符 == "") return In;
            if (MaxLength <= 0) return "";
            int i = 0;
            int j = 0;

            string[] separator = new string[1];
            separator[0] = 拆分符;
            string[] sreturn = In.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if ((排序 == -1) || (排序 == 1))
            {
                for (i = 0; i < (sreturn.Length - 1); i++)
                {
                    string curent = sreturn[i];
                    for (j = (i + 1); j < sreturn.Length; j++)
                    {
                        if (排序 == -1)
                        {
                            if (sreturn[j].CompareTo(curent) > 0)
                            {
                                sreturn[i] = sreturn[j];
                                sreturn[j] = curent;
                                curent = sreturn[i];
                            }
                        }
                        else
                        {
                            if (sreturn[j].CompareTo(curent) < 0)
                            {
                                sreturn[i] = sreturn[j];
                                sreturn[j] = curent;
                                curent = sreturn[i];
                            }
                        }
                    }
                }
            }
            if (sreturn.Length == 0) return "";

            string sOut = "";
            string tmp = "";

            if (format == "～")
            {
                long long1 = ValLng(sreturn[0]);
                long long2 = long1;
                tmp = long1.ToString();
                if (System.Text.Encoding.Default.GetByteCount(tmp) >= MaxLength) return "";
                if (sreturn.Length < 2) return tmp;
                for (i = 1; i < sreturn.Length; i++)
                {
                    if (System.Math.Abs(ValLng(sreturn[i]) - long2) == 1)
                    {
                        long2 = ValLng(sreturn[i]);
                    }
                    else
                    {
                        if (long2 != long1) tmp = tmp + "～" + long2.ToString();
                        if (System.Text.Encoding.Default.GetByteCount(tmp) >= MaxLength) return tmp;
                        long1 = ValLng(sreturn[i]);
                        long2 = long1;
                        tmp = tmp + 组合符 + long1.ToString();
                        if (System.Text.Encoding.Default.GetByteCount(tmp) >= MaxLength) return tmp;
                    }
                }
                if (sreturn.Length > 0)
                {
                    if ((long2 == ValLng(sreturn[sreturn.Length - 1])) && (long2 != long1))
                    {
                        tmp = tmp + "～" + long2.ToString();
                        if (System.Text.Encoding.Default.GetByteCount(tmp) >= MaxLength) return tmp;
                    }
                }
                if (Right(tmp, 组合符.Length) == 组合符) tmp = Left(tmp, tmp.Length - 组合符.Length);
                return tmp;
            }

            //Regex digitregex = new Regex("[^\x00-\xff]/g");
            //Regex digitregex = new Regex("[\u4e00-\u9fa5]/g");            
            for (i = 0; i < sreturn.Length; i++)
            {
                tmp = tmp + sreturn[i];
                if (i < (sreturn.Length - 1)) tmp = tmp + 组合符;
                //if(digitregex.Replace(tmp,"aa").Length>=MaxLength) break;
                if (System.Text.Encoding.Default.GetByteCount(tmp) >= MaxLength) break;
                sOut = tmp;
            }
            return sOut;
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
                        case "in":
                            str2 = " ";
                            str3 = " ";
                            break;
                        case ">'":
                        case ">='":
                        case "<'":
                        case "<='":
                        case "='":
                        case "<>'":
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

        /// <summary>
        /// 将小写的人民币转化为大写
        /// </summary>
        /// <param name="sLCaseIn">小写的人民币</param>
        /// <param name="Format">要转化成的格式</param>
        /// <returns></returns>
        public static string 货币转化为汉字(string sLCaseIn, string Format)
        {
            string str1 = "", str2 = "", str3 = "";
            int i = 0, k = 0, m = 0;
            //string[] 权= new string[] {"万","仟","佰","拾","亿","仟","佰","拾","万","仟","佰","拾","元","","角","分","厘","毫"};
            //string[] 权= new string[] {"毫","厘","分","角","","元","拾","佰","仟","万","拾","佰","仟","亿","拾","佰","仟","万"};
            const string 权 = "万仟佰拾亿仟佰拾万仟佰拾元.角分厘毫";
            string[] 数值 = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };

            str2 = Format.Replace(",", "");
            str2 = str2.Replace(" ", "");
            k = str2.IndexOf('.') + 1;
            if (k > 0)
            {
                if (k < (str2.Length - 4)) str2 = str2.Substring(0, k + 4);    //最多精确到毫
            }

            str3 = 权;
            m = str3.IndexOf('.') + 1;
            if (k > 0)
            {
                if ((str3.Length - m) > (str2.Length - k)) str3 = str3.Substring(0, m + str2.Length - k);
            }
            else
            {
                str3 = ReadWord(ref str3, ".");
            }

            str1 = sLCaseIn.Replace(",", "");
            str1 = str1.Replace(" ", "");
            if (str1.Substring(0, 1) == ".") str1 = "0" + str1;
            i = str1.IndexOf('.') + 1;
            if (i > 0)
            {
                if (i < (str1.Length - 4)) str1 = str1.Substring(0, i + 4);
            }
            else
            {
                if (k > 0)
                {
                    str1 = str1 + ".";
                    str1 = str1.PadRight(str1.Length + str2.Length - k, '0');
                }
            }
            i = str1.IndexOf('.') + 1;
            if (k > 0)
            {
                if ((str1.Length - i) > (str2.Length - k))
                    str1 = str1.Substring(0, i + str2.Length - k);
                else
                    str1 = str1.PadRight(i + str2.Length - k, '0');
            }
            else
            {
                str1 = ReadWord(ref str1, ".");
            }

            str1 = str1.Replace(".", "");
            str3 = str3.Replace(".", "");
            if (str1.Length > str3.Length) return "";
            str2 = "";
            k = str3.Length;
            for (i = str1.Length; i > 0; i--)
            {
                k--;
                str2 = 数值[int.Parse(str1.Substring(i - 1, 1))] + str3.Substring(k, 1) + str2;
            }

            return str2;
        }

        public static string 日期转化为汉字(string sIn)
        {
            if (sIn == "") return "";
            if ((sIn.IndexOf("0") < 0) && (sIn.IndexOf("1") < 0) && (sIn.IndexOf("2") < 0) && (sIn.IndexOf("3") < 0) && (sIn.IndexOf("4") < 0) && (sIn.IndexOf("5") < 0) && (sIn.IndexOf("6") < 0) && (sIn.IndexOf("7") < 0) && (sIn.IndexOf("8") < 0) && (sIn.IndexOf("9") < 0)) return sIn;

            string str1 = sIn;
            str1 = str1.Replace(@"/", @"-");
            string[] s = str1.Split(new string[1] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            switch (s.Length)
            {
                case 3:
                    str1 = s[0] + "年" + ValInt(s[1]).ToString() + "月" + ValInt(s[2]).ToString() + "日";
                    break;
                case 2:
                    str1 = s[0] + "年" + ValInt(s[1]).ToString() + "月";
                    break;
                case 1:
                    str1 = s[0] + "年";
                    break;
            }
            string str2 = "";
            if (str1.IndexOf("年") >= 0)
            {
                str2 = ReadWord(ref str1, "年");
                str2 = str2.Replace("0", "○").Replace("1", "一").Replace("2", "二").Replace("3", "三").Replace("4", "四").Replace("5", "五").Replace("6", "六").Replace("7", "七").Replace("8", "八").Replace("9", "九");
            }

            str1 = str1.Replace("10", "十").Replace("11", "十一").Replace("12", "十二").Replace("13", "十三").Replace("14", "十四").Replace("15", "十五").Replace("16", "十六").Replace("17", "十七").Replace("18", "十八").Replace("19", "十九");
            str1 = str1.Replace("20", "二十").Replace("21", "二十一").Replace("22", "二十二").Replace("23", "二十三").Replace("24", "二十四").Replace("25", "二十五").Replace("26", "二十六").Replace("27", "二十七").Replace("28", "二十八").Replace("29", "二十九");
            str1 = str1.Replace("30", "三十").Replace("31", "三十一");
            str1 = str1.Replace("0", "○").Replace("1", "一").Replace("2", "二").Replace("3", "三").Replace("4", "四").Replace("5", "五").Replace("6", "六").Replace("7", "七").Replace("8", "八").Replace("9", "九");

            if (str2 != "") str1 = str2 + "年" + str1;
            return str1;
        }

        public static string 日期转中写(string sIn)
        {
            if (sIn == "") return "";
            if ((sIn.IndexOf("0") < 0) && (sIn.IndexOf("1") < 0) && (sIn.IndexOf("2") < 0) && (sIn.IndexOf("3") < 0) && (sIn.IndexOf("4") < 0) && (sIn.IndexOf("5") < 0) && (sIn.IndexOf("6") < 0) && (sIn.IndexOf("7") < 0) && (sIn.IndexOf("8") < 0) && (sIn.IndexOf("9") < 0)) return sIn;

            string str1 = sIn;
            str1 = str1.Replace(@"/", @"-");
            string[] s = str1.Split(new string[1] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            switch (s.Length)
            {
                case 3:
                    str1 = s[0] + "年" + ValInt(s[1]).ToString() + "月" + ValInt(s[2]).ToString() + "日";
                    break;
                case 2:
                    str1 = s[0] + "年" + ValInt(s[1]).ToString() + "月";
                    break;
                case 1:
                    str1 = s[0] + "年";
                    break;
            }
            return str1;
        }

        /// <summary>
        /// 消除字符串里面的重复项,例如:张三/王五/张三,返回张三/王五
        /// </summary>
        /// <returns></returns>
        public static string 消除重复项(string strIn, string 分隔字符)
        {
            string str1 = strIn;
            string str2 = "";
            string sreturn = "";
            while (str1 != "")
            {
                str2 = ReadWord(ref str1, 分隔字符);
                if ((分隔字符 + sreturn + 分隔字符).IndexOf(分隔字符 + str2 + 分隔字符) < 0)
                {
                    sreturn = sreturn + str2 + 分隔字符;
                }
            }
            return sreturn;
        }
        public static string 转化为合法文件名(string 文件名)   //文件名 不包含后缀
        {
            return 文件名.Replace(@"/", "").Replace(@"\", "").Replace(@":", "").Replace(@"*", "").Replace(@"?", "").Replace(@"""", "").Replace(@"<", "").Replace(@">", "").Replace(@"|", "");
        }


        /// <summary>
        /// 例如：001-03增加一个整数-2后，返回001-01
        /// </summary>
        /// <param name="sFather"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static string 字符型数字增加某整数(string sFather, int step)
        {
            return 字符型数字增加某整数(sFather, step, true, Char.MaxValue);
        }
        /// <summary>
        /// 把一个字符串加上一个数字。例如：001-03增加一个整数-2后，返回001-01
        /// </summary>
        /// <param name="sFather">被加的字符串</param>
        /// <param name="step">要加的数字</param>
        /// <param name="变长截取">如果加上去后，长度比sFather长，则是否截取掉加上的数字的进位</param>
        /// <param name="变短左填字符">如果加上去后，长度比sFather短，则用该字符padleft填写。Char.MaxValue时表示不填写</param>
        /// <returns></returns>
        public static string 字符型数字增加某整数(string sFather, int step, bool 变长截取, char 变短左填字符)
        {
            int i = 0;
            int k = -1;
            string str1 = "";
            string str2 = "";

            try
            {
                for (i = sFather.Length - 1; i >= 0; i--)
                {
                    str2 = sFather.Substring(i, 1);
                    if ("0123456789".IndexOf(str2) >= 0)
                    {
                        str1 = str2 + str1;
                    }
                    else
                    {
                        k = i;
                        break;
                    }
                }
                string str3 = (ValLng(str1) + step).ToString();
                if ((str3.Length > str1.Length) && 变长截取) str3 = Right(str3, str1.Length);
                if ((str3.Length < str1.Length) && 变短左填字符 != Char.MaxValue) str3 = str3.PadLeft(str1.Length, 变短左填字符);
                if (k > -1)
                    return Left(sFather, k + 1) + str3;
                else
                    return str3;
            }
            catch
            {
                return sFather;
            }
        }


        private static bool ThumbnailCallback()
        {
            return false;
        }

        public static string[] 一维string数组排序(string[] 数组, bool 倒序)
        {
            string[] strIn = null;

            strIn = (string[])(数组.Clone());
            if (strIn == null) return null;

            for (int i = 0; i < strIn.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = strIn.Length - 1; j > i; j--)
                {
                    if (!倒序)
                    {
                        if (string.Compare(strIn[j], strIn[minIndex], false) < 0)
                            minIndex = j;
                    }
                    else
                    {
                        if (string.Compare(strIn[j], strIn[minIndex], false) > 0)
                            minIndex = j;
                    }
                }
                string str1 = strIn[i];
                strIn[i] = strIn[minIndex];
                strIn[minIndex] = str1;
            }
            return strIn;

            //ArrayList a = new ArrayList(数组);
            //a.Sort();
        }

        public static int[] 一维int数组排序(int[] 数组, bool 倒序)
        {
            int[] strIn = null;

            strIn = (int[])(数组.Clone());
            if (strIn == null) return null;

            for (int i = 0; i < strIn.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = strIn.Length - 1; j > i; j--)
                {
                    if (!倒序)
                    {
                        if (strIn[j] < strIn[minIndex])
                            minIndex = j;
                    }
                    else
                    {
                        if (strIn[j] > strIn[minIndex])
                            minIndex = j;
                    }
                }
                int str1 = strIn[i];
                strIn[i] = strIn[minIndex];
                strIn[minIndex] = str1;
            }
            return strIn;

            //ArrayList a = new ArrayList(数组);
            //a.Sort();
        }

        public static long[] 一维long数组排序(long[] 数组, bool 倒序)
        {
            long[] strIn = null;

            strIn = (long[])(数组.Clone());
            if (strIn == null) return null;

            for (int i = 0; i < strIn.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = strIn.Length - 1; j > i; j--)
                {
                    if (!倒序)
                    {
                        if (strIn[j] < strIn[minIndex])
                            minIndex = j;
                    }
                    else
                    {
                        if (strIn[j] > strIn[minIndex])
                            minIndex = j;
                    }
                }
                long str1 = strIn[i];
                strIn[i] = strIn[minIndex];
                strIn[minIndex] = str1;
            }
            return strIn;

            //ArrayList a = new ArrayList(数组);
            //a.Sort();
        }

        public static decimal[] 一维decimal数组排序(decimal[] 数组, bool 倒序)
        {
            decimal[] strIn = null;

            strIn = (decimal[])(数组.Clone());
            if (strIn == null) return null;

            for (int i = 0; i < strIn.Length - 1; i++)
            {
                int minIndex = i;
                for (int j = strIn.Length - 1; j > i; j--)
                {
                    if (!倒序)
                    {
                        if (strIn[j] < strIn[minIndex])
                            minIndex = j;
                    }
                    else
                    {
                        if (strIn[j] > strIn[minIndex])
                            minIndex = j;
                    }
                }
                decimal str1 = strIn[i];
                strIn[i] = strIn[minIndex];
                strIn[minIndex] = str1;
            }
            return strIn;

            //ArrayList a = new ArrayList(数组);
            //a.Sort();
        }

        public static void 二维string数组2位置调换(ref string[,] 数组, int 位置1, int 位置2)
        {
            int i = 0;
            string str1 = "";

            if (位置1 == 位置2) return;
            if (数组 == null) return;
            if (数组.Length < 1) return;

            for (i = 0; i <= 数组.GetUpperBound(1); i++)
            {
                str1 = 数组[位置1, i];
                数组[位置1, i] = 数组[位置2, i];
                数组[位置2, i] = str1;
            }
        }


        #region 二维数组排序，直接网络上拷贝而来
        //调用办法
        //    object[,] o = new object[6, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 11, 12 }, { 15, 16, 11, 17, }, { 5, 6, 7, 9 } };

        //    Console.WriteLine("没排序前的二维数组：");
        //    Print(o);

        //    Console.WriteLine("根据第3,4列升序排序后的数组：");
        //    Order.二维数组排序(o, new int[] { 2, 3 }, 0);
        //    Print(o);

        //    Console.WriteLine("根据第3,4列降序序排序后的数组：");
        //    Order.二维数组排序(o, new int[] { 2, 3 }, 1);
        //    Print(o);

        //    Console.Read();

        /// <summary>
        /// 对二维数组排序
        /// </summary>
        /// <param name="values">排序的二维数组</param>
        /// <param name="orderColumnsIndexs">排序根据的列的索引号数组</param>
        /// <param name="type">排序的类型，1代表降序，0代表升序</param>
        /// <returns>返回排序后的二维数组</returns>
        public static object[,] 二维数组排序(object[,] values, int[] orderColumnsIndexs, int type)
        {
            object[] temp = new object[values.GetLength(1)];
            int k;
            int compareResult;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (k = i + 1; k < values.GetLength(0); k++)
                {
                    if (type.Equals(1))
                    {
                        for (int h = 0; h < orderColumnsIndexs.Length; h++)
                        {
                            compareResult = Comparer.Default.Compare(GetRowByID(values, k).GetValue(orderColumnsIndexs[h]), GetRowByID(values, i).GetValue(orderColumnsIndexs[h]));
                            if (compareResult.Equals(1))
                            {
                                temp = GetRowByID(values, i);
                                Array.Copy(values, k * values.GetLength(1), values, i * values.GetLength(1), values.GetLength(1));
                                CopyToRow(values, k, temp);
                            }
                            if (compareResult != 0)
                                break;
                        }
                    }
                    else
                    {
                        for (int h = 0; h < orderColumnsIndexs.Length; h++)
                        {
                            compareResult = Comparer.Default.Compare(GetRowByID(values, k).GetValue(orderColumnsIndexs[h]), GetRowByID(values, i).GetValue(orderColumnsIndexs[h]));
                            if (compareResult.Equals(-1))
                            {
                                temp = GetRowByID(values, i);
                                Array.Copy(values, k * values.GetLength(1), values, i * values.GetLength(1), values.GetLength(1));
                                CopyToRow(values, k, temp);
                            }
                            if (compareResult != 0)
                                break;
                        }
                    }
                }
            }
            return values;
        }
        /// <summary>
        /// 获取二维数组中一行的数据
        /// </summary>
        /// <param name="values">二维数据</param>
        /// <param name="rowID">行ID</param>
        /// <returns>返回一行的数据</returns>
        private static object[] GetRowByID(object[,] values, int rowID)
        {
            if (rowID > (values.GetLength(0) - 1))
                throw new Exception("rowID超出最大的行索引号");

            object[] row = new object[values.GetLength(1)];
            for (int i = 0; i < values.GetLength(1); i++)
            {
                row[i] = values[rowID, i];
            }
            return row;
        }
        /// <summary>
        /// 复制一行数据到二维数组指定的行上
        /// </summary>
        /// <param name="values"></param>
        /// <param name="rowID"></param>
        /// <param name="row"></param>
        private static void CopyToRow(object[,] values, int rowID, object[] row)
        {
            if (rowID > (values.GetLength(0) - 1))
                throw new Exception("rowID超出最大的行索引号");
            if (row.Length > (values.GetLength(1)))
                throw new Exception("row行数据列数超过二维数组的列数");
            for (int i = 0; i < row.Length; i++)
            {
                values[rowID, i] = row[i];
            }
        }

        #endregion

        /// <summary>
        /// 强制转化为日期样式的字符（例2011-12-03）。不包含时间
        /// </summary>
        /// <param name="sIn"></param>
        /// <returns></returns>
        public static string valDateString(string sIn)
        {
            string str1 = "";
            str1 = sIn.Replace("年", "-").Replace("月", "-").Replace("日", "");
            str1 = ReadWordOnly(str1, "星", true, false);
            try
            {
                str1 = DateTime.Parse(str1).ToString("yyyy-MM-dd");
            }
            catch
            {
                str1 = "";
            }
            return str1;
        }

        /// <summary>
        /// 例如：12。３转化为12.3
        /// </summary>
        /// <param name="sIn"></param>
        /// <returns></returns>
        public static string 中文数字转英文数字(string sIn)
        {
            if (sIn == "") return "";
            string str1 = sIn;
            str1 = str1.Replace("０", "0").Replace("１", "1").Replace("２", "2").Replace("３", "3").Replace("４", "4").Replace("５", "5").Replace("６", "6").Replace("７", "7").Replace("８", "8").Replace("９", "9");
            str1 = str1.Replace("。", ".");
            return str1;
        }

        public static string 把字符格式化为指定的字节长度(string sIn, int 指定长度, string 填充字符, bool 左填充)
        {
            int i = 0;
            string str1 = sIn;
            while (System.Text.Encoding.Default.GetByteCount(str1) < 指定长度)
            {
                i++;
                if (i > 1000) break;

                if (左填充)
                    str1 = 填充字符 + str1;
                else
                    str1 = str1 + 填充字符;
            }
            i = 0;
            while (System.Text.Encoding.Default.GetByteCount(str1) > 指定长度)
            {
                i++;
                if (i > 1000) break;

                str1 = str1.Substring(0, str1.Length - 1);
            }
            return str1;
        }

        /// <summary>
        /// 判断sIn是否日期，如果不是，返回空字符串；如果是，继续判断是不是数据库设置的缺省值，如果是，返回空字符串，如果不是，返回sIn
        /// </summary>
        /// <param name="sIn"></param>
        /// <param name="判断类型">1：只判断是否<=1910-03-14；2：只判断是否>=2100-01-01；其他值：同时判断1和2的条件</param>
        /// <returns></returns>
        public static string 日期是否是缺省值(string sIn, int 判断类型)
        {
            DateTime dat1 = DateTime.MinValue;
            try { dat1 = DateTime.Parse(sIn); }
            catch { }
            if (dat1 == DateTime.MinValue) return "";
            if (判断类型 == 1)
            {
                if (dat1 <= DateTime.Parse("1910-01-01"))
                    return "";
                else
                    return sIn;
            }
            if (判断类型 == 2)
            {
                if (dat1 >= DateTime.Parse("2100-01-01"))
                    return "";
                else
                    return sIn;
            }

            if (dat1 <= DateTime.Parse("1910-01-01")) return "";
            if (dat1 >= DateTime.Parse("2100-01-01")) return "";
            return sIn;
        }

        public static Random ran = new Random();
        /// <summary>
        /// 随机生成指定长度的密码
        /// </summary>
        /// <param name="密码长度">密码长度</param>
        /// <param name="类型">0：包含字符和数字；1：只字符；2：只数字</param>
        /// <returns></returns>
        public static string 生成密码(int 密码长度, int 类型)
        {
            string str1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string str2 = "0123456789";
            string str3 = "";
            if (密码长度 < 1) return "";
            switch (类型)
            {
                case 1:
                    str3 = str1;
                    break;
                case 2:
                    str3 = str2;
                    break;
                default:
                    str3 = str1 + str2;
                    break;
            }
            str1 = "";
            for (int i = 0; i < 密码长度; i++)
            {
                str1 += str3.Substring(ran.Next(0, str3.Length), 1);
            }
            return str1;
        }

        /// <summary>
        /// 把类似20120419或者120419的值转换为2012-04-19
        /// </summary>
        /// <param name="In"></param>
        /// <returns></returns>
        public static string 日期值转换(string sIn)
        {
            string str1 = "";
            string strOld = "";
            if (sIn == "") return sIn;
            strOld = ValLng(sIn).ToString().PadLeft(sIn.Length, '0');
            if (strOld != sIn) return sIn;   //如果不全是数字

            if (strOld.Length == 8)   //如果输入类似20100912的格式，则认为是2010-09-12
            {
                try
                {
                    str1 = strOld.Substring(0, 4) + "-" + strOld.Substring(4, 2) + "-" + strOld.Substring(6);
                    strOld = (DateTime.Parse(str1)).ToString("yyyy-MM-dd");
                }
                catch { }
                return strOld;
            }
            if (strOld.Length == 6)   //如果输入类似100912的格式，则认为是2010-09-12；如果输入类似200912的格式，则认为是1920-09-12
            {
                try
                {
                    str1 = strOld.Substring(0, 2) + "-" + strOld.Substring(2, 2) + "-" + strOld.Substring(4);
                    if (DateTime.Parse("20" + str1) <= DateTime.Now)
                        str1 = "20" + str1;
                    else
                        str1 = "19" + str1;
                    strOld = (DateTime.Parse(str1)).ToString("yyyy-MM-dd");
                }
                catch { }
                return strOld;
            }
            return strOld;
        }


        /// <summary>
        /// 产生短GUID，字符串：（例：49f949d735f5c79e）
        /// </summary>
        /// <returns></returns>
        public static string 短Guid_String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 产生短GUID，Int64 类型：（例：4833055965497820814）
        /// </summary>
        /// <returns></returns>
        public static long 短Guid_Long()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 判断sIn首字母是否英文字母
        /// </summary>
        /// <param name="sIn"></param>
        /// <returns></returns>
        public static bool 是否英文(string sIn)
        {
            string str1 = "";
            str1 = Left(sIn, 1);
            if (str1 == "") return true;
            if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(str1) > -1) return true;
            return false;
        }

        public static string 获取类的所有属性(object obj)
        {
            string str1 = "";
            Type tp = obj.GetType();
            foreach (PropertyInfo pi in tp.GetProperties())
            {
                if ((pi.Name != "初始记录") && (pi.Name != "记录改变") && (Left(pi.Name, 3) != "管理_")) str1 = str1 + pi.Name + ",";
            }
            if (str1.EndsWith(",")) str1 = CommonFunctions.Left(str1, str1.Length - 1);
            return str1;
        }
        public static object 获取类属性的值(object obj, string 属性)
        {
            Type tp = obj.GetType();
            foreach (PropertyInfo pi in tp.GetProperties())
            {
                if (pi.Name == 属性)
                {
                    return pi.GetValue(obj, null);
                }
            }
            return null;
        }
        public static bool 设置类属性的值(object obj, string 属性, object value)
        {
            string str1 = "";
            Type tp = obj.GetType();
            foreach (PropertyInfo pi in tp.GetProperties())
            {
                if (pi.Name == 属性)
                {
                    try
                    {
                        str1 = pi.PropertyType.Name.ToString().ToLower();
                        if (str1.IndexOf("datetime") > -1)
                            pi.SetValue(obj, DateTime.Parse(value.ToString()), null);
                        else
                        {
                            if (str1.IndexOf("int") > -1)
                            {
                                pi.SetValue(obj, CommonFunctions.ValInt(value.ToString()), null);
                            }
                            else
                            {
                                if ((str1.IndexOf("decimal") > -1) | (str1.IndexOf("numeric") > -1) | (str1.IndexOf("money") > -1))
                                {
                                    pi.SetValue(obj, CommonFunctions.ValDec(value.ToString()), null);
                                }
                                else
                                {
                                    if ((str1.IndexOf("long") > -1) | (str1.IndexOf("float") > -1) | (str1.IndexOf("real") > -1))
                                    {
                                        pi.SetValue(obj, CommonFunctions.ValLng(value.ToString()), null);
                                    }
                                    else
                                    {
                                        if (str1.IndexOf("bit") > -1)
                                        {
                                            if ((value.ToString().ToLower() == "true") | (value.ToString().ToLower().IndexOf("1") > -1))
                                                pi.SetValue(obj, true, null);
                                            else
                                                pi.SetValue(obj, false, null);
                                        }
                                        else
                                        {
                                            pi.SetValue(obj, value.ToString(), null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string strErr = ex.Message;
                    }
                    return true;
                }
            }
            return false;
        }

        public static void 复制文件夹(string SourceDir, string TargetDir)
        {
            int i = 0;
            string str1 = "";

            string[] files = Directory.GetFiles(SourceDir);
            for (i = 0; i < files.Length; i++)
            {
                File.Copy(files[i], Path.Combine(TargetDir, Path.GetFileName(files[i])), true);
            }

            string[] dirs = Directory.GetDirectories(SourceDir);
            for (i = 0; i < dirs.Length; i++)
            {
                str1 = ReadLastWordOnly(dirs[i], @"\", false, true);
                str1 = Path.Combine(TargetDir, str1);
                if (!Directory.Exists(str1)) Directory.CreateDirectory(str1);
                复制文件夹(dirs[i], str1);
            }
        }


        public static void 写txt文件(string txtFile, string writeString)
        {
            try
            {
                FileStream fs = new FileStream(txtFile, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write(writeString);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch { }
        }
    }





    /// <summary>
    /// 通用数据
    /// </summary>
    public class commData
    {
        #region 属性
        public string ID { get; set; }

        public string Fld1 { get; set; }
        public string Fld2 { get; set; }
        public string Fld3 { get; set; }
        public string Fld4 { get; set; }
        public string Fld5 { get; set; }
        public string Fld6 { get; set; }
        public string Fld7 { get; set; }
        public string Fld8 { get; set; }
        public string Fld9 { get; set; }
        public string Fld10 { get; set; }

        #endregion
    }

    public class cls办理过程
    {
        #region 属性
        public string ID { get; set; }
        private string tmp_相关ID = "";
        public string 相关ID
        {
            get
            {
                return tmp_相关ID;
            }
            set
            {
                tmp_相关ID = value;
            }
        }

        private string tmp_相关业务 = "";
        public string 相关业务
        {
            get
            {
                return tmp_相关业务;
            }
            set
            {
                tmp_相关业务 = value;
            }
        }

        private string tmp_处理人 = "";
        public string 处理人
        {
            get
            {
                return tmp_处理人;
            }
            set
            {
                tmp_处理人 = value;
            }
        }

        private DateTime tmp_处理日期 = DateTime.Parse("1900-01-01");
        public DateTime 处理日期
        {
            get
            {
                return tmp_处理日期;
            }
            set
            {
                tmp_处理日期 = value;
            }
        }

        private string tmp_处理结果 = "";
        public string 处理结果
        {
            get
            {
                return tmp_处理结果;
            }
            set
            {
                tmp_处理结果 = value;
            }
        }

        private string tmp_处理意见 = "";
        public string 处理意见
        {
            get
            {
                return tmp_处理意见;
            }
            set
            {
                tmp_处理意见 = value;
            }
        }
        #endregion
    }



    #region 预算

    public class cls预算单
    {
        #region 属性

        public string 管理_1 { get; set; }
        public string 管理_2 { get; set; }
        public string 管理_3 { get; set; }
        public string 管理_4 { get; set; }
        public string 管理_5 { get; set; }
        public string 管理_6 { get; set; }
        public string 管理_7 { get; set; }
        public string 管理_8 { get; set; }
        public string 管理_9 { get; set; }
        public string 管理_10 { get; set; }

        private decimal tmp_管理_激励金额 = 0.00m;
        public decimal 管理_激励金额
        {
            get
            {
                return tmp_管理_激励金额;
            }
            set
            {
                tmp_管理_激励金额 = value;
            }
        }
        private decimal tmp_管理_转账金额 = 0.00m;
        public decimal 管理_转账金额
        {
            get
            {
                return tmp_管理_转账金额;
            }
            set
            {
                tmp_管理_转账金额 = value;
            }
        }
        private decimal tmp_管理_预算收入金额 = 0.00m;
        public decimal 管理_预算收入金额
        {
            get
            {
                return tmp_管理_预算收入金额;
            }
            set
            {
                tmp_管理_预算收入金额 = value;
            }
        }
        private decimal tmp_管理_预算支出金额 = 0.00m;
        public decimal 管理_预算支出金额
        {
            get
            {
                return tmp_管理_预算支出金额;
            }
            set
            {
                tmp_管理_预算支出金额 = value;
            }
        }
        private decimal tmp_管理_实际收入金额 = 0.00m;
        public decimal 管理_实际收入金额
        {
            get
            {
                return tmp_管理_实际收入金额;
            }
            set
            {
                tmp_管理_实际收入金额 = value;
            }
        }
        private decimal tmp_管理_实际支出金额 = 0.00m;
        public decimal 管理_实际支出金额
        {
            get
            {
                return tmp_管理_实际支出金额;
            }
            set
            {
                tmp_管理_实际支出金额 = value;
            }
        }
        
        private decimal tmp_管理_预算销项税额 = 0.00m;
        public decimal 管理_预算销项税额
        {
            get
            {
                return tmp_管理_预算销项税额;
            }
            set
            {
                tmp_管理_预算销项税额 = value;
            }
        }
        private decimal tmp_管理_预算进项税额 = 0.00m;
        public decimal 管理_预算进项税额
        {
            get
            {
                return tmp_管理_预算进项税额;
            }
            set
            {
                tmp_管理_预算进项税额 = value;
            }
        }


        /// <summary>
        /// 0固定预算； 1项目预算
        /// </summary>
        private int tmp_类型 = 0;
        public int 类型
        {
            get
            {
                return tmp_类型;
            }
            set
            {
                tmp_类型 = value;
            }
        }

        public string ID { get; set; }
        public string 父ID { get; set; }

        private string tmp_预算编号 = "";
        [StringLength(20)]
        public string 预算编号
        {
            get
            {
                return tmp_预算编号;
            }
            set
            {
                tmp_预算编号 = value;
            }
        }

        private string tmp_预算名称 = "";
        [StringLength(100)]
        public string 预算名称
        {
            get
            {
                return tmp_预算名称;
            }
            set
            {
                tmp_预算名称 = value;
            }
        }

        private string tmp_成本中心编号 = "";
        public string 成本中心编号
        {
            get
            {
                return tmp_成本中心编号;
            }
            set
            {
                tmp_成本中心编号 = value;
            }
        }

        private string tmp_业务类型 = "";
        public string 业务类型
        {
            get
            {
                return tmp_业务类型;
            }
            set
            {
                tmp_业务类型 = value;
            }
        }
        private string tmp_销售 = "";
        public string 销售
        {
            get
            {
                return tmp_销售;
            }
            set
            {
                tmp_销售 = value;
            }
        }
        private string tmp_预算说明 = "";
        public string 预算说明
        {
            get
            {
                return tmp_预算说明;
            }
            set
            {
                tmp_预算说明 = value;
            }
        }

        private string tmp_申请人 = "";
        public string 申请人
        {
            get
            {
                return tmp_申请人;
            }
            set
            {
                tmp_申请人 = value;
            }
        }

        private DateTime tmp_申请日期 = DateTime.Now;
        public DateTime 申请日期
        {
            get
            {
                return tmp_申请日期;
            }
            set
            {
                tmp_申请日期 = value;
            }
        }

        private string tmp_审批人 = "";
        public string 审批人
        {
            get
            {
                return tmp_审批人;
            }
            set
            {
                tmp_审批人 = value;
            }
        }

        private DateTime tmp_审批日期 = DateTime.Parse("1900-01-01");
        public DateTime 审批日期
        {
            get
            {
                return tmp_审批日期;
            }
            set
            {
                tmp_审批日期 = value;
            }
        }

        private string tmp_审批结果 = "";
        public string 审批结果
        {
            get
            {
                return tmp_审批结果;
            }
            set
            {
                tmp_审批结果 = value;
            }
        }

        private string tmp_审批意见 = "";
        public string 审批意见
        {
            get
            {
                return tmp_审批意见;
            }
            set
            {
                tmp_审批意见 = value;
            }
        }


        private string tmp_核定人 = "";
        public string 核定人
        {
            get
            {
                return tmp_核定人;
            }
            set
            {
                tmp_核定人 = value;
            }
        }

        private DateTime tmp_核定日期 = DateTime.Parse("1900-01-01");
        public DateTime 核定日期
        {
            get
            {
                return tmp_核定日期;
            }
            set
            {
                tmp_核定日期 = value;
            }
        }

        private string tmp_核定结果 = "";
        public string 核定结果
        {
            get
            {
                return tmp_核定结果;
            }
            set
            {
                tmp_核定结果 = value;
            }
        }

        private string tmp_核定意见 = "";
        public string 核定意见
        {
            get
            {
                return tmp_核定意见;
            }
            set
            {
                tmp_核定意见 = value;
            }
        }


        private string tmp_预算状态 = "";
        public string 预算状态
        {
            get
            {
                return tmp_预算状态;
            }
            set
            {
                tmp_预算状态 = value;
            }
        }

        private string tmp_来源 = "";
        public string 来源
        {
            get
            {
                return tmp_来源;
            }
            set
            {
                tmp_来源 = value;
            }
        }

        private DateTime tmp_完成日期 = DateTime.Parse("1900-01-01");
        public DateTime 完成日期
        {
            get
            {
                return tmp_完成日期;
            }
            set
            {
                tmp_完成日期 = value;
            }
        }

        private string tmp_是否中证通订单 = "";
        public string 是否中证通订单
        {
            get
            {
                return tmp_是否中证通订单;
            }
            set
            {
                tmp_是否中证通订单 = value;
            }
        }
        private string tmp_当事人 = "";
        public string 当事人
        {
            get
            {
                return tmp_当事人;
            }
            set
            {
                tmp_当事人 = value;
            }
        }
        private string tmp_当事人证件号 = "";
        public string 当事人证件号
        {
            get
            {
                return tmp_当事人证件号;
            }
            set
            {
                tmp_当事人证件号 = value;
            }
        }
        private DateTime tmp_订单日期 = DateTime.Parse("1900-01-01");
        public DateTime 订单日期
        {
            get
            {
                return tmp_订单日期;
            }
            set
            {
                tmp_订单日期 = value;
            }
        }
        private string tmp_公证事项 = "";
        public string 公证事项
        {
            get
            {
                return tmp_公证事项;
            }
            set
            {
                tmp_公证事项 = value;
            }
        }
        private string tmp_用地 = "";
        public string 用地
        {
            get
            {
                return tmp_用地;
            }
            set
            {
                tmp_用地 = value;
            }
        }
        private string tmp_用途 = "";
        public string 用途
        {
            get
            {
                return tmp_用途;
            }
            set
            {
                tmp_用途 = value;
            }
        }
        private string tmp_语种 = "";
        public string 语种
        {
            get
            {
                return tmp_语种;
            }
            set
            {
                tmp_语种 = value;
            }
        }
        private string tmp_承办公证处 = "";
        public string 承办公证处
        {
            get
            {
                return tmp_承办公证处;
            }
            set
            {
                tmp_承办公证处 = value;
            }
        }
        private string tmp_是否加急 = "";
        public string 是否加急
        {
            get
            {
                return tmp_是否加急;
            }
            set
            {
                tmp_是否加急 = value;
            }
        }
        private string tmp_渠道 = "";
        public string 渠道
        {
            get
            {
                return tmp_渠道;
            }
            set
            {
                tmp_渠道 = value;
            }
        }
        private string tmp_线上线下 = "";
        public string 线上线下
        {
            get
            {
                return tmp_线上线下;
            }
            set
            {
                tmp_线上线下 = value;
            }
        }
        private DateTime tmp_导入日期 = DateTime.Parse("1900-01-01");
        public DateTime 导入日期
        {
            get
            {
                return tmp_导入日期;
            }
            set
            {
                tmp_导入日期 = value;
            }
        }
        private string tmp_子订单号 = "";
        public string 子订单号
        {
            get
            {
                return tmp_子订单号;
            }
            set
            {
                tmp_子订单号 = value;
            }
        }

        private List<cls预算收支> tmp_预算收支 = new List<cls预算收支>();
        public List<cls预算收支> 预算收支
        {
            get
            {
                return tmp_预算收支;
            }
            set
            {
                tmp_预算收支 = value;
            }
        }

        private List<cls预算发票> tmp_预算发票 = new List<cls预算发票>();
        public List<cls预算发票> 预算发票
        {
            get
            {
                return tmp_预算发票;
            }
            set
            {
                tmp_预算发票 = value;
            }
        }

        private List<cls内部转账> tmp_内部转账 = new List<cls内部转账>();
        public List<cls内部转账> 内部转账
        {
            get
            {
                return tmp_内部转账;
            }
            set
            {
                tmp_内部转账 = value;
            }
        }

        private List<cls实际收支> tmp_实际收支 = new List<cls实际收支>();
        public List<cls实际收支> 实际收支
        {
            get
            {
                return tmp_实际收支;
            }
            set
            {
                tmp_实际收支 = value;
            }
        }

        private List<cls激励> tmp_激励 = new List<cls激励>();
        public List<cls激励> 激励
        {
            get
            {
                return tmp_激励;
            }
            set
            {
                tmp_激励 = value;
            }
        }

        private List<cls变更> tmp_变更 = new List<cls变更>();
        public List<cls变更> 变更
        {
            get
            {
                return tmp_变更;
            }
            set
            {
                tmp_变更 = value;
            }
        }

        //private List<cls附件> tmp_附件 = new List<cls附件>();
        //public List<cls附件> 附件
        //{
        //    get
        //    {
        //        return tmp_附件;
        //    }
        //    set
        //    {
        //        tmp_附件 = value;
        //    }
        //}

        
        #endregion
    }
    public class cls预算单s
    {
        public List<cls预算单> 记录集 { get; set; }
    }

    public class cls预算收支
    {
        #region 属性
        public string ID { get; set; }
        public string 预算ID { get; set; }

        [Required]
        private string tmp_收支 = "";   //收入或支出
        public string 收支
        {
            get
            {
                return tmp_收支;
            }
            set
            {
                tmp_收支 = value;
            }
        }

        private decimal tmp_金额 = 0.00m;
        public decimal 金额
        {
            get
            {
                return tmp_金额;
            }
            set
            {
                tmp_金额 = value;
            }
        }

        private string tmp_借贷方 = "";
        public string 借贷方
        {
            get
            {
                return tmp_借贷方;
            }
            set
            {
                tmp_借贷方 = value;
            }
        }

        private DateTime tmp_日期 = DateTime.Parse("1900-01-01");
        public DateTime 日期
        {
            get
            {
                return tmp_日期;
            }
            set
            {
                tmp_日期 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }


        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }

        private string tmp_类型 = "";
        public string 类型
        {
            get
            {
                return tmp_类型;
            }
            set
            {
                tmp_类型 = value;
            }
        }

        private decimal tmp_收付税额 = 0.00m;
        public decimal 收付税额
        {
            get
            {
                return tmp_收付税额;
            }
            set
            {
                tmp_收付税额 = value;
            }
        }

        private decimal tmp_收付税率 = 0.00m;
        public decimal 收付税率
        {
            get
            {
                return tmp_收付税率;
            }
            set
            {
                tmp_收付税率 = value;
            }
        }

        private string tmp_收付发票种类 = "";
        public string 收付发票种类
        {
            get
            {
                return tmp_收付发票种类;
            }
            set
            {
                tmp_收付发票种类 = value;
            }
        }

        #endregion
    }

    public class cls预算发票
    {
        #region 属性
        public string ID { get; set; }
        public string 预算ID { get; set; }

        [Required]
        private string tmp_收票方 = "";
        public string 收票方
        {
            get
            {
                return tmp_收票方;
            }
            set
            {
                tmp_收票方 = value;
            }
        }

        private string tmp_发票种类 = "";
        public string 发票种类
        {
            get
            {
                return tmp_发票种类;
            }
            set
            {
                tmp_发票种类 = value;
            }
        }

        private decimal tmp_应收税额 = 0.00m;
        public decimal 应收税额
        {
            get
            {
                return tmp_应收税额;
            }
            set
            {
                tmp_应收税额 = value;
            }
        }

        private decimal tmp_税点 = 0.00m;
        public decimal 税点
        {
            get
            {
                return tmp_税点;
            }
            set
            {
                tmp_税点 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }


        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }


        #endregion
    }

    public class cls内部转账
    {
        #region 属性
        public string ID { get; set; }
        public string 预算ID { get; set; }

        private string tmp_转账对象 = "";
        public string 转账对象
        {
            get
            {
                return tmp_转账对象;
            }
            set
            {
                tmp_转账对象 = value;
            }
        }

        private decimal tmp_转账比例 = 0.00m;
        public decimal 转账比例
        {
            get
            {
                return tmp_转账比例;
            }
            set
            {
                tmp_转账比例 = value;
            }
        }

        private decimal tmp_转账金额 = 0.00m;
        public decimal 转账金额
        {
            get
            {
                return tmp_转账金额;
            }
            set
            {
                tmp_转账金额 = value;
            }
        }
        
        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }
        
        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }

        #endregion
    }

    public class cls实际收支
    {
        #region 属性
        public string ID { get; set; }
        public string 预算ID { get; set; }

        [Required]
        private string tmp_收支 = "";   //收入或支出
        public string 收支
        {
            get
            {
                return tmp_收支;
            }
            set
            {
                tmp_收支 = value;
            }
        }

        private decimal tmp_金额 = 0.00m;
        public decimal 金额
        {
            get
            {
                return tmp_金额;
            }
            set
            {
                tmp_金额 = value;
            }
        }

        private string tmp_借贷方 = "";
        public string 借贷方
        {
            get
            {
                return tmp_借贷方;
            }
            set
            {
                tmp_借贷方 = value;
            }
        }

        private DateTime tmp_日期 = DateTime.Parse("1900-01-01");
        public DateTime 日期
        {
            get
            {
                return tmp_日期;
            }
            set
            {
                tmp_日期 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }


        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }

        private string tmp_类型 = "";
        public string 类型
        {
            get
            {
                return tmp_类型;
            }
            set
            {
                tmp_类型 = value;
            }
        }
        #endregion
    }

    public class cls激励
    {
        #region 属性

        public string 管理_1 { get; set; }
        public string 管理_2 { get; set; }
        public string 管理_3 { get; set; }
        public string 管理_4 { get; set; }
        public string 管理_5 { get; set; }

        public string ID { get; set; }
        public string 预算ID { get; set; }

        private string tmp_人员 = "";
        public string 人员
        {
            get
            {
                return tmp_人员;
            }
            set
            {
                tmp_人员 = value;
            }
        }

        private decimal tmp_比例 = 0.00m;
        public decimal 比例
        {
            get
            {
                return tmp_比例;
            }
            set
            {
                tmp_比例 = value;
            }
        }

        private decimal tmp_金额 = 0.00m;
        public decimal 金额
        {
            get
            {
                return tmp_金额;
            }
            set
            {
                tmp_金额 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }

        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }
        private string tmp_审批人 = "";
        public string 审批人
        {
            get
            {
                return tmp_审批人;
            }
            set
            {
                tmp_审批人 = value;
            }
        }

        private DateTime tmp_审批日期 = DateTime.Parse("1900-01-01");
        public DateTime 审批日期
        {
            get
            {
                return tmp_审批日期;
            }
            set
            {
                tmp_审批日期 = value;
            }
        }

        private string tmp_审批结果 = "";
        public string 审批结果
        {
            get
            {
                return tmp_审批结果;
            }
            set
            {
                tmp_审批结果 = value;
            }
        }

        private string tmp_审批意见 = "";
        public string 审批意见
        {
            get
            {
                return tmp_审批意见;
            }
            set
            {
                tmp_审批意见 = value;
            }
        }
        #endregion
    }

    public class cls附件
    {
        #region 属性

        public string ID { get; set; }
        public string 预算ID { get; set; }

        private int tmp_序号 = 0;
        public int 序号
        {
            get
            {
                return tmp_序号;
            }
            set
            {
                tmp_序号 = value;
            }
        }

        private string tmp_服务器文件名 = "";
        public string 服务器文件名
        {
            get
            {
                return tmp_服务器文件名;
            }
            set
            {
                tmp_服务器文件名 = value;
            }
        }
        #endregion
    }


    public class cls变更
    {
        #region 属性
        public string ID { get; set; }
        public string 预算ID { get; set; }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }

        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }
        private string tmp_变更原因 = "";
        public string 变更原因
        {
            get
            {
                return tmp_变更原因;
            }
            set
            {
                tmp_变更原因 = value;
            }
        }


        private string tmp_审批人 = "";
        public string 审批人
        {
            get
            {
                return tmp_审批人;
            }
            set
            {
                tmp_审批人 = value;
            }
        }

        private DateTime tmp_审批日期 = DateTime.Parse("1900-01-01");
        public DateTime 审批日期
        {
            get
            {
                return tmp_审批日期;
            }
            set
            {
                tmp_审批日期 = value;
            }
        }

        private string tmp_审批结果 = "";
        public string 审批结果
        {
            get
            {
                return tmp_审批结果;
            }
            set
            {
                tmp_审批结果 = value;
            }
        }

        private string tmp_审批意见 = "";
        public string 审批意见
        {
            get
            {
                return tmp_审批意见;
            }
            set
            {
                tmp_审批意见 = value;
            }
        }
        #endregion
    }



    #endregion



    #region 中证通

    public class cls中证通订单
    {
        #region 属性


        public string 管理_1 { get; set; }
        public string 管理_2 { get; set; }
        public string 管理_3 { get; set; }
        public string 管理_4 { get; set; }
        public string 管理_5 { get; set; }
        public string 管理_6 { get; set; }
        public string 管理_7 { get; set; }
        public string 管理_8 { get; set; }
        public string 管理_9 { get; set; }
        public string 管理_10 { get; set; }

        
        private decimal tmp_管理_预算收入金额 = 0.00m;
        public decimal 管理_预算收入金额
        {
            get
            {
                return tmp_管理_预算收入金额;
            }
            set
            {
                tmp_管理_预算收入金额 = value;
            }
        }
        private decimal tmp_管理_预算支出金额 = 0.00m;
        public decimal 管理_预算支出金额
        {
            get
            {
                return tmp_管理_预算支出金额;
            }
            set
            {
                tmp_管理_预算支出金额 = value;
            }
        }
        private decimal tmp_管理_实际收入金额 = 0.00m;
        public decimal 管理_实际收入金额
        {
            get
            {
                return tmp_管理_实际收入金额;
            }
            set
            {
                tmp_管理_实际收入金额 = value;
            }
        }
        private decimal tmp_管理_实际支出金额 = 0.00m;
        public decimal 管理_实际支出金额
        {
            get
            {
                return tmp_管理_实际支出金额;
            }
            set
            {
                tmp_管理_实际支出金额 = value;
            }
        }


        public string ID { get; set; }

        private string tmp_订单号 = "";
        public string 订单号
        {
            get
            {
                return tmp_订单号;
            }
            set
            {
                tmp_订单号 = value;
            }
        }

        private string tmp_订单ID = "";
        public string 订单ID
        {
            get
            {
                return tmp_订单ID;
            }
            set
            {
                tmp_订单ID = value;
            }
        }

        private string tmp_子订单号 = "";
        public string 子订单号
        {
            get
            {
                return tmp_子订单号;
            }
            set
            {
                tmp_子订单号 = value;
            }
        }

        private string tmp_子订单ID = "";
        public string 子订单ID
        {
            get
            {
                return tmp_子订单ID;
            }
            set
            {
                tmp_子订单ID = value;
            }
        }


        private string tmp_当事人 = "";
        public string 当事人
        {
            get
            {
                return tmp_当事人;
            }
            set
            {
                tmp_当事人 = value;
            }
        }

        private string tmp_当事人证件号 = "";
        public string 当事人证件号
        {
            get
            {
                return tmp_当事人证件号;
            }
            set
            {
                tmp_当事人证件号 = value;
            }
        }

        private DateTime tmp_订单日期 = DateTime.Parse("1900-01-01");
        public DateTime 订单日期
        {
            get
            {
                return tmp_订单日期;
            }
            set
            {
                tmp_订单日期 = value;
            }
        }

        private string tmp_公证事项 = "";
        public string 公证事项
        {
            get
            {
                return tmp_公证事项;
            }
            set
            {
                tmp_公证事项 = value;
            }
        }

        private string tmp_用地 = "";
        public string 用地
        {
            get
            {
                return tmp_用地;
            }
            set
            {
                tmp_用地 = value;
            }
        }

        private string tmp_用途 = "";
        public string 用途
        {
            get
            {
                return tmp_用途;
            }
            set
            {
                tmp_用途 = value;
            }
        }

        private string tmp_语种 = "";
        public string 语种
        {
            get
            {
                return tmp_语种;
            }
            set
            {
                tmp_语种 = value;
            }
        }

        private string tmp_承办公证处 = "";
        public string 承办公证处
        {
            get
            {
                return tmp_承办公证处;
            }
            set
            {
                tmp_承办公证处 = value;
            }
        }

        private string tmp_是否加急 = "";
        public string 是否加急
        {
            get
            {
                return tmp_是否加急;
            }
            set
            {
                tmp_是否加急 = value;
            }
        }

        private string tmp_渠道 = "";
        public string 渠道
        {
            get
            {
                return tmp_渠道;
            }
            set
            {
                tmp_渠道 = value;
            }
        }

        private DateTime tmp_导入日期 = DateTime.Parse("1900-01-01");
        public DateTime 导入日期
        {
            get
            {
                return tmp_导入日期;
            }
            set
            {
                tmp_导入日期 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }


        private List<cls中证通订单预算收支> tmp_预算收支 = new List<cls中证通订单预算收支>();
        public List<cls中证通订单预算收支> 预算收支
        {
            get
            {
                return tmp_预算收支;
            }
            set
            {
                tmp_预算收支 = value;
            }
        }
        private List<cls中证通订单实际收支> tmp_实际收支 = new List<cls中证通订单实际收支>();
        public List<cls中证通订单实际收支> 实际收支
        {
            get
            {
                return tmp_实际收支;
            }
            set
            {
                tmp_实际收支 = value;
            }
        }
        #endregion
    }

    public class cls中证通订单预算收支
    {
        #region 属性
        public string ID { get; set; }
        public string 证书ID { get; set; }

        [Required]
        private string tmp_收支 = "";   //收入或支出
        public string 收支
        {
            get
            {
                return tmp_收支;
            }
            set
            {
                tmp_收支 = value;
            }
        }

        private decimal tmp_金额 = 0.00m;
        public decimal 金额
        {
            get
            {
                return tmp_金额;
            }
            set
            {
                tmp_金额 = value;
            }
        }

        private string tmp_借贷方 = "";
        public string 借贷方
        {
            get
            {
                return tmp_借贷方;
            }
            set
            {
                tmp_借贷方 = value;
            }
        }

        private DateTime tmp_日期 = DateTime.Parse("1900-01-01");
        public DateTime 日期
        {
            get
            {
                return tmp_日期;
            }
            set
            {
                tmp_日期 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }


        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }

        private string tmp_类型 = "";
        public string 类型
        {
            get
            {
                return tmp_类型;
            }
            set
            {
                tmp_类型 = value;
            }
        }

        #endregion
    }


    public class cls中证通订单实际收支
    {
        #region 属性
        public string ID { get; set; }
        public string 证书ID { get; set; }

        [Required]
        private string tmp_收支 = "";   //收入或支出
        public string 收支
        {
            get
            {
                return tmp_收支;
            }
            set
            {
                tmp_收支 = value;
            }
        }

        private decimal tmp_金额 = 0.00m;
        public decimal 金额
        {
            get
            {
                return tmp_金额;
            }
            set
            {
                tmp_金额 = value;
            }
        }

        private string tmp_借贷方 = "";
        public string 借贷方
        {
            get
            {
                return tmp_借贷方;
            }
            set
            {
                tmp_借贷方 = value;
            }
        }

        private DateTime tmp_日期 = DateTime.Parse("1900-01-01");
        public DateTime 日期
        {
            get
            {
                return tmp_日期;
            }
            set
            {
                tmp_日期 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_录入人 = "";
        public string 录入人
        {
            get
            {
                return tmp_录入人;
            }
            set
            {
                tmp_录入人 = value;
            }
        }


        private DateTime tmp_录入日期 = DateTime.Parse("1900-01-01");
        public DateTime 录入日期
        {
            get
            {
                return tmp_录入日期;
            }
            set
            {
                tmp_录入日期 = value;
            }
        }

        private string tmp_类型 = "";
        public string 类型
        {
            get
            {
                return tmp_类型;
            }
            set
            {
                tmp_类型 = value;
            }
        }

        #endregion
    }

    #endregion

    #region 系统

    public class cls系统参数
    {
        public string ID { get; set; }

        private string tmp_参数名 = "";
        public string 参数名
        {
            get
            {
                return tmp_参数名;
            }
            set
            {
                tmp_参数名 = value;
            }
        }

        private string tmp_参数值 = "";
        public string 参数值
        {
            get
            {
                return tmp_参数值;
            }
            set
            {
                tmp_参数值 = value;
            }
        }

        private string tmp_说明 = "";
        public string 说明
        {
            get
            {
                return tmp_说明;
            }
            set
            {
                tmp_说明 = value;
            }
        }
    }

    public class cls部门
    {
        public string ID { get; set; }

        private string tmp_编号 = "";
        public string 编号
        {
            get
            {
                return tmp_编号;
            }
            set
            {
                tmp_编号 = value;
            }
        }

        private string tmp_名称 = "";
        public string 名称
        {
            get
            {
                return tmp_名称;
            }
            set
            {
                tmp_名称 = value;
            }
        }
    }
    public class cls用户
    {
        #region 属性
        public string ID { get; set; }

        private int tmp_序号 = 0;
        public int 序号
        {
            get
            {
                return tmp_序号;
            }
            set
            {
                tmp_序号 = value;
            }
        }
       
        private string tmp_登录名 = "";
        public string 登录名
        {
            get
            {
                return tmp_登录名;
            }
            set
            {
                tmp_登录名 = value;
            }
        }
       
        private string tmp_姓名 = "";
        public string 姓名
        {
            get
            {
                return tmp_姓名;
            }
            set
            {
                tmp_姓名 = value;
            }
        }

        private string tmp_密码 = "";
        public string 密码
        {
            get
            {
                return tmp_密码;
            }
            set
            {
                tmp_密码 = value;
            }
        }

        private string tmp_权限 = "";
        public string 权限
        {
            get
            {
                return tmp_权限;
            }
            set
            {
                tmp_权限 = value;
            }
        }

        private string tmp_部门 = "";
        public string 部门
        {
            get
            {
                return tmp_部门;
            }
            set
            {
                tmp_部门 = value;
            }
        }

        private string tmp_备注 = "";
        public string 备注
        {
            get
            {
                return tmp_备注;
            }
            set
            {
                tmp_备注 = value;
            }
        }

        private string tmp_角色 = "";
        public string 角色
        {
            get
            {
                return tmp_角色;
            }
            set
            {
                tmp_角色 = value;
            }
        }
        #endregion
    }
    
    public class cls成本中心
    {
        public string ID { get; set; }

        private string tmp_成本中心编号 = "";
        public string 成本中心编号
        {
            get
            {
                return tmp_成本中心编号;
            }
            set
            {
                tmp_成本中心编号 = value;
            }
        }

        private string tmp_成本中心名称 = "";
        public string 成本中心名称
        {
            get
            {
                return tmp_成本中心名称;
            }
            set
            {
                tmp_成本中心名称 = value;
            }
        }
        private string tmp_部门 = "";
        public string 部门
        {
            get
            {
                return tmp_部门;
            }
            set
            {
                tmp_部门 = value;
            }
        }

        private string tmp_停用 = "";
        public string 停用
        {
            get
            {
                return tmp_停用;
            }
            set
            {
                tmp_停用 = value;
            }
        }
    }
    public class cls业务类型
    {
        private string tmp_业务类型 = "";
        public string 业务类型
        {
            get
            {
                return tmp_业务类型;
            }
            set
            {
                tmp_业务类型 = value;
            }
        }
    }

    public class cls消息
    {
        #region 属性
        public string ID { get; set; }
        public string 业务ID { get; set; }

        private string tmp_接收人 = "";
        public string 接收人
        {
            get
            {
                return tmp_接收人;
            }
            set
            {
                tmp_接收人 = value;
            }
        }

        private string tmp_消息内容 = "";
        public string 消息内容
        {
            get
            {
                return tmp_消息内容;
            }
            set
            {
                tmp_消息内容 = value;
            }
        }

        private string tmp_业务类型 = "";
        public string 业务类型
        {
            get
            {
                return tmp_业务类型;
            }
            set
            {
                tmp_业务类型 = value;
            }
        }

        private string tmp_发送人 = "";
        public string 发送人
        {
            get
            {
                return tmp_发送人;
            }
            set
            {
                tmp_发送人 = value;
            }
        }

        private DateTime tmp_发送时间 = DateTime.Parse("1900-01-01");
        public DateTime 发送时间
        {
            get
            {
                return tmp_发送时间;
            }
            set
            {
                tmp_发送时间 = value;
            }
        }

        private DateTime tmp_失效时间 = DateTime.Parse("1900-01-01");
        public DateTime 失效时间
        {
            get
            {
                return tmp_失效时间;
            }
            set
            {
                tmp_失效时间 = value;
            }
        }

        private string tmp_已阅 = "";
        public string 已阅
        {
            get
            {
                return tmp_已阅;
            }
            set
            {
                tmp_已阅 = value;
            }
        }

        #endregion
    }


    /// <summary>
    /// 完成字符串的散列生成，以及3DES加密解密码
    /// </summary>
    public class 加解密
    {
        private string mIV = "09876543210987654321098765432132";   //使用固定值，32位长度，这样省力
        private string mKey = "09876543210987654321098765432132";  //使用固定值，32位长度，这样省力
        private byte[] bIV;
        private byte[] bKey;
        private TripleDES tDes;
        /// <summary>
        /// 解密3DES加密字符串时的构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public 加解密(string key, string iv)
        {
            Key = key;
            IV = iv;
            InitEncryptTransformer();
        }
        /// <summary>
        /// 加密数据时用的构造函数
        /// 系统自动生成Key,与IV的Base64字符串
        /// 请妥协保存
        /// </summary>
        public 加解密()
        {
            InitEncryptTransformer();
        }
        /// <summary>
        /// 用3DES加密字符串，结果已Base64编码返回
        /// </summary>
        /// <param name="inStr">需要加密的字符串</param>
        /// <returns>Base64编码的加密字符串</returns>
        public string EncryptString(string inStr)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(inStr);
                ICryptoTransform enTransform = tDes.CreateEncryptor(bKey, bIV);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, enTransform, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return "错误";
            }

        }
        /// <summary>
        /// 解密编码为Base64的加密字符串
        /// </summary>
        /// <param name="inStr">Base64编码的加密字符串</param>
        /// <returns>原字符串数据</returns>
        public string DecryptString(string inStr)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(inStr);
                ICryptoTransform deTransfrom = tDes.CreateDecryptor(bKey, bIV);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, deTransfrom, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return "错误";
            }

        }
        private void InitEncryptTransformer()
        {
            tDes = new TripleDESCryptoServiceProvider();
            if (Key == null || IV == null)
            {
                tDes.GenerateIV();
                bIV = tDes.IV;
                tDes.GenerateKey();
                bKey = tDes.Key;
                Key = Convert.ToBase64String(bKey);
                IV = Convert.ToBase64String(bIV);
            }
            else
            {
                bIV = Convert.FromBase64String(IV);
                bKey = Convert.FromBase64String(Key);
            }


        }
        /// <summary>
        /// 将字符串数据以MD5加密后编码成Base64
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string MD5(string inStr)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(inStr);
            byte[] enBytes = md5.ComputeHash(bytes);
            return Convert.ToBase64String(enBytes);
        }
        /// <summary>
        /// Base64编码的向量字符串
        /// </summary>
        public string IV
        {
            get
            {
                return mIV;
            }
            set
            {
                mIV = value;
                bIV = Convert.FromBase64String(value);

            }

        }
        /// <summary>
        /// Base64编码的密码
        /// </summary>
        public string Key
        {
            get
            {
                return mKey;
            }
            set
            {
                mKey = value;
                bKey = Convert.FromBase64String(value);
            }
        }
    }


    public class 登录
    {
        public static cls用户 登录密码验证(string UserName, string UserPwd)
        {
            List<cls用户> lst = new List<cls用户>();
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            string sql = "select * from Z用户 where 登录名=@name and 密码=@pwd";
            using (MySqlConnection cnn = new MySqlConnection(数据库连接字符串))
            {
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlParameter p1 = new MySqlParameter("@name", UserName);
                MySqlParameter p2 = new MySqlParameter("@pwd", UserPwd);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                try
                {
                    adp.Fill(ds);
                    DataTable tb = ds.Tables[0];
                    lst = (List<cls用户>)(main.ConvertTo<cls用户>(tb));
                }
                catch { }
                try { return lst[0]; }
                catch { }
            }
            return null;
        }
        public static cls用户 登录密码验证MSSQL(string UserName, string UserPwd)
        {
            List<cls用户> lst = new List<cls用户>();
            string 数据库连接字符串 = HttpContext.Current.Application["RKBudgetDBConn"].ToString();

            string sql = "select * from Z用户 where 登录名=@name and 密码=@pwd";
            using (SqlConnection cnn = new SqlConnection(数据库连接字符串))
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlParameter p1 = new SqlParameter("@name", UserName);
                SqlParameter p2 = new SqlParameter("@pwd", UserPwd);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                try
                {
                    adp.Fill(ds);
                    DataTable tb = ds.Tables[0];
                    lst = (List<cls用户>)(main.ConvertTo<cls用户>(tb));
                }
                catch { }
                try { return lst[0]; }
                catch { }
            }
            return null;
        }

        public static string 登录状态验证()
        {
            string str1 = "";
            string str2 = "";
            try
            {
                str1 = System.Web.HttpContext.Current.Session["loginname"].ToString();
            }
            catch { }
            try
            {
                str2 = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            }
            catch { }
            if ((str1 == "") | (str2 == ""))
                return "";
            else
                return str2;
        }

        public static void 登录状态注销()
        {
            try
            {
                System.Web.HttpContext.Current.Session["loginname"] = "";
            }
            catch { }
            try
            {
                System.Web.HttpContext.Current.Session["loginrealname"] = "";
            }
            catch { }
        }


        public static bool 登录状态验证1(AuthorizationContext filterContext)
        {
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies["RKBudgetDenglu"];
            if (cookie == null)
                return false;
            else
                return true;
        }
        public static bool 登录权限验证(string 功能项目)   //作废
        {
            string sUserName = "";
            string sUserPwd = "";
            cls用户 ry = null;

            HttpCookie cookie = null;
            cookie = HttpContext.Current.Request.Cookies["RKBudgetDenglu"];
            if (cookie == null)
            {
                return false;
            }
            sUserName = HttpUtility.UrlDecode(cookie.Values["loginname"], Encoding.GetEncoding("UTF-8"));   // cookie.Values["loginname"]; 
            sUserPwd = cookie.Values["loginpassword"];
            if (string.IsNullOrEmpty(sUserName)) sUserName = "";
            if (string.IsNullOrEmpty(sUserPwd)) sUserPwd = "";
            sUserPwd = (new 加解密()).DecryptString(sUserPwd);
            if (sUserName == "")
            {
                return false;
            }
            ry = 登录密码验证(sUserName, sUserPwd);
            if (ry == null)
            {
                return false;
            }
            if (ry.登录名 == "admin")
            {
                return true;
            }
            if (ry.权限.IndexOf("|" + 功能项目 + "|") > -1)
            {
                return true;
            }
            return false;
        }
        public static bool 登录权限验证1(string 功能项目)
        {
            string str1 = "";
            try
            {
                str1 = System.Web.HttpContext.Current.Session["loginname"].ToString();
            }
            catch { }

            HttpCookie cookie = null;
            cookie = HttpContext.Current.Request.Cookies["RKBudgetDenglu"];
            if (cookie == null)
            {
                return false;
            }
            string sUserName = HttpUtility.UrlDecode(cookie.Values["loginname"], Encoding.GetEncoding("UTF-8"));   // cookie.Values["loginname"];
            if (str1 != sUserName) return false;
            if (str1 == "admin") return true;

            string sqx = HttpUtility.UrlDecode(cookie.Values["loginauthorization"], Encoding.GetEncoding("UTF-8"));   // cookie.Values["loginname"];
            sqx = "" + sqx + "";
            if (sqx.IndexOf(" " + 功能项目 + " ") > -1)
            {
                //重新写下，延长有效期
                System.Web.HttpContext.Current.Session["loginname"] = System.Web.HttpContext.Current.Session["loginname"].ToString();
                System.Web.HttpContext.Current.Session["loginrealname"] = System.Web.HttpContext.Current.Session["loginrealname"].ToString();

                return true;
            }
            return false;
        }
        public static void 登录保存到Cookie(string UserName, string UserPwd, string 姓名, string 权限, string Jxw)
        {
            HttpCookie cookie = new HttpCookie("RKBudgetDenglu");

            if (Jxw.ToLower() == "on")
            {
                cookie.Values["loginname"] = HttpUtility.UrlEncode(UserName, Encoding.GetEncoding("UTF-8")); //直接写中文可能乱码
                cookie.Values["loginpassword"] = (new 加解密()).EncryptString(UserPwd);
                cookie.Values["loginrealname"] = HttpUtility.UrlEncode(姓名, Encoding.GetEncoding("UTF-8")); //直接写中文可能乱码
                cookie.Values["loginauthorization"] = HttpUtility.UrlEncode(权限, Encoding.GetEncoding("UTF-8")); //直接写中文可能乱码
            }
            cookie.Expires = DateTime.Now.AddDays(100);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void 登录删除Cookie()
        {
            HttpCookie cookie = null;
            cookie = HttpContext.Current.Request.Cookies["RKBudgetDenglu"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-10);
            }
        }
        public static string 获取Cookie(string itemName)
        {
            HttpCookie cookie = null;
            cookie = HttpContext.Current.Request.Cookies["RKBudgetDenglu"];
            if (cookie == null)
            {
                return "";
            }
            else
            {
                try { return cookie[itemName].ToString(); }
                catch { }
            }
            return "";
        }
    }
    #endregion
}