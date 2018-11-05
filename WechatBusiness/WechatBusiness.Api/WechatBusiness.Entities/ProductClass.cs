using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    public partial class ProductClass
    {
        public int ID { get; set; }
        public int RootID { get; set; }
        [Required]
        [StringLength(50)]
        public string ClassName { get; set; }
        public int State { get; set; }
        public int SortID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddTime { get; set; }
    }
}
