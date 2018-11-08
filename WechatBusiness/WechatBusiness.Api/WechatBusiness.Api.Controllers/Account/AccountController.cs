using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore.Framework.Cache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WechatBusiness.Api.ViewModels;
using WechatBusiness.Api.ViewModels.InputModels;
using WechatBusiness.Api.ViewModels.ResultModels;
using WechatBusiness.Entities;
using WechatBusiness.IService;
using WechatBusiness.Service;

namespace WechatBusiness.Api.Controllers.Account
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]//此处为新增
    [ApiController]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : BaseController
    {
        public readonly IAccountService _accountService;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly ICacheHelper _cacheHelper;

        public AccountController(IAccountService accountService, IHostingEnvironment environment, IMapper mapper, ICacheHelper cacheHelper) : base(0)
        {
            this._accountService = accountService;
            this._environment = environment;
            this._mapper = mapper;
            this._cacheHelper = cacheHelper;
        }

        /// <summary>
        /// 我简单的改了一个方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public string Get(int id)
        {
            AdminUsers model = _accountService.GetAdminUsersByID();
            return model.NickName;
        }

        [HttpPost]
        //[AllowAnonymous] //FromForm  -form -data x-wwww-form-urlencoded  [FromBody]:只能传递一个参数
        public async Task<IActionResult> AddUser(AdminUsersDto adminUsers)
        {
            var fileName = "";

            IFormFile formFile = adminUsers.HeadPortrait;

            if (formFile != null)
            {
                fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                string filePath = _environment.WebRootPath + $@"\Files\Pictures\";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string suffix = fileName.Split('.')[1];
                fileName = Guid.NewGuid() + "." + suffix;
                string fileFullName = filePath + fileName;
                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    await formFile.CopyToAsync(fs);
                    fs.Flush();
                }
            }

            string imgpath = $"/src/Pictures/{fileName}";
            var adminmodel = _mapper.Map<AdminUsers>(adminUsers);
            adminmodel.HeadPortraitUrl = imgpath;

            var optresult = await _accountService.AddAsync(adminmodel);

            _cacheHelper.SetCache(adminmodel.Id.ToString(), adminmodel, DateTimeOffset.Now.AddHours(1));

            //var product = new ProductInfoDto()
            //{
            //    ID = 1,
            //    //BusinessID = 1,
            //    ClassID = 1,
            //    ProductID = 1
            //};
            //var model = _mapper.Map<ProductInfoToClass>(product);
            var result = ApiResultBase.GetInstance(ResultCode.Access, result: adminmodel);
            return Ok(result);

            //_accountService.AddAsync()
        }

        [HttpPost("{Id}")]
        [AllowAnonymous]
        public IActionResult Post(long Id, IFormFile fi)
        {
            var model = new TechChangeSeachModel();
            var list = new List<ChilItems>();
            list.Add(new ChilItems() { Id = Num, Title = "A", IsSelect = false });
            list.Add(new ChilItems() { Id = 2, Title = "B", IsSelect = false });
            list.Add(new ChilItems() { Id = 3, Title = "c", IsSelect = false });
            list.Add(new ChilItems() { Id = 4, Title = "D", IsSelect = false });
            list.Add(new ChilItems() { Id = 5, Title = "F", IsSelect = false });

            model.TechLevelList = list;
            model.TechLevelList.Where(p => p.Id == 3).ToList().ForEach(p => p.IsSelect = true);
            model.TechLevelList.OrderBy(p => p.Id).FirstOrDefault().IsSelect = true;
            //if (Id > 0)
            //{
            //    var obj = _cacheHelper.GetCache<AdminUsers>(Id.ToString());
            //    var result1 = ApiResultBase.GetInstance(ResultCode.Access, result: obj);
            //    return Ok(result1);
            //}
            var result = ApiResultBase.GetInstance(ResultCode.Access, result: model);
            return Ok(result);
        }
    }

    public class TechChangeSeachModel
    {
        public List<ChilItems> TechSelectTypeList { get; set; }

        public List<ChilItems> TechLevelList { get; set; }

        public List<ChilItems> TechSexList { get; set; }
    }

    public class ChilItems
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsSelect { get; set; }
    }
}