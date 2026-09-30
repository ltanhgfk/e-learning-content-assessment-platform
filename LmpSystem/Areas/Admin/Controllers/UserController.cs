using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using LmpSystem.Common;
using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
//using PagedList;
//using PagedList.Mvc;

namespace LmpSystem.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        [Authorize(Roles = "Admin")]
        public ActionResult Index()//int? page)//, string titleStr)
        {            
            List<User> listPost = db.UserRepository.GetAll().OrderBy(m => m.CreatedOnDate).ToList();
            List<UserDetailViewModel> UserVmList = new List<UserDetailViewModel>();

            foreach (var item in listPost)
            {
                //int userRole = (int)item.UserRole;//Convert.ToInt32(item.UserRole.Trim());
                UserVmList.Add(new UserDetailViewModel
                {
                    Id = item.Id,
                    //Code = item.Code,
                    Title = item.Title,
                    Avatar = item.Avatar,
                    Username = item.Username,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserRole = item.UserRole.ToString(), //Extensions.GetDisplayNameEnum((Role)item.UserRole.ToString()).ToString(),//item.UserRole.ToString(),
                    Status = item.Status,
                    //DepartmentId = item.DepartmentId,
                });
            }           
            return View(UserVmList);
        }

        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {
           // List<SelectListItem> departmentList = db.DepartmentRepository.GetAll().Select(a => new SelectListItem()
           // {
           //     Value = a.Id.ToString(),
           //     Text = a.Name.ToString()
           // })
           //.ToList();

            GetData getGender = new GetData();
            ViewBag.Genders = getGender.GetListGenderEnum();

            UserEditViewModel model = new UserEditViewModel
            {
                UserRole = Role.Chuyenvien,
                Status = true,
                //Password = "123",   //model.Password,
                //RePassword = "123",
                //DepartmentId = false,
                //UserRegister = GetData.GetListPost(),
                //ChangeAvatar = true,
                //Department = departmentList,
            };
            //ViewBag.error = "Tên người dùng này đã tồn tại";
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(UserEditViewModel model)//, HttpPostedFileBase AvatarFile)
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
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), 130, 195);
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
                        //User user = db.UserRepository.FindByUsername(model.Username);
                        List<User> userList = db.UserRepository.GetAll().Where(m => m.Username == model.Username).ToList();

                        if (userList.Count <= 0)
                        {
                            User nuser = new User
                            {
                                //Code = model.Code,
                                Title = model.Title,
                                Username = model.Username,
                                FirstName = model.FirstName,
                                LastName = model.LastName,

                                UserRole = model.UserRole.ToString(),
                                //Password = Common.Function.CalculateMD5Hash(model.Password),
                                Password = Common.Function.CalculateMD5Hash("123456"),

                                Gender = model.Gender.ToString(),
                                Birthday = model.Birthday,
                                Phone = model.Phone,
                                Email = model.Email,
                                Address = model.Address,
                                Resume = model.Resume,
                                Content = model.Content,
                                Avatar = model.Avatar,
                                Status = model.Status,
                                //DepartmentId = model.DepartmentId,
                                CreatedOnDate = DateTime.Today,
                                LastModifiedOnDate = DateTime.Today,
                                CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap
                                LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap

                            };

                            //foreach (var i in postList)
                            //{
                            //    Post post = db.PostRepository.FindByID(i.Id);
                            //    nuser.Posts1.Add(post);
                            //    post.Users.Add(nuser);
                            //}

                            db.UserRepository.Add(nuser);
                            db.Commit();
                            return RedirectToAction("Index");
                        }
                        //ViewBag.error = "Tên người dùng này đã tồn tại";
                        TempData["error"] = "Tên người dùng này đã tồn tại";
                        return RedirectToAction("Create");
                    }
                    catch (Exception e)
                    {
                        //ViewBag.error = "Xảy ra lỗi \"" + e.Message + "\" trong quá trình xử lý!";
                        //return View();
                        TempData["error"] = "Xảy ra lỗi \"" + e.Message + "\" trong quá trình xử lý!";
                        return RedirectToAction("Create");
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Clone(int id)
        {
            User model = db.UserRepository.FindByID(id);
            Enum.TryParse(model.UserRole.ToString(), out Role role);

           // List<SelectListItem> departmentList = db.DepartmentRepository.GetAll().Select(a => new SelectListItem()
           // {
           //     Value = a.Id.ToString(),
           //     Text = a.Name.ToString()
           // })
           //.ToList();

            GetData getGender = new GetData();
            ViewBag.Genders = getGender.GetListGenderEnum();

            //List<SelectListItem> post = GetData.GetListPost();

            //foreach (var x in post)
            //{
            //    //foreach (var t in model.PostTags)
            //    foreach (var t in model.Posts1)
            //    {
            //        if (x.Value == t.Id.ToString())
            //        {
            //            x.Selected = true;
            //        }
            //    }
            //}

            UserEditViewModel userVM = new UserEditViewModel
            {
                //Id = model.Id,
                //Code = model.Code,
                Title = model.Title,
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
               
                UserRole = role,
                //Password = "123",   //model.Password,
                //RePassword = "123",
                Gender = model.Gender.ToString(),
                Birthday = model.Birthday,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                Resume = model.Resume,
                Content = model.Content,
                Avatar = model.Avatar,
                Status = model.Status,
                //DepartmentId = model.DepartmentId,
                
                //UserRegister = post                
                //CreatedOnDate = DateTime.Today,
                //LastModifiedOnDate = DateTime.Today,
                //CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap
                //LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap
            };
            return View(userVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Clone(UserEditViewModel model)//, HttpPostedFileBase AvatarFile)
        {
            //List<Post> postList = new List<Post>();
            //postList.AddRange(model.UserRegister.Where(m => m.Selected)
            //    .Select(m => new Post { Id = int.Parse(m.Value), Name = m.Text })
            //    );

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
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), 130, 195);
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
                    try
                    {
                        //User user = db.UserRepository.FindByUsername(model.Username);
                        List<User> userList = db.UserRepository.GetAll().Where(m => m.Username == model.Username).ToList();

                        if (userList.Count <= 0)
                        {
                            User nuser = new User
                            {
                                //Code = model.Code,
                                Title = model.Title,
                                Username = model.Username,
                                FirstName = model.FirstName,
                                LastName = model.LastName,

                                UserRole = model.UserRole.ToString(),
                                //Password = Common.Function.CalculateMD5Hash(model.Password),
                                Password = Common.Function.CalculateMD5Hash("123456"),
                                Gender = model.Gender.ToString(),
                                Birthday = model.Birthday,
                                Phone = model.Phone,
                                Email = model.Email,
                                Address = model.Address,
                                Resume = model.Resume,
                                Content = model.Content,
                                Avatar = model.Avatar,
                                Status = model.Status,
                                //DepartmentId = model.DepartmentId,
                                CreatedOnDate = DateTime.Today,
                                LastModifiedOnDate = DateTime.Today,
                                CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap
                                LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap

                            };

                            //foreach (var i in postList)
                            //{
                            //    Post post = db.PostRepository.FindByID(i.Id);
                            //    nuser.Posts1.Add(post);
                            //    post.Users.Add(nuser);
                            //}

                            db.UserRepository.Add(nuser);
                            db.Commit();
                            return RedirectToAction("Index");
                        }
                        TempData["error"] = "Tên người dùng này đã tồn tại";
                        return RedirectToAction("Clone");
                    }
                    catch(Exception e)
                    {
                        TempData["error"] = "Xảy ra lỗi \"" + e.Message + "\" trong quá trình xử lý!";
                        return RedirectToAction("Clone");
                    }
                }                
            }
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id)
        {
            User model = db.UserRepository.FindByID(id);
            Enum.TryParse(model.UserRole.ToString(), out Role role);

            ViewBag.ChangeAvatar = false;

           // List<SelectListItem> departmentList = db.DepartmentRepository.GetAll().Select(a => new SelectListItem()
           // {
           //     Value = a.Id.ToString(),
           //     Text = a.Name.ToString()
           // })
           //.ToList();

            GetData getGender = new GetData();
            ViewBag.Genders = getGender.GetListGenderEnum();

            //List<SelectListItem> post = GetData.GetListPost();

            //foreach (var x in post)
            //{
            //    //foreach (var t in model.PostTags)
            //    foreach (var t in model.Posts1)
            //    {
            //        if (x.Value == t.Id.ToString())
            //        {
            //            x.Selected = true;
            //        }
            //    }
            //}

            UserEditViewModel userVM = new UserEditViewModel
            {
                Id = model.Id,
                //Code = model.Code,
                Title = model.Title,
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,

                UserRole = role,

                //Password = "123",   //model.Password,
                //RePassword = "123",

                Gender = model.Gender.ToString(),
                Birthday = model.Birthday,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                Resume = model.Resume,
                Content = model.Content,
                Avatar = model.Avatar,
                Status = model.Status,
                //DepartmentId = model.DepartmentId,
                //Department = departmentList,
                //UserRegister = post                
                //CreatedOnDate = DateTime.Today,
                //LastModifiedOnDate = DateTime.Today,
                //CreatedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap
                //LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id,//nguoi dang nhap

            };
            return View(userVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(UserEditViewModel model)//, HttpPostedFile AvatarFile)
        {
            //List<Post> postList = new List<Post>();
            //postList.AddRange(model.UserRegister.Where(m => m.Selected)
            //    .Select(m => new Post { Id = int.Parse(m.Value), Name = m.Text })
            //    );

            //List<Post> unPostList = new List<Post>();
            //unPostList.AddRange(model.UserRegister.Where(m => m.Selected == false)
            //    .Select(m => new Post { Id = int.Parse(m.Value), Name = m.Text })
            //    );

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
                            Image img = Extensions.ResizeImage(Bitmap.FromStream(model.AvatarFile.InputStream), 130, 195);
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
                        //model.Avatar = "user" + "-" + dt + extension;
                        //model.AvatarFile.SaveAs(Server.MapPath("~/Upload/images/") + model.Avatar);
                    }
                }
                catch (Exception)
                {
                    isSavedSuccessfully = false;
                }
                if (isSavedSuccessfully)
                {
                    //List<User> userList = db.UserRepository.GetAll().Where(m => m.Username.Equals(model.Username)).ToList();
                    try
                    {
                        User user = db.UserRepository.FindByID(model.Id);

                        user.Id = model.Id;
                        //user.Code = model.Code;
                        user.Title = model.Title;
                        user.Username = model.Username;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Status = model.Status;
                        //user.DepartmentId = model.DepartmentId;
                        user.UserRole = model.UserRole.ToString();
                        //user.Password = Common.CommonFunction.CalculateMD5Hash(model.Password);
                        user.Gender = model.Gender;
                        user.Birthday = model.Birthday;
                        user.Phone = model.Phone;
                        user.Email = model.Email;
                        user.Address = model.Address;
                        user.Resume = model.Resume;
                        user.Content = model.Content;
                        user.Avatar = model.Avatar;
                        //user.CreatedOnDate = DateTime.Today;
                        user.LastModifiedOnDate = DateTime.Today;
                        user.LastModifiedByUserId = db.UserRepository.FindByUsername(User.Identity.Name).Id;    //nguoi dang nhap

                        //foreach (var i in postList)
                        //{
                        //    Post post = db.PostRepository.FindByID(i.Id);
                        //    if (!user.Posts1.Contains(post))
                        //    {
                        //        user.Posts1.Add(post);
                        //        post.Users.Add(user);
                        //    }                    
                        //}

                        //foreach (var i in unPostList)
                        //{
                        //    Post post = db.PostRepository.FindByID(i.Id);
                        //    if (user.Posts1.Contains(post))
                        //    {
                        //        user.Posts1.Remove(post);
                        //        post.Users.Remove(user);
                        //    }
                        //}

                        db.UserRepository.Update(user);
                        db.Commit();
                        return RedirectToAction("Index");
                    }
                    catch(Exception e)
                    {
                        //ViewBag.anno = "Tên người dùng này đã tồn tại";
                        TempData["error"] = "Xảy ra lỗi \"" + e.Message + "\" trong quá trình xử lý!";
                        return View();// RedirectToAction("Edit");                        
                    }                    
                }                
            }
            return View();
        }

        [Authorize(Roles="Admin")]
        public ActionResult Details(int id)
        {
            User model = db.UserRepository.FindByID(id);

            //Enum.TryParse(model.UserRole.ToString(), out Role role);
            //int userRole = Convert.ToInt32(model.UserRole.Trim());

            UserDetailViewModel userVM = new UserDetailViewModel
            {
                Id = model.Id,
                //Code = model.Code,
                Title = model.Title,
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,

                //UserRole = Extensions.GetDisplayNameEnum((Role)userRole).ToString(),//model.UserRole.ToString(),
                UserRole = model.UserRole,
                Password = Common.Function.CalculateMD5Hash(model.Password),
                Gender = model.Gender.ToString(),
                Birthday = model.Birthday,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                //Resume = model.Resume,
                //Content = model.Content,
                Avatar = model.Avatar,
                Status = model.Status,
                //DepartmentId = model.DepartmentId,

                CreatedOnDate = model.CreatedOnDate,
                LastModifiedOnDate = model.LastModifiedOnDate,
                CreatedByUser = db.UserRepository.FindByID((int)model.CreatedByUserId).FirstName,
                LastModifiedByUser = db.UserRepository.FindByID((int)model.LastModifiedByUserId).FirstName,

                //Invoices = GetInvoiceList(),
                //Posts = GetPostList(),
                //UserScores = GetUserScoreList(),
                //PostReviews = GetPostReviewList();

                ////Products = GetProductList(),
                ////Teachers = GetTeacherList(),
                ////UserPosts = GetUserPostList()
            };
            return View(userVM);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult ResetPassword(int id)
        {
            try
            {
                //string prefix = "Reset mật khẩu cho ";
                User user = db.UserRepository.FindByID(id);
                if (user != null)
                {
                    user.Password = Common.Function.CalculateMD5Hash("123456");
                    db.Commit();
                    //return Json(new { Message = prefix + " \"" + user.Username + "\" thành công!" }, JsonRequestBehavior.AllowGet);
                    return Json(new { Message = "<Strong>Đã reset mật khẩu thành công!</strong>" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "<Strong>Chưa reset được mật khẩu!</strong>" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Message = "<Strong>Có lỗi xảy ra (" + e.Message + "), chưa reset được!</strong>" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = true)
        {
            try
            {
                //string prefix = state ? "Đã duyệt" : "Đã hủy duyệt";
                User u = db.UserRepository.FindByID(id);
                if (u.UserRole != "admin")//coi lai cho này => dung userrole
                {
                    u.Status = state;
                    db.Commit();
                    //return Json(new { Message = prefix + " \"" + u.Username + "\"" }, JsonRequestBehavior.AllowGet);
                    return Json(new { Message = "<Strong>Đã thay đổi trạng thái thành công!</strong>" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Message = "<Strong>Không được thay đổi trạng thái Admin!</strong>" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Message = "<Strong>Có lỗi xảy ra ("+e.Message+"), chưa thay đổi trạng thái được!</strong>" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public JsonResult ChangeStatusTeacher(int id, bool state = true)
        //{
        //    string prefix = state ? "Đã duyệt giáo viên" : "Đã hủy duyệt giáo viên";
        //    User u = db.UserRepository.FindByID(id);
        //    if (u.Username != "admin")//coi lai cho này => dung userrole
        //    {
        //        //u.DepartmentId = state;
        //        db.Commit();
        //        return Json(new { Message = prefix + " \"" + u.Username + "\"" }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { Message = "Không được hủy duyệt admin" }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [Authorize(Roles = "Admin")]       
        public JsonResult DeleteUser(int id)
        {
            User UserObj = db.UserRepository.FindByID(id);
            
            try
            {
                db.UserRepository.Delete(UserObj);
                //xóa ảnh đại diện cũ
                bool exist = System.IO.File.Exists(Server.MapPath("~/Upload/images/" + UserObj.Avatar));
                if (exist)
                {
                    System.IO.File.Delete(Server.MapPath("~/Upload/images/" + UserObj.Avatar));
                }
                db.Commit();
                return Json(new { Message = "<strong>Đã xóa người dùng thành công!</strong>" }, JsonRequestBehavior.AllowGet);                
            }
            catch
            {
                return Json(new { Message = "<strong>Xóa không thành công!</strong> </br> Lỗi có thể do người dùng này vẫn còn bài viết chưa xóa, để xóa được trước tiên cần xóa các dữ liệu liên quan trước!" }, JsonRequestBehavior.AllowGet);
            }
        }       
    }
}