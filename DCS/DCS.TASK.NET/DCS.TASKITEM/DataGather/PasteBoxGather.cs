using CS.Base.AppSet;
using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.AssAndCollData;
using MESwebservice.AutoWeight;
using MESwebservice.CheckBomInventory;
using MESwebservice.GetBarCodeInfo;
using MESwebservice.Mescall;
using MESwebservice.ModuleEntry;
using System;
using System.Collections.Generic;
using System.Threading;
/// <summary>
/// M020-2贴箱体标
/// </summary>
namespace DCS.TASKITEM.DataGather
{
    internal class PasteBoxGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        static bool _outStaionResult = false;
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();
        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public PasteBoxGather(RequestToHttpHelper requestToHttpHelper)
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
        int code;
        string message;
        //日志帮助类
        static ILogOperator _log;
        string msg;
        //心跳变量
        int heartstat;

        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {


            // 获取打印条码信息
            //miGetSlotDataResponse miGetSlotDataResponse = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌");
            //code = miGetSlotDataResponse.@return.code;
            //msg = miGetSlotDataResponse.@return.message;

            // 进站
            //MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "贴箱体标进站");
            //code = miFindCustomAndSfcDataResponse.@return.code;
            //msg = miFindCustomAndSfcDataResponse.@return.message;

            // 校验贴纸PN以及库存
            //miCheckBOMInventoryResponse miCheckBOMInventoryResponse = PasteBoxCall.ChenkBomInvent(@AppConfig.WebserviceiniPath, "贴箱体标校验物料");
            //code = miCheckBOMInventoryResponse.@return.code;
            //msg = miCheckBOMInventoryResponse.@return.message;

            // 组装物料
            //miAssmebleAndCollectDataForSfcResponse miAssmebleAndCollectDataForSfcResponse = PasteBoxCall.AssemblyMater(@AppConfig.WebserviceiniPath, "贴箱体标组装物料");
            //code = miAssmebleAndCollectDataForSfcResponse.@return.code;
            //msg = miAssmebleAndCollectDataForSfcResponse.@return.message;

            // 收数
            //dataCollectForModuleTestResponse response = WeightCall.CheckWeight(@AppConfig.WebserviceiniPath, "贴箱体标收数");
            //code = response.@return.code;
            //msg = response.@return.message;

            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();

            string tagName = "\"TMCBoxlabelDB\".";
            data[tagName + "dataStatus_PLC"] = null;
           // data[tagName + "dataStatus_MES"] = null;
            mesDataStatus = tagName + "dataStatus_MES";
            data[mesDataStatus] = null;
            data[tagName + "dataStatus_Heart"] = null;
            data[tagName + $"\"Pack_Acode\""] = null;
            data[tagName + $"\"Pack_Bcode\""] = null;

            Thread.Sleep(1200);
            if (opc.ReadNodes(data))
            {
                heartstat = Convert.ToInt16(data[tagName + "dataStatus_Heart"].ToString());
                if (heartstat == 1)
                {
                    heartstat = 0;
                }
                else
                {
                    heartstat = 1;
                }
                writeTags[tagName + "dataStatus_Heart"] = Convert.ToInt16(heartstat);
                //写入PLC变量

                if (!opc.WriteNodes(writeTags))
                {
                    _log.AddUserLog("自动称重", "自动称重心跳", string.Format("自动称重心跳写入失败"));
                }
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}
                _collectTaskContext.TaskMsgOperator.SetPairText("心跳值", heartstat.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("PLC标志位", data[tagName + "dataStatus_PLC"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("MES标志位", data[tagName + "dataStatus_MES"].ToString());
                int PlcFalg = Convert.ToInt16(data[tagName + "dataStatus_PLC"]);
                int MesFalg = Convert.ToInt16(data[tagName + "dataStatus_MES"]);

                string ModuleCode1 = data[tagName + $"\"Pack_Acode\""].ToString();
                string ModuleCode2 = data[tagName + $"\"Pack_Bcode\""].ToString();
                //string PackCode = data[tagName + $"\"PackCode\""].ToString();
                //string rfid = data[tagName + $"\"RFID\""].ToString();

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    #region ocv mes交互
                    // 获取打印条码信息
                    //try
                    //{
                    //    miGetSlotDataResponse miGetSlotDataResponse = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌");
                    //    code = miGetSlotDataResponse.@return.code;
                    //    msg = miGetSlotDataResponse.@return.message;
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                    //if (code == 0)
                    //{
                    //    _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标获取打印条码信息接口调用成功", "");
                    //}
                    //else if (code > 0)
                    //{
                    //    _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标获取打印条码信息接口调用失败", "");
                    //    _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                    //    _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标获取打印条码信息返回代码：", code.ToString());
                    //    _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标获取打印条码信息返回信息", msg);
                    //    //_outStaionResult = false;
                    //}
                    //else if (code == -1)
                    //{
                    //    _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    //}
                    // 进站
                    try
                    {
                        MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "贴箱体标进站");
                        code = miFindCustomAndSfcDataResponse.@return.code;
                        msg = miFindCustomAndSfcDataResponse.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标进站接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标进站接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标进站返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标进站返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code == -1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    }
                    // 校验贴纸PN以及库存
                    try
                    {
                        miCheckBOMInventoryResponse miCheckBOMInventoryResponse = PasteBoxCall.ChenkBomInvent(@AppConfig.WebserviceiniPath, "贴箱体标校验物料");
                        code = miCheckBOMInventoryResponse.@return.code;
                        msg = miCheckBOMInventoryResponse.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标校验贴纸PN以及库存接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标校验贴纸PN以及库存接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标校验贴纸PN以及库存返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标校验贴纸PN以及库存返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code == -1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    }
                    // 组装物料
                    try
                    {
                        miAssmebleAndCollectDataForSfcResponse miAssmebleAndCollectDataForSfcResponse = PasteBoxCall.AssemblyMater(@AppConfig.WebserviceiniPath, "贴箱体标组装物料");
                        code = miAssmebleAndCollectDataForSfcResponse.@return.code;
                        msg = miAssmebleAndCollectDataForSfcResponse.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标组装物料接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标组装物料接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标组装物料返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标组装物料返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code == -1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    }
                    // 收数
                    try
                    {
                        dataCollectForModuleTestResponse response = WeightCall.CheckWeight(@AppConfig.WebserviceiniPath, "贴箱体标收数");
                        code = response.@return.code;
                        msg = response.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标收数接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标收数接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标收数返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("贴箱体标收数返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code == -1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
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
                        _outStaionResult = true;
                        //初始化实体类
                        PasteBoxEntity paste = new PasteBoxEntity();
                        //paste.RFID = rfid;
                        paste.ModuleCode1 = ModuleCode1;
                        paste.ModuleCode2 = ModuleCode2;
                       // paste.PackCode = PackCode;
                        //上传追溯数据
                        AddEntityAync(paste);

                    }
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst, "","贴箱体标");
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
                        _log.AddUserLog("贴箱体标", "贴箱体标", string.Format("贴箱体标resul写入失败"));
                    }

                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("贴箱体标", "贴箱体标", string.Format("贴箱体标标志位写0失败"));
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
                            _log.AddUserLog("贴箱体标", "贴箱体标", string.Format("贴箱体标标志位写4失败"));
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
        HttpResponseResultModel<string> AddEntityAync(PasteBoxEntity pasteBoxEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/PasteBox/insert",
                Data = pasteBoxEntity
            }).Result;
        }


        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}
