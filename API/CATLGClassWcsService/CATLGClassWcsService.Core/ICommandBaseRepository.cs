﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CATLGClassWcsService.Core;

namespace CATLGClassWcsService.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPrimaryKeyType"></typeparam>
    public interface ICommandBaseRepository<TPrimaryKeyType>
    {
        #region 新增
        /// <summary>
        /// 插入实体
        /// </summary>
        Task<object> InsertAsync<T>(T entity) where T : IEntity<TPrimaryKeyType>;




        /// <summary>
        /// 批量插入实体
        /// </summary>
        Task<bool> InsertBatchAsync<T>(IList<T> entities) where T : IEntity<TPrimaryKeyType>;
        #endregion


        #region 更新
        /// <summary>
        /// 更新单个实体
        /// </summary>
        Task<bool> UpdateAsync<T>(T entity) where T : IEntity<TPrimaryKeyType>;

        /// <summary>
        /// 根据实体更新部分字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T entity, Expression<Func<T, object>> fields) where T : IEntity<TPrimaryKeyType>;

        /// <summary>
        /// 更新实体的部分字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync<T>(T entity, IList<string> columns) where T : IEntity<TPrimaryKeyType>;


        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> UpdateBatchAsync<T>(IList<T> entities) where T : IEntity<TPrimaryKeyType>;


        /// <summary>
        /// 根据id集合更新多个记录的某个字段
        /// </summary>
        /// <typeparam name="TColunmValue">字段值类型 long，int等</typeparam>
        /// <param name="idList">id集合</param>
        /// <param name="column">字段数据，key字段名，value字段值</param>
        /// <returns></returns>
        Task<bool> UpdateSingleFieldByIdsAsync<TColunmValue>(IList<TPrimaryKeyType> idList, KeyValuePair<string, TColunmValue> column);


        /// <summary>
        /// 保存实体，有则更新，无则新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> SaveAsync<T>(T entity) where T : IEntity<TPrimaryKeyType>;

        /// <summary>
        /// 有则更新无则删除,不在entities的删除（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
       Task<bool> SaveBatchAsync<T>(IList<T> entities) where T : IEntity<TPrimaryKeyType>;

        /// <summary>
        ///  有则更新（增加），无则删除
        /// 1.entities中有， oldIdList没有的数据插入
        /// 2.oldIdList 和entities中有 都有的数据更新
        /// 3.oldIdList中有，entities中没有的数据删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">新数据</param>
        /// <param name="oldIdList">旧数据实体id</param>
        /// <returns></returns>
        Task<bool> UpsertDeleteAsync<T>(IList<T> entities, IList<TPrimaryKeyType> oldIdList) where T : IEntity<TPrimaryKeyType>;

        #endregion


        #region 删除

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(TPrimaryKeyType id);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> SoftDeleteBatchAsync(IList<TPrimaryKeyType> id);

        /// <summary>
        /// 删除记录
        /// </summary>
        // Task<bool> DeleteAsync<TPrimaryKeyType>(TPrimaryKeyType id);

        /// <summary>
        /// 批量删除记录
        /// </summary>
        //  Task<bool> DeleteBatchAsync<TPrimaryKeyType>(IList<TPrimaryKeyType> idList);

        #endregion




        #region 事务模块
        /// <summary>
        /// 开始事务（返回事务对象）
        /// </summary>
        /// <returns></returns>
        IDBTransaction BeginDBTransaction();



        /// <summary>
        /// 开启事务（不返回事务对象）
        /// </summary>
        /// <returns></returns>
        void BeginNewDBTransaction();


        /// <summary>
        /// 提交事务事务
        /// </summary>
        void CompleteDBTransaction();

        /// <summary>
        /// 中断结束事务
        /// </summary>
        void AbortDBTransaction();

        #endregion
    }
}
