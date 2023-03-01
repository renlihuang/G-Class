
 

using System.Collections.Generic;
using System.Threading.Tasks;
using CSViewAdminService.Model;
using CSViewAdminService.Core;
using System.Linq.Expressions;
using System;
namespace CSViewAdminService.IBLL.UserManage
{
    public interface ICommandUsersService
    {
	    /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<long>> InsertAsync(UsersEntity entity);


        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> InsertBatchAsync(List<UsersEntity> entityList);


         
          /// <summary>
        /// 根据主键更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> UpdateAsync(UsersEntity entity);

        /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteAsync(long id);

        /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="idList">主键集合</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteBatchAsync(IList<long> idList);
   
         
   }
}
