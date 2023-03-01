
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/9/7 15:56:49
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;

using CSViewAdminService.Core;
using CSViewAdminService.UserManage.Abstractions;
using CSViewAdminService.Utility;
namespace CSViewAdminService.UserManage
{
    internal class QueryUsersServiceImpl : IQueryUsersService, IAutoInject
    {
        private readonly IQueryUsersRepository queryRepository;

        public QueryUsersServiceImpl(IQueryUsersRepository queryRepository)
        {
            this.queryRepository = queryRepository;
        }

 

        /// <summary>
        /// 查询数据(分页) 返回指定实体T
        /// </summary>
        ///<typeparam name="T">返回实体类型</typeparam>
        /// <param name="page">页码</param>
        /// <param name="size">每页数量</param>
        /// <param name="condition">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public async Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseUsersCondition condition=null, string field = null, string orderBy = null)
        {
            return await queryRepository.GetListPagedAsync<T>(page, size, condition, field, orderBy).ConfigureAwait(false);
        }


        


        /// <summary>
        /// 查询数据(不分页) 返回指定实体T
        /// </summary>
        ///<typeparam name="T">返回实体类型</typeparam>
        /// <param name="page">页码</param>
        /// <param name="size">每页数量</param>
        /// <param name="condition">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<T>(BaseUsersCondition  condition=null, string field = null, string orderBy = null)
        {
            return await queryRepository.GetListAsync<T>(condition, field, orderBy).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(long id)
        {
            return await queryRepository.GetAsync<T,long>(id).ConfigureAwait(false);
        }


		 /// <summary>
        /// 根据主键集合获取多个实体
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        public async Task<List<T>> GetListByIdsAsync<T>(List<long> ids)
        {
            return await queryRepository.GetListByIdsAsync<T,long>(ids).ConfigureAwait(false);
        }

		  /// <summary>
        /// 根据主键判断数据是否存在
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(long id)
        {
            return await queryRepository.ExistsAsync(id).ConfigureAwait(false);
        }


        /// <summary>
        /// 检查字段是否唯一
        /// </summary>
        ///<typeparam name="TColunmValue">数据库对应字段值类型long，int等</typeparam>
        /// <param name="column"></param>
        /// <returns></returns>
        private async Task<bool> IsUniqueAsync<TColunmValue>(KeyValuePair<string, TColunmValue> column)
        {
            return await queryRepository.IsUniqueAsync(column).ConfigureAwait(false);
        }


		
     
	  
          

    }
}
