
using CATLGClassWcsService.Core;
using NPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CATLGClassWcsService.Repository
{
    internal abstract partial class AbstractRepository<TEntity, TCondition, TPrimaryKeyType> where TEntity : IEntity<TPrimaryKeyType> where TCondition : class, new()
    {

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public virtual async Task<bool> SoftDeleteAsync(TPrimaryKeyType id)
        {

            CheckMarksField();
            if (id == null) return false;
            var sql = $"Update {PocoData.TableInfo.TableName} SET IsDeleted=1 WHERE Id=@0";
            var result = await Db.ExecuteAsync(sql, id);
            return result > 0;

          
        }
 


        /// <summary>
        /// 根据id批量删除(物理删除)
        /// </summary>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteBatchAsync(IList<TPrimaryKeyType> idList)
        {
            if (idList == null || idList.Count == 0) return false;
            int revt;
            
            var sql = $"DELETE FROM {PocoData.TableInfo.TableName} WHERE {PocoData.TableInfo.PrimaryKey} IN ({string.Join(",", idList)})";
            revt = await Db.ExecuteAsync(sql);
            return revt > 0;
        }


        /// <summary>
        /// 物理删除
        /// </summary>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(TPrimaryKeyType id)
        {
            if (id == null) return false;
            TEntity entity = default(TEntity);
            var result = await Db.DeleteAsync(PocoData.TableInfo.TableName, PocoData.TableInfo.PrimaryKey, entity, id);
            return result > 0;
        }


       

        /// <summary>
        /// 逻辑删除 批量
        /// </summary>
        /// <param name="idList">主键id集合</param>
        /// <returns></returns>
        public virtual async Task<bool> SoftDeleteBatchAsync(IList<TPrimaryKeyType> idList)
        {
          
          
            if (idList == null || idList.Count == 0) return false;
            CheckMarksField();
            var sql = $"Update {PocoData.TableInfo.TableName} SET IsDeleted=1 WHERE Id IN ({string.Join(",", idList)})";
            var result = await Db.ExecuteAsync(sql, $"({string.Join(",", idList)})");
            return result > 0;
        }
    }
}
