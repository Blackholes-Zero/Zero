using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    public partial class BuyersInfo
    {
        public int ID { get; set; }
        public int AdminID { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(11)]
        public string Mobile { get; set; }
        [Required]
        [StringLength(50)]
        public string WeChat { get; set; }
        [Required]
        [StringLength(100)]
        public string Content { get; set; }
        public int State { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddTime { get; set; }
    }
}
