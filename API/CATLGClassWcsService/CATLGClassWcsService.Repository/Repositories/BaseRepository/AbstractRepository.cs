using CATLGClassWcsService.Core;
using NPoco;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using CATLGClassWcsService.Repository.Repositories;

namespace CATLGClassWcsService.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TCondition"></typeparam>
    internal abstract partial class AbstractRepository<TEntity, TCondition, TPrimaryKeyType> where TEntity : IEntity<TPrimaryKeyType> where TCondition : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        //  protected readonly Database Db;

        protected readonly CustomDatabase Db;
        /// <summary>
        /// 
        /// </summary>
        protected static PocoData PocoData;

        IDBTransaction scopeTransaction;
        /// <summary>
        /// 
        /// </summary>
        public AbstractRepository(IScopeDBFactory scopeDBFactory)
        {
            Db = scopeDBFactory.GetScopeDb();
            PocoData = Db.PocoDataFactory.ForType(typeof(TEntity));// new PocoDataFactory(new MapperCollection()).ForType(typeof(T));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        protected bool IsGetAllField(string field)
        {
            if (string.IsNullOrEmpty(field) || field.Trim() == "*")
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void CheckMarksField()
        {
            var valid = false;
            foreach (var i in PocoData.Columns)
            {
                if (i.Value.ColumnName.ToLower() != "isdeleted") continue;

                valid = true;
                break;
            }
            if (!valid)
            {
                throw new ArgumentException($"表{PocoData.TableInfo.TableName}IsDeleted");
            }
        }

        #region 事务模块

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IDBTransaction BeginDBTransaction()
        {
            return new DBTransactionImpl(Db);
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public void BeginNewDBTransaction()
        {
            scopeTransaction = new DBTransactionImpl(Db);
        }

        /// <summary>
        /// 完成事务
        /// </summary>
        public void CompleteDBTransaction()
        {
            scopeTransaction.Complete();
        }

        /// <summary>
        /// 中断事务
        /// </summary>
        public void AbortDBTransaction()
        {
            scopeTransaction.Dispose();
        }

        #endregion
    }

}
