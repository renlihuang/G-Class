using CS.Base.HttpHelper;
using CS.Model.Entiry;
using CS.Model.QueryCondition;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.IBLL
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userEntiry"></param>
        /// <returns></returns>
        Task<bool> AddUserAync(UserEntiry userEntiry);

        /// <summary>
        /// 更新条码
        /// </summary>
        /// <param name="userEntiry"></param>
        /// <returns></returns>
        Task<bool> UpdateUserAync(UserEntiry userEntiry);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userEntiry"></param>
        /// <returns></returns>
        Task<bool> DeleteUserAync(UserEntiry userEntiry);

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userQueryCondition"></param>
        /// <returns></returns>
        Task<List<UserEntiry>> GetUserAync(string userName);

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        Task<List<UserEntiry>> GetAllUsersAync();

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userQueryCondition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<UserEntiry>> GetUsersAync(int pageIndex, int pageSize, UserQueryCondition userQueryCondition);
    }
}