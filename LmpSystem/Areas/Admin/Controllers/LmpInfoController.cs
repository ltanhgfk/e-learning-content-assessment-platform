using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using LmpSystem.Common;
using System.Web.Security;

namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LmpInfoController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());
        GetData data = new GetData();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<LmpInfo> listLmpInfo = db.LmpInfoRepository.GetAll().ToList();

            List<LmpInfoViewModel> LmpInfoVmList = new List<LmpInfoViewModel>();

            foreach (var item in listLmpInfo)
            {
                LmpInfoVmList.Add(new LmpInfoViewModel
                {
                    Id = item.Id,
                    Code = item.Code.ToString(),
                    Avatar = item.Avatar,
                    Name = item.Name,
                    Content = item.Content,
                    Link = item.Link,
                    Status = item.Status                    
                });
            }
            return View(LmpInfoVmList);
        }

        [Authorize(Roles="admin")]
        public ActionResult Create()
        {
            //List<SelectListItem> cateList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            //{
            //    Value = a.UserID.ToString(),
            //    Text = a.Username.ToString()
            //})
            //.ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });

            ////ViewBag.ParentCateList = cateList; 
            
            ViewBag.CodeList = data.GetListInfoCodeEnum();

            LmpInfoEditViewModel model = new LmpInfoEditViewModel
            {
                //Code = Code.web_name,
                Status = true
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(LmpInfoEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            if (ModelState.IsValid)
            {
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                try
                {
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }

                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        model.Avatar = "LmpInfo" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
                        model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                    }
                    else
                    {
                        model.Avatar = "no-image.png";
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully == true)
                {
                    LmpInfo nLmpInfo = new LmpInfo
                    {
                        Code = model.Code.ToString(),//model.CodeList.ToString(),
                        Name = model.Name,
                        Link = model.Link,
                        Avatar = model.Avatar,                       
                        Content = model.Content,                        
                        Status = model.Status,                        
                        CreatedOnDate = DateTime.Today,
                        CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                        LastModifiedOnDate = DateTime.Today,
                        LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id
                    };

                    db.LmpInfoRepository.Add(nLmpInfo);
                    db.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Clone(int id)
        {
            
            //List<SelectListItem> cateList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            //{
            //    Value = a.UserID.ToString(),
            //    Text = a.Username.ToString()
            //})
            //.ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });

            ViewBag.CodeList = data.GetListInfoCodeEnum();

            LmpInfo model = db.LmpInfoRepository.FindByID(id);

            //Enum.TryParse(model.UserRole.ToString(), out Role role);
            Enum.TryParse(model.Code.ToString(), out Code Code);

            LmpInfoEditViewModel LmpInfoVM = new LmpInfoEditViewModel
            {
                Id = model.Id,
                //CodeList = Code,
                Code = Code,
                Name = model.Name,
                Link = model.Link,
                Avatar = model.Avatar,                
                Content = model.Content,                
                Status = model.Status,               

                CreatedOnDate = DateTime.Today,
                CreatedByUser = db.UserRepository.FindByUsername(User.Identity.Name).Username,
                //LastModifiedOnDate = DateTime.Today,
                //LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).UserID,
                
            };
            return View(LmpInfoVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Clone(LmpInfoEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            if (ModelState.IsValid)
            {
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                try
                {
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }

                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        model.Avatar = "LmpInfo" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
                        model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully == true)
                {
                    LmpInfo nLmpInfo = new LmpInfo
                    {
                        Code = model.Code.ToString(),//model.CodeList.ToString(),
                        Name = model.Name,
                        Link = model.Link,
                        Avatar = model.Avatar,                        
                        Content = model.Content,                        
                        Status = model.Status,                        
                        CreatedOnDate = DateTime.Today,
                        CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                        LastModifiedOnDate = DateTime.Today,
                        LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id
                    };
                    db.LmpInfoRepository.Add(nLmpInfo);
                    db.Commit();
                    return RedirectToAction("Index");
                }

            }
            return View();
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            //List<SelectListItem> cateList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            //{
            //    Value = a.UserID.ToString(),
            //    Text = a.Username.ToString()
            //})
            //.ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });

            ViewBag.CodeList = data.GetListInfoCodeEnum();

            LmpInfo model = db.LmpInfoRepository.FindByID(id);

            Enum.TryParse(model.Code, out Code Code);

            ViewBag.ChangeAvatar = false;

            LmpInfoEditViewModel LmpInfoVM = new LmpInfoEditViewModel
            {
                //CodeList = Code,
                Code = Code,
                Name = model.Name,
                Link = model.Link,
                Avatar = model.Avatar,                
                Content = model.Content,                
                Status = model.Status,               

                //CreatedOnDate = DateTime.Today,
                //CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).UserID,
                //LastModifiedOnDate = DateTime.Today,
                //LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).UserID,               
            };
            return View(LmpInfoVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(LmpInfoEditViewModel model)//, bool ChangeAvatar)//, HttpPostedFile AvatarFile)
        {
            if (ModelState.IsValid)
            {
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                try
                {//model.ChangeAvatar == true && ChangeAvatar == true && 
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }
                        //xóa ảnh cũ
                        bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + model.Avatar));
                        if (exist)
                        {
                            if (model.Avatar != "no-image.png")
                            {
                                System.IO.File.Delete(Server.MapPath("~/Upload/images/" + model.Avatar));
                            }
                        }
                        // lưu ảnh mới với tên là slug tiêu đề mới
                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        model.Avatar = "LmpInfo" + "-" + dt + extension;
                        model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully)
                {
                    LmpInfo LmpInfo = db.LmpInfoRepository.FindByID(model.Id);

                    LmpInfo.Id = model.Id;
                    LmpInfo.Code = model.Code.ToString();  //model.CodeList.ToString();
                    LmpInfo.Name = model.Name;
                    LmpInfo.Link = model.Link;
                    LmpInfo.Avatar = model.Avatar;                    
                    LmpInfo.Content = model.Content;                    
                    LmpInfo.Status = model.Status;                   

                    //LmpInfo.CreatedOnDate = DateTime.Today;
                    //LmpInfo.CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).UserID;

                    LmpInfo.LastModifiedOnDate = DateTime.Today;
                    LmpInfo.LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id;

                    db.LmpInfoRepository.Update(LmpInfo);
                    db.Commit();

                    return RedirectToAction("Index");
                }
                return View();
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            LmpInfo model = db.LmpInfoRepository.FindByID(id);

            LmpInfoViewModel LmpInfoVM = new LmpInfoViewModel
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                Link = model.Link,
                Avatar = model.Avatar,                
                Content = model.Content,               
                Status = model.Status,
                
                CreatedOnDate = DateTime.Today,
                CreatedByUser = db.UserRepository.FindByID((int)model.CreatedByUserId).Username,
                LastModifiedOnDate = DateTime.Today,
                LastModifiedByUser = db.UserRepository.FindByID((int)model.LastModifiedByUserId).Username,
                
            };
            return View(LmpInfoVM);
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            try
            {
                //string prefix = state ? "Đã duyệt" : "Đã hủy duyệt";
                LmpInfo u = db.LmpInfoRepository.FindByID(id);

                u.Status = state;
                db.Commit();
                //return Json(new { Message = prefix + " \"" + u.Name + "\"" }, JsonRequestBehavior.AllowGet);
                return Json(new { Message = "<Strong>Đã thay đổi trạng thái thành công!</strong>" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Message = "<Strong>Có lỗi xảy ra (" + e.Message + "), chưa thay đổi trạng thái được!</strong>" }, JsonRequestBehavior.AllowGet);
            }
        }

        ////[Authorize(Roles = "admin")]
        //[HttpPost]
        //public JsonResult ChangeAccepted(int id, bool state = true)
        //{
        //    string prefix = state ? "Đã duyệt" : "Đã hủy duyệt";
        //    LmpInfo u = db.LmpInfoRepository.FindByID(id);
        //    if (u.Name != "admin")//coi lai cho này => dung LmpInforole
        //    {
        //        u.Accepted = state;
        //        db.Commit();
        //        return Json(new { Message = prefix + " \"" + u.Name + "\"" }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { Message = "Không được hủy duyệt admin" }, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            LmpInfo LmpInfoObj = db.LmpInfoRepository.FindByID(id);
            try
            {
                db.LmpInfoRepository.Delete(LmpInfoObj);

                //xóa ảnh đại diện cũ
                bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + LmpInfoObj.Avatar));
                if (exist)
                {
                    if (LmpInfoObj.Avatar != "no-image.png")
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/images/" + LmpInfoObj.Avatar));
                    }
                }

                db.Commit();
                return Json(new { Message = "<strong>Đã xóa thành công!</strong>" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "<strong>Xóa không thành công</strong> </br>!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}