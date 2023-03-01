using CATLGClassWcsService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CATLGClassWcsService.Repository
{
    internal class DefaultUnitOfWorkV2Impl<TPrimaryKeyType, TEntity, TCommandRepository> : IDefaultUnitOfWorkV2<TPrimaryKeyType, TEntity, TCommandRepository> where TEntity : IEntity<TPrimaryKeyType> where TCommandRepository : ICommandBaseRepository<TPrimaryKeyType>
    {

        private readonly IUnitOfWork<TPrimaryKeyType> unitOfWork;
        private readonly TCommandRepository commandRepository;

        public DefaultUnitOfWorkV2Impl(TCommandRepository commandRepository, IUnitOfWork<TPrimaryKeyType> unitOfWork)
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

        public void RegisterUpdateBatch(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                unitOfWork.RegisterUpdate(entity, commandRepository);
            }
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
