using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    public partial class RoleAuthorizedAdmin
    {
        public int ID { get; set; }
        public int BusinessID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int AdminID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddTime { get; set; }
    }
}
