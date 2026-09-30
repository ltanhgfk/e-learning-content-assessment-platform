using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using PagedList;

namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int post_type=1)
        {
            var enumDisplayStatus = ((PostType)post_type).ToString();
            string stringValue = enumDisplayStatus.ToString();
            List<Post> listPost = null;
            listPost = db.PostRepository.GetAll().Where(m => m.PostType == stringValue).OrderBy(m => m.CreatedOnDate).ToList();            
            List<PostDetailViewModel> PostVmList = new List<PostDetailViewModel>();
            foreach (var item in listPost)
            {
                Enum.TryParse(item.PostType, out PostType PostType);
                PostVmList.Add(new PostDetailViewModel
                {
                    Id = item.Id,
                    Avatar = item.Avatar,
                    Code =  item.Code,
                    Name = item.Name,
                    Pagelink = item.Pagelink,
                    Status = item.Status,
                    Position = item.Position,
                    CateName = db.CategoryRepository.FindByID(item.CateId).Name,
                    BeginDate = string.Format("{0:dd/MM/yyyy}",item.BeginDate),
                    PostType = Function.GetEnumDisplayName(PostType),//PostType.ToString() //item.PostType.ToString()
                    Lessons = db.LessonRepository.GetAll().Where(m => m.PostId == item.Id).ToList(),
                    Questions = item.Questions,
                });
            }
            
            return View(PostVmList);
        }

        //Index có pageList
        //public ActionResult Index(int? page, int post_type = 1) //string titleStr, 
        //{
        //    int pageSize = 5;
        //    int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

        //    var enumDisplayStatus = ((PostType)post_type).ToString();
        //    string stringValue = enumDisplayStatus.ToString();

        //    IPagedList<Post> listPost = null;

        //    listPost = db.PostRepository.GetAll().Where(m => m.PostType == stringValue).OrderBy(m => m.CreatedOnDate).ToPagedList(pageIndex, pageSize);

        //    //List<Post> listPost = db.PostRepository.GetAll().ToList();

        //    //if (String.IsNullOrWhiteSpace(titleStr))
        //    //{
        //    //    listPost = db.PostRepository.GetAll().Where(m => m.PostType == post_type).OrderBy(m => m.CreatedOnDate).ToPagedList(pageIndex, pageSize);
        //    //    //listPost = db.PostRepository.GetAll().OrderBy(m => m.CreatedOnDate).ToPagedList(pageIndex, pageSize);
        //    //}
        //    //else
        //    //{
        //    //    listPost = db.PostRepository.GetAll().Where(m => m.Name.ToLower().Contains(titleStr.ToLower())).Where(m => m.PostType == post_type).OrderBy(m => m.CreatedOnDate).ToPagedList(pageIndex, pageSize);
        //    //    //listPost = db.PostRepository.GetAll().Where(m => m.Name.ToLower().Contains(titleStr.ToLower())).OrderBy(m => m.CreatedOnDate).ToPagedList(pageIndex, pageSize);
        //    //}

        //    List<PostDetailViewModel> PostVmList = new List<PostDetailViewModel>();

        //    foreach (var item in listPost)
        //    {
        //        Enum.TryParse(item.PostType, out PostType PostType);

        //        //var Status = GetEnumDisplayName((PostType)PostType);

        //        PostVmList.Add(new PostDetailViewModel
        //        {
        //            Id = item.Id,
        //            Avatar = item.Avatar,
        //            Code = item.Code,
        //            Name = item.Name,
        //            Pagelink = item.Pagelink,
        //            Status = item.Status,
        //            Position = item.Position,
        //            CateName = db.CategoryRepository.FindByID(item.CateId).Name,
        //            BeginDate = string.Format("{0:dd/MM/yyyy}", item.BeginDate),
        //            PostType = Function.GetEnumDisplayName(PostType),//PostType.ToString() //item.PostType.ToString()
        //        });
        //    }

        //    IPagedList<PostDetailViewModel> pageOrders = new StaticPagedList<PostDetailViewModel>(PostVmList, pageIndex, pageSize, db.PostRepository.GetAll().Where(m => m.PostType == stringValue).Count());

        //    return View(pageOrders);
        //    //return View(listPost);
        //}

        //public ActionResult UploadFiles()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult UploadFiles(PostImageViewModel model)
        //{
        //    //Ensure model state is valid  
        //    if (ModelState.IsValid)
        //    {   //iterating through multiple file collection   
        //        foreach (HttpPostedFileBase file in model.files)
        //        {
        //            //Checking file is available to save.  
        //            if (file != null)
        //            {
        //                var InputFileName = Path.GetFileName(file.FileName);
        //                var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
        //                //Save file to server folder  
        //                file.SaveAs(ServerSavePath);
        //                //assigning file uploaded status to ViewBag for showing message to user.  
        //                ViewBag.UploadStatus = model.files.Count().ToString() + " files uploaded successfully.";
        //            }
        //        }
        //    }
        //    return View();
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            List<SelectListItem> userList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Username.ToString()
            })
           .ToList();
            //userList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });                      

            List<SelectListItem> cateList = db.CategoryRepository.GetAll().Where(m => m.ParentId == -1).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
           .ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn danh mục" });

            int max = 0;
            if (db.PostRepository.GetAll().Count() >=1 )
            {
                max = (int)db.PostRepository.GetAll().Select(m => m.Position).Max();
            }

            PostEditViewModel model = new PostEditViewModel
            {
                PostType = PostType.article,
                TagList = GetData.GetListTags(), //PostData.getTagList(),
                Status = true,
                CateList = cateList,
                Users = userList,                
                //ChangeAvatar = true,
                Position = max + 1
            };

            return View(model);           
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(PostEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            List<Tag> taglist = new List<Tag>();
            taglist.AddRange(model.TagList.Where(m => m.Selected)
                .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
                );

            if (ModelState.IsValid)
            {
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                try
                {
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        //".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG"
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == "jpge" || extension.ToLower() == ".jpe" || extension.ToLower() == ".bmp" || extension.ToLower() == ".gif" || extension.ToLower() == ".png")
                        {
                            string subPath = Server.MapPath("~/Upload/images/");
                            bool exists = System.IO.Directory.Exists(subPath);
                            if (!exists)
                            {
                                System.IO.Directory.CreateDirectory(subPath);
                            }

                            model.Avatar = "Post" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;

                            int width = 464;
                            int height = 262;
                            if (model.PostType.ToString() == "special")
                            {
                                width = 980;
                                height = 450;
                            }
                            //Resize image
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), width, height);
                            //image.Save(System.IO.Path.GetPathRoot() + "\\Image.jpg", ImageFormat.Jpeg);                        
                            img.Save(Server.MapPath("~/Upload/images/") + model.Avatar);
                            //model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                        }
                        else
                        {
                            TempData["error"] = "Ảnh đại diện bạn tải lên không đúng định dạng ảnh!";
                            //return View();
                        }                            
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
                    try
                    {
                        Post nPost = new Post
                        {
                            PostType = model.PostType.ToString(),
                            CateId = model.CateId,
                            Code = model.Code,
                            Name = model.Name,
                            Title = model.Title,
                            Pagelink = model.Pagelink,
                            Avatar = model.Avatar,
                            Video = model.Video,
                            Resume = model.Resume,
                            Content = model.Content,
                            Position = model.Position,
                            TotalTime = model.TotalTime,
                            Price = model.Price,
                            BeginDate = model.BeginDate,
                            Discount = model.Discount,
                            Quantity = model.Quantity,
                            Status = model.Status,
                            Id = model.Id,
                            UserId = model.UserId,

                            CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                            LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                            CreatedOnDate = DateTime.Today,
                            LastModifiedOnDate = DateTime.Today
                        };

                        //foreach (var i in taglist)
                        //{
                        //    PostTag PostTagObj = new PostTag { PostId = model.Id, TagId = i.TagID };
                        //    //db.PostTagRepository.Add(PostTagObj);
                        //}

                        foreach (var i in taglist)
                        {
                            Tag tags = db.TagRepository.FindByID(i.TagID);
                            nPost.Tags.Add(tags);
                            tags.Posts.Add(nPost);
                        }

                        //foreach (var i in taglist)
                        //{
                        //    Tag tags = db.tagRepository.FindByID(i.TagID);
                        //    pOST.Tbl_Tags.Add(tags);
                        //    tags.Tbl_POST.Add(pOST);
                        //}

                        db.PostRepository.Add(nPost);
                        db.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        TempData["error"] = "Có lỗi xảy ra khi thêm dữ liệu, lỗi: " + e.Message;
                        return RedirectToAction("Create");
                    }
                }
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Clone(int id)
        {
            Post model = db.PostRepository.FindByID(id);
            int ParentId = (int)db.CategoryRepository.FindByID(model.CateId).ParentId;

            List<SelectListItem> userList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Username.ToString()
            }).ToList();

            List<SelectListItem> superCateList = db.CategoryRepository.GetAll().Where(x => x.ParentId == -1).Select(a => new SelectListItem()   //db.CategoryRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            }).ToList();

            ViewBag.SuperCateList = superCateList;

            List<SelectListItem> cateList = db.CategoryRepository.GetAll().Where(x => x.ParentId == ParentId).Select(a => new SelectListItem()    //db.CategoryRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            }).ToList();            

            Enum.TryParse(model.PostType.ToString(), out PostType PostType);

            ViewBag.ChangeAvatar = false;

            List<SelectListItem> tag = GetData.GetListTags();

            foreach (var x in tag)
            {
                //foreach (var t in model.PostTags)
                foreach (var t in model.Tags)
                {
                    if (x.Value == t.TagID.ToString())
                    {
                        x.Selected = true;
                    }
                }
            }

            int max = 0;
            if (db.PostRepository.GetAll().Count() >= 1)
            {
                max = (int)db.PostRepository.GetAll().Select(m => m.Position).Max();
            }

            PostEditViewModel PostVM = new PostEditViewModel
            {
                PostType = PostType,
                SuperCateId = ParentId,
                CateId = model.CateId,
                Code = model.Code,
                Name = model.Name,
                Title = model.Title,
                Pagelink = model.Pagelink,
                Avatar = model.Avatar,
                Video = model.Video,
                Resume = model.Resume,
                Content = model.Content,
                Position = max + 1,
                TotalTime = model.TotalTime,
                Price = model.Price,
                BeginDate = model.BeginDate,
                Discount = model.Discount,
                Quantity = model.Quantity,
                Status = model.Status,
                Id = (int)model.Id,
                UserId = (int)model.UserId,

                CateList = cateList,
                Users = userList,
                TagList = tag,  //GetData.GetListTags(),

                //CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                //LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                //CreatedOnDate = DateTime.Today,
                //LastModifiedOnDate = DateTime.Today
            };
            return View(PostVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Clone(PostEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            List<Tag> taglist = new List<Tag>();
            taglist.AddRange(model.TagList.Where(m => m.Selected)
                .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
                );

            if (ModelState.IsValid)
            {
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                try
                {
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        //".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG"
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == "jpge" || extension.ToLower() == ".jpe" || extension.ToLower() == ".bmp" || extension.ToLower() == ".gif" || extension.ToLower() == ".png")
                        {
                            string subPath = Server.MapPath("~/Upload/images/");
                            bool exists = System.IO.Directory.Exists(subPath);
                            if (!exists)
                            {
                                System.IO.Directory.CreateDirectory(subPath);
                            }

                            model.Avatar = "Post" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;

                            int width = 464;
                            int height = 262;
                            if (model.PostType.ToString() == "special")
                            {
                                width = 980;
                                height = 450;
                            }
                            //Resize image
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), width, height);
                            //image.Save(System.IO.Path.GetPathRoot() + "\\Image.jpg", ImageFormat.Jpeg);                        
                            img.Save(Server.MapPath("~/Upload/images/") + model.Avatar);
                            //model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                        }
                        else
                        {
                            TempData["error"] = "Ảnh đại diện bạn tải lên không đúng định dạng ảnh!";
                            //return View();
                        }
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully == true)
                {
                    Post nPost = new Post
                    {
                        PostType = model.PostType.ToString(),
                        CateId = model.CateId,
                        Code = model.Code,
                        Name = model.Name,
                        Title = model.Title,
                        Pagelink = model.Pagelink,
                        Avatar = model.Avatar,
                        Video = model.Video,
                        Resume = model.Resume,
                        Content = model.Content,
                        Position = model.Position,
                        TotalTime = model.TotalTime,
                        Price = model.Price,
                        BeginDate = model.BeginDate,
                        Discount = model.Discount,
                        Quantity = model.Quantity,
                        Status = model.Status,
                        Id = model.Id,
                        UserId = model.UserId,

                        CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                        LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                        CreatedOnDate = DateTime.Today,
                        LastModifiedOnDate = DateTime.Today
                    };

                    //foreach (var i in taglist)
                    //{
                    //    PostTag PostTagObj = new PostTag { PostId = model.Id, TagId = i.TagID };
                    //    //db.PostTagRepository.Add(PostTagObj);
                    //}

                    foreach (var i in taglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        nPost.Tags.Add(tags);
                        tags.Posts.Add(nPost);
                    }

                    db.PostRepository.Add(nPost);
                    db.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult TransferToPost(int lessonid)
        {
            Lesson model = db.LessonRepository.FindByID(lessonid);
            //int ParentId = (int)db.CategoryRepository.FindByID(model.CateId).ParentId;
            List<SelectListItem> tag = GetData.GetListTags();

            TempData["lesson_id"] = lessonid;
            TempData.Keep();

            foreach (var x in tag)
            {
                //foreach (var t in model.PostTags)
                foreach (var t in model.Tags)
                {
                    if (x.Value == t.TagID.ToString())
                    {
                        x.Selected = true;
                    }
                }
            }

            List<SelectListItem> userList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Username.ToString()
            })
           .ToList();
            //userList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });                      

            List<SelectListItem> cateList = db.CategoryRepository.GetAll().Where(m => m.ParentId == -1).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
           .ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn danh mục" });

            int max = 0;
            if (db.PostRepository.GetAll().Count() >= 1)
            {
                max = (int)db.PostRepository.GetAll().Select(m => m.Position).Max();
            }

            PostEditViewModel PostVM = new PostEditViewModel
            {
                PostType = PostType.article,
                //SuperCateId = ParentId,
                //CateId = model.CateId,
                //Code = model.Code,
                Name = model.Name,
                //Title = model.Title,
                //Pagelink = model.Pagelink,
                Avatar = model.Avatar,
                Video = model.Video,
                Resume = model.Resume,
                Content = model.Content,
                Position = max + 1,
                //TotalTime = model.TotalTime,
                //Price = model.Price,
                //BeginDate = model.BeginDate,
                //Discount = model.Discount,
                //Quantity = model.Quantity,
                Status = model.Status,
                //Id = (int)model.Id,
                //UserId = (int)model.UserId,

                CateList = cateList,
                Users = userList,
                TagList = tag,  //GetData.GetListTags(),

                //CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                //LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                //CreatedOnDate = DateTime.Today,
                //LastModifiedOnDate = DateTime.Today
            };
            return View(PostVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult TransferToPost(PostEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            int lesson_id = Convert.ToInt32(TempData["lesson_id"].ToString());

            List<Tag> taglist = new List<Tag>();
            taglist.AddRange(model.TagList.Where(m => m.Selected)
                .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
                );

            if (ModelState.IsValid)
            {
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                try
                {
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        //".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG"
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == "jpge" || extension.ToLower() == ".jpe" || extension.ToLower() == ".bmp" || extension.ToLower() == ".gif" || extension.ToLower() == ".png")
                        {
                            string subPath = Server.MapPath("~/Upload/images/");
                            bool exists = System.IO.Directory.Exists(subPath);
                            if (!exists)
                            {
                                System.IO.Directory.CreateDirectory(subPath);
                            }

                            model.Avatar = "Post" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;

                            int width = 464;
                            int height = 262;
                            if (model.PostType.ToString() == "special")
                            {
                                width = 980;
                                height = 450;
                            }
                            //Resize image
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), width, height);
                            //image.Save(System.IO.Path.GetPathRoot() + "\\Image.jpg", ImageFormat.Jpeg);                        
                            img.Save(Server.MapPath("~/Upload/images/") + model.Avatar);
                            //model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                        }
                        else
                        {
                            TempData["error"] = "Ảnh đại diện bạn tải lên không đúng định dạng ảnh!";
                            //return View();
                        }
                    }                    
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully == true)
                {
                    Post nPost = new Post
                    {
                        PostType = model.PostType.ToString(),
                        CateId = model.CateId,
                        Code = model.Code,
                        Name = model.Name,
                        Title = model.Title,
                        Pagelink = model.Pagelink,
                        Avatar = model.Avatar,
                        Video = model.Video,
                        Resume = model.Resume,
                        Content = model.Content,
                        Position = model.Position,
                        TotalTime = model.TotalTime,
                        Price = model.Price,
                        BeginDate = model.BeginDate,
                        Discount = model.Discount,
                        Quantity = model.Quantity,
                        Status = model.Status,
                        Id = model.Id,
                        UserId = model.UserId,

                        CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                        LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                        CreatedOnDate = DateTime.Today,
                        LastModifiedOnDate = DateTime.Today
                    };

                    //foreach (var i in taglist)
                    //{
                    //    PostTag PostTagObj = new PostTag { PostId = model.Id, TagId = i.TagID };
                    //    //db.PostTagRepository.Add(PostTagObj);
                    //}

                    foreach (var i in taglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        nPost.Tags.Add(tags);
                        tags.Posts.Add(nPost);
                    }

                    db.PostRepository.Add(nPost);
                    db.PostRepository.SaveChanges();

                    var maxPost = db.PostRepository.GetAll().OrderByDescending(m => m.Id).Take(1).FirstOrDefault();
                    var question = db.QuestionRepository.GetAll().Where(m => m.LessonId == lesson_id).ToList();
                    foreach (var item in question)
                    {
                        item.PostId = maxPost.Id;
                        item.LessonId = null;
                        db.QuestionRepository.Update(item);
                    }
                    db.LessonRepository.Delete(db.LessonRepository.FindByID(lesson_id));

                    db.Commit();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            Post model = db.PostRepository.FindByID(id);
            int ParentId = (int)db.CategoryRepository.FindByID(model.CateId).ParentId;

            List<SelectListItem> userList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Username.ToString()
            })
         .ToList();

            List<SelectListItem> superCateList = db.CategoryRepository.GetAll().Where(x => x.ParentId == -1).Select(a => new SelectListItem()   //db.CategoryRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
          .ToList();

            ViewBag.SuperCateList = superCateList;

            List<SelectListItem> cateList = db.CategoryRepository.GetAll().Where(x => x.ParentId == ParentId).Select(a => new SelectListItem()    //db.CategoryRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
            .ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn danh mục" });


            Enum.TryParse(model.PostType, out PostType PostType);

            ViewBag.ChangeAvatar = false;

            List<SelectListItem> tag = GetData.GetListTags();
            
            foreach (var x in tag)
            {
                //foreach (var t in model.PostTags)
                foreach (var t in model.Tags)
                {
                    if (x.Value == t.TagID.ToString())
                    {
                        x.Selected = true;
                    }
                }
            }

            int max = 0;
            if (db.PostRepository.GetAll().Count() >= 1)
            {
                max = (int)db.PostRepository.GetAll().Select(m => m.Position).Max();
            }

            int pos = 1;

            if (model.Position != null)
                pos = (int)model.Position;
            else pos = max + 1;

            PostEditViewModel PostVM = new PostEditViewModel
            {
                PostType = PostType,
                SuperCateId = ParentId,//(int)db.CategoryRepository.FindByID(model.CateId).ParentId,
                CateId = model.CateId,
                Code = model.Code,
                Name = model.Name,
                Title = model.Title,
                Pagelink = model.Pagelink,
                Avatar = model.Avatar,
                Video = model.Video,
                Resume = model.Resume,
                Content = model.Content,
                Position = pos,     // model.Position,
                TotalTime = model.TotalTime,
                Price = model.Price,
                BeginDate = model.BeginDate,
                Discount = model.Discount,
                Quantity = model.Quantity,
                Status = model.Status,
                Id = (int)model.Id,
                UserId = (int)model.UserId,

                CateList = cateList,
                Users = userList,
                TagList = tag,

                //CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                //LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                //CreatedOnDate = DateTime.Today,
                //LastModifiedOnDate = DateTime.Today
            };
            return View(PostVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(PostEditViewModel model)//, bool ChangeAvatar)//, HttpPostedFile AvatarFile)
        {
            List<Tag> taglist = new List<Tag>();
            taglist.AddRange(model.TagList.Where(m => m.Selected)
                .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
                );

            List<Tag> untaglist = new List<Tag>();
            untaglist.AddRange(model.TagList.Where(m => m.Selected == false)
                .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
                );

            if (ModelState.IsValid)
            {
                //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
                bool isSavedSuccessfully = true;
                string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                try
                {//model.ChangeAvatar == true && 
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(model.AvatarFile.FileName);
                        //".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG"
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == "jpge" || extension.ToLower() == ".jpe" || extension.ToLower() == ".bmp" || extension.ToLower() == ".gif" || extension.ToLower() == ".png")
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

                            model.Avatar = "Post" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;

                            int width = 464;
                            int height = 262;
                            if (model.PostType.ToString() == "special")
                            {
                                width = 980;
                                height = 450;
                            }
                            //Resize image
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), width, height);
                            //image.Save(System.IO.Path.GetPathRoot() + "\\Image.jpg", ImageFormat.Jpeg);                        
                            img.Save(Server.MapPath("~/Upload/images/") + model.Avatar);
                            //model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                        }
                        else
                        {
                            TempData["error"] = "Ảnh đại diện bạn tải lên không đúng định dạng ảnh!";
                            //return View();
                        }
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully)
                {
                    Post Post = db.PostRepository.FindByID(model.Id);

                    Post.Id = model.Id;
                    Post.PostType = model.PostType.ToString();
                    Post.Code = model.Code;
                    Post.Name = model.Name;
                    Post.Title = model.Title;
                    Post.Pagelink = model.Pagelink;
                    Post.Avatar = model.Avatar;                    
                    Post.Content = model.Content;                    
                    Post.Status = model.Status;
                    Post.Position = model.Position;
                    Post.Resume = model.Resume;
                    Post.Video = model.Video;
                    Post.CateId = model.CateId;

                    Post.TotalTime = model.TotalTime;
                    Post.Price = model.Price;
                    Post.BeginDate = model.BeginDate;
                    Post.Discount = model.Discount;
                    Post.Quantity = model.Quantity;
                    Post.Id = model.Id;
                    Post.UserId = model.UserId;

                    //Post.CreatedOnDate = DateTime.Today;
                    //Post.CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id;
                    Post.LastModifiedOnDate = DateTime.Today;
                    Post.LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id;

                    foreach (var i in taglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        if (!Post.Tags.Contains(tags))
                        {
                            Post.Tags.Add(tags);
                            tags.Posts.Add(Post);
                        }
                        //else if (Post.Tags.Contains(tags))
                        //{
                        //    Post.Tags.Remove(tags);
                        //    tags.Posts.Remove(Post);
                        //}
                    }

                    foreach (var i in untaglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        if (Post.Tags.Contains(tags))
                        {
                            Post.Tags.Remove(tags);
                            tags.Posts.Remove(Post);
                        }
                    }


                    ///------------------------------------------------------------------------------------------//

                    //List<PostTag> ptList = db.PostTagRepository.GetAll().Where(m => m.PostId == Post.Id).ToList();

                    //foreach (var i in ptList)
                    //{
                    //    PostTag pt = db.PostTagRepository.FindByID(1);
                    //    db.PostTagRepository.Delete(pt);//(db.PostTagRepository.FindByID(i.Id));
                    //}
                    //foreach (var i in taglist)
                    //{
                    //    PostTag PostTagObj = new PostTag { PostId = model.Id, TagId = i.TagID };
                    //    db.PostTagRepository.Add(PostTagObj);
                    //}

                    //--------------------------------------------------------------------------------//

                    //foreach (var i in taglist)
                    //{
                    //    Tag tags = db.tagRepository.FindByID(i.TagID);
                    //    if (!pOST.Tbl_Tags.Contains(tags))
                    //    {
                    //        pOST.Tbl_Tags.Add(tags);
                    //        tags.Tbl_POST.Add(pOST);
                    //    }
                    //    //else if(pOST.Tbl_Tags.Contains(tags))
                    //    //{
                    //    //    pOST.Tbl_Tags.Remove(tags);
                    //    //    tags.Tbl_POST.Remove(pOST);
                    //    //}

                    //}
                    //foreach (var i in untaglist)
                    //{
                    //    Tag tags = db.tagRepository.FindByID(i.TagID);
                    //    if (pOST.Tbl_Tags.Contains(tags))
                    //    {
                    //        pOST.Tbl_Tags.Remove(tags);
                    //        tags.Tbl_POST.Remove(pOST);
                    //    }
                    //}

                    db.PostRepository.Update(Post);
                    db.Commit();
                    return RedirectToAction("Index");
                }
                //return View();
            }
            return View();
        }
        //TransferToLesson


        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            Post model = db.PostRepository.FindByID(id);
            

            PostDetailViewModel PostVM = new PostDetailViewModel
            {
                Id = model.Id,
                PostType = model.PostType,
                CateName = db.CategoryRepository.FindByID(model.CateId).Name,
                Code = model.Code,
                Name = model.Name,
                Title = model.Title,
                Pagelink = model.Pagelink,
                Avatar = model.Avatar,
                Content = model.Content,
                Status = model.Status,
                Resume = model.Resume,
                Video = model.Video,
                Position = model.Position,
                TotalTime = model.TotalTime,
                Price = model.Price,
                BeginDate = string.Format("{0:dd/MM/yyyy}", model.BeginDate),
                Discount = model.Discount,
                Quantity = model.Quantity,

                Username = db.UserRepository.FindByID((int)model.UserId).Username.ToString(),
                CreatedByUserId = (int)model.LastModifiedByUserId,
                CreatedOnDate = DateTime.Today,
                LastModifiedOnDate = DateTime.Today,
                LastModifiedByUserId = (int)model.LastModifiedByUserId,                

                Reviews = db.PostReviewRepository.GetAll().Where(m => m.PostId == model.Id).ToList(),
                Lessons = db.LessonRepository.GetAll().Where(m => m.PostId == model.Id).ToList(),
                //PostTags =       
                //PostImages = 
            };
            ViewBag.LastModifiedByUser = db.UserRepository.FindByID((int)model.LastModifiedByUserId).Username.ToString();
            ViewBag.CreatedByUser = db.UserRepository.FindByID((int)model.CreatedByUserId).Username.ToString();

            return View(PostVM);
        }

        //public List<Invoice> GetInvoiceListByPost()
        //{
        //    List<Invoice> InList = db.InvoiceRepository().InvoiceListByPost().ToList();
        //    return InList;
        //}

        //public List<Post> GetPostListByPost()
        //{
        //    List<Post> InList = db.PostRepository.PostListByPost().ToList();
        //    return InList;
        //}

        //public List<Product> GetProductListByPost()
        //{
        //    List<Product> InList = db.ProductRepository.ProductListByPost().ToList();
        //    return InList;
        //}

        //public List<Post> GetPostListByPost()
        //{
        //    List<Post> InList = db.PostRepository.PostListByPost().ToList();
        //    return InList;
        //}

        //public List<PostScore> GetPostScoreListByPost()
        //{
        //    List<PostScore> InList = db.PostScoreRepository.ScoreListByPost().ToList();
        //    return InList;
        //}
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            string prefix = state ? "Đã hiển thị" : "Đã ẩn";
            Post u = db.PostRepository.FindByID(id);
            try
            {
                u.Status = state;
                db.Commit();
                return Json(new { Message = prefix + " \"" + u.Name + "\" thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Có lỗi xảy ra!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult GetCateByPostType(int id)
        {
            try
            {
                var enumDisplayStatus = ((PostType)id).ToString();
                string stringValue = enumDisplayStatus.ToString();
               
                List<SelectListItem> cateList = db.CategoryRepository.GetAll().Where(x => x.CateType == stringValue && x.ParentId == -1).Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name.ToString()
                }).ToList();
                return Json(cateList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Message = "Xảy ra lỗi: " + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult GetSubCate(int id)
        {
            try
            {
                List<SelectListItem> cateList = db.CategoryRepository.GetAll().Where(x => x.ParentId == id).Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name.ToString()
                }).ToList();
                return Json(cateList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Message = "Xảy ra lỗi: " + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult Delete(int id)//xóa post phải xóa các thông tin bảng khác liên quan, không thì không được xóa
        {
            Post PostObj = db.PostRepository.FindByID(id);
            string Name = PostObj.Name;
            try
            {
                db.PostRepository.Delete(PostObj);

                //xóa ảnh đại diện cũ
                bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + PostObj.Avatar));
                if (exist)
                {
                    if (PostObj.Avatar != "no-image.png")
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/images/" + PostObj.Avatar));
                    }
                }

                db.Commit();
                return Json(new { Message = "Xóa '" + Name + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "<strong>Xóa không thành công!</strong> </br> Lỗi có thể do bài viết này còn chứa các thông tin khác chưa xóa, để xóa được trước tiên cần xóa các dữ liệu liên quan trước!" }, JsonRequestBehavior.AllowGet);
                //return Json(new { Message = "Không thể xóa '" + Name + "-" + "' <br />." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult DeleteQuestion(int id)//xóa post phải xóa các thông tin bảng khác liên quan, không thì không được xóa
        {
            //Post PostObj = db.PostRepository.FindByID(id);
            //string Name = PostObj.Name;
            try
            {
                //db.PostRepository.Delete(PostObj);
                db.QuestionRepository.DeleteByPost(id);
                //xóa ảnh đại diện cũ
                //bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + PostObj.Avatar));
                //if (exist)
                //{
                //    if (PostObj.Avatar != "no-image.png")
                //    {
                //        System.IO.File.Delete(Server.MapPath("~/Upload/images/" + PostObj.Avatar));
                //    }
                //}

                db.Commit();
                return Json(new { Message = "Xóa toàn bộ câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "<strong>Xóa không thành công!</strong> </br> Lỗi có thể do bài viết này còn chứa các thông tin khác chưa xóa, để xóa được trước tiên cần xóa các dữ liệu liên quan trước!" }, JsonRequestBehavior.AllowGet);
                //return Json(new { Message = "Không thể xóa '" + Name + "-" + "' <br />." }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Cate/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestion(int id)
        {
            //GetData data = new GetData();
            //ViewBag.PostType = data.GetListPostType();

            //var cateList = db.CategoryRepository.GetAll().Where(m => m.CateType == "quizze" && m.ParentId != -1).Select(a => new SelectListItem()
            //{
            //    Value = a.Id.ToString(),
            //    Text = a.Name.ToString()
            //})
            //.ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn môn học" });
            //ViewBag.ParentCateList = cateList;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////

            // var ParentCateList = db.CategoryRepository.GetAll().Where(m => m.ParentId == -1).Select(a => new SelectListItem()
            // {
            //     Value = a.Id.ToString(),
            //     Text = a.Name.ToString()
            // })
            //.ToList();

            //ParentCateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn danh mục cha" });

            //ViewBag.ParentCateList = ParentCateList;

            Post model = db.PostRepository.FindByID(id);

            //Enum.TryParse(model.CateType, out PostType CateType);

            PostEditViewModel CateEditVM = new PostEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                //Icon = model.Icon,
                //ParentId = (int)model.ParentId,
                //Position = model.Position,
                //CateType = data.GetListPostType(),
                //Status = model.Status,
                QuestionFile = model.QuestionFile
            };
            return View(CateEditVM);
        }

        // POST: Admin/Cate/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestion(PostEditViewModel model)
        {
            //int PostId = Convert.ToInt32(Request.Form["PostId"]);

            //if (ModelState.IsValid)//Kiểm tra trong PostEditViewModel mấy trường có required không, nếu có mà view không có control tương ứng thì gặp lỗi chỗ này invalid.
            //{
            string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                bool isSavedSuccessfully = true;
                try
                {
                    if (model.UploadedQuestionFile != null && model.UploadedQuestionFile.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
                        if (extension == ".txt")
                        {
                            string subPath = Server.MapPath("~/Upload/files/");
                            bool exists = System.IO.Directory.Exists(subPath);
                            if (!exists)
                            {
                                System.IO.Directory.CreateDirectory(subPath);
                            }
                            //string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
                            model.QuestionFile = "Question" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
                            model.UploadedQuestionFile.SaveAs(Server.MapPath("~/Upload/files/") + model.QuestionFile);
                        }
                        else
                        {
                            ViewBag.error = "Thể loại tập tin không phù hợp, phải chọn loại tập tin '.txt'!";
                            return View();
                        }
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully == true)
                {
                    try
                    {
                        Post post = db.PostRepository.FindByID(model.Id);

                        bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/files/" + post.QuestionFile));
                        if (exist)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Upload/files/" + post.QuestionFile));
                        }

                        post.Name = model.Name;
                        //Cate.ParentId = model.ParentId;
                        //Cate.Position = model.Position;
                        //Cate.CateType = model.CateType.ToString();
                        //Cate.Status = model.Status;
                        //Cate.Icon = model.Icon;
                        post.QuestionFile = model.QuestionFile;

                        db.PostRepository.Update(post);

                        //********************Khuc luu question vao csdl************************************************************/
                        string QuestionContent = "";
                        string Option1 = "";
                        string Option2 = "";
                        string Option3 = "";
                        string Option4 = "";
                        string Answer = "";
                        string AnswerExplain = "";
                        string line;

                        string path = Server.MapPath("~/Upload/files/" + post.QuestionFile);

                        int counter = 0;

                        string[] readText = System.IO.File.ReadAllLines(path).Where(s => s.Trim() != string.Empty).ToArray();

                        foreach (string s in readText)
                        {
                            //line = file.ReadLine().Trim();
                            line = Regex.Replace(s.Trim(), @"\s+", " ");
                            //line = Regex.Replace(line.Trim(), @"\s+", " ");

                            if (line.ToLower().Contains("<cauhoi>"))
                            {
                                QuestionContent = line.Substring(8, line.Length - 8);
                            }
                            else if (line.ToLower().Contains("<*>"))
                            {
                                if (line.ToLower().Contains("<*>a."))
                                {
                                    Answer = "Option1";
                                    Option1 = line.Substring(5, line.Length - 5);
                                }
                                else if (line.ToLower().Contains("<*>b."))
                                {
                                    Answer = "Option2";
                                    Option2 = line.Substring(5, line.Length - 5);
                                }
                                else if (line.ToLower().Contains("<*>c."))
                                {
                                    Answer = "Option3";
                                    Option3 = line.Substring(5, line.Length - 5);
                                }
                                else if (line.ToLower().Contains("<*>d."))
                                {
                                    Answer = "Option4";
                                    Option4 = line.Substring(5, line.Length - 5);
                                }
                                counter++;
                            }
                            else if (line.ToLower().Contains("<exp>"))
                            {
                                AnswerExplain = line.Substring(5, line.Length - 5);
                                counter++;
                            }
                            else if (line.ToLower().Contains("a."))
                            {
                                Option1 = line.Substring(2, line.Length - 2);
                                counter++;
                            }
                            else if (line.ToLower().Contains("b."))
                            {
                                Option2 = line.Substring(2, line.Length - 2);
                                counter++;
                            }
                            else if (line.ToLower().Contains("c."))
                            {
                                Option3 = line.Substring(2, line.Length - 2);
                                counter++;
                            }
                            else if (line.ToLower().Contains("d."))
                            {
                                Option4 = line.Substring(2, line.Length - 2);
                                counter++;
                            }
                            else
                            {
                                //remove dong dinh dang khong co trong cu phap quy dinh
                                readText = readText.Where(m => m.Trim() != line).ToArray();
                            }
                            if (counter == 5)
                            {
                                Question nQuestion = new Question
                                {
                                    //Id = model.Id,
                                    QuestionContent = QuestionContent,
                                    Option1 = Option1,
                                    Option2 = Option2,
                                    Option3 = Option3,
                                    Option4 = Option4,
                                    Status = true,
                                    Answer = Answer,
                                    AnswerExplain = AnswerExplain,
                                    PostId = model.Id,
                                    //CateId = model.Id,
                                    //PostId = model.PostId,
                                    CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                                    CreatedOnDate = DateTime.Now,

                                    //LastModifiedOnDate = DateTime.Now,
                                    //LastModifiedByUserId = 1
                                };
                                db.QuestionRepository.Add(nQuestion);
                                counter = 0;
                            }
                        }
                        db.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                    string loi = e.Message;
                        //isSavedSuccessfully = false;
                        return View();
                    }
                }
            //}
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestionNoExp(int id)
        {
            Post model = db.PostRepository.FindByID(id);
            PostEditViewModel CateEditVM = new PostEditViewModel
            {
                Id = model.Id,
                Name = model.Name,                
                QuestionFile = model.QuestionFile
            };
            return View(CateEditVM);
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestionNoExp(PostEditViewModel model)
        {
            //int PostId = Convert.ToInt32(Request.Form["PostId"]);

            //if (ModelState.IsValid)//Kiểm tra trong PostEditViewModel mấy trường có required không, nếu có mà view không có control tương ứng thì gặp lỗi chỗ này invalid.
            //{
            string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
            bool isSavedSuccessfully = true;
            try
            {
                if (model.UploadedQuestionFile != null && model.UploadedQuestionFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
                    if (extension == ".txt")
                    {
                        string subPath = Server.MapPath("~/Upload/files/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }
                        //string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
                        model.QuestionFile = "Question" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
                        model.UploadedQuestionFile.SaveAs(Server.MapPath("~/Upload/files/") + model.QuestionFile);
                    }
                    else
                    {
                        ViewBag.error = "Thể loại tập tin không phù hợp, phải chọn loại tập tin '.txt'!";
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully == true)
            {
                try
                {
                    Post post = db.PostRepository.FindByID(model.Id);

                    bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/files/" + post.QuestionFile));
                    if (exist)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/files/" + post.QuestionFile));
                    }

                    post.Name = model.Name;
                    //Cate.ParentId = model.ParentId;
                    //Cate.Position = model.Position;
                    //Cate.CateType = model.CateType.ToString();
                    //Cate.Status = model.Status;
                    //Cate.Icon = model.Icon;
                    post.QuestionFile = model.QuestionFile;

                    db.PostRepository.Update(post);

                    //********************Khuc luu question vao csdl************************************************************/
                    string QuestionContent = "";
                    string Option1 = "";
                    string Option2 = "";
                    string Option3 = "";
                    string Option4 = "";
                    string Answer = "";
                    //string AnswerExplain = "";
                    string line;

                    string path = Server.MapPath("~/Upload/files/" + post.QuestionFile);

                    int counter = 0;

                    string[] readText = System.IO.File.ReadAllLines(path).Where(s => s.Trim() != string.Empty).ToArray();

                    foreach (string s in readText)
                    {
                        //line = file.ReadLine().Trim();
                        line = Regex.Replace(s.Trim(), @"\s+", " ");
                        //line = Regex.Replace(line.Trim(), @"\s+", " ");

                        if (line.ToLower().Contains("<cauhoi>"))
                        {
                            QuestionContent = line.Substring(8, line.Length - 8);
                        }
                        else if (line.ToLower().Contains("<*>"))
                        {
                            if (line.ToLower().Contains("<*>a."))
                            {
                                Answer = "Option1";
                                Option1 = line.Substring(5, line.Length - 5);
                            }
                            else if (line.ToLower().Contains("<*>b."))
                            {
                                Answer = "Option2";
                                Option2 = line.Substring(5, line.Length - 5);
                            }
                            else if (line.ToLower().Contains("<*>c."))
                            {
                                Answer = "Option3";
                                Option3 = line.Substring(5, line.Length - 5);
                            }
                            else if (line.ToLower().Contains("<*>d."))
                            {
                                Answer = "Option4";
                                Option4 = line.Substring(5, line.Length - 5);
                            }
                            counter++;
                        }
                        //else if (line.ToLower().Contains("<exp>"))
                        //{
                        //    AnswerExplain = line.Substring(5, line.Length - 5);
                        //    counter++;
                        //}
                        else if (line.ToLower().Contains("a."))
                        {
                            Option1 = line.Substring(2, line.Length - 2);
                            counter++;
                        }
                        else if (line.ToLower().Contains("b."))
                        {
                            Option2 = line.Substring(2, line.Length - 2);
                            counter++;
                        }
                        else if (line.ToLower().Contains("c."))
                        {
                            Option3 = line.Substring(2, line.Length - 2);
                            counter++;
                        }
                        else if (line.ToLower().Contains("d."))
                        {
                            Option4 = line.Substring(2, line.Length - 2);
                            counter++;
                        }
                        else
                        {
                            //remove dong dinh dang khong co trong cu phap quy dinh
                            readText = readText.Where(m => m.Trim() != line).ToArray();
                        }
                        if (counter == 4)
                        {
                            Question nQuestion = new Question
                            {
                                //Id = model.Id,
                                QuestionContent = QuestionContent,
                                Option1 = Option1,
                                Option2 = Option2,
                                Option3 = Option3,
                                Option4 = Option4,
                                Status = true,
                                Answer = Answer,
                                AnswerExplain = "Không có",
                                PostId = model.Id,
                                //CateId = model.Id,
                                //PostId = model.PostId,
                                CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                                CreatedOnDate = DateTime.Now,

                                //LastModifiedOnDate = DateTime.Now,
                                //LastModifiedByUserId = 1
                            };
                            db.QuestionRepository.Add(nQuestion);
                            counter = 0;
                        }
                    }
                    db.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    string loi = e.Message;
                    //isSavedSuccessfully = false;
                    return View();
                }
            }
            //}
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestionNoExpNoABC(int id)
        {
            Post model = db.PostRepository.FindByID(id);
            PostEditViewModel CateEditVM = new PostEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                QuestionFile = model.QuestionFile
            };
            return View(CateEditVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestionNoExpNoABC(PostEditViewModel model)
        {
            //int PostId = Convert.ToInt32(Request.Form["PostId"]);

            //if (ModelState.IsValid)//Kiểm tra trong PostEditViewModel mấy trường có required không, nếu có mà view không có control tương ứng thì gặp lỗi chỗ này invalid.
            //{
            string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
            bool isSavedSuccessfully = true;
            try
            {
                if (model.UploadedQuestionFile != null && model.UploadedQuestionFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
                    if (extension == ".txt")
                    {
                        string subPath = Server.MapPath("~/Upload/files/");
                        bool exists = System.IO.Directory.Exists(subPath);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }
                        //string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
                        model.QuestionFile = "Question" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
                        model.UploadedQuestionFile.SaveAs(Server.MapPath("~/Upload/files/") + model.QuestionFile);
                    }
                    else
                    {
                        ViewBag.error = "Thể loại tập tin không phù hợp, phải chọn loại tập tin '.txt'!";
                        return View();
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully == true)
            {
                try
                {
                    Post post = db.PostRepository.FindByID(model.Id);

                    bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/files/" + post.QuestionFile));
                    if (exist)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/files/" + post.QuestionFile));
                    }

                    post.Name = model.Name;
                    //Cate.ParentId = model.ParentId;
                    //Cate.Position = model.Position;
                    //Cate.CateType = model.CateType.ToString();
                    //Cate.Status = model.Status;
                    //Cate.Icon = model.Icon;
                    post.QuestionFile = model.QuestionFile;

                    db.PostRepository.Update(post);

                    //********************Khuc luu question vao csdl************************************************************/
                    string QuestionContent = "";
                    string Option1 = "";
                    string Option2 = "";
                    string Option3 = "";
                    string Option4 = "";
                    string Answer = "";
                    //string AnswerExplain = "";
                    string line;

                    string path = Server.MapPath("~/Upload/files/" + post.QuestionFile);

                    int counter = 0;

                    string[] readText = System.IO.File.ReadAllLines(path).Where(s => s.Trim() != string.Empty).ToArray();

                    foreach (string s in readText)
                    {
                        //line = file.ReadLine().Trim();
                        line = Regex.Replace(s.Trim(), @"\s+", " ");
                        //line = Regex.Replace(line.Trim(), @"\s+", " ");

                        if (line.ToLower().Contains("<cauhoi>"))
                        {
                            QuestionContent = line.Substring(8, line.Length - 8);
                            counter++;
                        }
                        else if (line.ToLower().Contains("<*>"))
                        {
                            if (counter == 1)
                            {
                                Answer = "Option1";
                                Option1 = line.Substring(3, line.Length - 3);
                            }
                            else if (counter == 2)
                            {
                                Answer = "Option2";
                                Option2 = line.Substring(3, line.Length - 3);
                            }
                            else if (counter == 3)
                            {
                                Answer = "Option3";
                                Option3 = line.Substring(3, line.Length - 3);
                            }
                            else if (counter == 4)
                            {
                                Answer = "Option4";
                                Option4 = line.Substring(3, line.Length - 3);
                            }
                            counter++;
                        }
                        //else if (line.Substring(0, 5).ToLower() == "<exp>")
                        //{
                        //    AnswerExplain = line.Substring(5, line.Length - 5);
                        //    counter++;
                        //}
                        else if (counter == 1 )
                        {
                            Option1 = line.Substring(0, line.Length);
                            counter++;
                        }
                        else if (counter == 2 )
                        {
                            Option2 = line.Substring(0, line.Length);
                            counter++;
                        }
                        else if (counter == 3)
                        {
                            Option3 = line.Substring(0, line.Length);
                            counter++;
                        }
                        else if (counter == 4)
                        {
                            Option4 = line.Substring(0, line.Length);
                            counter++;
                        }
                        else
                        {
                            //remove dong dinh dang khong co trong cu phap quy dinh
                            readText = readText.Where(m => m.Trim() != line).ToArray();
                        }
                        if (counter == 5)
                        {
                            Question nQuestion = new Question
                            {
                                //Id = model.Id,
                                QuestionContent = QuestionContent,
                                Option1 = Option1,
                                Option2 = Option2,
                                Option3 = Option3,
                                Option4 = Option4,
                                Status = true,
                                Answer = Answer,
                                AnswerExplain = "Không có",
                                PostId = model.Id,
                                //CateId = model.Id,
                                //PostId = model.PostId,
                                CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,
                                CreatedOnDate = DateTime.Now,

                                //LastModifiedOnDate = DateTime.Now,
                                //LastModifiedByUserId = 1
                            };
                            db.QuestionRepository.Add(nQuestion);
                            counter = 0;
                        }
                    }
                    db.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    string loi = e.Message;
                    return View();
                }
            }
            //}
            return View();
        }
    }    
}