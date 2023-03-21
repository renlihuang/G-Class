using CS.Model.Entiry;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.IBLL
{
    public interface IMenuRightsService
    {
        /// <summary>
        /// 根据角色ID获取菜单ID
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        Task<List<MenuRightsEntity>> GetMenuIds(long roleID);

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuRightsEntity"></param>
        /// <returns></returns>
        Task<long> AddMenuRights(MenuRightsEntity menuRightsEntity);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuRightsEntity"></param>
        /// <returns></returns>
        Task<bool> DeleteMenuRights(long id);

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menuRightsEntity"></param>
        /// <returns></returns>
        Task<bool> UpdateMenuRights(long id, MenuRightsEntity menuRightsEntity);
    }
}