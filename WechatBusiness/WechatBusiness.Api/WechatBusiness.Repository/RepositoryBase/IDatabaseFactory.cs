using System;
using System.Collections.Generic;
using System.Text;
using WechatBusiness.DataSource;

namespace WechatBusiness.Repository.RepositoryBase
{
    public interface IDatabaseFactory : IDisposable
    {
        EfDbContext GetEfDbContext();
    }
}