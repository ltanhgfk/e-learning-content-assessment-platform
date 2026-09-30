using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;

namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserScoreController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());       

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {            
            List<UserScore> FbList = db.UserScoreRepository.GetAll().OrderBy(m => m.CreatedOnDate).ToList();
            return View(FbList);
        }

        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id)
        {
            UserScore model = db.UserScoreRepository.FindByID(id);
            //ViewBag.ChangeNote = false;
            //GetData getGender = new GetData();
            //ViewBag.Genders = getGender.GetListGenderEnum();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(UserScore model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserScore UserScore = db.UserScoreRepository.FindByID(model.Id);

                    UserScore.Id = model.Id;
                    UserScore.PostId = model.PostId;                   
                    UserScore.CreatedOnDate = DateTime.Today;
                    UserScore.Email = model.Email;
                    UserScore.TestDetail = model.TestDetail;
                    UserScore.TotalNumberQuestion = model.TotalNumberQuestion;
                    UserScore.TrueNumberAnswer = model.TrueNumberAnswer;
                    UserScore.UserName = model.UserName;

                    db.UserScoreRepository.Update(UserScore);
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

        [HttpPost]
        [Authorize(Roles = "Admin")]       
        public JsonResult DeleteUserScore(int id)
        {
            UserScore UserScoreObj = db.UserScoreRepository.FindByID(id);            
            try
            {
                db.UserScoreRepository.Delete(UserScoreObj);                
                db.Commit();
                return Json(new { Message = "<strong>Đã xóa thành công!</strong>" }, JsonRequestBehavior.AllowGet);                
            }
            catch
            {
                return Json(new { Message = "<strong>Xóa không thành công!</strong>" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[Authorize(Roles = "Admin")]
        //public ActionResult Details(int id)
        //{
        //    UserScore model = db.UserScoreRepository.FindByID(id);
        //    return View(model);
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            List<Question> listQuestion = new List<Question>();
            //UserScore uc = db.UserScoreRepository.GetAll().Where(m => m.Email == email).OrderByDescending(u => u.Id).FirstOrDefault();
            UserScore uc = db.UserScoreRepository.FindByID(id);

            if (uc != null)
            {
                string[] list = uc.TestDetail.Split('#');
                ViewBag.SoCauDung = uc.TrueNumberAnswer;
                ViewBag.TongSoCau = uc.TotalNumberQuestion;
                ViewBag.HoVaTen = uc.UserName;
                ViewBag.Email = uc.Email;
                ViewBag.MonHoc = db.PostRepository.FindByID((int)uc.PostId).Name;
                ViewBag.NgayGioLamKT = uc.CreatedOnDate;

                string[] listYourAns = new string[list.Length - 1];

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
                        YourAnswer = listYourAns[j] == "Option1" ? item.Option1 : listYourAns[j] == "Option2" ? item.Option2 : listYourAns[j] == "Option3" ? item.Option3 : listYourAns[j] == "Option4" ? item.Option4 : "Bạn không trả lời câu hỏi!"
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
    }
}