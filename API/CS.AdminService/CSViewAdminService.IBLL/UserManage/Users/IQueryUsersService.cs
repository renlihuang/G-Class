
 

using System.Collections.Generic;
using System.Threading.Tasks;
using CSViewAdminService.Model;
using CSViewAdminService.Core;
using CSViewAdminService.Model.UserManage.Conditions;

namespace CSViewAdminService.IBLL.UserManage
{
    public interface IQueryUsersService
    {
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
        Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseUsersCondition condition=null, string field = null, string orderBy = null);


        

        /// <summary>
        /// 查询数据(不分页) 返回指定实体T
        /// </summary>
        /// <param name="condition">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>(BaseUsersCondition condition=null, string field = null, string orderBy = null);

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(long id);


		/// <summary>
        /// 根据主键集合获取多个实体
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        Task<List<T>> GetListByIdsAsync<T>(List<long> ids);

       
             }
}
