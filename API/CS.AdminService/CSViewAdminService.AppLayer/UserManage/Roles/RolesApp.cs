
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/19 9:09:55
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using CSViewAdminService.UserManage.Abstractions;
using CSViewAdminService.Utility;
using CSViewAdminService.Core;
using CSViewAdminService.AppLayer;
using CSViewAdminService.AppLayer.UserManage.ViewObject;
using CSViewAdminService.AppLayer.UserManage.DTOS;
namespace CSViewAdminService.AppLayer
{
    public class RolesApp:ISelfScopedAutoInject
    {
        private readonly ICommandRolesService commandRolesService;
		private readonly IQueryRolesService queryRolesService;


         public RolesApp(ICommandRolesService commandRolesService,IQueryRolesService queryRolesService)
        {
            this.commandRolesService = commandRolesService;
			this.queryRolesService = queryRolesService;
        }

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<RolesView> GetAsync(long id)
        {
		   
           return   await queryRolesService.GetAsync<RolesView>(id).ConfigureAwait(false);
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
        public async Task<QueryPagedResponseModel<RolesView>> GetListPagedAsync(int page, int size, RolesConditionDTO conditionDTO=null, string field = null, string orderBy = null)
        {
		     var condition=conditionDTO.MapTo<BaseRolesCondition>();
		     return await queryRolesService.GetListPagedAsync<RolesView>(page, size, condition, field, orderBy).ConfigureAwait(false);
        }

        /// <summary>
        /// 查询数据(不分页) 返回指定实体T
        /// </summary>
        /// <param name="conditionDTO">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public async Task<List<RolesView>> GetListAsync(RolesConditionDTO  conditionDTO, string field = null, string orderBy = null)
        {
		     var condition=conditionDTO.MapTo<BaseRolesCondition>();
             return await queryRolesService.GetListAsync<RolesView>(condition, field, orderBy).ConfigureAwait(false);
	    }

        /// <summary>
        /// 根据id集合获取多条数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<RolesView>> GetListByIdsAsync(List<long> ids)
        {
             return await queryRolesService.GetListByIdsAsync<RolesView>(ids).ConfigureAwait(false);
	    }

	    /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="insertDto">对应实体数据传输对象</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<long>> InsertAsync(RolesInsertDTO insertDto)
        { 
		    var entity=insertDto.MapTo<RolesEntity>();
            var result = await commandRolesService.InsertAsync(entity).ConfigureAwait(false);
            return result;
        }

		 

		   /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="insertDtoList">对应实体数据传输对象集合</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> InsertBatchAsync(List<RolesInsertDTO> insertDtoList)
        {
		    var entities=insertDtoList.MapToList<RolesEntity>();
            var result = await commandRolesService.InsertBatchAsync(entities).ConfigureAwait(false);
            return result;
        }
       
		  
        /// <summary>
        /// 根据主键更新实体
        /// </summary>
		/// <param name="id">主键</param>
        /// <param name="updateDto">对应实体数据传输对象</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdateAsync(long id,RolesUpdateDTO updateDto)
        {
		    var entity=updateDto.MapTo<RolesEntity>();
			entity.Id = id;
            var result = await commandRolesService.UpdateAsync(entity).ConfigureAwait(false);  
            return result;
        }
	  
 
       /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteAsync(long id)
        {
            var result = await commandRolesService.DeleteAsync(id).ConfigureAwait(false);
            return result;
	   }


	     /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="idList">主键集合</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteBatchAsync(IList<long> idList)
        {
            var result = await commandRolesService.DeleteBatchAsync(idList).ConfigureAwait(false);
            return result;

        }

       

    }
}
