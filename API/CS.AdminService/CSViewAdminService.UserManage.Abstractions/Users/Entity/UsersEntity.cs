//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/9/7 15:56:49
using CSViewAdminService.Core;
using System;

namespace CSViewAdminService.UserManage.Abstractions
{
    /// <summary>
    ///
    /// </summary>
    [MyTableName("Users")]
    [MyPrimaryKey("ID", AutoIncrement = false)]
    public class UsersEntity : BaseField, IEntity<long>
    {
        public UsersEntity()
        {
            Id = GeneratePrimaryKeyIdHelper.GetPrimaryKeyId();
        }

        public long Id { get; set; }

        /// <summary>
        ///  用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        ///  密码
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        ///  用户角设
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        ///  角色ID
        /// </summary>
        public long? UserRoleID { get; set; }

        /// <summary>
        ///  最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
    }
}