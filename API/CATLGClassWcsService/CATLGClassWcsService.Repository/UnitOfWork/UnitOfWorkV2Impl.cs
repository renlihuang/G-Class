using CATLGClassWcsService.Core;
using CATLGClassWcsService.DAL;
using CATLGClassWcsService.Repository.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CATLGClassWcsService.Utility;

namespace CATLGClassWcsService.Repository
{
    internal class RepositoryCommonModel<TPrimaryKeyType>
    { 
        public ICommandBaseRepository<TPrimaryKeyType> CommandBaseRepository { set; get; }
        public IEntity<TPrimaryKeyType> Entity { set; get; }
    }

    internal class UnitOfWorV2Impl<TPrimaryKeyType> : IUnitOfWork<TPrimaryKeyType>, IAutoInject
    {
        /// <summary>
        /// 新增
        /// </summary>
        private ConcurrentDictionary<RepositoryCommonModel<TPrimaryKeyType>, List<IEntity<TPrimaryKeyType>>> addedEntities;

        /// <summary>
        /// 修改
        /// </summary>
        private ConcurrentDictionary<RepositoryCommonModel<TPrimaryKeyType>, List<IEntity<TPrimaryKeyType>>> changedEntities;

        /// <summary>
        /// 删除
        /// </summary>
        private ConcurrentDictionary<ICommandBaseRepository<TPrimaryKeyType>, List<TPrimaryKeyType>> deletedEntities;

        /// <summary>
        /// 保存
        /// </summary>
        private ConcurrentDictionary<RepositoryCommonModel<TPrimaryKeyType>, List<UpdateModel<TPrimaryKeyType>>> saveEntities;



        private readonly IScopeDBFactory scopeDBFactory;

        public UnitOfWorV2Impl(IScopeDBFactory scopeDBFactory)
        {

            addedEntities = new ConcurrentDictionary<RepositoryCommonModel<TPrimaryKeyType>, List<IEntity<TPrimaryKeyType>>>();
            changedEntities = new ConcurrentDictionary<RepositoryCommonModel<TPrimaryKeyType>, List<IEntity<TPrimaryKeyType>>>();
            deletedEntities = new ConcurrentDictionary<ICommandBaseRepository<TPrimaryKeyType>, List<TPrimaryKeyType>>();
            saveEntities = new ConcurrentDictionary<RepositoryCommonModel<TPrimaryKeyType>, List<UpdateModel<TPrimaryKeyType>>>();
            this.scopeDBFactory = scopeDBFactory;

        }

        public void RegisterInsert(IEntity<TPrimaryKeyType> entity, ICommandBaseRepository<TPrimaryKeyType> unitofWorkRepository)
        {
            var keyList = addedEntities.Keys.ToList();
            var repositoryCommonModel = keyList.Find(x => x.CommandBaseRepository == unitofWorkRepository && x.Entity != null && x.Entity.GetType() == entity.GetType());
            if (repositoryCommonModel == null)
            {

                addedEntities.TryAdd(new RepositoryCommonModel<TPrimaryKeyType>()
                {
                    Entity = entity,
                    CommandBaseRepository = unitofWorkRepository
                }, new List<IEntity<TPrimaryKeyType>>() { entity });
            }
            else
            {
                List<IEntity<TPrimaryKeyType>> list = addedEntities[repositoryCommonModel];
                if (!list.Contains(entity))
                {
                    addedEntities[repositoryCommonModel].Add(entity);
                }
            }

        }



        public void RegisterSaveBatch(List<IEntity<TPrimaryKeyType>> entityList, ICommandBaseRepository<TPrimaryKeyType> unitofWorkRepository)
        {
            UpdateModel<TPrimaryKeyType> updateModel = new UpdateModel<TPrimaryKeyType>();
            updateModel.SaveBatchEntityList = entityList;
            updateModel.UpdateType = UpdateTypeEnum.SaveBatch;

            var keyList = saveEntities.Keys.ToList();
            var repositoryCommonModel = keyList.Find(x => x.CommandBaseRepository == unitofWorkRepository && x.Entity != null && x.Entity.GetType() == entityList.FirstOrDefault().GetType());
            if (repositoryCommonModel == null)
            {
                saveEntities.TryAdd(new RepositoryCommonModel<TPrimaryKeyType>()
                {
                    Entity = entityList.FirstOrDefault(),
                    CommandBaseRepository = unitofWorkRepository
                }, new List<UpdateModel<TPrimaryKeyType>>() { updateModel });
            }
            else
            {
                List<UpdateModel<TPrimaryKeyType>> list = saveEntities[repositoryCommonModel];
                if (!list.Contains(updateModel))
                {
                    saveEntities[repositoryCommonModel].Add(updateModel);
                }
            }
        }

        /// <summary>
        ///  有则更新（增加），无则删除
        /// 1.entities中有， oldIdList没有的数据插入
        /// 2.oldIdList 和entities中有 都有的数据更新
        /// 3.oldIdList中有，entities中没有的数据删除
        /// </summary>
        /// <param name="newEntityList"></param>
        /// <param name="oldEntityList"></param>
        /// <param name="unitofWorkRepository"></param>
        public void RegisterUpsertDelete(List<IEntity<TPrimaryKeyType>> newEntityList, List<TPrimaryKeyType> oldIdList, ICommandBaseRepository<TPrimaryKeyType> unitofWorkRepository)
        {
            if (oldIdList.IsListNullOrEmpty())
            {
                RegisterSaveBatch(newEntityList, unitofWorkRepository);
            }
            else
            {
                var detailIds = newEntityList.Select(t => t.Id).ToList();
                var insertIds = detailIds.Except(oldIdList);//差集
                var deleteIds = oldIdList.Except(detailIds).ToList();//差集
                var updateIds = detailIds.Intersect(oldIdList);//交集
                var insertEntityList = newEntityList.Where(t => insertIds.Contains(t.Id)).ToList();
                var updateEntityList = newEntityList.Where(t => updateIds.Contains(t.Id)).ToList();
                foreach (var entity in insertEntityList)
                {
                    RegisterInsert(entity, unitofWorkRepository);
                }
                foreach (var entity in updateEntityList)
                {
                    RegisterUpdate(entity, unitofWorkRepository);
                }
                foreach (var deleteId in deleteIds)
                {
                    RegisterDelete(deleteId, unitofWorkRepository);
                }
            }
        }




        public void RegisterUpdate(IEntity<TPrimaryKeyType> entity, ICommandBaseRepository<TPrimaryKeyType> unitofWorkRepository)
        {
            var keyList = changedEntities.Keys.ToList();
            var repositoryCommonModel = keyList.Find(x => x.CommandBaseRepository == unitofWorkRepository && x.Entity != null && x.Entity.GetType() == entity.GetType());
            if (repositoryCommonModel == null)
            {
                changedEntities.TryAdd(new RepositoryCommonModel<TPrimaryKeyType>()
                {
                    Entity = entity,
                    CommandBaseRepository = unitofWorkRepository
                }, new List<IEntity<TPrimaryKeyType>>() { entity });
            }
            else
            {
                List<IEntity<TPrimaryKeyType>> list = changedEntities[repositoryCommonModel];

                if (!list.Contains(entity))
                {
                    changedEntities[repositoryCommonModel].Add(entity);
                }
            }
        }










        public void RegisterDelete(TPrimaryKeyType id, ICommandBaseRepository<TPrimaryKeyType> unitofWorkRepository)
        {

            if (!deletedEntities.ContainsKey(unitofWorkRepository))
            {
                deletedEntities.TryAdd(unitofWorkRepository, new List<TPrimaryKeyType>() { id });
            }
            else
            {
                List<TPrimaryKeyType> list = deletedEntities[unitofWorkRepository];
                if (!list.Contains(id))
                {
                    deletedEntities[unitofWorkRepository].Add(id);

                }
            }


        }



        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        private IDBTransaction BeginNewDBTransaction(CustomDatabase db)
        {

            var scopeTransaction = new DBTransactionImpl(db);
            return scopeTransaction;
        }

        IDBTransaction scope;
        public async Task CommitAsync()
        {
            if (scope == null)
            {
                var db = scopeDBFactory.GetScopeDb();
                scope = BeginNewDBTransaction(db);
            }
            using (scope)
            {
                try
                {

                    ///插入新增的实体
                    foreach (var repository in this.addedEntities.Keys)
                    {
                        var entityList = addedEntities[repository];
                        if (entityList.Count > 1)
                        {
                            await repository.CommandBaseRepository.InsertBatchAsync(entityList).ConfigureAwait(false);
                        }
                        else
                        {
                            await repository.CommandBaseRepository.InsertAsync(entityList[0]).ConfigureAwait(false);
                        }
                    }
                    //更新需要修改的实体
                    foreach (var repository in this.changedEntities.Keys)
                    {
                        var entityList = changedEntities[repository];
                        if (entityList.Count > 1)
                        {
                            await repository.CommandBaseRepository.UpdateBatchAsync(entityList).ConfigureAwait(false);
                        }
                        else
                        {
                            await repository.CommandBaseRepository.UpdateAsync(entityList[0]).ConfigureAwait(false);
                        }
                    }

                    ///删除实体
                    foreach (var repository in this.deletedEntities.Keys)
                    {
                        var entityList = deletedEntities[repository];
                        if (entityList.Count > 1)
                        {
                            await repository.SoftDeleteBatchAsync(entityList).ConfigureAwait(false);
                        }
                        else
                        {
                            await repository.SoftDeleteAsync(entityList[0]).ConfigureAwait(false);
                        }

                    }
                    foreach (var repository in this.saveEntities.Keys)
                    {
                        var updateModelList = saveEntities[repository];

                        foreach (var updateModel in updateModelList)
                        {
                            if (updateModel.UpdateType == UpdateTypeEnum.Save)
                            {
                                await repository.CommandBaseRepository.SaveAsync(updateModel.SaveEntity).ConfigureAwait(false);
                            }
                            if (updateModel.UpdateType == UpdateTypeEnum.SaveBatch)
                            {
                                await repository.CommandBaseRepository.SaveBatchAsync(updateModel.SaveBatchEntityList).ConfigureAwait(false);
                            }
                        }
                    }
                    scope.Complete();
                }
                finally
                {
                    addedEntities.Clear();
                    changedEntities.Clear();
                    deletedEntities.Clear();
                    saveEntities.Clear();
                    scope = null;
                }
            }
        }
        public IDBTransaction BeginTransaction()
        {
            var db = scopeDBFactory.GetScopeDb();
            scope = new DBTransactionImpl(db);
            return scope;
        }
    }
}
