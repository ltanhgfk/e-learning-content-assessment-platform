using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CateController : Controller
    {
        private readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        [Authorize(Roles = "admin")]
        public ActionResult Index()//int post_type = 1)
        {
            //var enumDisplayStatus = ((PostType)post_type).ToString();
            //string stringValue = enumDisplayStatus.ToString();

            //List<Category> cateList = db.CategoryRepository.GetAll().Where(m => m.CateType == stringValue).ToList();//Where(m => m.Name.ToLower().Contains(titleStr.ToLower())).OrderBy(m => m.CreatedOnDate)
            List<Category> cateList = db.CategoryRepository.GetAll().ToList();

            List<CateListViewModel> cateVmList = new List<CateListViewModel>();            

            foreach (var item in cateList)
            {
                //string Status;
                //if (item.Status == true)
                //{
                //    Status =  "Hien thi";
                //}
                //else
                //{
                //    Status = "Chua hien thi";
                //}

                Enum.TryParse(item.CateType, out PostType CateType);

                cateVmList.Add(new CateListViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Position = item.Position,                    
                    ParentName = ((int)item.ParentId!=-1?db.CategoryRepository.FindByID((int)item.ParentId).Name:"Là danh mục gốc"), //GetCateParentById((int)item.ParentId),
                    ////CateType = GetCateTypeById((int)item.TypeId),// CateType.//getCateTypeById((int)item.TypeId),
                    //CateType = Function.GetEnumDisplayName(CateType), // CateType.ToString(), // cai nay dang dung moi nhat
                    Icon = item.Icon,
                    Status = (bool)item.Status
                    
                });
            }

            return View(cateVmList);
            
        }
        
        //public string GetCateParentById(int ParentId)
        //{
        //    if (ParentId != -1)
        //    {
        //        Category cateByParent = db.CategoryRepository.FindByID(ParentId);
        //        return cateByParent.Name;
        //    }
        //    else{
        //        return "Không có cha";
        //    }
        //}

        //public string GetCateTypeById(int typeId)
        //{
        //    if (typeId == 1)
        //        return Enum.GetName(typeof(CateType),CateType.Normal);//.Normal.ToString();
        //    else if (typeId == 2)
        //        return Enum.GetName(typeof(CateType), CateType.Discuss);//.Normal.ToString();
        //    else if (typeId == 3)
        //        return Enum.GetName(typeof(CateType), CateType.Slide);//.Normal.ToString();
        //    else
        //        return "khong co";
        //}

        // GET: Admin/Cate/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: Admin/Cate/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            var cateList = db.CategoryRepository.GetAll().Where(m => m.ParentId == -1).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })            
            .ToList();

            cateList.Insert(0, new SelectListItem() { Value = "-1" , Text = "Chọn danh mục cha"});
            ViewBag.ParentCateList = cateList;           

            CateEditViewModel model = new CateEditViewModel
            {
                Status = true,
                //CateType = PostType.article
            };

            return View(model);
        }

        // POST: Admin/Cate/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(CateEditViewModel model)
        {
            try
            {
                Category cate = new Category
                {
                    Name = model.Name,
                    Icon = model.Icon,
                    ParentId = model.ParentId,
                    CateType = "article",       //model.CateType.ToString(),
                    Position = model.Position,
                    Status = model.Status
                };
                db.CategoryRepository.Add(cate);
                db.CategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }            
        }

        [Authorize(Roles = "admin")]
        public ActionResult Clone(int id)
        {
            var ParentCateList = db.CategoryRepository.GetAll().Where(m => m.ParentId == -1).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
           .ToList();

            ParentCateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn danh mục cha" });

            ViewBag.ParentCateList = ParentCateList;

            Category model = db.CategoryRepository.FindByID(id);

            //Enum.TryParse(model.CateType, out PostType CateType);

            CateEditViewModel CateEditVM = new CateEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Icon = model.Icon,
                ParentId = (int)model.ParentId,
                Position = model.Position,
                //CateType = CateType,
                Status = model.Status
            };
            return View(CateEditVM);
        }

        // POST: Admin/Cate/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Clone(CateEditViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                Category cate = new Category
                {
                    Name = model.Name,
                    Icon = model.Icon,
                    ParentId = model.ParentId,
                    CateType = "article",       //model.CateType.ToString(),
                    Position = model.Position,
                    Status = model.Status
                };
                db.CategoryRepository.Add(cate);
                db.CategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Cate/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            var ParentCateList = db.CategoryRepository.GetAll().Where(m => m.ParentId == -1).Select(a => new SelectListItem()
            {
                Value = a.Id.ToString(),
                Text = a.Name.ToString()
            })
           .ToList();

            ParentCateList.Insert(0, new SelectListItem() { Value = "-1", Text = "Chọn danh mục cha" });

            ViewBag.ParentCateList = ParentCateList;

            Category model = db.CategoryRepository.FindByID(id);

            //Enum.TryParse(model.CateType, out PostType CateType);

            CateEditViewModel CateEditVM = new CateEditViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Icon = model.Icon,
                ParentId = (int)model.ParentId,
                Position = model.Position,
                //CateType = CateType,
                Status = model.Status               
            };
            return View(CateEditVM);                 
        }

        // POST: Admin/Cate/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(CateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {                    
                    Category Cate = db.CategoryRepository.FindByID(model.Id);
                    Cate.Name = model.Name;
                    Cate.ParentId = model.ParentId;
                    Cate.Position = model.Position;
                    //Cate.CateType = model.CateType.ToString();
                    Cate.Status = model.Status;
                    Cate.Icon = model.Icon;

                    db.CategoryRepository.Update(Cate);
                    db.Commit();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }        

        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            try
            {
                string prefix = state ? "khả dụng" : "không khả dụng";
                Category u = db.CategoryRepository.FindByID(id);
                if (u != null)
                {
                    u.Status = state;
                    db.Commit();
                    return Json(new { Message = "Thư mục \"" + u.Name + "\" đã " + prefix }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "<strong>Không thay đổi được trạng thái hiển thị</strong>" }, JsonRequestBehavior.AllowGet);
            }catch(Exception e)
            {
                return Json(new { Message = "Xảy ra lỗi: " + e.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult DeleteCate(int id)
        {
            try
            {
                Category Cate = db.CategoryRepository.FindByID(id);
                //int parentId = Convert.ToInt32(Cate.Id);

                if (Cate.ParentId == -1)//nếu là danh mục gốc thì
                {
                    List<Category> subcate = db.CategoryRepository.GetAll().Where(m => m.ParentId == id).ToList();
                    if (subcate.Count >= 1)//nếu có con thì không xóa được
                    {
                        return Json(new { reload = false, Message = "<strong>Không xóa được!</strong></br>Danh mục có danh mục con nên không xóa được!" }, JsonRequestBehavior.AllowGet);
                    }//Nếu không có con mà có bài viết trực tiếp thì cũng không xóa được
                    else if (db.PostRepository.GetAll().Where(m => m.CateId == id).ToList().Count >= 1)
                    {
                        return Json(new { reload = false, Message = "<strong>Không xóa được!</strong></br>Danh mục có chứa bài viết nên không xóa được!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.CategoryRepository.Delete(Cate);
                        //xóa ảnh đại diện cũ
                        bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/files/" + Cate.QuestionFile));
                        if (exist)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Upload/files/" + Cate.QuestionFile));
                        }
                        //db.CategoryRepository.SaveChanges();
                        db.Commit();
                        return Json(new { reload = true, Message = "<strong>Xóa thành công!</strong>" }, JsonRequestBehavior.AllowGet);
                    }
                }//Nếu không phải là danh mục gốc
                else if (db.PostRepository.GetAll().Where(m => m.CateId == id).ToList().Count >= 1)
                {
                    return Json(new { reload = false, Message = "<strong>Không xóa được!</strong></br>Danh mục có chứa bài viết nên không xóa được!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.CategoryRepository.Delete(Cate);
                    bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/files/" + Cate.QuestionFile));
                    if (exist)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/files/" + Cate.QuestionFile));
                    }
                    //db.CategoryRepository.SaveChanges();
                    db.Commit();
                    //db.CategoryRepository.SaveChanges();
                    return Json(new { reload = true, Message = "<strong>Xóa thành công!</strong>" }, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception e)
            {
                return Json(new { reload = false, Message = "<strong>Có lỗi xảy ra!</strong></br>Có thể danh mục có chứa bài viết nên không xóa được!: "+e.Message }, JsonRequestBehavior.AllowGet);
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

    }
}
