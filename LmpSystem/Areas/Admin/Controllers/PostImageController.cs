using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostImageController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int id) //string titleStr, 
        {
            List<PostImage> listPostImage = db.PostImageRepository.GetAll().Where(m => m.PostId == id).ToList();

            List<PostImageListViewModel> PostImageVmList = new List<PostImageListViewModel>();

            ViewBag.PostId = id;

            foreach (var item in listPostImage)
            {
                PostImageVmList.Add(new PostImageListViewModel
                {
                    Id = item.Id,
                    Avatar = item.Avatar,
                    PostName = (item.Id >= 1 ? db.PostRepository.FindByID((int)item.PostId).Name : "")
                });
            }
            return View(PostImageVmList);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create(int id)
        {
            PostImageCreateViewModel model = new PostImageCreateViewModel
            {
                PostId = id
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(PostImageCreateViewModel model)//, HttpPostImageedFileBase AvatarFile)
        {
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {
                //int i = 0;
                string ImageFileNames = "";
                //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in model.ImageFiles)
                {
                    string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                    //bool isSavedSuccessfully = true;

                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string subPath = Server.MapPath("~/Upload/images/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }

                        string extension = Path.GetExtension(file.FileName);
                        ImageFileNames = "PostImage" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
                        file.SaveAs(Server.MapPath("~/Upload/images/") + ImageFileNames);

                        ViewBag.UploadStatus = model.ImageFiles.Count().ToString() + " files uploaded successfully.";

                        PostImage nPostImage = new PostImage
                        {
                            Avatar = ImageFileNames,
                            PostId = model.PostId
                        };

                        db.PostImageRepository.Add(nPostImage);
                    }                    
                }

                db.Commit();
                return RedirectToAction("Index", "PostImage", new { id = model.PostId });
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            PostImage model = db.PostImageRepository.FindByID(id);

            PostImageEditViewModel PostImageVM = new PostImageEditViewModel
            {
                Avatar = model.Avatar,
                PostId = (int)model.PostId
            };
            return View(PostImageVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(PostImageEditViewModel model)//, bool ChangeAvatar)//, HttpPostImageedFile AvatarFile)
        {
            if (ModelState.IsValid)
            {
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                try
                {//model.ChangeAvatar == true && 
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
                            System.IO.File.Delete(Server.MapPath("~/Upload/images/" + model.Avatar));
                        }
                        // lưu ảnh mới với tên là slug tiêu đề mới
                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        model.Avatar = "PostImage" + "-" + dt + extension;
                        model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully)
                {
                    PostImage PostImage = db.PostImageRepository.FindByID(model.Id);                    

                    PostImage.Id = model.Id;
                    PostImage.Avatar = model.Avatar;
                    //PostImage.PostId = PostId;

                    db.PostImageRepository.Update(PostImage);
                    db.Commit();

                    //return Redirect("PostImage/Index/" + model.PostId);

                    return RedirectToAction("Index","PostImage",new {id= model.PostId });
                }
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            PostImage model = db.PostImageRepository.FindByID(id);

            PostImageListViewModel PostImageVM = new PostImageListViewModel
            {
                Id = model.Id,
                PostName = db.PostRepository.FindByID((int)model.PostId).Name,
                Avatar = model.Avatar,
            };

            return View(PostImageVM);
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            PostImage PostImageObj = db.PostImageRepository.FindByID(id);
            string Name = PostImageObj.Id.ToString();
            try
            {
                db.PostImageRepository.Delete(PostImageObj);

                //xóa ảnh đại diện cũ
                bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + PostImageObj.Avatar));
                if (exist)
                {
                    System.IO.File.Delete(Server.MapPath("~/Upload/images/" + PostImageObj.Avatar));
                }

                db.Commit();
                return Json(new { Message = "Xóa '" + Name + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Có lỗi xảy ra, chưa xóa được '" + Name + "-" + "' <br />." }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}