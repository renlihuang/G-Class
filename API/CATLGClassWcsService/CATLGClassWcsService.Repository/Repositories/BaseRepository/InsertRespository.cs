using CATLGClassWcsService.Core;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATLGClassWcsService.Repository
{
    internal abstract partial class AbstractRepository<TEntity, TCondition, TPrimaryKeyType> where TEntity : IEntity<TPrimaryKeyType> where TCondition : class, new()

    {


        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<object> InsertAsync<T>(T entity) where T : IEntity<TPrimaryKeyType>
        {

            var result = await Db.InsertAsync(entity);
            return result;
        }





        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertBatchAsync<T>(IList<T> entities) where T : IEntity<TPrimaryKeyType>
        {

            if (entities != null && entities.Any())
            {
                using (var scope = BeginDBTransaction())
                {
                    var result = await Db.InsertBatchAsync(entities);
                    scope.Complete();
                    return result > 0;
                }
            }
            return false;
        }


        /// <summary>
        /// 大批量批量插入实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertBulkAsync<T>(IList<T> entities) where T : IEntity<TPrimaryKeyType>
        {

            if (entities != null && entities.Any())
            {
                await Db.InsertBulkAsync(entities);
                return true;
            }
            return false;
        }

    }
}
