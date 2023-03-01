//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/19 9:09:55
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CSViewAdminService.AppLayer.UserManage.DTOS
{
    /// <summary>
    ///
    /// </summary>
    public class RolesUpdateDTO
    {
        /// <summary>
        ///  ID
        /// </summary>
        [Description("ID")]
        [Required]
        [Range(0, long.MaxValue)]
        public long ID { get; set; }

        /// <summary>
        ///  角色名称
        /// </summary>
        [Description("角色名称")]
        [MinLength(0)]
        [MaxLength(50)]
        public string RoleName { get; set; }

        /// <summary>
        ///  是否是管理员
        /// </summary>
        [Description("是否是管理员")]
        [Range(0, int.MaxValue)]
        public int? IsManage { get; set; }

        /// <summary>
        ///  是否删除
        /// </summary>
        [Description("是否删除")]
        [Range(0, int.MaxValue)]
        public int? IsDeleted { get; set; }
    }
}