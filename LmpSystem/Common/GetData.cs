using LmpSystem.Models;
using LmpSystem.Repository;
using LmpSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace LmpSystem.Common
{
    public class GetData
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        public LmpInfo GetWebInfo()
        {
            return db.LmpInfoRepository.GetLmpInfo();
        }

        public List<LmpInfo> GetListLmpInfoByCode(string code = "")
        {
            return db.LmpInfoRepository.GetAll()
                .Where(m => m.Status == true && m.Code == code).ToList();
        }

        public LmpInfo GetLmpInfoByCode(string code = "")
        {
            var data = db.LmpInfoRepository.GetLmpInfoByCode(code, true);            
            return data;
        }

        public List<Post> GetPostByType(string type = "")//PostDetailViewModel
        {
            return db.PostRepository.GetAll() //
                .Where(m => m.Status == true && m.PostType == type).OrderByDescending(m => m.LastModifiedOnDate).ToList();
        }

        public List<Category> GetRootCategories()
        {
            List<Category> Categories = db.CategoryRepository.GetAll().Where(m => m.Status == true && m.ParentId ==-1).OrderBy(m=>m.Position).ToList();
            return Categories;
        }

        public List<Category> GetRootCategoriesByType(string other = "")
        {
            List<Category> Categories = db.CategoryRepository.GetAll().Where(m => m.Status == true && m.ParentId == -1 && m.CateType == other).OrderBy(m => m.Position).ToList();
            return Categories;
        }

        public List<Category> GetSubCategories(int catid)
        {
            List<Category> Categories = db.CategoryRepository.GetAll().Where(m => m.Status == true && m.ParentId == catid).OrderBy(m => m.Position).ToList();
            return Categories;
        }

        public List<Category> GetOtherSubCategories(int subcatid)
        {
            int parentId = (int)db.CategoryRepository.FindByID(subcatid).ParentId;
            List<Category> Categories = db.CategoryRepository.GetAll().Where(m => m.Status == true && m.ParentId == parentId).OrderBy(m => m.Position).ToList();
            return Categories;
        }
        
        public string GetFullnameByUserId(int userId)
        {
            //return db.UserRepository.FindByID(userId).FirstName + " " + db.UserRepository.FindByID(userId).LastName + " (" + db.UserRepository.FindByID(userId).Username + ")";
            if (userId >= 1)
                return db.UserRepository.FindByID(userId).FirstName + " " + db.UserRepository.FindByID(userId).LastName;        // + " (" + db.DepartmentRepository.FindByID((int)db.UserRepository.FindByID(userId).DepartmentId).Code + ")";
            else
                return "Không có chỉnh sửa";
        }        
        public static List<SelectListItem> GetListTags()
        {
            UnitOfWork db = new UnitOfWork(new LmpSystemEntities());
            List<SelectListItem> lstTag = db.TagRepository.GetAll()
                .Select(m =>
                new SelectListItem
                {
                    Text = m.TagName,
                    Value = m.TagID.ToString(),
                }
                ).ToList();
            return lstTag;
        }

        public List<Tag> GetTagList()//PostDetailViewModel
        {
            return db.TagRepository.GetAll().ToList();
        }      

        public static List<SelectListItem> GetListPost()
        {
            UnitOfWork db = new UnitOfWork(new LmpSystemEntities());
            List<SelectListItem> lstPost = db.PostRepository.GetAll().Where(m=>m.Status==true).OrderByDescending(m => m.LastModifiedOnDate)//.OrderBy(m => m.Position)//them dieu kien sau nay...
                .Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString(),
                }
                ).ToList();
            return lstPost;
        }

        //public IEnumerable<ViewModel.PostType> GetListPostType()
        public SelectList GetListPostType()
        {
            //List<Days> days = Enum.GetValues(typeof(Days))
            //              .Cast<Days>()
            //            .ToList();

            return new SelectList(Enum.GetValues(typeof(LmpSystem.Common.PostType)).Cast<LmpSystem.Common.PostType>().Select(e => new { Id = Convert.ToInt32(e), Name = e.GetDisplayName() }), "Id", "Name");


            //.Cast<ViewModel.PostType>().ToList();

            //db.postRepository.tagRepository.AllTags().Select(
            //m => new ViewModel.ListTagViewModel
            //{
            //    TagID = m.TagID,
            //    TagName = m.TagName
            //}).ToList();
        }       

        public SelectList GetListGenderEnum()
        {
            return new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(e => new { Id = Convert.ToInt32(e), Name = e.GetDisplayName() }), "Id", "Name");
        }

        public SelectList GetListInfoCodeEnum()
        {
            return new SelectList(Enum.GetValues(typeof(Code)).Cast<Code>().Select(e => new { Id = Convert.ToInt32(e), Name = e.GetDisplayName() }), "Id", "Name");
        }
        //Rewrite URL---------------------------
        public string GenerateFriendlyName(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD).Trim();
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');

            //sb = sb.Replace(' ','-');
            //sb = sb.Append(".html");

            string str = GetByteArray(sb.ToString()).ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");// Remove invalid characters for param  
            str = Regex.Replace(str, @"\s+", "-").Trim(); // convert multiple spaces into one hyphens   
            str = Regex.Replace(str, @"\s", "-"); // Replaces spaces with hyphens   

            //string ret = GenerateParam(sb.ToString().Normalize(NormalizationForm.FormD).ToLower());
            //return sb.ToString().Normalize(NormalizationForm.FormD).ToLower();

            return str;
        }
        public string GenerateParam(string name)
        {            
            string str = GetByteArray(name).ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");// Remove invalid characters for param  
            str = Regex.Replace(str, @"\s+", "-").Trim(); // convert multiple spaces into one hyphens   
            //str = str.Substring(0, str.Length <= 30 ? str.Length : 30).Trim(); //Trim to max 30 char  
            str = Regex.Replace(str, @"\s", "-"); // Replaces spaces with hyphens     
            return str;
        }
        public string GenerateItemNameAsParam(int Id, string Name)
        {
            string phrase = string.Format("{0}-{1}", Id, Name);// Creates in the specific pattern  
            string str = GetByteArray(phrase).ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");// Remove invalid characters for param  
            str = Regex.Replace(str, @"\s+", "-").Trim(); // convert multiple spaces into one hyphens   
            str = str.Substring(0, str.Length <= 30 ? str.Length : 30).Trim(); //Trim to max 30 char  
            str = Regex.Replace(str, @"\s", "-"); // Replaces spaces with hyphens     
            return str;
        }
        private string GetByteArray(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        //-------------------------------------
    }
}