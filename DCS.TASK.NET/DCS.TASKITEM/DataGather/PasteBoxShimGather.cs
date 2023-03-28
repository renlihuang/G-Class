using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.Mescall;
using MESwebservice.ShimCollForSfc;
using MESwebservice.ShimEntry;
using System;
using System.Collections.Generic;
using System.Text;
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
        string pnA = string.Empty;
        string pnB = string.Empty;


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
            //MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse = BlockCall.BlockDatacoll(@AppConfig.WebserviceiniPath, "Packing垫片收数", ModuleCode1, pasteBoxShim1);
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

            data[tagName + $"\"Shimnum1\""] = null;
            data[tagName + $"\"Shimnum2\""] = null;

            data[tagName + $"\"4_4BlockMPA1\""] = null;
            data[tagName + $"\"4_4BlockMPA2\""] = null;
            data[tagName + $"\"4_4BlockTime1\""] = null;
            data[tagName + $"\"4_4BlockTime2\""] = null;

            data[tagName + $"\"10BlockMPA1\""] = null;
            data[tagName + $"\"10BlockMPA2\""] = null;
            data[tagName + $"\"10BlockTime1\""] = null;
            data[tagName + $"\"10BlockTime2\""] = null;

            data[tagName + $"\"BlockL1\""] = null;
            data[tagName + $"\"BlockL2\""] = null;

            data[tagName + $"\"BlockL1_DB_CZ\""] = null;
            data[tagName + $"\"BlockL2_DB_CZ\""] = null;

            data[tagName + $"\"StandPressure1\""] = null; // 静置压力1  20220726
            data[tagName + $"\"StandPressure2\""] = null; // 静置压力2  20220726

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

                string ModuleCode1 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + $"\"ModuleCode1\""]))).Trim();
                string ModuleCode2 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + $"\"ModuleCode2\""]))).Trim();
                if (!string.IsNullOrEmpty(ModuleCode1) && !string.IsNullOrEmpty(ModuleCode2))
                {
                    ModuleCode1 = ModuleCode1.Substring(0, 24);
                    ModuleCode2 = ModuleCode2.Substring(0, 24);
                }

                _collectTaskContext.TaskMsgOperator.SetPairText("虚拟条码1", ModuleCode1);
                _collectTaskContext.TaskMsgOperator.SetPairText("虚拟条码2", ModuleCode2);
                //string ModuleCode2 = data[tagName + $"\"ModuleCode2\""].ToString();
                string Shimnum1 = data[tagName + $"\"Shimnum1\""].ToString();
                string Shimnum2 = data[tagName + $"\"Shimnum1\""].ToString();

                string BlockMPA144b = data[tagName + $"\"4_4BlockMPA1\""].ToString();
                string BlockMPA244b = data[tagName + $"\"4_4BlockMPA2\""].ToString();
                string BlockTime144b = data[tagName + $"\"4_4BlockTime1\""].ToString();
                string BlockTime244b = data[tagName + $"\"4_4BlockTime2\""].ToString();

                string BlockMPA110 = data[tagName + $"\"10BlockMPA1\""].ToString();
                string BlockMPA210 = data[tagName + $"\"10BlockMPA2\""].ToString();
                string BlockTime110 = data[tagName + $"\"10BlockTime1\""].ToString();
                string BlockTime210 = data[tagName + $"\"10BlockTime2\""].ToString();

                string BlockL1 = data[tagName + $"\"BlockL1\""].ToString(); // 模组长度1
                string BlockL2 = data[tagName + $"\"BlockL2\""].ToString(); // 模组长度2

                string BlockL1DbCz = data[tagName + $"\"BlockL1_DB_CZ\""].ToString();
                string BlockL2DbCz = data[tagName + $"\"BlockL2_DB_CZ\""].ToString();

                string StandPressure1 = data[tagName + $"\"StandPressure1\""].ToString(); // 静置压力1
                string StandPressure2 = data[tagName + $"\"StandPressure2\""].ToString(); // 静置压力2

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    #region 获取实体
                    //初始化实体类
                    List<PasteBoxShimEntity> pasteBoxShimEntities = new List<PasteBoxShimEntity>();
                    pasteBoxShimEntities.Clear();
                    // 插入两个实体 模组1 这里长度用 BlockL1字段  BlockL2字段多余
                    PasteBoxShimEntity pasteBoxShim1 = new PasteBoxShimEntity();
                    pasteBoxShim1.ModuleCode1 = ModuleCode1;
                    pasteBoxShim1.ShimNum = Convert.ToDecimal(Shimnum1);
                    pasteBoxShim1.BlockMPA_4 = Convert.ToDecimal(BlockMPA144b);
                    pasteBoxShim1.BlockTime_4 = Convert.ToDecimal(BlockTime144b);
                    pasteBoxShim1.BlockMPA_10 = Convert.ToDecimal(BlockMPA110);
                    pasteBoxShim1.BlockTime_10 = Convert.ToDecimal(BlockTime110);
                    pasteBoxShim1.BlockL1 = Convert.ToDecimal(BlockL1);
                    pasteBoxShim1.StandPressure = Convert.ToDecimal(StandPressure1); // 静置压力1
                    pasteBoxShim1.BlockLDbCz = Convert.ToDecimal(BlockL1DbCz);

                    //上传追溯数据
                    // 插入两个实体 模组2
                    PasteBoxShimEntity pasteBoxShim2 = new PasteBoxShimEntity();
                    pasteBoxShim2.ModuleCode1 = ModuleCode2;
                    pasteBoxShim2.ShimNum = Convert.ToDecimal(Shimnum2);
                    pasteBoxShim2.BlockMPA_4 = Convert.ToDecimal(BlockMPA244b);
                    pasteBoxShim2.BlockTime_4 = Convert.ToDecimal(BlockTime244b);
                    pasteBoxShim2.BlockMPA_10 = Convert.ToDecimal(BlockMPA210);
                    pasteBoxShim2.BlockTime_10 = Convert.ToDecimal(BlockTime210);
                    pasteBoxShim2.BlockL1 = Convert.ToDecimal(BlockL2);
                    pasteBoxShim2.StandPressure = Convert.ToDecimal(StandPressure2); // 静置压力1
                    pasteBoxShim2.BlockLDbCz = Convert.ToDecimal(BlockL2DbCz);


                    pasteBoxShimEntities.Add(pasteBoxShim1);
                    pasteBoxShimEntities.Add(pasteBoxShim2);
                    #endregion

                    


                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        //验证数据和业务逻辑写在这个里面
                        do
                        {
                            #region ocv mes交互
                            var mesStatus = IniFileAPI.INIGetStringValue(@AppConfig.WebserviceiniPath, "MES状态", "MesStatus", "");
                            if (mesStatus == "ON") // ON 开
                            {
                                // 收数
                                try
                                {
                                    // 调用两次
                                    // 判断模组号不为空
                                    if (!string.IsNullOrEmpty(ModuleCode1) && !string.IsNullOrEmpty(ModuleCode2))
                                    {
                                        // 根据模组号A先获取Pn号和资源号
                                        HttpResponseResultModel<List<ModuleAndPnEntity>> pnResultA = GetModuleAndPnAync(ModuleCode1);
                                        if (pnResultA.BackResult.Count > 0)
                                        {
                                            pnA = pnResultA.BackResult[0].PnNo;
                                        }

                                        BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                        MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse1 = BlockCall.BlockDatacoll(@AppConfig.WebserviceiniPath, "Packing垫片收数", ModuleCode1, pasteBoxShim1, pnA);
                                        code = sfcResponse1.@return.code;
                                        msg = sfcResponse1.@return.message;

                                        if (!checkCode("箱体贴垫片", "箱体贴垫片收数接口A", code, msg, ModuleCode1))
                                        {
                                            _outStaionResult = false;
                                            _isOutStation = true;
                                            break;
                                        }
                                        else
                                        {
                                            _outStaionResult = true;
                                        }

                                        // 根据模组号B先获取Pn号和资源号
                                        HttpResponseResultModel<List<ModuleAndPnEntity>> pnResultB = GetModuleAndPnAync(ModuleCode2);
                                        if (pnResultB.BackResult.Count > 0)
                                        {
                                            pnB = pnResultB.BackResult[0].PnNo;
                                        }
                                        MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse2 = BlockCall.BlockDatacoll(@AppConfig.WebserviceiniPath, "Packing垫片收数", ModuleCode2, pasteBoxShim2, pnB);
                                        code = sfcResponse2.@return.code;
                                        msg = sfcResponse2.@return.message;

                                        if (!checkCode("箱体贴垫片", "箱体贴垫片收数接口B", code, msg, ModuleCode2))
                                        {
                                            _outStaionResult = false;
                                            _isOutStation = true;
                                            break;
                                        }
                                        else
                                        {
                                            _outStaionResult = true;
                                        }
                                    }
                                    else 
                                    {
                                        _collectTaskContext.TaskMsgOperator.SetPairText("箱体贴垫片模组码为空", "");
                                        _log.AddUserLog("箱体贴垫片", "箱体贴垫片", string.Format("箱体贴垫片模组码为空"));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                            #endregion

                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //保存出站结果

                        AddEntityAync(pasteBoxShimEntities);
                    }
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst,"", "箱体贴垫片");
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
        HttpResponseResultModel<string> AddEntityAync(List<PasteBoxShimEntity> pasteBoxShimEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/PasteBoxShim/InsertBatch",
                Data = pasteBoxShimEntity
            }).Result;
        }

        /// <summary>
        /// 调用api获取焊前寻址数据
        /// </summary>
        /// <param name="seekSiteEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<List<ModuleAndPnEntity>> GetModuleAndPnAync(string ModuleCode)
        {
            return _requestToHttpHelper.GetAsync<List<ModuleAndPnEntity>>(new HttpRequestModel
            {
                Host = Apihost,
                Path = $"/ModuleAndPn/GetList?ModuleCode=" + ModuleCode,
                Data = new ModuleAndPnEntity { ModuleCode = ModuleCode }
            }).Result;
        }

        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 接口返回信息方法
        /// </summary>
        /// <param name="siteName">站点名称</param>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="code">接口返回代码</param>
        /// <param name="msg">接口返回信息</param>
        public bool checkCode(string siteName, string interfaceName, int code, string msg, string sfc)
        {
            if (code == 0)
            {
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "调用成功", sfc);
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "调用成功,sfc:"+ sfc));
            }
            else if (code > 0)
            {
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "调用失败", sfc);
                _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "返回代码：", code.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "返回信息", msg);
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "异常，异常代码为{0}，返回信息为{1}，错误模组号为{2}", code.ToString(), msg, sfc));
                return false;
            }
            else if (code == -1)
            {
                _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "异常，请检查网络"));
                return false;
            }
            return true;
        }


    }
}
