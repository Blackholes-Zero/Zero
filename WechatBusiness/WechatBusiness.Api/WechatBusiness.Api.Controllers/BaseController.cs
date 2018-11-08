using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WechatBusiness.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    //启用跨域
    [EnableCors("CoresDomain")]
    public class BaseController : ControllerBase
    {
        public int Num { get; set; }

        public BaseController(int _num)
        {
            this.Num = _num;
        }
    }
}