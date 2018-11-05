using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WechatBusiness.DataSource;
using WechatBusiness.Entities;
using WechatBusiness.IRepository;

namespace WechatBusiness.Repository.RepositoryBase
{
    public class EfRepositoryBase<T> : IRepository<T> where T : Entity
    {
        private EfDbContext dataContext;
        private readonly DbSet<T> dbSet;

        protected IDatabaseFactory DbFactory
        {
            private set;
            get;
        }

        public EfDbContext DataContext
        {
            get { return dataContext ?? (dataContext = DbFactory.GetEfDbContext()); }
        }

        public EfRepositoryBase(IDatabaseFactory databaseFactory)
        {
            DbFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }

        public virtual async void Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        //新增方法
        public virtual void AddAll(IEnumerable<T> entities)
        {
            dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        //新增方法
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T obj in entities)
            {
                dbSet.Attach(obj);
                dataContext.Entry(obj).State = EntityState.Modified;
            }
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            dbSet.RemoveRange(objects);
        }

        //新增方法
        public virtual void DeleteAll(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual T GetById(long id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        public virtual IEnumerable<T> GetAllLazy()
        {
            return dbSet;
        }

        public virtual async void Addasync(T entity)
        {
            await dbSet.AddAsync(entity);
        }
    }
}