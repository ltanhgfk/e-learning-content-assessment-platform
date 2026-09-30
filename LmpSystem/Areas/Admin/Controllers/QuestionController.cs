using LmpSystem.Models;
using LmpSystem.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LmpSystem.ViewModels;
using LmpSystem.Common;

namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class QuestionController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        [Authorize(Roles = "Admin")]
        public ActionResult Index(int postid = -1, int lessonid = -1)//(int id)
        {
            //var question_cate = db.CategoryRepository.GetAll().Where(m => m.CateType == "quizze" && m.ParentId != -1);

            //var cateList = db.CategoryRepository.GetAll().Where(m => m.CateType == "quizze" &&m.ParentId != -1).Select(a => new SelectListItem()
            //{
            //    Value = a.Id.ToString(),
            //    Text = a.Name.ToString()
            //})
            //.ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn môn học" });
            //ViewBag.ParentCateList = cateList;

            //ViewBag.cateId = id;
           
            if(lessonid != -1)
            {
                List<Question> listQuestion = db.QuestionRepository.GetAll().Where(m => m.LessonId == lessonid).ToList();
                List<QuestionViewModel> QuestionVmList = new List<QuestionViewModel>();

                foreach (var item in listQuestion)
                {
                    QuestionVmList.Add(new QuestionViewModel
                    {
                        Id = item.Id,
                        QuestionContent = item.QuestionContent,
                        Option1 = item.Option1,
                        Option2 = item.Option2,
                        Option3 = item.Option3,
                        Option4 = item.Option4,
                        Status = item.Status,
                        Answer = item.Answer,
                        AnswerExplain = item.AnswerExplain,
                    });
                }
                return View(QuestionVmList);
            }    
            if (postid != -1)
            {
                List<Question> listQuestion = db.QuestionRepository.GetAll().Where(m => m.PostId == postid).ToList();
                List<QuestionViewModel> QuestionVmList = new List<QuestionViewModel>();
               
                foreach (var item in listQuestion)
                {
                    QuestionVmList.Add(new QuestionViewModel
                    {
                        Id = item.Id,
                        QuestionContent = item.QuestionContent,
                        Option1 = item.Option1,
                        Option2 = item.Option2,
                        Option3 = item.Option3,
                        Option4 = item.Option4,
                        Status = item.Status,
                        Answer = item.Answer,
                        AnswerExplain = item.AnswerExplain,
                    });
                }
                return View(QuestionVmList);
            }
            else
            {
                List<QuestionViewModel> QuestionVmList = new List<QuestionViewModel>();
                return View(QuestionVmList);
            }
        }

        //[Authorize(Roles = "admin")]
        //public ActionResult Create(int id)
        //{            
        //    QuestionViewModel model = new QuestionViewModel
        //    {                
        //        Status = true,
        //        CateId = id,                
        //    };
        //    return View(model);           
        //}

        //[HttpPost]
        //[Authorize(Roles = "admin")]
        //public ActionResult Create(QuestionViewModel model)//, HttpPostedFileBase AvatarFile)
        //{
        //    //List<Tag> taglist = new List<Tag>();
        //    //taglist.AddRange(model.TagList.Where(m => m.Selected)
        //    //    .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
        //    //    );

        //    if (ModelState.IsValid)
        //    {
        //        //string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
        //        //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
        //        //bool isSavedSuccessfully = true;

        //        Question nQuestion = new Question
        //        {
        //            //Id = model.Id,
        //            QuestionContent = model.QuestionContent,
        //            Option1 = model.Option1,
        //            Option2 = model.Option2,
        //            Option3 = model.Option3,
        //            Option4 = model.Option4,
        //            Status = model.Status,
        //            Answer = model.Answer,
        //            AnswerExplain = model.AnswerExplain,
        //            PostId = (int)model.PostId,
        //            CateId = (int)model.CateId,
        //            //PostId = model.PostId,
        //            CreatedByUserId = 1,
        //            CreatedOnDate = DateTime.Now,
        //            ChangePos = true,

        //            LastModifiedOnDate = DateTime.Now,
        //            LastModifiedByUserId = 1
        //        };

        //        //foreach (var i in taglist)
        //        //{
        //        //    Tag tags = db.TagRepository.FindByID(i.TagID);
        //        //    nQuestion.Tags.Add(tags);
        //        //    tags.Questions.Add(nQuestion);
        //        //}                 

        //        db.QuestionRepository.Add(nQuestion);
        //        db.Commit();

        //        return RedirectToAction("Index", "Question", new { id = model.PostId });
        //    }
        //    return View();
        //}

        //[Authorize(Roles = "admin")]
        //public ActionResult CreateAuto()
        //{
        //    GetData data = new GetData();
        //    ViewBag.PostType = data.GetListPostType();

        //    var cateList = db.CategoryRepository.GetAll().Where(m => m.CateType == "quizze" && m.ParentId != -1).Select(a => new SelectListItem()
        //    {
        //        Value = a.Id.ToString(),
        //        Text = a.Name.ToString()
        //    })
        //    .ToList();

        //    cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn môn học" });
        //    ViewBag.ParentCateList = cateList;

        //    //CateEditViewModel CateEditVM = new CateEditViewModel
        //    //{
        //    //    Id = model.Id,
        //    //    Name = model.Name,
        //    //    Icon = model.Icon,
        //    //    ParentId = (int)model.ParentId,
        //    //    Position = model.Position,
        //    //    CateType = CateType,
        //    //    Status = model.Status
        //    //};
        //    //return View(CateEditVM);

        //    return View();
        //}

        //[HttpPost]
        //[Authorize(Roles = "admin")]
        //public ActionResult CreateAuto(CateEditViewModel model)//, HttpPostedFileBase QuestionFile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //string Avatar = "";
        //        string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
        //        //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
        //        bool isSavedSuccessfully = true;
        //        try
        //        {
        //            if (model.UploadedQuestionFile != null && model.UploadedQuestionFile.ContentLength > 0)
        //            {
        //                string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
        //                if (extension == ".txt")
        //                {
        //                    string subPath = Server.MapPath("~/Upload/files/");
        //                    bool exists = System.IO.Directory.Exists(subPath);
        //                    if (!exists)
        //                    {
        //                        System.IO.Directory.CreateDirectory(subPath);
        //                    }
        //                    //string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
        //                    model.QuestionFile = "Question" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
        //                    model.UploadedQuestionFile.SaveAs(Server.MapPath("~/Upload/images/") + model.QuestionFile);
        //                }
        //                else
        //                {
        //                    ViewBag.error = "Thể loại tập tin không phù hợp, phải chọn loại tập tin '.txt'!";
        //                    return View();
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            isSavedSuccessfully = false;
        //        }
        //        if (isSavedSuccessfully == true)
        //        {
        //            try
        //            {
        //                Category Cate = db.CategoryRepository.FindByID(model.Id);

        //                //Cate.Name = model.Name;
        //                //Cate.ParentId = model.ParentId;
        //                //Cate.Position = model.Position;
        //                //Cate.CateType = model.CateType.ToString();
        //                //Cate.Status = model.Status;
        //                Cate.Icon = model.Icon;
        //                Cate.QuestionFile = model.QuestionFile;

        //                db.CategoryRepository.Update(Cate);
        //                db.Commit();
        //                //return RedirectToAction("Index");
        //            }
        //            catch
        //            {
        //                //return View();
        //            }
        //        }
        //    }
        //    return View();

        //    //if (ModelState.IsValid)
        //    //{
        //    //    //string Avatar = "";
        //    //    string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
        //    //    //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
        //    //    bool isSavedSuccessfully = true;
        //    //    try
        //    //    {
        //    //        if (model.UploadedQuestionFile != null && model.UploadedQuestionFile.ContentLength > 0)
        //    //        {
        //    //            string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
        //    //            if (extension == ".txt")
        //    //            {
        //    //                string subPath = Server.MapPath("~/Upload/images/");
        //    //                bool exists = System.IO.Directory.Exists(subPath);
        //    //                if (!exists)
        //    //                {
        //    //                    System.IO.Directory.CreateDirectory(subPath);
        //    //                }

        //    //                //string extension = Path.GetExtension(model.UploadedQuestionFile.FileName);
        //    //                model.QuestionFile = "Question" + "-" + dt + extension;  //new Random().Next(1, 100) + extension;
        //    //                model.UploadedQuestionFile.SaveAs(Server.MapPath("~/Upload/images/") + model.QuestionFile);

        //    //            }
        //    //            else
        //    //            {
        //    //                ViewBag.error = "Thể loại tập tin không phù hợp, phải chọn loại tập tin '.txt'!";
        //    //                return View();
        //    //            }
        //    //        }
        //    //    }
        //    //    catch (Exception)
        //    //    {
        //    //        isSavedSuccessfully = false;
        //    //    }
        //    //    if (isSavedSuccessfully == true)
        //    //    {
        //    //        string QuestionContent = "";
        //    //        string Option1 = "";
        //    //        string Option2 = "";
        //    //        string Option3 = "";
        //    //        string Option4 = "";
        //    //        string Answer = "";
        //    //        string AnswerExplain = "";
        //    //        //bool Status = true;
        //    //        int PostId = (int)model.PostId;
        //    //        int CateId = (int)model.CateId;

        //    //        Question nQuestion = new Question
        //    //        {
        //    //            //Id = model.Id,
        //    //            QuestionContent = QuestionContent,
        //    //            Option1 = Option1,
        //    //            Option2 = Option2,
        //    //            Option3 = Option3,
        //    //            Option4 = Option4,
        //    //            Status = true,
        //    //            Answer = Answer,
        //    //            AnswerExplain = AnswerExplain,
        //    //            PostId = PostId,
        //    //            CateId = CateId,
        //    //            //PostId = model.PostId,
        //    //            CreatedByUserId = 1,
        //    //            CreatedOnDate = DateTime.Now,
        //    //            //QuestionFile = model.QuestionFile,
        //    //            //ChangePos = true,

        //    //            LastModifiedOnDate = DateTime.Now,
        //    //            LastModifiedByUserId = 1
        //    //        };

        //    //        //foreach (var i in taglist)
        //    //        //{
        //    //        //    Tag tags = db.TagRepository.FindByID(i.TagID);
        //    //        //    nQuestion.Tags.Add(tags);
        //    //        //    tags.Questions.Add(nQuestion);
        //    //        //}                 

        //    //        db.QuestionRepository.Add(nQuestion);
        //    //        db.Commit();

        //    //        //return RedirectToAction("Index", "Question", new { id = model.PostId });
        //    //    }
        //    //}
        //    //return View();
        //}

        //[Authorize(Roles = "admin")]
        //public ActionResult Clone(int id)
        //{
        //    Question model = db.QuestionRepository.FindByID(id);
        //    //ViewBag.ChangeAvatar = false;
        //    //List<SelectListItem> tag = GetData.GetListTags();
        //    //foreach (var x in tag)
        //    //{
        //    //    //foreach (var t in model.QuestionTags)
        //    //    foreach (var t in model.Tags)
        //    //    {
        //    //        if (x.Value == t.TagID.ToString())
        //    //        {
        //    //            x.Selected = true;
        //    //        }
        //    //    }
        //    //}

        //    QuestionViewModel QuestionVM = new QuestionViewModel
        //    {
        //        QuestionContent = model.QuestionContent,
        //        Option1 = model.Option1,
        //        Option2 = model.Option2,
        //        Option3 = model.Option3,
        //        Option4 = model.Option4,
        //        Status = model.Status,
        //        Answer = model.Answer,
        //        AnswerExplain = model.AnswerExplain,
        //        PostId = model.PostId,
        //        //PostId = model.PostId
        //    };
        //    return View(QuestionVM);
        //}

        //[HttpPost]
        //[Authorize(Roles = "admin")]
        //public ActionResult Clone(QuestionViewModel model)//, HttpPostedFileBase AvatarFile)
        //{
        //    //List<Tag> taglist = new List<Tag>();
        //    //taglist.AddRange(model.TagList.Where(m => m.Selected)
        //    //    .Select(m => new Tag { TagID = int.Parse(m.Value), TagName = m.Text })
        //    //    );
        //    if (ModelState.IsValid)
        //    {
        //        //string dt = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
        //        //Upload ảnh và lưu ảnh với slug trùng tên tiêu đề bài viết
        //        //bool isSavedSuccessfully = true;
        //        Question nQuestion = new Question
        //        {
        //            QuestionContent = model.QuestionContent,
        //            Option1 = model.Option1,
        //            Option2 = model.Option2,
        //            Option3 = model.Option3,
        //            Option4 = model.Option4,
        //            Status = model.Status,
        //            Answer = model.Answer,
        //            AnswerExplain = model.AnswerExplain,
        //            PostId = model.PostId,
        //            //PostId = model.PostId
        //        };
        //        //foreach (var i in taglist)
        //        //{
        //        //    Tag tags = db.TagRepository.FindByID(i.TagID);
        //        //    nQuestion.Tags.Add(tags);
        //        //    tags.Questions.Add(nQuestion);
        //        //}
        //        db.QuestionRepository.Add(nQuestion);
        //        db.Commit();
        //        return RedirectToAction("Index", "Question", new { id = model.PostId });
        //    }
        //    return View();
        //}

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            Question model = db.QuestionRepository.FindByID(id);

            //ViewBag.ChangeAvatar = false;

            //List<SelectListItem> tag = GetData.GetListTags();

            //foreach (var x in tag)
            //{
            //    //foreach (var t in model.QuestionTags)
            //    foreach (var t in model.Tags)
            //    {
            //        if (x.Value == t.TagID.ToString())
            //        {
            //            x.Selected = true;
            //        }
            //    }
            //}

            QuestionViewModel QuestionVM = new QuestionViewModel
            {
                QuestionContent = model.QuestionContent,
                Option1 = model.Option1,
                Option2 = model.Option2,
                Option3 = model.Option3,
                Option4 = model.Option4,
                Status = model.Status,
                Answer = model.Answer,
                AnswerExplain = model.AnswerExplain,
                PostId = model.PostId,
                LessonId = model.LessonId
                //CateId = model.CateId,                        
            };
            return View(QuestionVM);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(QuestionViewModel model)//, bool ChangeAvatar)//, HttpPostedFile AvatarFile)
        {
            if (ModelState.IsValid)
            {
                Question Question = db.QuestionRepository.FindByID(model.Id);

                Question.Id = model.Id;
                Question.QuestionContent = model.QuestionContent;
                Question.Option1 = model.Option1;
                Question.Option2 = model.Option2;
                Question.Option3 = model.Option3;
                Question.Option4 = model.Option4;
                Question.Answer = model.Answer;
                Question.AnswerExplain = model.AnswerExplain;
                Question.Status = model.Status;

                db.QuestionRepository.Update(Question);
                db.Commit();

                return RedirectToAction("Index", "Question", new { id = model.PostId });
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            Question model = db.QuestionRepository.FindByID(id);

            QuestionViewModel QuestionVM = new QuestionViewModel
            {
                Id = model.Id,

                QuestionContent = model.QuestionContent,
                Option1 = model.Option1,
                Option2 = model.Option2,
                Option3 = model.Option3,
                Option4 = model.Option4,
                Status = model.Status,
                Answer = model.Answer,
                AnswerExplain = model.AnswerExplain,
                PostId = model.PostId,
                LessonId = model.LessonId,
                //CateId = model.CateId,
                //PostId = model.PostId      

            };

            return View(QuestionVM);
        }        

        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            string prefix = state ? "Đã cho hiển thị" : "Đã cho ẩn";
            Question u = db.QuestionRepository.FindByID(id);
            try
            {
                u.Status = state;
                db.Commit();
                return Json(new { Message = prefix + " \"" + u.QuestionContent + "\"" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Có lỗi xảy ra, không thay đổi được!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult Delete(int id)
        {
            Question QuestionObj = db.QuestionRepository.FindByID(id);
            string Name = QuestionObj.QuestionContent;            
            try { 
                db.QuestionRepository.Delete(QuestionObj);

                //xóa ảnh đại diện cũ
                //bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + QuestionObj.Avatar));
                //if (exist)
                //{
                //    System.IO.File.Delete(Server.MapPath("~/Upload/images/" + QuestionObj.Avatar));
                //}

                db.Commit();
                return Json(new { Message = "Xóa câu hỏi '" + Name + "' thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Có lỗi xảy ra,  chưa xóa câu hỏi '" + Name + "-" + "' được! <br />." }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetPostList(int id)
        {
            try
            {
                List<SelectListItem> cateList = db.PostRepository.GetAll().Where(x => x.CateId == id).Select(a => new SelectListItem()
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

        /////////////////////////////---------------------------------------------------------------------/////////////////////////////////////        

    }
}