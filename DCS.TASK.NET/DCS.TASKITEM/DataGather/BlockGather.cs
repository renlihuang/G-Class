using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.BlockCheckinvent;
//using MESwebservice.BlockdataCollect;
using MESwebservice.BlockMiAssembleAndCollectData;
using MESwebservice.BlockReleaseSfc;
using MESwebservice.BlocksfcComplete;
using MESwebservice.Mescall;
using MESwebservice.ShimEntry;
using System;
using System.Collections.Generic;
/// <summary>
/// block预压
/// </summary>
namespace DCS.TASKITEM.DataGather
{
    public class BlockGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        static bool _outStaionResult = false;
        //csvlist
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();
        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public BlockGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        RequestToHttpHelper _requestToHttpHelper;
        //采集任务
        static CollectTaskContext _collectTaskContext;
        //需要采集的参数，根据业务增加
        string plcDataStatus;
        string mesDataStatus;
        string Apihost;
        //日志帮助类
        static ILogOperator _log;
        int code;
        string msg;
        string messfc;

        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            //plcDataStatus = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            //mesDataStatus = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            // 释放模组号
            //miReleaseSfcWithActivityResponse releaseResponse = BlockCall.BlockReleaseSfc(@AppConfig.WebserviceiniPath, "Block释放模组号");
            //code = releaseResponse.@return.code;
            //msg = releaseResponse.@return.message;

            // 电芯校验
            //miCheckInventoryAttributesResponse checkResponse = BlockCall.BlockCheckinvent(@AppConfig.WebserviceiniPath, "Block电芯校验");
            //code = checkResponse.@return.code;
            //msg = checkResponse.@return.message;

            // 模组装配
            // miAssmebleAndCollectDataForSfcResponse collectResponse = BlockCall.BlockMiAssemble(@AppConfig.WebserviceiniPath, "Block模组装配电芯");
            // code = collectResponse.@return.code;
            // msg = collectResponse.@return.message;

            // 数据收集
            //MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse = BlockCall.BlockDatacoll(@AppConfig.WebserviceiniPath, "Block收数");
            //code = sfcResponse.@return.code;
            //msg = sfcResponse.@return.message;

            // 出站
            //MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponse = BlockCall.BlocksfcComplete(@AppConfig.WebserviceiniPath, "Block出站");
            //code = sfcCompleteResponse.@return.code;
            //msg = sfcCompleteResponse.@return.message;

            // 进站 
            //MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "Block入packing进站");
            //code = miFindCustomAndSfcDataResponse.@return.code;
            //msg = miFindCustomAndSfcDataResponse.@return.message;

            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();


            string tagName = "\"TMCblockDB1\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[tagName + $"\"BlockF1\""] = null;
            data[tagName + $"\"BlockT1\""] = null;
            data[tagName + $"\"BlockL1\""] = null;

            if (opc.ReadNodes(data))
            {
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}
                _collectTaskContext.TaskMsgOperator.SetPairText("PLC标志位", data[tagName + "dataStatus_PLC"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("MES标志位", data[tagName + "dataStatus_MES"].ToString());
                int PlcFalg = Convert.ToInt16(data[plcDataStatus]);
                int MesFalg = Convert.ToInt16(data[mesDataStatus]);

                string BlockF1 = data[tagName + $"\"BlockF1\""].ToString();
                string BlockT1 = data[tagName + $"\"BlockT1\""].ToString();
                string BlockL1 = data[tagName + $"\"BlockL1\""].ToString();
                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    #region ocv mes交互
                    // 判断mes状态是否启用
                    var mesStatus = IniFileAPI.INIGetStringValue(@AppConfig.WebserviceiniPath, "MES状态", "MesStatus", "");
                    if (mesStatus == "ON") // ON 开
                    {
                        //// 出站
                        //try
                        //{
                        //    MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponse = BlockCall.BlocksfcComplete(@AppConfig.WebserviceiniPath, "Block出站");
                        //    code = sfcCompleteResponse.@return.code;
                        //    msg = sfcCompleteResponse.@return.message;
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw ex;
                        //}
                        //if (code == 0)
                        //{
                        //    _collectTaskContext.TaskMsgOperator.SetPairText("BLOCK出站接口调用成功", "");
                        //    _log.AddUserLog("BLOCK预压", "BLOCK预压", string.Format("BLOCK出站接口调用成功"));
                        //}
                        //else if (code > 0)
                        //{
                        //    _collectTaskContext.TaskMsgOperator.SetPairText("BLOCK出站接口调用失败", "");
                        //    _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        //    _collectTaskContext.TaskMsgOperator.SetPairText("BLOCK出站返回代码：", code.ToString());
                        //    _collectTaskContext.TaskMsgOperator.SetPairText("BLOCK出站返回信息", msg);
                        //    _log.AddUserLog("BLOCK预压", "BLOCK预压", string.Format("BLOCK出站接口异常，异常代码为{0}，返回信息为{1}", code.ToString(), msg));
                        //    //_outStaionResult = false;
                        //}
                        //else if (code == -1)
                        //{
                        //    _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                        //    _log.AddUserLog("BLOCK预压", "BLOCK预压", string.Format("BLOCK出站接口异常，请检查网络"));
                        //}
                        // 进站

                        // 收数出站
                        try
                        {
                            //MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse = BlockCall.BlockDatacoll(@AppConfig.WebserviceiniPath, "Packing垫片收数", ModuleCode1);
                            //code = sfcResponse.@return.code;
                            //msg = sfcResponse.@return.message;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        if (code == 0)
                        {
                            _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数出站接口调用成功", "");
                            _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("A120箱体贴垫片收数出站接口调用成功"));
                        }
                        else if (code > 0)
                        {
                            _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数出站接口调用失败", "");
                            _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                            _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数返回代码：", code.ToString());
                            _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数返回信息", msg);
                            _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("A120箱体贴垫片收数出站接口异常，异常代码为{0}，返回信息为{1}", code.ToString(), msg));
                            //_outStaionResult = false;
                        }
                        else if (code == -1)
                        {
                            _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                            _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("A120箱体贴垫片收数出站接口异常，请检查网络"));
                        }
                    }
                    #endregion


                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        //验证数据和业务逻辑写在这个里面
                        do
                        {
                            //

                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //保存出站结果
                        _outStaionResult = true ;
                        //初始化实体类
                        BlockPressInBoxEntity blockPressInBox = new BlockPressInBoxEntity();
                        blockPressInBox.BlockF1 = BlockF1;
                        if (!string.IsNullOrWhiteSpace(BlockT1))
                        {
                            blockPressInBox.BlockT1 = Convert.ToDateTime(BlockT1);
                        }
                        blockPressInBox.BlockL1 = BlockL1;
                        //上传追溯数据
                        AddEntityAync(blockPressInBox);
                    }
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst,"", "block预压");
                    //根据是否出站成功来回写标准位
                    if (_outStaionResult)
                    {
                        writeResult = 1;
                    }
                    else
                    {
                        writeResult = 2;
                    }
                    writeTags[mesDataStatus] = Convert.ToInt16(writeResult);
                    //写入PLC变量
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("BLOCK预压", "BLOCK预压", string.Format("BLOCK预压resul写入失败"));
                    }
                    else
                    {

                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("BLOCK预压", "BLOCK预压", string.Format("BLOCK预压resul写入2失败"));
                        }
                        else
                        {
                            //标记结束出站
                            _isOutStation = false;
                        }
                    }


                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("BLOCK预压", "BLOCK预压", string.Format("BLOCK预压标志位写0失败"));
                    }
                    _isOutStation = false;
                }
                if (PlcFalg == 3)
                {
                    
                    _isOutStation = false;

                    writeTags[mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("BLOCK预压", "BLOCK预压", string.Format("BLOCK预压标志位写4失败"));
                        }
                    }

                }
            }

        }
        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<string> AddEntityAync(BlockPressInBoxEntity blockPressInBoxEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/BlockPressInBox/insert",
                Data = blockPressInBoxEntity
            }).Result;
        }


        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}
