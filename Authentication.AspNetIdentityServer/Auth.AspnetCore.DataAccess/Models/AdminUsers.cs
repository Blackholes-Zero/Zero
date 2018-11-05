using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.AspnetCore.DataAccess.Models
{
    [Table("AdminUsers")]
    public class AdminUsers
    {
        [Key]
        public long Id { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string LoginName { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string NickName { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string Mobile { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string WeChat { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string QQ { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string Email { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string PassWord { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string Content { get; set; }

        /// <summary>
        ///
        /// <summary>
        public int State { get; set; }

        /// <summary>
        ///
        /// <summary>
        public int ReferrerID { get; set; }

        /// <summary>
        ///
        /// <summary>
        public int BusinessID { get; set; }

        /// <summary>
        ///
        /// <summary>
        public string HeadPortraitUrl { get; set; }

        /// <summary>
        ///
        /// <summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        ///
        /// <summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        ///
        /// <summary>
        public DateTime ValidityTime { get; set; }
    }
}