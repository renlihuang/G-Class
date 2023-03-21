using System;

namespace CS.Model.Entiry
{
    public class RoleEntiry
    {
        public long? Id { get; set; }

        /// <summary>
        ///  角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        ///  是否是管理员
        /// </summary>
        public int IsManage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}