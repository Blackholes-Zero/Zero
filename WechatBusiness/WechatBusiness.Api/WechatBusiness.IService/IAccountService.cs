using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WechatBusiness.Entities;
using WechatBusiness.IService;

namespace WechatBusiness.IService
{
    public interface IAccountService : IBaseService
    {
        AdminUsers GetAdminUsersByID();

        Task<bool> AddAsync(AdminUsers admin);
    }
}