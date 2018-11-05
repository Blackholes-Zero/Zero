using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WechatBusiness.Api.Commons;
using WechatBusiness.Entities;
using WechatBusiness.IRepository;
using WechatBusiness.IService;
using WechatBusiness.Repository.UnitOfWork;

namespace WechatBusiness.Service
{
    [Intercept(typeof(AopInterceptor))]
    public class AccountService : BaseService, IAccountService
    {
        private readonly IAdminUsersRepository _adminUsersRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAdminUsersRepository adminUsersRepository, IUnitOfWork unitOfWork)
        {
            this._adminUsersRepository = adminUsersRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(AdminUsers admin)
        {
            _adminUsersRepository.Addasync(admin);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public AdminUsers GetAdminUsersByID()
        {
            return new AdminUsers() { NickName = "admin" };
            //return _adminUsersRepository.Get(p => p.ID == 2);
        }
    }
}