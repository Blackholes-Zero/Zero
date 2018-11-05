using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    [Table("AdminUsers")]
    public partial class AdminUsers : Entity
    {
        [Key]
        [Column("ID")]
        public override long Id { get; set; }

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

        [Range(0, 9)]
        public virtual int State { get; set; }

        public virtual long ReferrerID { get; set; }
        public virtual long BusinessID { get; set; }

        [Required]
        [StringLength(200)]
        public string HeadPortraitUrl { get; set; }

        public bool IsDisabled { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime AddTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ValidityTime { get; set; }
    }
}