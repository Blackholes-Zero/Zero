using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    public partial class OperationRole
    {
        public int ID { get; set; }
        public int RootID { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(100)]
        public string Content { get; set; }
        public int State { get; set; }
        public int BusinessID { get; set; }
        public int SortID { get; set; }
    }
}
