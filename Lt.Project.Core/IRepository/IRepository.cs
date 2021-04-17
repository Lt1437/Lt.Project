using Lt.Project.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lt.Project.Domain.IRepository
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 添加实体(单个)
        /// </summary>
        /// <param name="entity">实体对象</param>
        int Add(T entity);

        /// <summary>
        /// 批量插入实体(多个)
        /// </summary>
        /// <param name="list">实体列表</param>
        int AddRange(List<T> list);

        /// <summary>
        /// 删除实体(单个)
        /// </summary>
        /// <param name="entity"></param>
        int Remove(T entity);

        /// <summary>
        /// 批量删除实体(多个)
        /// </summary>
        /// <param name="list">实体列表</param>
        int RemoveRange(List<T> list);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// 分页条件查询
        /// </summary>
        /// <typeparam name="TKey">排序类型</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="predicate">条件表达式</param>
        /// <param name="isAsc">是否升序排列</param>
        /// <param name="keySelector">排序表达式</param>
        /// <returns></returns>
        PageHelpers<T> SearchFor<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate, bool isAsc, Expression<Func<T, TKey>> keySelector);

        /// <summary>
        /// 获取实体（主键）
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        T GetModelById(object id);

        /// <summary>
        /// 获取实体（条件）
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        T GetModel(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>记录数</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        bool Exist(Expression<Func<T, bool>> anyLambda);
    }
}