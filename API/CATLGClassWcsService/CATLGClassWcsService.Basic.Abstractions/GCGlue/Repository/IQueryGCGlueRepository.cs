

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:19:01
using System.Collections.Generic;
using System.Threading.Tasks;
using CATLGClassWcsService.Core;
using System.Runtime.CompilerServices;
///限定只能由固定的程序集访问
/// 限定仓储接口只能由对应的服务程序集使用
[assembly: InternalsVisibleTo(assemblyName: "CATLGClassWcsService.Basic")]
[assembly: InternalsVisibleTo(assemblyName: "CATLGClassWcsService.Repository")]
namespace CATLGClassWcsService.Basic.Abstractions
{
    internal interface IQueryGCGlueRepository : IQueryBaseRepository
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
        Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseGCGlueCondition condition = null, string field = null, string orderBy = null);


        /// <summary>
        /// 不分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="field"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>(BaseGCGlueCondition condition, string field = null, string orderBy = null);


            

        
                }
}
