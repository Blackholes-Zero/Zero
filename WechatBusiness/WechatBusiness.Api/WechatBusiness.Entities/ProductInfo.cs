using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    public partial class ProductInfo
    {
        public int ID { get; set; }
        public int BusinessID { get; set; }
        public int AdminID { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductContent { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductPictureUrl { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal ProductNums { get; set; }
        public decimal Price { get; set; }
        public int State { get; set; }
        public bool IsDisabled { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddTime { get; set; }
    }
}
