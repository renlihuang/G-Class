using CS.Base.HttpHelper;
using CS.Model.Entiry;
using CS.Model.QueryCondition;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.IBLL
{
    public interface IRolesService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleEntiry"></param>
        /// <returns></returns>
        Task<bool> AddRole(RoleEntiry roleEntiry);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleEntiry"></param>
        /// <returns></returns>
        Task<bool> UpdateRole(long id, RoleEntiry roleEntiry);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleEntiry"></param>
        /// <returns></returns>
        Task<bool> DeleteRole(long id);

        /// <summary>
        /// 获取所有的角色
        /// </summary>
        /// <returns></returns>
        Task<List<RoleEntiry>> GetRoles();

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <returns></returns>
        Task<RoleEntiry> GetRole(long id);

        /// <summary>
        /// 查找角色
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<RoleEntiry>> GetRolesPage(int pageIndex, int pageSize, RoleQueryCondition condition);
    }
}