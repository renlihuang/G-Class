
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:16:19
using System;
using CATLGClassWcsService.Core;
namespace CATLGClassWcsService.Basic.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    [MyTableName("GCPrestack")]
    [MyPrimaryKey("ID",AutoIncrement =false)]
    public class GCPrestackEntity: BaseField,IEntity<long>
    {

       public  GCPrestackEntity()
       {
                    Id = GeneratePrimaryKeyIdHelper.GetPrimaryKeyId();
                         }
       public long Id{get;set;}
            /// <summary>
        ///  虚拟模组码
        /// </summary>
                  public string VirtualCode {get;set;}
        
          /// <summary>
        ///  模组类型
        /// </summary>
                  public string MoudleType {get;set;}
        
          /// <summary>
        ///  资源号
        /// </summary>
                  public string Resource {get;set;}
        
          /// <summary>
        ///  工单
        /// </summary>
                  public string WorkOrder {get;set;}
        
     

    }

   
}
