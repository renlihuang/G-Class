using CS.Base.AppSet;
using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.Mescall;
using MESwebservice.ShimCollForSfc;
using MESwebservice.ShimEntry;
using System;
using System.Collections.Generic;
/// <summary>
/// A120箱体贴垫片
/// </summary>
namespace DCS.TASKITEM.DataGather
{
    internal class PasteBoxShimGather : IPeriodicTask
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
        public PasteBoxShimGather(RequestToHttpHelper requestToHttpHelper)
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

        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            plcDataStatus = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            mesDataStatus = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            // 进站
            //MESwebservice.FindCusAndsSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "Packing垫片进站");
            //code = miFindCustomAndSfcDataReponse.@return.code;
            //msg = miFindCustomAndSfcDataResponse.@return.message;

            // 收数出站
            //MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse = BlockCall.BlockDatacoll(@AppConfig.WebserviceiniPath, "Packing垫片收数");
            //code = sfcResponse.@return.code;
            //msg = sfcResponse.@return.message;

            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();


            string tagName = "\"TMCShimDB\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[tagName + $"\"ModuleCode1\""] = null;
            data[tagName + $"\"ModuleCode2\""] = null;
            data[tagName + $"\"Shimnum\""] = null;

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

                string ModuleCode1 = data[tagName + $"\"ModuleCode1\""].ToString();
                string ModuleCode2 = data[tagName + $"\"ModuleCode2\""].ToString();
                string Shimnum = data[tagName + $"\"Shimnum\""].ToString();

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    #region ocv mes交互
                    // 进站
                    try
                    {
                        MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "Packing垫片进站");
                        code = miFindCustomAndSfcDataResponse.@return.code;
                        msg = miFindCustomAndSfcDataResponse.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片进站接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片进站接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片进站返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片进站返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code == -1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    }
                    // 收数出站
                    try
                    {
                        MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse = BlockCall.BlockDatacoll(@AppConfig.WebserviceiniPath, "Packing垫片收数", ModuleCode1);
                        code = sfcResponse.@return.code;
                        msg = sfcResponse.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数出站接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数出站接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("A120箱体贴垫片收数返回信息", msg);
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
                        PasteBoxShimEntity pasteBoxShim = new PasteBoxShimEntity();
                        pasteBoxShim.ModuleCode1 = ModuleCode1;
                        pasteBoxShim.ModuleCode2 = ModuleCode2;
                        pasteBoxShim.ShimNum = Shimnum;
                        //上传追溯数据
                        AddEntityAync(pasteBoxShim);

                    }
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst, "","箱体贴垫片");
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
                        _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("箱体贴垫片resul写入失败"));
                    }
                    else
                    {

                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("箱体贴垫片resul写入2失败"));
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
                        _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("箱体贴垫片标志位写0失败"));
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
                            _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("箱体贴垫片标志位写4失败"));
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
        HttpResponseResultModel<string> AddEntityAync(PasteBoxShimEntity pasteBoxShimEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/PasteBoxShim/insert",
                Data = pasteBoxShimEntity
            }).Result;
        }


        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}
