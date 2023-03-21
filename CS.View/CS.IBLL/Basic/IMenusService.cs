using CS.Model.Entiry;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.IBLL
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IMenusService
    {
        /// <summary>
        /// 当前ID是否有子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> HasChildren(long id);

        /// <summary>
        /// 添加菜单信息
        /// </summary>
        /// <param name="mensEntiry"></param>
        /// <returns></returns>
        Task<long> AddMenuAsync(MenuEntiry mensEntiry);

        /// <summary>
        /// 根据ID删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteMenuAsync(long id);

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="mensEntiry"></param>
        /// <returns></returns>
        Task<bool> UpdateMenuAsync(long id, MenuEntiry mensEntiry);

        /// <summary>
        /// 根据ID获取菜单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<MenuEntiry> GetMenuByID(long id);

        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        Task<List<MenuEntiry>> GetMenusByParentID(long parentID);
    }
}