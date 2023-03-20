
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:19:01
using System;
using CATLGClassWcsService.Core;
namespace CATLGClassWcsService.AppLayer.ViewObject
{
    /// <summary>
    /// 
    /// </summary>
	[MyTableName("GCGlue")]
    [MyPrimaryKey("ID",AutoIncrement =false)]
    public class GCGlueView:ViewBaseField
    {
            /// <summary>
        ///  ID
        /// </summary>
        public long ID {get;set;}
        
          /// <summary>
        ///  模组码
        /// </summary>
        public string ModuleCode {get;set;}
        
          /// <summary>
        ///  涂胶开始时间
        /// </summary>
        public DateTime? GlueStartTime {get;set;}
        
          /// <summary>
        ///  涂胶结束时间
        /// </summary>
        public DateTime? GlueEndTime {get;set;}
        
          /// <summary>
        ///  涂胶重量
        /// </summary>
        public float? GlueWeight {get;set;}
        
     

    }
}
