using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using applyvisa.Models;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace applyvisa.Controllers
{
    [Authentication]   //[Authorization]//如果将此特性加在Controller上，那么访问这个Controller里面的方法都需要验证用户登录状态
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ShowLoginName();
            
            return View();
        }

        //public void ShowLoginName()
        //{
        //    string surrentloginrealname = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
        //    if ((surrentloginrealname == string.Empty) | (surrentloginrealname == null)) surrentloginrealname = "";
        //    if (surrentloginrealname != "")
        //    {
        //        ViewBag.dlzx = surrentloginrealname;
        //    }
        //}
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


        [HttpPost]
        public string newguid()
        {
            string str1 = Guid.NewGuid().ToString();
            return str1;
        }


        [AllowAnonymous]  //[AllowAnonymous]//这里是一个特例，有这个特性，表示这个方法不需要验证用户登录状态
        public ActionResult Login()
        {
            string str1 = "";
            str1 = 登录.获取Cookie("loginname");
            ViewBag.loginname = str1;

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public int denglu(string actName, string UserName, string UserPwd, string Jxw)
        {
            System.Web.HttpContext.Current.Session["loginname"] = "";
            System.Web.HttpContext.Current.Session["loginrealname"] = "";
            登录.登录删除Cookie();

            string sactName = actName;
            string sUserName = UserName;
            string sUserPwd = UserPwd;
            string sJxw = Jxw;
            if (string.IsNullOrEmpty(sactName)) sactName = "";
            if (string.IsNullOrEmpty(sUserName)) sUserName = "";
            if (string.IsNullOrEmpty(sUserPwd)) sUserPwd = "";
            if (string.IsNullOrEmpty(sJxw)) sJxw = "";

            if (sUserName.Trim() == "") return 0;

            cls用户 ry = null;
            ry = 登录.登录密码验证(sUserName, sUserPwd);   //ry = 登录.登录密码验证MSSQL(sUserName, sUserPwd);   //MSSQL和MYSQL            
            if (ry == null) return 0;

            System.Web.HttpContext.Current.Session["loginname"] = sUserName;
            System.Web.HttpContext.Current.Session["loginrealname"] = ry.姓名;
            登录.登录保存到Cookie(sUserName, sUserPwd, ry.姓名, ry.权限, sJxw);


            return 100;
        }

        [AllowAnonymous]
        [HttpPost]
        public string zhuxiao()
        {
            登录.登录状态注销();

            string str1 = "";
            str1 = 登录.登录状态验证();
            if (str1 == "")
            {
                return "ok";
            }
            else
            {
                return "fail";
            }
        }
        [HttpPost]
        public string loginagain()
        {
            return "";
        }

        [AllowAnonymous]
        public ActionResult Noauthorization()
        {
            return View();
        }

        #region 消息
        /// <summary>
        /// 获取当前登录者消息数量
        /// </summary>
        /// <param name="loginrealname"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetInformation(string loginrealname)
        {
            string returnstr = "";
            string s1 = loginrealname;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            if (s1 == "")
            {
                returnstr = "300001";    //未知的登录人
                return Content(returnstr);
            }

            //string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            //if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "";

            int m = Budget.获取通知消息条数(s1);

            returnstr = m.ToString();
            return Content(returnstr);
        }

        public ActionResult MessageList(int p, int r)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            //if (!登录.登录权限验证1("消息管理")) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";

            TempData["dlzx"] = sdenglurenming;

            //string str1 = "where 接收人='" + sdenglurenming + "' and 失效时间>getdate() and 已阅=''";
            string str1 = "where 接收人='" + sdenglurenming + "' and 失效时间>now() and 已阅=''";   //Mysql
            List<cls消息> sbs = Budget.获取消息记录s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");

            TempData["sbcxs"] = sbs;

            TempData["pagenum"] = p;

            //if (r == 1)
            //{
            //    TempData["sbcxs"] = null;
            //}

            if (p > 0)
            {
                //try
                //{
                //    sbs = (List<cls消息>)TempData["sbcxs"];
                //}
                //catch { }
                //if (sbs == null) sbs = new List<cls消息>();

                //TempData["sbcxs"] = sbs;
                //TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls消息> sbs1 = new List<cls消息>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }

            return View();
        }


        [HttpPost]
        public ActionResult HandleMessage(string xgID, string xgyw)
        {
            string s1 = xgID;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = xgyw;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";

            if ((s1 == "") | (s2 == ""))
            {
                return Content("900001");
            }
            int i = 0;

            if (s2 == "设为已阅")
            {
                i = Budget.设置消息为已阅(s1);
                if (i < 1)
                {
                    return Content("900001");    //设置失败
                }
            }

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }

        [HttpPost]
        public ActionResult AllMessageReaded(string loginrealname)
        {
            string returnstr = "";
            string s1 = loginrealname;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            if (s1 == "")
            {
                returnstr = "300001";    //未知的登录人
                return Content(returnstr);
            }
            int i = 0;
            i = Budget.设置全部消息为已阅(loginrealname);                

            returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }


        /// <summary>
        /// 消息(包含已阅)查询
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public ActionResult MsgList(int p, int r)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            if (!登录.登录权限验证1("消息管理")) return RedirectToAction("Noauthorization", "Home");

            string sdenglurenming = System.Web.HttpContext.Current.Session["loginrealname"].ToString();
            if ((sdenglurenming == string.Empty) | (sdenglurenming == null)) sdenglurenming = "（匿名）";
            
            if (r == 1)
            {
                TempData["sbcxs"] = null;
            }
            TempData["dlzx"] = sdenglurenming;

            List<cls消息> sbs = new List<cls消息>();
            TempData["pagenum"] = p;

            if (p > 0)
            {
                try
                {
                    sbs = (List<cls消息>)TempData["sbcxs"];
                }
                catch { }
                if (sbs == null) sbs = new List<cls消息>();

                TempData["sbcxs"] = sbs;
                TempData.Keep("sbcxs");

                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sbs.Count / percount;
                x2 = sbs.Count % percount;
                if (x2 > 0) x1++;   //总页数
                TempData["pagetotal"] = x1;

                List<cls消息> sbs1 = new List<cls消息>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sbs.Count) break;
                    sbs1.Add(sbs[k]);
                }
                TempData["pagedata"] = sbs1;
            }

            return View();
        }

        //我的预算申请查询
        [HttpPost]
        public ActionResult MsgListCx(string t1, string t2, string t3, string t4, string t5)
        {
            string s1 = t1;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = t2;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = t3;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = t4;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = t5;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";

            string str1 = "";
            string[] sOpe = new string[5];
            string[] sFld = new string[5];
            string[] sVal = new string[5];

            sFld[0] = "发送时间";
            sFld[1] = "发送时间";
            sFld[2] = "发送人";
            sFld[3] = "已阅";
            sFld[4] = "消息内容";

            sOpe[0] = ">='";
            sOpe[1] = "<='";
            sOpe[2] = "='";
            sOpe[3] = "='";
            sOpe[4] = "like";

            sVal[0] = s1;
            sVal[1] = s2;
            sVal[2] = s3;
            sVal[3] = s4;
            sVal[4] = s5;

            if (sVal[2] != "")
            {
                sVal[2] = sVal[2] + "' or 接收人='" + sVal[2];
            }
            if (sVal[3] == "否")
            {
                sVal[3] = "△";
            }

            str1 = CommonFunctions.CreateWhereClause(sOpe, sFld, sVal, "and");
            str1 = str1.Replace("△", "");
            str1 = str1 + " order by 发送时间 desc";

            List<cls消息> sbs = Budget.获取消息记录s(str1);
            TempData["wheresql"] = str1;
            TempData.Keep("wheresql");

            TempData["sbcxs"] = sbs;
            TempData.Keep("sbcxs");

            TempData["sbcx_t1"] = s1; TempData["sbcx_t2"] = s2; TempData["sbcx_t3"] = s3; TempData["sbcx_t4"] = s4; TempData["sbcx_t5"] = s5; 
            TempData.Keep("sbcx_t1"); TempData.Keep("sbcx_t2"); TempData.Keep("sbcx_t3"); TempData.Keep("sbcx_t4"); TempData.Keep("sbcx_t5");

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }
        #endregion

        #region 维护

        #region 用户维护
        //用户列表
        public ActionResult Yonghulist(int p)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("用户管理")) return RedirectToAction("Noauthorization", "Home");

            List<cls部门> bms = Budget.获取部门("order by 编号");
            ViewBag.bms = bms;

            List<cls用户> sysdata1 = Budget.获取用户(" order by 部门,序号");

            TempData["yonghulist"] = sysdata1;

            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sysdata1.Count / percount;
                x2 = sysdata1.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls用户> sbs1 = new List<cls用户>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sysdata1.Count) break;
                    sbs1.Add(sysdata1[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }

        //用户密码修改
        public ActionResult Yonghumima()
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");

            string loginname = System.Web.HttpContext.Current.Session["loginname"].ToString();
            if ((loginname == string.Empty) | (loginname == null)) loginname = "";

            cls用户 yh = null;
            try
            {
                yh = Budget.获取用户("where 登录名='" + loginname + "'")[0];
            }
            catch { }
            if (yh == null)
            {
                yh = new cls用户();
            }
            ViewBag.yonghu = yh;

            return View();
        }

        [HttpPost]
        public ActionResult Yonghumimiedit(string dlm, string ymm, string xmm)
        {
            string s1 = dlm;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = ymm;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = xmm;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";

            if (s1 == "")
            {
                return Content("900001");
            }

            cls用户 yh = null;
            try
            {
                yh = Budget.获取用户("where 登录名='" + s1 + "' and 密码='" + s2 + "'")[0];
            }
            catch { }
            if (yh == null)
            {
                return Content("900002");
            }

            int i = 0;
            i = Budget.修改密码(s1, s3);
            if (i < 1)
            {
                return Content("900003");
            }

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }

        [HttpPost]
        public ActionResult Yonghushanchu(string id)
        {
            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            if (s1 == "")
            {
                return Content("900001");
            }

            int i = 0;
            i = main.执行SQL命令("delete from Z用户 where id='" + id + "'");
            if (i < 1)
            {
                return Content("900003");
            }

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }


        [HttpPost]
        public ActionResult SaveYonghu(string id, string xh, string dlm, string xm, string mm, string qx, string bz, string bm, string js)
        {
            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = xh;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = dlm;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = xm;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";
            string s5 = mm;
            if ((s5 == string.Empty) | (s5 == null)) s5 = "";
            string s6 = qx;
            if ((s6 == string.Empty) | (s6 == null)) s6 = "";
            string s7 = bz;
            if ((s7 == string.Empty) | (s7 == null)) s7 = "";
            string s8 = bm;
            if ((s8 == string.Empty) | (s8 == null)) s8 = "";
            string s9 = js;
            if ((s9 == string.Empty) | (s9 == null)) s9 = "";

            int i = 0;
            bool b1 = true;
            cls用户 yh = new cls用户();
            if (s1 == "")  //新增
            {
                yh.ID = Guid.NewGuid().ToString();
                b1 = true;
            }
            else
            {
                yh.ID = s1;
                b1 = false;
            }
            yh.序号 = CommonFunctions.ValInt(s2);
            yh.登录名 = s3;
            yh.姓名 = s4;
            yh.密码 = s5;
            yh.权限 = s6;
            yh.备注 = s7;
            yh.部门 = s8;
            yh.角色 = s9;
            if ((yh.登录名.Trim() == "") | (yh.姓名.Trim() == ""))
            {
                return Content("401");
            }
            i = Budget.保存用户(b1, yh);
            if (i < 1)
            {
                return Content("402");
            }

            string returnstr = "10";
            return Content(returnstr);
        }

        #endregion


        #region 部门维护
        //用户列表
        public ActionResult Deplist(int p)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("公司部门")) return RedirectToAction("Noauthorization", "Home");

            List<cls部门> sysdata1 = Budget.获取部门("");

            TempData["deplist"] = sysdata1;

            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sysdata1.Count / percount;
                x2 = sysdata1.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls部门> sbs1 = new List<cls部门>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sysdata1.Count) break;
                    sbs1.Add(sysdata1[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }

        [HttpPost]
        public ActionResult Depshanchu(string id)
        {
            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            if (s1 == "")
            {
                return Content("900001");
            }

            int i = 0;
            i = main.执行SQL命令("delete from Z部门 where id='" + id + "'");
            if (i < 1)
            {
                return Content("900003");
            }

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }


        [HttpPost]
        public ActionResult SaveDep(string id, string bh, string mc)
        {
            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = bh;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = mc;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";

            int i = 0;
            bool b1 = true;
            cls部门 yh = new cls部门();
            if (s1 == "")  //新增
            {
                yh.ID = Guid.NewGuid().ToString();
                b1 = true;
            }
            else
            {
                yh.ID = s1;
                b1 = false;
            }
            yh.编号 = s2;
            yh.名称 = s3;
            if ((yh.编号.Trim() == "") | (yh.名称.Trim() == ""))
            {
                return Content("401");
            }
            i = Budget.保存部门(b1, yh);
            if (i < 1)
            {
                return Content("402");
            }

            string returnstr = "10";
            return Content(returnstr);
        }

        [HttpPost]
        public ActionResult SaveSysset(string id, string csz)
        {
            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = csz;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";

            int i = 0;
            cls系统参数 yh = new cls系统参数();
            yh.ID = s1;
            yh.参数值 = s2;
            i = Budget.保存系统参数(false, yh);
            if (i < 1)
            {
                return Content("402");
            }

            string returnstr = "10";
            return Content(returnstr);
        }
        #endregion


        #region 成本中心维护
        public ActionResult Costlist(int p)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("成本中心")) return RedirectToAction("Noauthorization", "Home");

            List<cls部门> bms = Budget.获取部门("order by 编号");
            ViewBag.bms = bms;

            List<cls成本中心> sysdata1 = Budget.获取成本中心("");

            TempData["Costlist"] = sysdata1;

            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sysdata1.Count / percount;
                x2 = sysdata1.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls成本中心> sbs1 = new List<cls成本中心>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sysdata1.Count) break;
                    sbs1.Add(sysdata1[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }

        [HttpPost]
        public ActionResult Costshanchu(string id)
        {
            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";

            if (s1 == "")
            {
                return Content("900001");
            }

            int i = 0;
            i = main.执行SQL命令("delete from Z成本中心 where id='" + id + "'");
            if (i < 1)
            {
                return Content("900003");
            }

            string returnstr = "123456789";    //内容要足够长才行
            return Content(returnstr);
        }
        
        [HttpPost]
        public ActionResult SaveCost(string id, string bh, string mc, string bm)
        {
            string s1 = id;
            if ((s1 == string.Empty) | (s1 == null)) s1 = "";
            string s2 = bh;
            if ((s2 == string.Empty) | (s2 == null)) s2 = "";
            string s3 = mc;
            if ((s3 == string.Empty) | (s3 == null)) s3 = "";
            string s4 = bm;
            if ((s4 == string.Empty) | (s4 == null)) s4 = "";

            int i = 0;
            bool b1 = true;
            cls成本中心 yh = new cls成本中心();
            if (s1 == "")  //新增
            {
                yh.ID = Guid.NewGuid().ToString();
                b1 = true;
            }
            else
            {
                yh.ID = s1;
                b1 = false;
            }
            yh.成本中心编号 = s2;
            yh.成本中心名称 = s3;
            yh.部门 = s4;
            if ((yh.成本中心编号.Trim() == "") | (yh.成本中心名称.Trim() == ""))
            {
                return Content("401");
            }
            i = Budget.保存成本中心(b1, yh);
            if (i < 1)
            {
                return Content("402");
            }

            string returnstr = "10";
            return Content(returnstr);
        }

        #endregion


        #region 系统参数
        public ActionResult Syssetlist(int p)
        {
            if (ShowLoginName() == "") return RedirectToAction("Login", "Home");
            if (!登录.登录权限验证1("系统参数")) return RedirectToAction("Noauthorization", "Home");

            List<cls系统参数> sysdata1 = Budget.获取系统参数("");

            TempData["syssetlist"] = sysdata1;

            ViewBag.pagenum = p;

            if (p > 0)
            {
                int i = 0;
                int percount = 20;   //每页20行
                int x1 = 0;
                int x2 = 0;
                x1 = sysdata1.Count / percount;
                x2 = sysdata1.Count % percount;
                if (x2 > 0) x1++;   //总页数
                ViewBag.pagetotal = x1;

                List<cls系统参数> sbs1 = new List<cls系统参数>();
                for (i = 0; i < percount; i++)
                {
                    int k = (p - 1) * percount + i;
                    if (k >= sysdata1.Count) break;
                    sbs1.Add(sysdata1[k]);
                }
                ViewBag.pagedata = sbs1;   //当前页的数据

            }
            return View();
        }
        #endregion




        #endregion
    }
}
