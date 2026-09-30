using System.Web.Mvc;
using System.Web.Routing;

namespace LmpSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Home_Index",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "LmpSystem.Controllers" }
            );

            //routes.MapRoute(
            //   name: "Home_TestSubjectList",
            //   url: "danh-muc-chinh-trac-nghiem-{id}",
            //   new { controller = "Home", action = "TestSubjectList", id = UrlParameter.Optional },
            //   new[] { "LmpSystem.Controllers" }
            //);

            routes.MapRoute(
              name: "Home_Testquiz",
              url: "de-trac-nghiem-mon-{postname}-{postid}",
              new { controller = "Home", action = "TestQuiz", id = UrlParameter.Optional },
              new[] { "LmpSystem.Controllers" }
           );

            routes.MapRoute(
              name: "Home_CommonPage",
              url: "thong-tin-chung-{code}-{id}",
              new { controller = "Home", action = "CommonPage", id = UrlParameter.Optional, code = UrlParameter.Optional },
              new[] { "LmpSystem.Controllers" }
            );

            routes.MapRoute(
              name: "Home_Introduction",
              url: "gioi-thieu-website",
              new { controller = "Home", action = "Introduction", id = UrlParameter.Optional },
              new[] { "LmpSystem.Controllers" }
            );

            routes.MapRoute(
             name: "Home_Contact",
             url: "lien-he-website",
             new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
             new[] { "LmpSystem.Controllers" }
            );

            //routes.MapRoute(
            //   name: "Home_Testquiz",
            //   url: "kiem-tra-trac-nghiem/{cateid}",
            //   new { controller = "Home", action = "TestQuiz", cateid = UrlParameter.Optional },
            //   new[] { "LmpSystem.Controllers" }
            //);

            routes.MapRoute(
               name: "Home_PostByCate",
               url: "danh-muc-goc-{catename}-{cateid}",
               defaults: new { controller = "Home", action = "PostByCate", cateid = UrlParameter.Optional, catename = UrlParameter.Optional },
               new[] { "LmpSystem.Controllers" }
           );

            routes.MapRoute(
               name: "Home_PostBySubCate",
               url: "danh-muc-con-{subcatename}-{subcateid}",
               defaults: new { controller = "Home", action = "PostBySubCate", id = UrlParameter.Optional, subcatename = UrlParameter.Optional },
               new[] { "LmpSystem.Controllers" }
           );

            routes.MapRoute(
              name: "Home_PostByTag",
              url: "the-tag-{tagname}-{tagid}",
              defaults: new { controller = "Home", action = "PostByTag", tagid = UrlParameter.Optional, tagname = UrlParameter.Optional },
              new[] { "LmpSystem.Controllers" }
            );

            routes.MapRoute(
              name: "Home_PostByType",
              url: "the-loai-bai-viet-{post_type}",
              defaults: new { controller = "Home", action = "PostByType", post_type = UrlParameter.Optional },
              new[] { "LmpSystem.Controllers" }
            );

            routes.MapRoute(
                name: "Home_PostDetail",
                //url: "chi-tiet-bai-viet-{postname}-{postid}-html",
                url: "chi-tiet-bai-viet-{postname}-{postid}-html",
                defaults: new { controller = "Home", action = "PostDetail", postid = UrlParameter.Optional , postname = UrlParameter.Optional },
                new[] { "LmpSystem.Controllers" }
            );
            routes.MapRoute(
                name: "Home_PostDetailByType",
                //url: "chi-tiet-bai-viet-{postname}-{postid}-html",
                url: "bai-viet-chi-tiet-theo-the-loai-{postname}-{postid}-html",
                defaults: new { controller = "Home", action = "PostDetailByType", postid = UrlParameter.Optional, postname = UrlParameter.Optional },
                new[] { "LmpSystem.Controllers" }
            );

            routes.MapRoute(
                name: "Home_LessonDetail",
                url: "bai-hoc-chi-tiet-{lessonname}-{lessonid}-html",
                //url: "bai-hoc-chi-tiet-{lessonname}-{lessonid}-html",
                defaults: new { controller = "Home", action = "LessonDetail", lessonid = UrlParameter.Optional, lessonname = UrlParameter.Optional },
                new[] { "LmpSystem.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "LmpSystem.Controllers" }
            );
        }
    }
}
