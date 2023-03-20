
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:19:11
using System;
using CATLGClassWcsService.Core;
namespace CATLGClassWcsService.Basic.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    [MyTableName("GCPDP")]
    [MyPrimaryKey("ID",AutoIncrement =false)]
    public class GCPDPEntity: BaseField,IEntity<long>
    {

       public  GCPDPEntity()
       {
                    Id = GeneratePrimaryKeyIdHelper.GetPrimaryKeyId();
                         }
       public long Id{get;set;}
            /// <summary>
        ///  模组条码
        /// </summary>
                  public string ModuleCode {get;set;}
        
          /// <summary>
        ///  等离子转速
        /// </summary>
                  public string PDPrpm {get;set;}
        
          /// <summary>
        ///  等离子电压
        /// </summary>
                  public string PDPvoltage {get;set;}
        
          /// <summary>
        ///  等离子电流
        /// </summary>
                  public string PDPelectricity {get;set;}
        
          /// <summary>
        ///  等离子气压
        /// </summary>
                  public string PDPkPa {get;set;}
        
          /// <summary>
        ///  等离子速度
        /// </summary>
                  public string PDPspeed {get;set;}
        
          /// <summary>
        ///  等离子功率
        /// </summary>
                  public string PDPpower {get;set;}
        
     

    }

   
}
