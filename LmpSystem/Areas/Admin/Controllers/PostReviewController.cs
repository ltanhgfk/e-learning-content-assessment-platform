using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace LmpSystem.Areas.Admin.Controllers
{
    public class PostReviewController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {  
            List<PostReview> listPostReview = db.PostReviewRepository.GetAll().ToList();

            List<PostReviewDetailViewModel> PostReviewVmList = new List<PostReviewDetailViewModel>();

            //ViewBag.PostId = id;

            foreach (var item in listPostReview)
            {
                PostReviewVmList.Add(new PostReviewDetailViewModel
                {
                    Id = item.Id,
                    Time = item.Time,
                    Rating = item.Rating,
                    Comment = item.Comment,
                    LikeIt = item.LikeIt,
                    Username = db.UserRepository.FindByID(item.UserId).Username,                            
                    Status = item.Status,                   
                    PostName = db.PostRepository.FindByID((int)item.PostId).Name,                    
                });
            }           
           
            return View(PostReviewVmList);
        }

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

            List<SelectListItem> postList = db.PostRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
            .ToList();

            //postList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn bài viết" });            

            PostReviewEditViewModel model = new PostReviewEditViewModel
            {
                Users = userList,
                Posts = postList
            };

            return View(model);           
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(PostReviewEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            if (ModelState.IsValid)
            {
                PostReview nPostReview = new PostReview
                {  
                    Time = DateTime.Now,//model.Time,
                    PostId = model.PostId,
                    Rating = (int)model.Rating,
                    Comment = model.Comment,
                    LikeIt = model.LikeIt,
                    Id = model.Id,
                    Status = model.Status                    
                };               

                db.PostReviewRepository.Add(nPostReview);
                db.Commit();

                //return View(model);

                return RedirectToAction("Index", "PostReview");//, new { id = model.PostId });
            }
            else
            {
                List<SelectListItem> userList = db.UserRepository.GetAll().Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Username.ToString()
                })
            .ToList();

                //userList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });           

                List<SelectListItem> postList = db.PostRepository.GetAll().Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.Name.ToString()
                })
                .ToList();

                model.Users = userList;
                model.Posts = postList;

                return View(model);
            }

            //return View(model);

            //return RedirectToAction("Create", "PostReview");//, new { id = model.PostId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Clone(int id)
        {
            PostReview model = db.PostReviewRepository.FindByID(id);            

            ViewBag.ChangeAvatar = false;

            List<SelectListItem> userList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Username.ToString()
            })
            .ToList();

            //userList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });           

            List<SelectListItem> postList = db.PostRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
            .ToList();

            //postList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn bài viết" });

            PostReviewEditViewModel PostReviewVM = new PostReviewEditViewModel
            {
                Time = model.Time,
                PostId = (int)model.PostId,
                Rating = (int)model.Rating,
                Comment = model.Comment,
                LikeIt = model.LikeIt,
                Id = model.Id,
                Status = model.Status,
                Users = userList,
                Posts = postList

            };
            return View(PostReviewVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Clone(PostReviewEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            if (ModelState.IsValid)
            {

                PostReview nPostReview = new PostReview
                {
                    Time = DateTime.Now,  //model.Time,
                    PostId = model.PostId,
                    Rating = (int)model.Rating,
                    Comment = model.Comment,
                    LikeIt = model.LikeIt,
                    Id = model.Id,
                    Status = model.Status
                };

                db.PostReviewRepository.Add(nPostReview);
                db.Commit();

                return RedirectToAction("Index", "PostReview", new { id = model.PostId });
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            PostReview model = db.PostReviewRepository.FindByID(id);           

            ViewBag.ChangeAvatar = false;

            List<SelectListItem> userList = db.UserRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Username.ToString()
            })
           .ToList();

            //userList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn User" });           

            List<SelectListItem> postList = db.PostRepository.GetAll().Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
            .ToList();

            //postList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn bài viết" });

            PostReviewEditViewModel PostReviewVM = new PostReviewEditViewModel
            {
                Time = model.Time,
                PostId = (int)model.PostId,
                Rating = (int)model.Rating,
                Comment = model.Comment,
                LikeIt = model.LikeIt,
                Id = model.Id,
                Status = model.Status,
                Users = userList,
                Posts = postList
            };
            return View(PostReviewVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(PostReviewEditViewModel model)//, bool ChangeAvatar)//, HttpPostedFile AvatarFile)
        {
            if (ModelState.IsValid)
            {
                PostReview PostReview = db.PostReviewRepository.FindByID(model.Id);

                PostReview.Id = model.Id;
                PostReview.Time = DateTime.Now; //model.Time;
                PostReview.Comment = model.Comment;
                PostReview.Rating = (int)model.Rating;
                PostReview.PostId = model.PostId;
                PostReview.LikeIt = model.LikeIt;
                PostReview.Id = model.Id;                
                PostReview.Status = model.Status;               

                db.PostReviewRepository.Update(PostReview);
                db.Commit();

                return RedirectToAction("Index", "PostReview", new { id = model.PostId });
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            PostReview model = db.PostReviewRepository.FindByID(id);

            PostReviewDetailViewModel PostReviewVM = new PostReviewDetailViewModel
            {
                Id = model.Id,
                Time = model.Time,
                Rating = model.Rating,
                Comment = model.Comment,
                LikeIt = model.LikeIt,
                Username = db.UserRepository.FindByID(model.Id).Username,
                Status = model.Status,
                PostName = db.PostRepository.FindByID((int)model.PostId).Name,
            };

            return View(PostReviewVM);
        }        

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            string prefix = state ? "Đã duyệt" : "Đã hủy duyệt";
            PostReview u = db.PostReviewRepository.FindByID(id);
            try
            {
                u.Status = state;
                db.Commit();
                return Json(new { Message = prefix + " \"" + u.Comment + "\"" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Có lỗi xảy ra, chưa đổi trạng thái được!" }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            PostReview PostReviewObj = db.PostReviewRepository.FindByID(id);
            string Name = PostReviewObj.Comment;
            try
            {
                db.PostReviewRepository.Delete(PostReviewObj);
                db.Commit();
                return Json(new { Message = "Xóa '" + Name + "' thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Message = "Có lỗi xảy ra, chưa xóa '" + Name + "' được! <br />." }, JsonRequestBehavior.AllowGet);
            }
        }
   
    }
}