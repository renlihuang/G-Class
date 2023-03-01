
 
 //使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/9 9:23:36
using System.Collections.Generic;
using System.Threading.Tasks;
using CSViewAdminService.Core;
using System;
namespace CSViewAdminService.UserManage.Abstractions
{
    public interface ICommandMenusService
    {
	  /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<long>> InsertAsync(MenusEntity entity,bool isCommit = true);


        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> InsertBatchAsync(List<MenusEntity> entityList,bool isCommit = true);


        /// <summary>
        /// 根据主键更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> UpdateAsync(MenusEntity entity,bool isCommit = true);

        
        
        

        /// <summary>
        /// 根据主键批量更新实体
        /// </summary>
        /// <param name="entityList">实体集合</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> UpdateBatchAsync(List<MenusEntity> entityList,bool isCommit = true);

        /// <summary>
        /// 保存实体，有则更新，无则新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> SaveAsync(MenusEntity entity,bool isCommit = true);


      
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
        Task<HttpResponseResultModel<bool>> UpsertDeleteAsync(List<MenusEntity> entities, List<long> oldIdList, bool isCommit = true);



        /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteAsync(long id,bool isCommit = true);

        /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="idList">主键集合</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteBatchAsync(IList<long> idList,bool isCommit = true);
   
   }
}
