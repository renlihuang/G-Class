using CSViewAdminService.Core;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CSViewAdminService.Utility;

namespace CSViewAdminService.Repository
{
    internal abstract partial class AbstractRepository<TEntity, TCondition, TPrimaryKeyType> where TEntity : IEntity<TPrimaryKeyType> where TCondition : class, new()
    {
        /// <summary>
        /// 根据主键更新单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync<T>(T entity) where T : IEntity<TPrimaryKeyType>
        {
            return await Db.UpdateAsync(entity) > 0;
        }

        public virtual async Task<bool> UpdateAsync<T>(T entity, Expression<Func<T, object>> fields) where T : IEntity<TPrimaryKeyType>
        {
            return await Db.UpdateAsync(entity, fields) > 0;
        }

        /// <summary>
        /// 更新实体的部分字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync<T>(T entity, IList<string> columns) where T : IEntity<TPrimaryKeyType>
        {
            return await Db.UpdateAsync(entity, columns) > 0;
        }



        /// <summary>
        /// /批量更新实体数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateBatchAsync<T>(IList<T> entities) where T : IEntity<TPrimaryKeyType>
        {

            if (entities != null && entities.Any())
            {
                List<UpdateBatch<T>> pocos = new List<UpdateBatch<T>>();
                foreach (var poco in entities)
                {
                    UpdateBatch<T> updateBatch = new UpdateBatch<T> { Poco = poco };
                    pocos.Add(updateBatch);
                }
                using (var scope = BeginDBTransaction())
                {
                    var result = await Db.UpdateBatchAsync(pocos);
                    scope.Complete();
                    return result > 0;
                }
            }
            return false;

        }

        /// <summary>
        /// 根据id集合更新某个字段更新
        /// </summary>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <typeparam name="TColunmValue"></typeparam>
        /// <param name="idList"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateSingleFieldByIdsAsync<TColunmValue>(IList<TPrimaryKeyType> idList, KeyValuePair<string, TColunmValue> column)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append($"Update {PocoData.TableInfo.TableName} SET {column.Key }=@0 WHERE Id IN ({ string.Join(",", idList)})");
            return await Db.ExecuteAsync(sql.ToString(), column.Value) > 0;
        }

        /// <summary>
        /// 保存实体，有则更新，无则新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> SaveAsync<T>(T entity) where T : IEntity<TPrimaryKeyType>
        {
            await Db.SaveAsync(entity);
            return true;
        }

        /// <summary>
        /// 有则更新无则插入（批量）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual async Task<bool> SaveBatchAsync<T>(IList<T> entities) where T : IEntity<TPrimaryKeyType>
        {
            var detailIds = entities.Select(t => t.Id).ToList();
            var oldDetailList = await GetListByIdsAsync<MyId<TPrimaryKeyType>, TPrimaryKeyType>(detailIds).ConfigureAwait(false);
            var oldDetailIds = oldDetailList.Select(t => t.Id).ToList();
            var insertIds = detailIds.Except(oldDetailIds);//差集
            var updateIds = detailIds.Intersect(oldDetailIds);//交集
            var insertEntityList = entities.Where(t => insertIds.Contains(t.Id)).ToList();
            var updateEntityList = entities.Where(t => updateIds.Contains(t.Id)).ToList();

            using (var scope = BeginDBTransaction())
            {
                bool insertResult = false;
                bool updateResult = false;
                bool deleteResult = false;
                if (insertEntityList != null && insertEntityList.Count > 0)
                {
                    insertResult = await InsertBatchAsync(insertEntityList);
                }
                if (updateEntityList != null && updateEntityList.Count > 0)
                {
                    updateResult = await UpdateBatchAsync(updateEntityList);
                }
                scope.Complete();
                return insertResult && updateResult && deleteResult;
            }
        }


        /// <summary>
        ///  有则更新（增加），无则删除
        /// 1.entities中有， oldEntities没有的数据插入
        /// 2.oldEntities 和entities中有 都有的数据更新
        /// 3.oldEntities中有，entities中没有的数据删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">新数据</param>
        /// <param name="oldEntities">旧数据实体</param>
        /// <returns></returns>
        public virtual async Task<bool> UpsertDeleteAsync<T>(IList<T> entities, IList<TPrimaryKeyType> oldIdList) where T : IEntity<TPrimaryKeyType>
        {
            var detailIds = entities.Select(t => t.Id).ToList();

            //如果oldIdList，直接已entities的id进行判断
            if (oldIdList.IsListNullOrEmpty())
            {
                var oldIdListObjects = await GetListByIdsAsync<MyId<TPrimaryKeyType>, TPrimaryKeyType>(detailIds).ConfigureAwait(false);
                oldIdList = oldIdListObjects.Select(t => t.Id).ToList();
            }
            var insertIds = detailIds.Except(oldIdList);//差集
            var deleteIds = oldIdList.Except(detailIds).ToList();//差集
            var updateIds = detailIds.Intersect(oldIdList);//交集
            var insertEntityList = entities.Where(t => insertIds.Contains(t.Id)).ToList();
            var updateEntityList = entities.Where(t => updateIds.Contains(t.Id)).ToList();

            using (var scope = BeginDBTransaction())
            {
                bool insertResult = false;
                bool updateResult = false;
                bool deleteResult = false;
                if (insertEntityList != null && insertEntityList.Count > 0)
                {
                    insertResult = await InsertBatchAsync(insertEntityList);
                }
                if (updateEntityList != null && updateEntityList.Count > 0)
                {
                    updateResult = await UpdateBatchAsync(updateEntityList);
                }
                if (deleteIds != null && deleteIds.Count > 0)
                {
                    deleteResult = await SoftDeleteBatchAsync(deleteIds);
                }
                scope.Complete();
                return insertResult && updateResult && deleteResult;
            }
        }
    }
}
