using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LessonController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        [Authorize(Roles = "Admin")]
        public ActionResult Index(int id)
        {
            List<Lesson> listLesson = db.LessonRepository.GetAll().Where(m => m.PostId == id).ToList();

            List<LessonDetailViewModel> LessonVmList = new List<LessonDetailViewModel>();

            ViewBag.PostId = id;

            foreach (var item in listLesson)
            {
                LessonVmList.Add(new LessonDetailViewModel
                {
                    Id = item.Id,
                    Avatar = item.Avatar,
                    Name = item.Name,
                    Status = item.Status,
                    Position = item.Position,
                    PostName = db.PostRepository.FindByID(item.PostId).Name,
                    PostId = item.PostId,
                    Questions = item.Questions
                });
            }

            return View(LessonVmList);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create(int id)
        {
            LessonEditViewModel model = new LessonEditViewModel
            {
                TagList = GetData.GetListTags(), //LessonData.getTagList(),
                Status = true,
                PostId = id
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(LessonEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            List<Tag> taglist = new List<Tag>();
            taglist.AddRange(model.TagList.Where(m => m.Selected)
                .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
                );

            if (ModelState.IsValid && taglist.Count > 0)
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

                            //Resize image
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), 130, 73);
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
                    Lesson nLesson = new Lesson
                    {
                        Name = model.Name,
                        Avatar = model.Avatar,
                        Video = model.Video,
                        Resume = model.Resume,
                        Content = model.Content,
                        Exercises = model.Exercises,
                        Position = (int)model.Position,
                        Status = model.Status,
                        PostId = model.PostId
                    };

                    foreach (var i in taglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        nLesson.Tags.Add(tags);
                        tags.Lessons.Add(nLesson);
                    }

                    db.LessonRepository.Add(nLesson);
                    db.Commit();

                    return RedirectToAction("Index", "Lesson", new { id = model.PostId });
                }
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Clone(int id)
        {
            Lesson model = db.LessonRepository.FindByID(id);

            ViewBag.ChangeAvatar = false;

            List<SelectListItem> tag = GetData.GetListTags();

            foreach (var x in tag)
            {
                //foreach (var t in model.LessonTags)
                foreach (var t in model.Tags)
                {
                    if (x.Value == t.TagID.ToString())
                    {
                        x.Selected = true;
                    }
                }
            }

            LessonEditViewModel LessonVM = new LessonEditViewModel
            {
                Name = model.Name,
                Avatar = model.Avatar,
                Video = model.Video,
                Resume = model.Resume,
                Content = model.Content,
                Exercises = model.Exercises,
                Position = model.Position,
                Status = model.Status,
                PostId = model.PostId,
                TagList = tag,  //GetData.GetListTags(),

            };
            return View(LessonVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Clone(LessonEditViewModel model)//, HttpPostedFileBase AvatarFile)
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

                            //Resize image
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), 130, 73);
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
                    Lesson nLesson = new Lesson
                    {
                        Name = model.Name,
                        Avatar = model.Avatar,
                        Video = model.Video,
                        Resume = model.Resume,
                        Content = model.Content,
                        Exercises = model.Exercises,
                        Position = (int)model.Position,
                        Status = model.Status,
                        PostId = model.PostId
                    };

                    foreach (var i in taglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        nLesson.Tags.Add(tags);
                        tags.Lessons.Add(nLesson);
                    }

                    db.LessonRepository.Add(nLesson);
                    db.Commit();

                    return RedirectToAction("Index", "Lesson", new { id = model.PostId });
                }
            }
            return View();
        }
        [Authorize(Roles = "admin")]
        public ActionResult TransferToLesson(int postid)
        {
            Post model = db.PostRepository.FindByID(postid);
            //Lesson model = db.LessonRepository.FindByID(id);
            var lesson = db.LessonRepository.GetAll().Where(m => m.PostId == postid).ToList();
            if(lesson.Count >= 1)
            {
                @ViewBag.error = "Không thể chuyển thành lesson được do vẫn còn lesson";
                return RedirectToAction("Index", "Post");
            }
            var order = db.OrderRepository.GetAll().Where(m => m.PostId == postid).ToList();
            if (order.Count >= 1)
            {
                @ViewBag.error = "Không thể chuyển thành lesson được do vẫn còn order";
                return RedirectToAction("Index", "Post");
            }

            //--Cac bang co lien quan can kiem tra---------
            //this.Lessons = new HashSet<Lesson>();//Neu co thi khong cho chuyen
            //this.Orders = new HashSet<Order>();//Neu co thi khong cho chuyen
            //this.PostImages = new HashSet<PostImage>();//xoa
            //this.Questions = new HashSet<Question>();//chuyen qua lesson
            //this.PostReviews = new HashSet<PostReview>();//xoa
            //this.Tags = new HashSet<Tag>();//ok rui//chuyen qua lesson
            //---------------------------------------------

            //ViewBag.ChangeAvatar = false;
            List<SelectListItem> postList = db.PostRepository.GetAll().OrderByDescending(m => m.LastModifiedOnDate)
                .Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name.ToString()
                    }).ToList();
            ViewBag.PostList = postList;

            TempData["post_id"] = postid;
            TempData.Keep();

            List<SelectListItem> tag = GetData.GetListTags();

            foreach (var x in tag)
            {
                //foreach (var t in model.LessonTags)
                foreach (var t in model.Tags)
                {
                    if (x.Value == t.TagID.ToString())
                    {
                        x.Selected = true;
                    }
                }
            }

            LessonEditViewModel LessonVM = new LessonEditViewModel
            {
                Name = model.Name,
                Avatar = model.Avatar,
                Video = model.Video,
                Resume = model.Resume,
                Content = model.Content,
                //Exercises = model.Exercises,
                Position = model.Position,
                Status = model.Status,
                //PostId = model.PostId,
                TagList = tag,  //GetData.GetListTags(),

            };
            return View(LessonVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult TransferToLesson(LessonEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            int post_id = Convert.ToInt32(TempData["post_id"].ToString());

            List<Tag> taglist = new List<Tag>();
            taglist.AddRange(model.TagList.Where(m => m.Selected)
                .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
                );

            if (ModelState.IsValid)
            {
                Lesson nLesson = new Lesson
                {
                    Name = model.Name,
                    Avatar = model.Avatar,
                    Video = model.Video,
                    Resume = model.Resume,
                    Content = model.Content,
                    Exercises = model.Exercises,
                    Position = (int)model.Position,
                    Status = model.Status,
                    PostId = model.PostId
                };

                foreach (var i in taglist)
                {
                    Tag tags = db.TagRepository.FindByID(i.TagID);
                    nLesson.Tags.Add(tags);
                    tags.Lessons.Add(nLesson);
                }

                db.LessonRepository.Add(nLesson);
                db.LessonRepository.SaveChanges();

                db.PostImageRepository.DeleteByPost(post_id);// khoi xoa tu dong - de xoa tay
                db.PostReviewRepository.DeleteByPost(post_id);

                var maxLesson = db.LessonRepository.GetAll().OrderByDescending(m => m.Id).Take(1).FirstOrDefault();

                var question = db.QuestionRepository.GetAll().Where(m => m.PostId == post_id).ToList();
                foreach (var item in question)
                {
                    item.PostId = model.PostId;
                    item.LessonId = maxLesson.Id;
                    db.QuestionRepository.Update(item);
                }
                db.PostRepository.Delete(db.PostRepository.FindByID(post_id));

                db.Commit();

                return RedirectToAction("Index", "Lesson", new { id = model.PostId });
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            Lesson model = db.LessonRepository.FindByID(id);
            TempData["post_id"] = model.PostId;
            ViewBag.ChangeAvatar = false;

            List<SelectListItem> postList = db.PostRepository.GetAll().OrderByDescending(m => m.LastModifiedOnDate)
                .Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name.ToString()
                }).ToList();
            ViewBag.PostList = postList;

            List<SelectListItem> tag = GetData.GetListTags();

            foreach (var x in tag)
            {
                //foreach (var t in model.LessonTags)
                foreach (var t in model.Tags)
                {
                    if (x.Value == t.TagID.ToString())
                    {
                        x.Selected = true;
                    }
                }
            }

            LessonEditViewModel LessonVM = new LessonEditViewModel
            {
                Name = model.Name,
                Avatar = model.Avatar,
                Video = model.Video,
                Resume = model.Resume,
                Content = model.Content,
                Exercises = model.Exercises,
                Position = model.Position,
                PostId = model.PostId,
                Status = model.Status,
                TagList = tag,
            };
            return View(LessonVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(LessonEditViewModel model)//, bool ChangeAvatar)//, HttpPostedFile AvatarFile)
        {
            int post_id = Convert.ToInt32(TempData["post_id"]);
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

                            //Resize image
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), 130, 73);
                            //image.Save(System.IO.Path.GetPathRoot() + "\\Image.jpg", ImageFormat.Jpeg);                        
                            img.Save(Server.MapPath("~/Upload/images/") + model.Avatar);
                            //model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                        }
                        else
                        {
                            TempData["error"] = "Ảnh đại diện bạn tải lên không đúng định dạng ảnh!";
                            //return View();
                        }                       
                        // lưu ảnh mới với tên là slug tiêu đề mới
                        //string extension = Path.GetExtension(model.AvatarFile.FileName);
                        //model.Avatar = "Lesson" + "-" + dt + extension;
                        //model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully)
                {
                    Lesson Lesson = db.LessonRepository.FindByID(model.Id);

                    Lesson.Id = model.Id;
                    Lesson.Name = model.Name;
                    Lesson.Avatar = model.Avatar;
                    Lesson.Position = (int)model.Position;
                    Lesson.PostId = model.PostId;
                    Lesson.Resume = model.Resume;
                    Lesson.Video = model.Video;
                    Lesson.Content = model.Content;
                    Lesson.Exercises = model.Exercises;
                    Lesson.Status = model.Status;

                    foreach (var i in taglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        if (!Lesson.Tags.Contains(tags))
                        {
                            Lesson.Tags.Add(tags);
                            tags.Lessons.Add(Lesson);
                        }
                    }

                    foreach (var i in untaglist)
                    {
                        Tag tags = db.TagRepository.FindByID(i.TagID);
                        if (Lesson.Tags.Contains(tags))
                        {
                            Lesson.Tags.Remove(tags);
                            tags.Lessons.Remove(Lesson);
                        }
                    }

                    db.LessonRepository.Update(Lesson);

                    if (post_id != model.PostId)
                    {
                        var question = db.QuestionRepository.GetAll().Where(m => m.PostId == post_id && m.LessonId == model.Id).ToList();
                        foreach (var item in question)
                        {
                            item.PostId = model.PostId;
                            //item.LessonId = maxLesson.Id;
                            db.QuestionRepository.Update(item);
                        }
                    }

                    db.Commit();

                    return RedirectToAction("Index", "Lesson", new { id = model.PostId });
                }
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            Lesson model = db.LessonRepository.FindByID(id);

            LessonDetailViewModel LessonVM = new LessonDetailViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Avatar = model.Avatar,
                Content = model.Content,
                Status = model.Status,
                Resume = model.Resume,
                Video = model.Video,
                Position = model.Position,
                PostName = db.PostRepository.FindByID(model.PostId).Name,

                //LessonTags = db.LessonRepositoryRepository.GetAll().Where(m => m. == model.Id).ToList(),
                //Questions = db.QuestionRepository.GetAll().Where(m => m.LessonId == model.Id).ToList(),
                //UserScores = db.UserScoreRepository.GetAll().Where(m => m.LessonId == model.Id).ToList(),
            };

            return View(LessonVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            string prefix = state ? "Đã hiển thị" : "Đã ẩn";
            Lesson u = db.LessonRepository.FindByID(id);
            try
            {
                u.Status = state;
                db.Commit();
                return Json(new { Message = prefix + " \"" + u.Name + "\"" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Xảy ra lỗi, chưa thay đổi được!" }, JsonRequestBehavior.AllowGet);
            }
        }

        //Lesson: Tbl_Lesson/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            Lesson LessonObj = db.LessonRepository.FindByID(id);
            string Name = LessonObj.Name;
            try
            {
                db.LessonRepository.Delete(LessonObj);

                //xóa ảnh đại diện cũ
                bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + LessonObj.Avatar));
                if (exist)
                {
                    if (LessonObj.Avatar != "no-image.png")
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/images/" + LessonObj.Avatar));
                    }
                }

                db.Commit();
                return Json(new { Message = "Xóa '" + Name + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Có lỗi xảy ra, chưa xóa được '" + Name + "-" + "' <br />." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult DeleteQuestion(int postid, int lessonid)//xóa post phải xóa các thông tin bảng khác liên quan, không thì không được xóa
        {
            //Post PostObj = db.PostRepository.FindByID(id);
            //string Name = PostObj.Name;
            try
            {
                //db.PostRepository.Delete(PostObj);
                db.QuestionRepository.DeleteByLesson(postid, lessonid);
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
                return Json(new { Message = "Xóa toàn bộ câu hỏi thành công! postid = " + postid + ", lessonid = " + lessonid }, JsonRequestBehavior.AllowGet);
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
            Lesson model = db.LessonRepository.FindByID(id);

            //Enum.TryParse(model.CateType, out PostType CateType);

            LessonEditViewModel CateEditVM = new LessonEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                //Icon = model.Icon,
                //ParentId = (int)model.ParentId,
                //Position = model.Position,
                //CateType = data.GetListPostType(),
                //Status = model.Status,
                QuestionFile = model.QuestionFile,
                PostId = model.PostId
            };
            return View(CateEditVM);
        }

        // POST: Admin/Cate/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestion(LessonEditViewModel model)
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
                    Lesson post = db.LessonRepository.FindByID(model.Id);

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

                    db.LessonRepository.Update(post);

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
                                PostId = model.PostId,
                                LessonId = model.Id,
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

                    return RedirectToAction("Index", new { id = model.PostId });
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
            Lesson model = db.LessonRepository.FindByID(id);
            LessonEditViewModel CateEditVM = new LessonEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                QuestionFile = model.QuestionFile,
                PostId = model.PostId
            };
            return View(CateEditVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestionNoExp(LessonEditViewModel model)
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
                    Lesson post = db.LessonRepository.FindByID(model.Id);

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

                    db.LessonRepository.Update(post);

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
                                PostId = model.PostId,
                                LessonId = model.Id,
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

                    return RedirectToAction("Index", new { id = model.PostId });
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
            Lesson model = db.LessonRepository.FindByID(id);
            LessonEditViewModel CateEditVM = new LessonEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                QuestionFile = model.QuestionFile,
                PostId = model.PostId
            };
            return View(CateEditVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AutoUpdateQuestionNoExpNoABC(LessonEditViewModel model)
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
                    Lesson post = db.LessonRepository.FindByID(model.Id);

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

                    db.LessonRepository.Update(post);

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
                        else if (counter == 1)
                        {
                            Option1 = line.Substring(0, line.Length);
                            counter++;
                        }
                        else if (counter == 2)
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
                                PostId = model.PostId,
                                LessonId = model.Id,
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

                    return RedirectToAction("Index", new { id = model.PostId });
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