using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LmpSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Kiểm tra nếu chưa tồn tại file thì tạo file Count_Visited.txt
            if (!File.Exists(Server.MapPath("~/Upload/files/Count_Visited.txt")))
                File.WriteAllText(Server.MapPath("~/Upload/files/Count_Visited.txt"), "0");
            Application["DaTruyCap"] = int.Parse(File.ReadAllText(Server.MapPath("~/Upload/files/Count_Visited.txt")));
        }

        void Application_End(object sender, EventArgs e)
        {
        }

        void Application_Error(object sender, EventArgs e)
        {
        }
        void Session_Start(object sender, EventArgs e)
        {
            // Tăng số đang truy cập lên 1 nếu có khách truy cập
            if (Application["DangTruyCap"] == null)
                Application["DangTruyCap"] = 1;
            else
                Application["DangTruyCap"] = (int)Application["DangTruyCap"] + 1;
            // Tăng số đã truy cập lên 1 nếu có khách truy cập
            Application["DaTruyCap"] = (int)Application["DaTruyCap"] + 1;
            File.WriteAllText(Server.MapPath("~/Upload/files/Count_Visited.txt"), Application["DaTruyCap"].ToString());
        }

        void Session_End(object sender, EventArgs e)
        {
            //Khi hết session hoặc người dùng thoát khỏi website thì giảm số người đang truy cập đi 1
            Application["DangTruyCap"] = (int)Application["DangTruyCap"] - 1;
        }
    }
}
