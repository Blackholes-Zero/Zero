using System;
using System.Collections.Generic;
using System.Text;

namespace WechatBusiness.Api.Commons.AppSetting
{
    public class RedisSettings
    {
        public bool IsEnabled { get; set; }

        public string Connection { get; set; }

        public string InstanceName { get; set; }

        public int? Database { get; set; }
    }
}