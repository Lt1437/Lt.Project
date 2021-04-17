using Lt.Project.Common.Helpers;
using Lt.Project.Domain.IRepository;
using Lt.Project.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lt.Project.EntityFrameworkCore.Repository
{
    /// <summary>
    /// 仓储类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CoreDbContext _dbContext;
        private DbSet<T> _entity;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<T> Entity => _entity ?? (_entity = _dbContext.Set<T>());

        /// <summary>
        /// 添加实体(单个)
        /// </summary>
        /// <param name="entity">实体对象</param>
        public int Add(T entity)
        {
            Entity.Add(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量插入实体(多个)
        /// </summary>
        /// <param name="list">实体列表</param>
        public int AddRange(List<T> list)
        {
            Entity.AddRange(list);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 删除实体(单个)
        /// </summary>
        /// <param name="entity"></param>
        public int Remove(T entity)
        {
            Entity.Remove(entity);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 批量删除实体(多个)
        /// </summary>
        /// <param name="list">实体列表</param>
        public int RemoveRange(List<T> list)
        {
            Entity.RemoveRange(list);
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return Entity.AsQueryable().AsNoTracking();
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="isAsc">是否升序排列</param>
        /// <param name="predicate">条件表达式</param>
        /// <param name="keySelector">排序表达式</param>
        /// <returns></returns>
        public PageHelpers<T> SearchFor<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate, bool isAsc,
            Expression<Func<T, TKey>> keySelector)
        {
            if (pageIndex <= 0 || pageSize <= 0) throw new Exception("pageIndex或pageSize不能小于等于0");
            var page = new PageHelpers<T> { PageIndex = pageIndex, PageSize = pageSize };
            var skip = (pageIndex - 1) * pageSize;
            var able = Entity.AsQueryable().AsNoTracking();
            if (predicate == null)
            {
                var count = Entity.Count();
                var query = isAsc
                    ? able.OrderBy(keySelector).Skip(skip).Take(pageSize)
                    : able.OrderByDescending(keySelector).Skip(skip).Take(pageSize);
                page.TotalRows = count;
                page.LsList = query.ToList();
                page.TotalPages = page.TotalRows / pageSize;
                if (page.TotalRows % pageSize != 0) page.TotalPages++;
            }
            else
            {
                var queryable = able.Where(predicate);
                var count = queryable.Count();
                var query = isAsc
                    ? queryable.OrderBy(keySelector).Skip(skip).Take(pageSize)
                    : queryable.OrderByDescending(keySelector).Skip(skip).Take(pageSize);
                page.TotalRows = count;
                page.LsList = query.ToList();
                page.TotalPages = page.TotalRows / pageSize;
                if (page.TotalRows % pageSize != 0) page.TotalPages++;
            }
            return page;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public T GetModelById(object id)
        {
            return Entity.Find(id);
        }

        /// <summary>
        /// 获取实体（条件）
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> predicate)
        {
            return Entity.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? Entity.Where(predicate).Count() : Entity.Count();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return Entity.Any(anyLambda);
        }
    }
}