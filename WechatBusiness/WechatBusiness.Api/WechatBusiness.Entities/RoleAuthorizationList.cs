using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatBusiness.Entities
{
    public partial class RoleAuthorizationList
    {
        public int ID { get; set; }
        public int BusinessID { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
    }
}
