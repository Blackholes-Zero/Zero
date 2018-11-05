using System;
using System.Collections.Generic;
using System.Text;
using WechatBusiness.Entities;
using WechatBusiness.IRepository;
using WechatBusiness.Repository.RepositoryBase;

namespace WechatBusiness.Repository
{
    public class AdminUsersRepository : EfRepositoryBase<AdminUsers>, IAdminUsersRepository
    {
        public AdminUsersRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}