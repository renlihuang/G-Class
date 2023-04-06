
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/4/6 10:57:28
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using CATLGClassWcsService.Core;
using CATLGClassWcsService.Basic.Abstractions;
using CATLGClassWcsService.Utility;
namespace CATLGClassWcsService.Basic
{
    internal class CommandOCVTESTServiceImpl : ICommandOCVTESTService, IAutoInject
    {
        private readonly IDefaultUnitOfWorkV2<long,IEntity<long>, ICommandOCVTESTRepository<long>> unitOfWork;
		private readonly IQueryOCVTESTService  queryOCVTESTService;
 
        public CommandOCVTESTServiceImpl(IDefaultUnitOfWorkV2<long,IEntity<long>, ICommandOCVTESTRepository<long>> unitOfWork,IQueryOCVTESTService  queryOCVTESTService)
        {
            this.unitOfWork = unitOfWork;
			this.queryOCVTESTService=queryOCVTESTService;
         }

        #region 插入

        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<long>> InsertAsync(OCVTESTEntity entity,bool isCommit = true)
        {
            HttpResponseResultModel<long> httpResponseResultModel = new HttpResponseResultModel<long> { IsSuccess = false };
            unitOfWork.RegisterInsert(entity);
            if (isCommit)
            {
                 await  CommitAsync();
            }
            httpResponseResultModel.BackResult = entity.Id;
            httpResponseResultModel.IsSuccess = true;
            return httpResponseResultModel;
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> InsertBatchAsync(List<OCVTESTEntity> entityList,bool isCommit = true)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
             foreach (var entity in entityList)
            {
                unitOfWork.RegisterInsert(entity);
            }
            if (isCommit)
            {
                await  CommitAsync();
            }
            httpResponseResultModel.BackResult = true;
            httpResponseResultModel.IsSuccess = true;
            return httpResponseResultModel;
        }


        #endregion

	    #region 更新
        /// <summary>
        /// 根据主键更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdateAsync(OCVTESTEntity entity,bool isCommit = true)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            unitOfWork.RegisterUpdate(entity);
            if (isCommit)
            {
                  await  CommitAsync();
            }
            httpResponseResultModel.IsSuccess = true;
            httpResponseResultModel.BackResult = true;
            return httpResponseResultModel;
        }


        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdateBatchAsync(List<OCVTESTEntity> entityList,bool isCommit = true)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
             foreach (var entity in entityList)
            {
                unitOfWork.RegisterUpdate(entity);
            }
            if (isCommit)
            {
                await  CommitAsync();
            }
            httpResponseResultModel.IsSuccess = true;
            httpResponseResultModel.BackResult = true;
            return httpResponseResultModel;
        }

         

         


        

		#endregion

        #region 删除

        /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteAsync(long id,bool isCommit = true)
        {
		    HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            unitOfWork.RegisterDelete(id);
            if (isCommit)
            {
                 await  CommitAsync();
            }
            httpResponseResultModel.IsSuccess = true;
            httpResponseResultModel.BackResult = true;
            return httpResponseResultModel;
	   }

        /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="idList">主键集合</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteBatchAsync(IList<long> idList,bool isCommit = true)
        {
		  HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
           unitOfWork.RegisterDeleteBatch(idList);
            if (isCommit)
            {
                await  CommitAsync();
            }
            httpResponseResultModel.IsSuccess = true;
            httpResponseResultModel.BackResult = true;
            return httpResponseResultModel;
		
		}


        #endregion


        #region 保存 有则更新 无则插入
         

          /// <summary>
        /// 保存实体，有则更新，无则新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> SaveAsync(OCVTESTEntity entity, bool isCommit = true)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            var isExist = await queryOCVTESTService.ExistsAsync(entity.Id).ConfigureAwait(false);
            if (isExist)
            {
                unitOfWork.RegisterUpdate(entity);
            }
            else
            {
                unitOfWork.RegisterInsert(entity);
            }
            if (isCommit)
            {
                await CommitAsync();
            }
            httpResponseResultModel.IsSuccess = true;
            return httpResponseResultModel;
        }


          

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
        public virtual async Task<HttpResponseResultModel<bool>> UpsertDeleteAsync(List<OCVTESTEntity> entities, List<long> oldIdList, bool isCommit = true)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            var newEntityList = new List<IEntity<long>>();
            foreach (var entity in entities)
            {
                newEntityList.Add(entity);
            }
            unitOfWork.RegisterUpsertDelete(newEntityList, oldIdList);
            if (isCommit)
            {
                await CommitAsync();
            }
            httpResponseResultModel.BackResult = true;
            httpResponseResultModel.IsSuccess = true;
            return httpResponseResultModel;
        }
        #endregion

        #region 事务
		 
		 /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            await unitOfWork.CommitAsync().ConfigureAwait(false);
        }
	     #endregion

    }
}
