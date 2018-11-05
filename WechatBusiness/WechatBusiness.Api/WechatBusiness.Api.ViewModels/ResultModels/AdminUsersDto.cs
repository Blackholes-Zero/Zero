using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WechatBusiness.Api.ViewModels.ResultModels
{
    public class AdminUsersDto
    {
        [Required]
        [StringLength(50)]
        public virtual string LoginName { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string NickName { get; set; }

        [Required]
        [StringLength(11)]
        [MinLength(11)]
        public virtual string Mobile { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string WeChat { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string QQ { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public virtual string Email { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string PassWord { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Content { get; set; }

        [FileExtensions(Extensions = ".jpg,.png", ErrorMessage = "图片格式错误")]
        public virtual IFormFile HeadPortrait { get; set; }
    }
}