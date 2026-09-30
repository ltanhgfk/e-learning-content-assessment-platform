using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using PagedList;

namespace LmpSystem.Controllers
{ 
    //[RoutePrefix("Home")]
    public class HomeController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());
        GetData data = new GetData();
        // GET: Home        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CommonPage(int id = -1)
        {
            //LmpInfo model = data.GetLmpInfoByCode(code);
            LmpInfo model = db.LmpInfoRepository.FindByID(id);
            return View(model);
        }

        //[Route("{id}/{catename}")]
        public ActionResult PostByCate(int cateid = -1)
        {
            List<Category> categories = db.CategoryRepository.GetAll().Where(m => m.ParentId == cateid && m.Status == true).OrderBy(m => m.Position).ToList();
            return View(categories);
        }

        //[Route("sub-cate-{subcateid}-{subcatename}")]
        public ActionResult PostBySubCate(int subcateid = -1)
        {
            ViewBag.subCateId = subcateid;
            return View();
        }

        public ActionResult PostByTag(int tagid = -1)
        {
            ViewBag.tagid = tagid;
            return View();
        }

        public ActionResult PostByType(string post_type = "")
        {
            ViewBag.type = post_type;
            return View();
        }

        //public ActionResult PostByTagType(int tagid = -1, string post_type = "")
        //{
        //    ViewBag.tagid = tagid;
        //    ViewBag.type = post_type;
        //    return View();
        //}

        //[Route("gioi-thieu")]
        public ActionResult Introduction()
        {
            return View();
        }

        //[Route("thong-tin-lien-he")]
        public ActionResult Contact()
        {
            Feedback model = new Feedback();
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(Feedback model)//Send feedback
        {            
            if (ModelState.IsValid)
            {
                Feedback feedback = new Feedback();
                try
                {
                    feedback.Id = model.Id;
                    feedback.FullName = model.FullName;
                    feedback.Subject = model.Subject;
                    feedback.Email = model.Email;
                    feedback.Content = model.Content;
                    feedback.Status = true;
                    feedback.Content = model.Content;
                    feedback.Note = model.Note;
                    feedback.CreatedOnDate = DateTime.Now;                   

                    db.FeedbackRepository.Add(feedback);
                    db.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {                    
                    TempData["error"] = "Xảy ra lỗi \"" + e.Message + "\" trong quá trình xử lý!";
                    return View();// RedirectToAction("Edit");                        
                }
            }
            return View();
        }
        
        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult TestSubjectList()
        {
            var QuizPostList = db.PostRepository.GetAll().Where(m => m.PostType == "quizze" && m.Status == true).OrderByDescending(m=>m.LastModifiedOnDate).ToList();
                        
            List<PostDetailViewModel> TestSubjectList = new List<PostDetailViewModel>();

            foreach (var item in QuizPostList)
            {
                TestSubjectList.Add(new PostDetailViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Position = item.Position,
                    Avatar = item.Avatar,
                    Resume = item.Resume,
                    CateName = item.Category.Name,
                    LastModifiedOnDate = (DateTime)item.LastModifiedOnDate,
                    UserId =(int)item.UserId,
                    CateId = item.CateId
                    //Status = item.Status,
                });
            }
            return View(TestSubjectList);
        }

        //[Route("lam-bai-trac-nghiem-truc-tuyen")]
        public ActionResult TestQuiz(int postid = -1, int lessonid = -1)
        {            
            //var cateList = db.CategoryRepository.GetAll().Where(m => m.CateType == "quizze" && m.ParentId != -1 && m.Status == true).Select(a => new SelectListItem()
            //{
            //    Value = a.Id.ToString(),
            //    Text = a.Name.ToString()
            //})
            //.ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn môn học" });
            //ViewBag.ParentCateList = cateList;            
            
            if (lessonid != -1)
            {
                int number_quiz = Convert.ToInt32(data.GetLmpInfoByCode("number_quiz_lesson").Name);
                //ViewBag.postid = lessonid;
                List<Question> listQuestion = db.QuestionRepository.GetAll().OrderBy(r => Guid.NewGuid()).Where(m => m.LessonId == lessonid && m.Status == true).Take(number_quiz).ToList();
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
                        PostId = item.PostId,
                        LessonId = item.LessonId
                    });
                }
                return View(QuestionVmList);
            }
            else if (postid != -1)
            {
                int number_quiz = Convert.ToInt32(data.GetLmpInfoByCode("number_quiz_post").Name);
                //ViewBag.postid = postid;
                List<Question> listQuestion = db.QuestionRepository.GetAll().OrderBy(r => Guid.NewGuid()).Where(m => m.PostId == postid && m.Status == true).Take(number_quiz).ToList();
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
                        PostId = item.PostId,
                        LessonId = item.LessonId
                    });
                }
                return View(QuestionVmList);
            }            
            else
            {
                List<QuestionViewModel> QuestionVmList = new List<QuestionViewModel>();
                return View(QuestionVmList);
            }
            //return View();
        }

        [HttpPost]
        public ActionResult TestQuiz(UserScore model)
        {
            //UserScore uc = db.UserScoreRepository.GetAll().Where(m => m.CateId == cateId).FirstOrDefault();
            try
            {
                UserScore nuserScore = new UserScore
                {                    
                    PostId = model.PostId,
                    LessonId = model.LessonId,
                    UserName = model.UserName, //sửa lại bằng người đăng nhập
                    Email = model.Email,
                    TestDetail = model.TestDetail,
                    TrueNumberAnswer = model.TrueNumberAnswer,
                    TotalNumberQuestion = model.TotalNumberQuestion,
                    CreatedOnDate = DateTime.Today
                };
                db.UserScoreRepository.Add(nuserScore);
                db.Commit();
                return RedirectToAction("ViewTestResult",new { email = model.Email});// Json(new { Message = "Đã lưu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return View();// Json(new { Message = "Có lỗi xảy ra, chưa lưu được: " + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Nen tao 1 hàm kiem tra xem user dang nhap vo duong link nay có dang nhap chua và có mua khoa hoc nay không?
        //nếu có đăng nhập mà không mua khóa học này thì không vô được bài học, không vô được câu hỏi của bài học.
        //public ActionResult UserTestQuiz(int postId, int lessonId)
        //{
        //    ViewBag.LessonByPost = db.LessonRepository.GetAll().Where(m => m.PostId == postId);
        //    //goi ham kiem tra xem user đang vào test có mua khóa học này chưa? có đăng nhập chưa?...
        //    return View(db.QuestionRepository.GetAll().Where(m => m.LessonId == lessonId).ToList());
        //}       

        //[Route("ket_qua-bai-trac-nghiem-truc-tuyen")]
        public ActionResult ViewTestResult(string email)
        {
            //var cateList = db.CategoryRepository.GetAll().Where(m => m.CateType == "quizze" && m.ParentId != -1 && m.Status == true).Select(a => new SelectListItem()
            //{
            //    Value = a.Id.ToString(),
            //    Text = a.Name.ToString()
            //})
            //.ToList();

            //cateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn môn học" });
            //ViewBag.ParentCateList = cateList;

            //ViewBag.Lesson = questioncate;

            List<Question> listQuestion = new List<Question>();
            UserScore uc = db.UserScoreRepository.GetAll().Where(m=>m.Email == email).OrderByDescending(u => u.Id).FirstOrDefault();

            if (uc != null)
            {
                string[] list = uc.TestDetail.Split('#');
                ViewBag.SoCauDung = uc.TrueNumberAnswer;
                ViewBag.TongSoCau = uc.TotalNumberQuestion;
                ViewBag.HoVaTen = uc.UserName;
                ViewBag.Email = uc.Email;
                ViewBag.MonHoc = db.PostRepository.FindByID((int)uc.PostId).Name;
                if (uc.LessonId !=null)
                {
                    ViewBag.BaiHoc = "&bull; Bài: " + db.LessonRepository.FindByID((int)uc.LessonId).Name;
                }
                else ViewBag.BaiHoc = "";
                ViewBag.NgayGioLamKT = uc.CreatedOnDate;

                string[] listYourAns = new string[list.Length-1];

                int pos2cham = 0;
                int qid = 0;
                string yourAns = "";

                for (int i = 0; i < list.Length - 1; i++)
                {
                    pos2cham = list[i].IndexOf(":");
                    qid = Convert.ToInt32(list[i].Substring(0, pos2cham));//hên
                    yourAns = list[i].Substring(pos2cham + 1, list[i].Length - (pos2cham + 1));
                    listYourAns[i] = yourAns;

                    listQuestion.Add(db.QuestionRepository.FindByID(qid));
                }
                List<QuestionViewModel> QuestionVmList = new List<QuestionViewModel>();
                int j = 0;
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
                        Answer = item.Answer == "Option1" ? item.Option1 : item.Answer == "Option2" ? item.Option2 : item.Answer == "Option3" ? item.Option3 : item.Option4, //item.Answer == "Option4" ? item.Option4 : "Bạn không trả lời câu hỏi!"//item.Answer,
                        AnswerExplain = item.AnswerExplain,
                        YourAnswer = listYourAns[j] == "Option1" ? item.Option1 : listYourAns[j] == "Option2" ? item.Option2 : listYourAns[j] == "Option3" ? item.Option3 : listYourAns[j] == "Option4"? item.Option4: "Bạn không trả lời câu hỏi!"
                    });
                    j++;
                }
                return View(QuestionVmList);
            }
            else
            {
                List<QuestionViewModel> QuestionVmList = new List<QuestionViewModel>();
                return View(QuestionVmList);
            }
        }

        [HttpPost]
        public JsonResult SaveTestResult(int lessonid = -1, int postId = -1, string userName="", string email="", string testDetail = "-", int TrueNumberAnswer = -1, int TotalNumberQuestion = -1)//
        {
            //UserScore uc = db.UserScoreRepository.GetAll().Where(m => m.CateId == cateId).FirstOrDefault();
            try
            {
                UserScore nuserScore = new UserScore
                {
                    PostId = postId,
                    LessonId = lessonid,
                    UserName = userName, //sửa lại bằng người đăng nhập
                    Email = email,
                    TestDetail = testDetail,
                    TrueNumberAnswer = TrueNumberAnswer,
                    TotalNumberQuestion = TotalNumberQuestion,
                    CreatedOnDate = DateTime.Today                    
                };
                db.UserScoreRepository.Add(nuserScore);
                db.Commit();
                return Json(new { Message = "Đã lưu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Message = "Có lỗi xảy ra, chưa lưu được: "+e.Message }, JsonRequestBehavior.AllowGet);
            }            
        }

        //public ActionResult PostBySubCate(int subcateid)
        //{            
        //    Post model = db.PostRepository.FindByID(subcateid);
        //    PostDetailViewModel PostVM = new PostDetailViewModel
        //    {
        //        Id = model.Id,
        //        PostType = model.PostType,
        //        CateName = db.CategoryRepository.FindByID(model.CateId).Name,
        //        Code = model.Code,
        //        Name = model.Name,
        //        Title = model.Title,
        //        Pagelink = model.Pagelink,
        //        Avatar = model.Avatar,
        //        Content = model.Content,
        //        Status = model.Status,
        //        Resume = model.Resume,
        //        Video = model.Video,
        //        Position = model.Position,
        //        TotalTime = model.TotalTime,
        //        Price = model.Price,
        //        BeginDate = string.Format("{0:dd/MM/yyyy}", model.BeginDate),
        //        Discount = model.Discount,
        //        Quantity = model.Quantity,

        //        Username = db.UserRepository.FindByID((int)model.Id).Username.ToString(),

        //        //CreatedByUserId = (int)model.LastModifiedByUserId,
        //        CreatedOnDate = DateTime.Today,
        //        LastModifiedOnDate = DateTime.Today,
        //        //LastModifiedByUserId = model.LastModifiedByUserId!=null? (int)model.LastModifiedByUserId:0,

        //        Reviews = db.PostReviewRepository.GetAll().Where(m => m.PostId == model.Id).ToList(),
        //        //PostTags = GetData.
        //        //PostImages =
        //        Lessons = db.LessonRepository.GetAll().Where(m => m.PostId == model.Id).ToList(),
        //    };
        //    return View(PostVM);
        //}        

        //[Route("ket-qua-tim-kiem-bai-viet")]
        public ActionResult SearchPost(int? page, string search_input = "")//, int subCateId = -1) //int cateId = -1, //string titleStr, //int post_type = 1
        {
            int pageSize = 20;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            //var enumDisplayStatus = ((PostType)post_type).ToString();
            //string stringValue = enumDisplayStatus.ToString();

            List<Post> listPostV = new List<Post>();
            //IPagedList<Post> listPostV = null;

            IPagedList<Post> listPost = null; //m => m.PostType == stringValue&&

            if (!search_input.Equals("") || search_input != null)
            {
                ViewBag.searchStr = search_input;
                listPost = db.PostRepository.GetAll().Where(m => m.Status == true && (m.Name.Contains(search_input) || m.Resume.Contains(search_input) || m.Content.Contains(search_input))).OrderBy(m => m.LastModifiedOnDate).ToPagedList(pageIndex, pageSize);
            }

            //if (cateId != -1)
            //{
            //    List<Category> cateList = db.CategoryRepository.GetAll().Where(m => m.Status == true && m.ParentId == cateId).ToList();
            //    ViewBag.subCateList = cateList;
            //    List<Post> post = null;
            //    foreach (var item in cateList)
            //    {
            //        post = db.PostRepository.GetAll().Where(m => m.Status == true && m.CateId == item.Id).ToList();
            //        listPostV.AddRange(post);
            //    }
            //    listPost = listPostV.ToPagedList(pageIndex, pageSize);
            //}
            //if (subCateId != -1)
            //{
            //    int parentId = (int)db.CategoryRepository.FindByID(subCateId).ParentId;
            //    List<Category> cateList = db.CategoryRepository.GetAll().Where(m => m.Status == true && m.ParentId == parentId).ToList();
            //    ViewBag.subCateList = cateList;
            //    ViewBag.subCateId = subCateId;

            //    listPost = db.PostRepository.GetAll().Where(m => m.Status == true && m.CateId == subCateId).ToList().ToPagedList(pageIndex, pageSize);
            //}
            if (search_input.Equals("") || search_input == null)
            {
                ViewBag.searchStr = "Bạn không nhập tiêu chí tìm kiếm!";
            }         

            IPagedList<Post> pageOrders = new StaticPagedList<Post>(listPost, pageIndex, pageSize, db.PostRepository.GetAll().Count());//Where(m => m.PostType == stringValue).

            return View(pageOrders);
            //return View(listPost);
        }

        //[Route("{postid}764/{postname}")]//postname o trang view, la bien truyen tham so giong nhu id
        //[Route("{alias}-{postid}764-{postname}.html")]
        //{alias}-{id}764.html",
        //[Route("{alias}-{postid}764.html")]
        //[Route("{postid}764-{postname}")]
        public ActionResult PostDetail(int postid = -1)
        {
            //TempData.Keep();
            Post model = db.PostRepository.FindByID(postid);
            //var review = db.PostReviewRepository.GetAll().Where(m => m.PostId == model.Id).ToList();

            PostDetailViewModel PostVM = new PostDetailViewModel
            {
                Id = model.Id,
                PostType = model.PostType,
                CateId = model.CateId,
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

                UserId = (int)model.UserId,
                Username = db.UserRepository.FindByID((int)model.UserId).Username.ToString(),

                //CreatedByUserId = (int)model.LastModifiedByUserId,
                CreatedOnDate = DateTime.Today,
                LastModifiedOnDate = DateTime.Today,
                //LastModifiedByUserId = model.LastModifiedByUserId!=null? (int)model.LastModifiedByUserId:0,

                Reviews = db.PostReviewRepository.GetAll().Where(m => m.PostId == model.Id).ToList(),
                PostTags = model.Tags,
                //PostImages =
                Lessons = db.LessonRepository.GetAll().Where(m => m.PostId == model.Id && m.Status == true).OrderBy(m=>m.Position).ToList(),
                Questions = model.Questions
            };

            if (PostVM == null)
            {
                return RedirectToAction("Error404");
            }
            return View(PostVM);
        }

        public ActionResult PostDetailByType(int postid = -1)
        {
            //TempData.Keep();
            Post model = db.PostRepository.FindByID(postid);
            //var review = db.PostReviewRepository.GetAll().Where(m => m.PostId == model.Id).ToList();

            PostDetailViewModel PostVM = new PostDetailViewModel
            {
                Id = model.Id,
                PostType = model.PostType,
                CateId = model.CateId,
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

                UserId = (int)model.UserId,
                Username = db.UserRepository.FindByID((int)model.UserId).Username.ToString(),

                //CreatedByUserId = (int)model.LastModifiedByUserId,
                CreatedOnDate = DateTime.Today,
                LastModifiedOnDate = DateTime.Today,
                //LastModifiedByUserId = model.LastModifiedByUserId!=null? (int)model.LastModifiedByUserId:0,

                Reviews = db.PostReviewRepository.GetAll().Where(m => m.PostId == model.Id).ToList(),
                PostTags = model.Tags,
                //PostImages =
                Lessons = db.LessonRepository.GetAll().Where(m => m.PostId == model.Id && m.Status == true).OrderBy(m => m.Position).ToList(),
                Questions = model.Questions
            };

            if (PostVM == null)
            {
                return RedirectToAction("Error404");
            }
            return View(PostVM);
        }


        public ActionResult LessonDetail(int lessonid = -1)
        {
            //TempData.Keep();
            //ViewBag.PostId = postid;
            Lesson model = db.LessonRepository.FindByID(lessonid);
            //var review = db.PostReviewRepository.GetAll().Where(m => m.PostId == model.Id).ToList();

            LessonDetailViewModel PostVM = new LessonDetailViewModel
            {
                Id = model.Id,
                PostName = db.PostRepository.FindByID(model.PostId).Name,
                PostId = model.PostId,                
                Name = model.Name,                
                Avatar = model.Avatar,
                Content = model.Content,
                Status = model.Status,
                Resume = model.Resume,
                Video = model.Video,
                Position = model.Position,                
                LessonTags = model.Tags,
                Exercises = model.Exercises,
                Questions = model.Questions
            };

            if (PostVM == null)
            {
                return RedirectToAction("Error404");
            }
            return View(PostVM);
        }

    }
}