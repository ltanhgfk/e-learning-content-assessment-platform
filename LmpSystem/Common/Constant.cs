using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace LmpSystem.Common
{
    public class Constant
    {
        public static string CREATE_SUCCESSFULLY = "CREATE_SUCCESSFULLY";
        public static string SAVE_SUCCESSFULLY = "SAVE_SUCCESSFULLY";
        public static string SAVE_FAILED = "SAVE_FAILED";
        public static string DELETE_SUCCESSFULLY = "DELETE_SUCCESSFULLY";
        public static string DELETE_FAILED = "DELETE_FAILED";
        public static string TEMP_CATEGORY_IMAGE = "TEMP_CATEGORY_IMAGE";
        public static string TEMP_CONFIGURATION_IMAGE = "TEMP_CONFIGURATION_IMAGE";
        public static string TEMP_BLOG_IMAGE = "TEMP_BLOG_IMAGE";
        public static string TEMP_PRODUCT_IMAGE = "TEMP_PRODUCT_IMAGE";
        public static string ADMIN_LOGIN_SESSION = "ADMIN_LOGIN_SESSION";
        public static string LOGIN_SUCCESSFULLY = "LOGIN_SUCCESSFULLY";
        public static string CLIENT_LOGIN_SUCCESSFULLY = "CLIENT_LOGIN_SUCCESSFULLY";
        public static string TOTAL_VISITORS = "TOTAL_VISITORS";
        public static string TEMP_ADMIN_IMAGE = "TEMP_ADMIN_IMAGE";
        public static string USER_LOGIN_MODEL = "USER_LOGIN_MODEL";
        public static string CART_SESSION = "CART_SESSION";
        public static string REGISTER_SUCCESSFULLY = "REGISTER_SUCCESSFULLY";
    }

    public enum Code : int
    {
        [Display(Name = "Tên web")]
        web_name = 0,
        [Display(Name = "Logo")]
        web_logo = 1,
        [Display(Name = "Khẩu ngữ")]
        web_slogan = 2,
        [Display(Name = "Danh mục đầu trang")]
        top_menu_list = 3,
        [Display(Name = "Banner QC đầu trang")]
        top_ad_banner = 4,        
        [Display(Name = "Mạng xã hội")]
        social_network_list = 5,
        [Display(Name = "Video giới thiệu (youtube)")]
        top_right_youtube_video = 6,
        [Display(Name = "Banner quảng cáo số 1 bên phải")]
        first_right_ad_banner = 7,
        [Display(Name = "Banner quảng cáo số 2 bên phải")]
        second_right_ad_banner = 8,
        [Display(Name = "Facebook Fanpage")]
        fanpage_facebook = 9,
        [Display(Name = "Google map")]
        googlemap = 10,
        [Display(Name = "Dòng bản quyền (copyright)")]
        copyright = 11,
        [Display(Name = "Người phát triển (developer)")]
        developed = 12,
        [Display(Name = "Giới thiệu trang liên hệ")]
        contact_intro = 13,
        [Display(Name = "Giới thiệu website")]
        web_intro = 14,
        [Display(Name = "Thông tin liên hệ (address, mail, phone)")]
        contact_info_list = 15,
        [Display(Name = "Liên kết website")]
        website_link_list = 16,
        [Display(Name = "Các trang thông tin chung")]
        common_page_list = 17,
        [Display(Name = "Nội dung thông báo lỗi 404")]
        error_404_content = 18, 
        [Display(Name = "Hình ảnh trượt trái (slide_left)")]
        slide_left = 19,
        [Display(Name = "Hình ảnh trượt phải (slide_right)")]
        slide_right = 20,
        [Display(Name = "Số lượng câu hỏi kiểm tra trắc nghiệm theo môn (post)")]
        number_quiz_post = 21,
        [Display(Name = "Mô tả website seo google")]
        google_seo_description = 22,
        [Display(Name = "Từ khóa website seo google")]
        google_seo_keyword = 23,
        [Display(Name = "Đường dẫn tên miền website (url)")]
        website_url = 24,
        [Display(Name = "Chia sẻ bài viết lên mạng xã hội (đoạn mã inline tại vị trí hiển thị)")]
        share_social_network_inline = 25,
        [Display(Name = "Chia sẻ bài viết lên mạng xã hội (đoạn mã javascript trong body)")]
        share_social_network_javascript = 26,
        [Display(Name = "Các trang tĩnh (link ở footer)")]
        common_page_footer_list = 27,
        [Display(Name = "Link code bình luận facebook")]
        facebook_commnent_code = 28,
        [Display(Name = "Link code like/share facebook")]
        facebook_likeshare_code = 29,
        [Display(Name = "Logo footer")]
        footer_logo = 30,
        [Display(Name = "Link code twitter (share/follow/hash...)")]
        twitter_list = 31,
        [Display(Name = "Link share goolge+")]
        googleplus_list = 32,
        [Display(Name = "Thông tin giới thiệu")]
        teacher_list = 33,
        [Display(Name = "Số lượng câu hỏi kiểm tra trắc nghiệm theo bài (lesson)")]
        number_quiz_lesson = 34,
    }

    public enum PostType
    {
        [Display(Name = "Bài viết đặc biệt (slide)")]
        special = 0,
        [Display(Name = "Bài viết thông thường (article)")]
        article = 1,              
        [Display(Name = "Tin tức-Sự kiện (news)")] // Tin tức, sự kiện công nghệ: ra mắt iphone 8, xiaomi ...
        news = 2,            
        [Display(Name = "Thông báo")]
        inform = 3,
        [Display(Name = "Việc làm")]
        job = 4,        
        //[Display(Name = "Tài liệu - ebook")]
        //ebook = 5,
        //[Display(Name = "Source code - phần mềm")]
        //sourcecode = 6,
        //[Display(Name = "Bài kiểm tra trắc nghiệm (quizze)")]
        //quizze = 7, 
    }

    //public enum Level
    //{       
    //    [Display(Name = "Cấp độ 1 (đỏ)")]
    //    level1 = 1,
    //    [Display(Name = "Cấp độ 2 (cam)")]
    //    level2 = 2,
    //    [Display(Name = "Cấp độ 3 (vàng)")]
    //    level3 = 3,
    //    [Display(Name = "Cấp độ 4 (xanh)")]
    //    level4 = 4,
    //}  

    public enum Month
    {
        [Display(Name = "Tất cả")]
        m0 = 0,
        [Display(Name = "Tháng 1")]
        m1 = 1,
        [Display(Name = "Tháng 2")]
        m2 = 2,
        [Display(Name = "Tháng 3")]
        m3 = 3,
        [Display(Name = "Tháng 4")]
        m4 = 4,
        [Display(Name = "Tháng 5")]
        m5 = 5,
        [Display(Name = "Tháng 6")]
        m6 = 6,
        [Display(Name = "Tháng 7")]
        m7 = 7,
        [Display(Name = "Tháng 8")]
        m8 = 8,
        [Display(Name = "Tháng 9")]
        m9 = 9,
        [Display(Name = "Tháng 10")]
        m10 = 10,
        [Display(Name = "Tháng 11")]
        m11 = 11,
        [Display(Name = "Tháng 12")]
        m12 = 12,
    }
    public enum TaskRole
    {
        [Display(Name = "Chọn")]
        all = 0,
        [Display(Name = "Chủ trì")]
        header = 1,
        [Display(Name = "Nhóm trưởng")]
        leader = 2,
        [Display(Name = "Thành viên")]
        member = 3,
        [Display(Name = "Khác")]
        others = 4,
        [Display(Name = "Phối hợp")]
        partner = 4,
    }

    public enum TaskStatus
    {
        [Display(Name = "Tất cả")]
        all = 0,
        [Display(Name = "Mới")]
        New = 1,
        [Display(Name = "Đang thực hiện")]
        Processing = 2,
        [Display(Name = "Đã hoàn thành")]
        Close = 3,        
    }

    public enum UserTaskStatus
    {
        [Display(Name = "Tất cả")]
        all = 0,
        [Display(Name = "Mới")]
        New = 1,
        [Display(Name = "Đang thực hiện")]
        Processing = 2,
        [Display(Name = "Đã hoàn thành")]
        Close = 3,
        [Display(Name = "Chuyển xử lý")]
        Forward = 4,
        [Display(Name = "Từ chối xử lý")]
        Reject = 5,       
    }


    public class RequiredSelectListItem : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as List<SelectListItem>;
            if (list != null)
            {
                return list.Where(x => x.Selected == true).Count() > 0;
            }
            return false;
        }
    }

    public enum Role : int
    {
        [Display(Name = "Quản trị")] // tạo và quản trị người dùng, phòng ban, cấu hình nếu có
        Admin = 1,
        [Display(Name = "Lãnh đạo")]  // tạo và quản trị công việc, duyệt, đóng công việc (không có qt người dùng và phòng ban)
        Leader = 2,
        [Display(Name = "Nhân viên")]
        Chuyenvien = 3,
        [Display(Name = "Giảng viên")]// tiếp nhận công việc và phân công -> giống lãnh đạo phòng
        Teacher = 4,
    }

    public enum Gender
    {
        [Display(Name = "Nam")]
        Male = 1,
        [Display(Name = "Nữ")]
        Female = 2,
        [Display(Name = "Khác")]
        Others = 3,
    }

    //public enum Status
    //{
    //    [Display(Name = "Khả dụng")]
    //    enable = true,
    //    [Display(Name = "Không khả dụng")]
    //    Female = false       
    //}
}