
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:19:01
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using CATLGClassWcsService.Basic.Abstractions;
using CATLGClassWcsService.Utility;
using CATLGClassWcsService.Core;
using CATLGClassWcsService.AppLayer;
using CATLGClassWcsService.AppLayer.ViewObject;
using CATLGClassWcsService.AppLayer.DTOS;
namespace CATLGClassWcsService.AppLayer
{
    public class GCGlueApp:ISelfScopedAutoInject
    {
        private readonly ICommandGCGlueService commandGCGlueService;
		private readonly IQueryGCGlueService queryGCGlueService;


         public GCGlueApp(ICommandGCGlueService commandGCGlueService,IQueryGCGlueService queryGCGlueService)
        {
            this.commandGCGlueService = commandGCGlueService;
			this.queryGCGlueService = queryGCGlueService;
        }

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<GCGlueView> GetAsync(long id)
        {
		   
           return   await queryGCGlueService.GetAsync<GCGlueView>(id).ConfigureAwait(false);
		}


        /// <summary>
        /// 查询数据(分页) 返回指定实体T
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">每页数量</param>
        /// <param name="conditionDTO">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public async Task<QueryPagedResponseModel<GCGlueView>> GetListPagedAsync(int page, int size, GCGlueConditionDTO conditionDTO=null, string field = null, string orderBy = null)
        {
		     var condition=conditionDTO.MapTo<BaseGCGlueCondition>();
		     return await queryGCGlueService.GetListPagedAsync<GCGlueView>(page, size, condition, field, orderBy).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询数据(不分页) 返回指定实体T
        /// </summary>
        /// <param name="conditionDTO">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public async Task<List<GCGlueView>> GetListAsync(GCGlueConditionDTO  conditionDTO, string field = null, string orderBy = null)
        {
		     var condition=conditionDTO.MapTo<BaseGCGlueCondition>();
             return await queryGCGlueService.GetListAsync<GCGlueView>(condition, field, orderBy).ConfigureAwait(false);
	    }

        /// <summary>
        /// 根据id集合获取多条数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<GCGlueView>> GetListByIdsAsync(List<long> ids)
        {
             return await queryGCGlueService.GetListByIdsAsync<GCGlueView>(ids).ConfigureAwait(false);
	    }

	    /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="insertDto">对应实体数据传输对象</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<long>> InsertAsync(GCGlueInsertDTO insertDto)
        { 
		    var entity=insertDto.MapTo<GCGlueEntity>();
            var result = await commandGCGlueService.InsertAsync(entity).ConfigureAwait(false);
            return result;
        }

		 

		   /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="insertDtoList">对应实体数据传输对象集合</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> InsertBatchAsync(List<GCGlueInsertDTO> insertDtoList)
        {
		    var entities=insertDtoList.MapToList<GCGlueEntity>();
            var result = await commandGCGlueService.InsertBatchAsync(entities).ConfigureAwait(false);
            return result;
        }
       
		  
        /// <summary>
        /// 根据主键更新实体
        /// </summary>
		/// <param name="id">主键</param>
        /// <param name="updateDto">对应实体数据传输对象</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdateAsync(long id,GCGlueUpdateDTO updateDto)
        {
		    var entity=updateDto.MapTo<GCGlueEntity>();
			entity.Id = id;
            var result = await commandGCGlueService.UpdateAsync(entity).ConfigureAwait(false);  
            return result;
        }
	  
 
       /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteAsync(long id)
        {
            var result = await commandGCGlueService.DeleteAsync(id).ConfigureAwait(false);
            return result;
	   }


	     /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="idList">主键集合</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteBatchAsync(IList<long> idList)
        {
            var result = await commandGCGlueService.DeleteBatchAsync(idList).ConfigureAwait(false);
            return result;

        }

       

    }
}
