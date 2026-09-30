using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;


namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());       

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {            
            List<Feedback> FbList = db.FeedbackRepository.GetAll().OrderBy(m => m.CreatedOnDate).ToList();

            //List<Feedback> FBList = new List<Feedback>();
            //foreach (var item in listPost)
            //{                
            //    FBList.Add(new Feedback
            //    {
            //        Id = item.Id,                    
            //        FullName = item.FullName,
            //        Note = item.Note,
            //        Subject = item.Subject,
            //        Email = item.Email,
            //        Content = item.Content,                    
            //        Status = item.Status,   
            //        CreatedOnDate = item.CreatedOnDate
            //    });
            //}
            
            return View(FbList);
        }

        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id)
        {
            Feedback model = db.FeedbackRepository.FindByID(id);
            ViewBag.ChangeNote = false;
            GetData getGender = new GetData();
            ViewBag.Genders = getGender.GetListGenderEnum();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Feedback model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Feedback Feedback = db.FeedbackRepository.FindByID(model.Id);

                    Feedback.Id = model.Id;                    
                    //Feedback.FullName = model.FullName;
                    //Feedback.Subject = model.Subject;
                    //Feedback.Email = model.Email;
                    //Feedback.CreatedOnDate = model.CreatedOnDate;
                    
                    Feedback.Status = model.Status;                   
                    Feedback.Content = model.Content;
                    Feedback.Note = model.Note;                   

                    db.FeedbackRepository.Update(Feedback);
                    db.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    //ViewBag.anno = "Tên người dùng này đã tồn tại";
                    TempData["error"] = "Xảy ra lỗi \"" + e.Message + "\" trong quá trình xử lý!";
                    return View();// RedirectToAction("Edit");                        
                }
            }
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult Details(int id)
        {
            Feedback model = db.FeedbackRepository.FindByID(id);
            return View(model);
        }     

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            try
            {                
                Feedback u = db.FeedbackRepository.FindByID(id);
                u.Status = state;
                db.Commit();                
                return Json(new { Message = "<Strong>Đã thay đổi trạng thái thành công!</strong>" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Message = "<Strong>Có lỗi xảy ra (" + e.Message + "), chưa thay đổi trạng thái được!</strong>" }, JsonRequestBehavior.AllowGet);
            }
        }      

        [HttpPost]
        [Authorize(Roles = "Admin")]       
        public JsonResult DeleteFeedback(int id)
        {
            Feedback FeedbackObj = db.FeedbackRepository.FindByID(id);            
            try
            {
                db.FeedbackRepository.Delete(FeedbackObj);                
                db.Commit();
                return Json(new { Message = "<strong>Đã xóa thành công!</strong>" }, JsonRequestBehavior.AllowGet);                
            }
            catch
            {
                return Json(new { Message = "<strong>Xóa không thành công!</strong>" }, JsonRequestBehavior.AllowGet);
            }
        }       
    }
}