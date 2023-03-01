using CATLGClassWcsService.Core;
using NPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CATLGClassWcsService.Repository
{
    internal abstract partial class AbstractRepository<TEntity, TCondition, TPrimaryKeyType> where TEntity : IEntity<TPrimaryKeyType> where TCondition : class, new()
    {
        public abstract Sql TrunConditionToSql(Sql sql, TCondition condition);
        /// <summary>
        /// 根据id获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T, TKey>(TKey id)
        {

            var sql = new Sql();
            sql.From(PocoData.TableInfo.TableName).Where(" IsDeleted = 0  AND Id = @0", id);
            return await Db.SingleOrDefaultAsync<T>(sql).ConfigureAwait(false);
           
        }

        /// <summary>
        /// 根据某个唯一字段列获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TColunmValue"></typeparam>
        /// <param name="column"></param>
        /// <returns></returns>
        public virtual async Task<T> GetSingleAsync<T, TColunmValue>(KeyValuePair<string, TColunmValue> column)
        {
            var sql = new Sql();
            sql.From(PocoData.TableInfo.TableName).Where($" IsDeleted = 0  AND {column.Key} = @0", column.Value);
            return await Db.SingleOrDefaultAsync<T>(sql).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据主键id判断数据是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync<TKey>(TKey id)
        {
            var sqlCondition = $"{PocoData.TableInfo.PrimaryKey} = @0";
            if (PocoData.Columns.ContainsKey("IsDeleted"))
            {
                sqlCondition += " AND IsDeleted = 0";
            }
            var result = await Db.ExecuteScalarAsync<int>(sqlCondition, id).ConfigureAwait(false);
            return result != 0;
        }

         





        /// <summary>
        ///  某个字段是否唯一
        /// </summary>
        /// <typeparam name="TColunmValue"></typeparam>
        /// <param name="column"></param>
        /// <returns>true  唯一  false 不唯一</returns>
        public virtual async Task<bool> IsUniqueAsync<TColunmValue>(KeyValuePair<string, TColunmValue> column)
        {
            var sqlCondition = $"Select count(0) From {PocoData.TableInfo.TableName } Where {column.Key} = @0";
            if (PocoData.Columns.ContainsKey("IsDeleted"))
            {
                sqlCondition += " AND IsDeleted = 0";
            }
            var result = await Db.ExecuteScalarAsync<int>(sqlCondition, column.Value).ConfigureAwait(false);
            return result == 0;
        }

        /// <summary>
        /// 根据id集合获取多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListByIdsAsync<T,TKey>(List<TKey> ids)
        {
            var sql = new Sql();
            sql.From(PocoData.TableInfo.TableName).Where($" IsDeleted = 0  AND Id In({string.Join(", ", ids)})");
            return await Db.FetchAsync<T>(sql).ConfigureAwait(false);
        }

        public virtual async Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, TCondition condition, string field = null, string orderBy = null)
        {
            bool isAllField = IsGetAllField(field);
            var sql = new Sql();
            if (!isAllField)
            {
                sql.Select(field);
            }
            sql.From(PocoData.TableInfo.TableName).Where(" IsDeleted = 0 ");
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "CreateTime ASC";
            }
            sql = TrunConditionToSql(sql, condition);
            sql.OrderBy(orderBy);
            var data = await Db.PageAsync<T>(page, size, sql).ConfigureAwait(false);
            var total = (int)data.TotalItems;
            return new QueryPagedResponseModel<T> { Data = data.Items, Total = total };
        }


        public virtual async Task<List<T>> GetListAsync<T>(TCondition condition, string field = null, string orderBy = null)
        {
            bool isAllField = IsGetAllField(field);
            var sql = new Sql();
            if (!isAllField)
            {
                sql.Select(field);
            }
            sql.From(PocoData.TableInfo.TableName).Where(" IsDeleted = 0 ");
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "CreateTime ASC";
            }
            sql = TrunConditionToSql(sql, condition);
            sql.OrderBy(orderBy);
            var data = await Db.FetchAsync<T>(sql).ConfigureAwait(false);
            return data;
        }

    }
}
