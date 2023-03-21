using System;

namespace CS.Model.Entiry
{
    public class UserEntiry
    {
        /// <summary>
        ///  ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        ///  用户名
        /// </summary>
        public string UserName { get; set; }

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
        public long UserRoleID { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///  最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }


        /// <summary>
        /// 自动退出登录时间
        /// </summary>
        public int LogoutTime { set; get; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDeleted { set; get; }
    }
}