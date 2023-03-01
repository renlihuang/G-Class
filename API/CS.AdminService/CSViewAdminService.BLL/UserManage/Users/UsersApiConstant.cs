
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
namespace CSViewAdminService.BLL.UserManage
{
    public static  class  UsersApiConstant
    {
        private const string apiControllerPath = "Users";

       

        /// <summary>
        /// 插入接口路径
        /// </summary>
        public const string InsertPath = apiControllerPath + "/insert";

        /// <summary>
        /// 批量插入接口路径
        /// </summary>
        public const string InsertBatchPath = apiControllerPath + "/insertBatch";

      

         /// <summary>
        /// 更新接口路径
        /// </summary>
        public const string UpdatePath = apiControllerPath + "/UpdateById";

        /// <summary>
        /// 删除接口路径
        /// </summary>
        public const string DeletePath = apiControllerPath + "/delete";

         /// <summary>
        /// 删除接口路径
        /// </summary>
        public const string DeleteBatchPath = apiControllerPath + "/deleteBatch";

        /// <summary>
        /// 查询分页列表路径
        /// </summary>
        public const string QueryPagedListPath = apiControllerPath + "/GetListPaged";

        // <summary>
        /// 查询不分页列表路径
        /// </summary>
        public const string QueryListPath = apiControllerPath + "/GetList";

        // <summary>
        /// 根据id集合查询接口路径
        /// </summary>
        public const string QueryListByIdsPath = apiControllerPath + "/GetListByIds";

        // <summary>
        /// 根据id查询接口路径
        /// </summary>
        public const string QuerySingleByIdPath = apiControllerPath + "/get";


         
              }
}
