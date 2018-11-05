using Microsoft.EntityFrameworkCore;
using System;
using WechatBusiness.Entities;

namespace WechatBusiness.DataSource
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        public virtual DbSet<AdminUsers> AdminUsers { get; set; }
    }
}