using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.Areas.Admin.Controllers
{
    public class TagController : Controller
    {
        private readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        //// GET: Admin
        ////[Authorize(Roles = "admin")]
        //public ActionResult Index()
        //{
        //    //List<TagListViewModel> listTags = db.TagRepository.GetAll()
        //    //    .Select(m => new TagListViewModel
        //    //    {
        //    //        TagID = m.TagID,
        //    //        TagName = m.TagName,
        //    //        PostCount = m.ProductPostTags.Where(n=>n.ProductId >= 1).Count(),
        //    //    }).ToList();
        //    //return View(listTags);

        //    /*******Cach 2*****************/
        //    List<Tag> tagList = db.TagRepository.GetAll().ToList();

        //    List<TagListViewModel> tagVmList = new List<TagListViewModel>();
        //    foreach (var item in tagList)
        //        tagVmList.Add(new TagListViewModel { TagID =item.TagID , TagName = item.TagName });

        //    return View(tagVmList);
        //    //return View();
        //}

        //// GET: Admin/Details/5
        //public ActionResult Details(int id)
        //{
        //    var tag = db.TagRepository.FindByID(id);
        //    TagEditViewModel tagVM = new TagEditViewModel
        //    {
        //        TagID = tag.TagID,
        //        TagName = tag.TagName
        //    };
        //    return View(tagVM);
        //}

        //// GET: Admin/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/Create
        //[HttpPost]
        //public ActionResult Create(TagEditViewModel NewTag)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        Tag tags = new Tag
        //        {
        //            TagName = NewTag.TagName,
        //        };
        //        db.TagRepository.AddTag(tags);
        //        db.TagRepository.SaveChanges();

        //        return RedirectToAction("TagList");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Admin/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    Tag tag = db.TagRepository.FindByID(id);

        //    TagEditViewModel tagVM = new TagEditViewModel
        //    {
        //        TagID = tag.TagID,
        //        TagName = tag.TagName
        //    };
        //    return View(tagVM);
        //}

        //// POST: Admin/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, TagEditViewModel EditTag)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here
        //        Tag tags = db.TagRepository.FindByID(id);
        //        tags.TagName = EditTag.TagName;
        //        db.TagRepository.SaveChanges();
        //        return RedirectToAction("TagList");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Admin/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    var tag = db.TagRepository.FindByID(id);
        //    TagEditViewModel tagVM = new TagEditViewModel
        //    {
        //        TagID = tag.TagID,
        //        TagName = tag.TagName
        //    };
        //    return View(tagVM);
        //}

        //// POST: Admin/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        Tag tags = db.TagRepository.FindByID(id);
        //        db.TagRepository.DeleteTag(tags);
        //        db.TagRepository.SaveChanges();
        //        return RedirectToAction("TagList");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //[Authorize(Roles = "admin")]
        
        public ActionResult Index()
        {
            List<TagListViewModel> listTags = db.TagRepository.GetAll()
                .Select(m => new TagListViewModel
                {
                    TagID = m.TagID,
                    TagName = m.TagName,
                    PostCount = m.Posts.Count,
                }).ToList();
            return View(listTags);
        }
        [HttpPost]
        public JsonResult DeleteTag(int id)
        {
            Tag tags = db.TagRepository.FindByID(id);
            if (tags.Posts.Count > 0)
            {
                Response.StatusCode = 500;
                return Json(new { reload = false, Message = "Tags còn chứa bài viết. Không xóa được!" }, JsonRequestBehavior.AllowGet);
            }
            db.TagRepository.Delete(tags);
            db.TagRepository.SaveChanges();
            return Json(new { reload = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateTag(int id, string name)
        {
            Tag tags = db.TagRepository.FindByID(id);
            if (String.IsNullOrWhiteSpace(name))
            {
                Response.StatusCode = 500;
                return Json(new { reload = true, Message = "Chưa nhập tên" }, JsonRequestBehavior.AllowGet);
            }
            tags.TagName = name;
            db.TagRepository.SaveChanges();
            return Json(new { reload = true, Message = "Sửa '" + tags.TagName + "' thành công" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult NewTag(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                Response.StatusCode = 500;
                return Json(new { reload = true, Message = "Chưa nhập tên" }, JsonRequestBehavior.AllowGet);
            }
            foreach(var item in name.Split(',').Where(s => s.Trim() != string.Empty).ToArray())
            {
                Tag tags = new Tag
                {
                    TagName = item,
                };
                db.TagRepository.Add(tags);
            }
            
            db.TagRepository.SaveChanges();
            return Json(new { reload = true, Message = "Thêm các thẻ thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}
