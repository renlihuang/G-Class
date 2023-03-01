

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/9/7 15:56:49
using System.Collections.Generic;
using System.Threading.Tasks;
using CSViewAdminService.Core;
using System.Runtime.CompilerServices;
///限定只能由固定的程序集访问
/// 限定仓储接口只能由对应的服务程序集使用
[assembly: InternalsVisibleTo(assemblyName: "CSViewAdminService.UserManage")]
[assembly: InternalsVisibleTo(assemblyName: "CSViewAdminService.Repository")]
namespace CSViewAdminService.UserManage.Abstractions
{
    internal interface IQueryUsersRepository : IQueryBaseRepository
    {
       
            /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="condition"></param>
        /// <param name="field"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseUsersCondition condition = null, string field = null, string orderBy = null);


        /// <summary>
        /// 不分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="field"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>(BaseUsersCondition condition, string field = null, string orderBy = null);


                }
}
