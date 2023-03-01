using CATLGClassWcsService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CATLGClassWcsService.Repository
{
    internal class DefaultUnitOfWorkImpl<TPrimaryKeyType, TEntity, TCommandRepository> : IDefaultUnitOfWork<TPrimaryKeyType, TEntity, TCommandRepository> where TEntity : IEntity<TPrimaryKeyType> where TCommandRepository : ICommandBaseRepository<TPrimaryKeyType>
    {

        private readonly IUnitOfWork<TPrimaryKeyType> unitOfWork;
        private readonly TCommandRepository commandRepository;

        public DefaultUnitOfWorkImpl(TCommandRepository commandRepository, IUnitOfWork<TPrimaryKeyType> unitOfWork)
        {
            this.commandRepository = commandRepository;

            this.unitOfWork = unitOfWork;
        }

        public void RegisterInsert(TEntity entity)
        {
            unitOfWork.RegisterInsert(entity, commandRepository);
        }



        public void RegisterInsertBatch(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                unitOfWork.RegisterInsert(entity, commandRepository);
            }

        }

        public void RegisterUpdate(TEntity entity)
        {
            unitOfWork.RegisterUpdate(entity, commandRepository);

        }


        /// <summary>
        ///  保存， 有则更新 无则新增 
        /// </summary>
        /// <param name="entity"></param>
        /// 
        [Obsolete("V2版本已弃用")]
        public void RegisterSave(TEntity entity)
        {
            unitOfWork.RegisterSave(entity, commandRepository);
        }


        /// <summary>
        /// 保存， 有则更新 无则新增 entities不存在的删除
        /// </summary>
        /// <param name="entity"></param>
        /// 
        [Obsolete("V2版本已弃用")]
        public void RegisterSaveBatch(IList<TEntity> entities)
        {
            var list = new List<IEntity<TPrimaryKeyType>>();
            foreach (var entity in entities)
            {
                list.Add(entity);
            }
            //请勿修改成unitOfWork.RegisterSave循环，会导致循环更新而不是批量，效率低，
            unitOfWork.RegisterSaveBatch(list, commandRepository);
        }

        /// <summary>
        ///  有则更新（增加），无则删除
        /// 1.entities中有， oldIdList没有的数据插入
        /// 2.oldIdList 和entities中有 都有的数据更新
        /// 3.oldIdList中有，entities中没有的数据删除
        /// </summary>
        /// <param name="newEntityList"></param>
        /// <param name="oldIdList"></param>
        /// <param name="unitofWorkRepository"></param>
        public void RegisterUpsertDelete(List<TEntity> newEntityList, List<TPrimaryKeyType> oldIdList)
        {
            List<IEntity<TPrimaryKeyType>> newEntityList1 = new List<IEntity<TPrimaryKeyType>>();
            foreach (var entity in newEntityList)
            {
                IEntity<TPrimaryKeyType> newEntity = entity;
                newEntityList1.Add(newEntity);
            }
            unitOfWork.RegisterUpsertDelete(newEntityList1, oldIdList, commandRepository);
        }

        [Obsolete("V2版本已弃用")]
        public void RegisterUpdateBatch(IList<TEntity> entities)
        {
            var list = new List<IEntity<TPrimaryKeyType>>();
            foreach (var entity in entities)
            {
                list.Add(entity);
            }
            unitOfWork.RegisterUpdateBatch(list, commandRepository);
        }

        [Obsolete("已弃用")]
        public void RegisterUpdateByFields(TEntity entity, List<string> fields)
        {
            unitOfWork.RegisterUpdateById(entity, fields, commandRepository);
        }

        [Obsolete("V2版本已弃用")]
        public void RegisterUpdateById(TEntity entity, List<string> fields)
        {
            unitOfWork.RegisterUpdateById(entity, fields, commandRepository);
        }


        [Obsolete("V2版本已弃用")]
        public void RegisterUpdateSingleFieldByIds(IList<TPrimaryKeyType> idList, KeyValuePair<string, object> column)
        {
            unitOfWork.RegisterUpdateSingleFieldByIds(idList.ToList(), column, commandRepository);
        }


        public void RegisterDelete(TPrimaryKeyType id)
        {
            unitOfWork.RegisterDelete(id, commandRepository);
        }




        public void RegisterDeleteBatch(IList<TPrimaryKeyType> idList)
        {
            foreach (var id in idList)
            {
                unitOfWork.RegisterDelete(id, commandRepository);
            }

        }
        public async Task CommitAsync()
        {
            await unitOfWork.CommitAsync().ConfigureAwait(false);
        }
        public IDBTransaction BeginTransaction()
        {
            return unitOfWork.BeginTransaction();
        }

    }
}
