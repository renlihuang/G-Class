using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using FastReport;
using MESwebservice.AttrDataEntry;
using MESwebservice.GetBarCodeInfo;
using MESwebservice.Mescall;
using MESwebservice.ModuleEntry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
/// <summary>
/// 贴模组标
/// </summary>
namespace DCS.TASKITEM.DataGather
{
    internal class PasteModuleGather : IPeriodicTask
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
        public PasteModuleGather(RequestToHttpHelper requestToHttpHelper)
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
        int heartstat;
        string pna;
        string pnb;

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
            //PasteBoxCall pasteBox11 = new PasteBoxCall(_collectTaskContext);
            //miGetSlotDataResponse slotDataResponseA11 = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌", "001MEAVN000002C7M0500047");
            //code = slotDataResponseA11.@return.code;
            //msg = slotDataResponseA11.@return.message;
            //if (slotDataResponseA11.@return.labelListArray != null)
            //{
            //    pna = slotDataResponseA11.@return.labelListArray[0].value;
            //}

            //PasteBoxCall pasteBox1 = new PasteBoxCall(_collectTaskContext);
            //miGetSlotDataResponse slotDataResponseA1 = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌", "001MEAVN000002C7J0500005");

            //001MEAVN000002C7H0500051

            //miFindCustomAndSfcDataResponse miFindCustomAndSfcData = PasteBoxCall.ShimEntry(@AppConfig.WebserviceiniPath, "贴模组标进站", "001MEAVN000002C7H0500051", "830100-02436");
            //MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponse1 = BlockCall.BlocksfcComplete(@AppConfig.WebserviceiniPath, "Block出站", "001MEAVN000002C7H0500051", "830100-02437");
            //PasteBoxCall pasteBoxCall = new PasteBoxCall(_collectTaskContext);
            //MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponseA = BlockCall.BlocksfcComplete(@AppConfig.WebserviceiniPath, "焊前寻址出站", "001MEAVN000002C7G0500020", "");
            //code = sfcCompleteResponseA.@return.code;
            //msg = sfcCompleteResponseA.@return.message;
            //string pna1 = "830100 - 02437";
            //string typem1 = "1P30S";
            //string marking1 = "TMC";
            //string ModuleCode1 = "001MEAVN000002C700500004";
            //if (!Mesprint(ModuleCode1, pna1, typem1, marking1))
            //{
            //    _collectTaskContext.TaskMsgOperator.SetPairText("打印出错", "请检查打印机");
            //}

            // Mesprint("001MEAVN000002C8H0500007", "830100-02437", "1P30S", "TMC");

            //调用2次 两个模组号
            //PasteBoxCall pasteBox1 = new PasteBoxCall(_collectTaskContext);
            //miGetSlotDataResponse slotDataResponseA1 = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌", "001MEAVN000002C7H0500054");
            //code = slotDataResponseA1.@return.code;
            //msg = slotDataResponseA1.@return.message;
            //if (slotDataResponseA1.@return.labelListArray != null)
            //{
            //    pna = slotDataResponseA1.@return.labelListArray[0].value;
            //}
            //Mesprint("001MEAVN000002C820500002", pna, "1P30S", "TMC");


            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();


            string tagName = "\"TMCModuleTagDB\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[tagName + $"\"Module_Code\""] = null;
            data[tagName + "dataStatus_Heart"] = null;
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
                    _log.AddUserLog("贴模组标", "贴模组标心跳", string.Format("贴模组标心跳写入失败"));
                }
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}
                _collectTaskContext.TaskMsgOperator.SetPairText("PLC标志位", data[tagName + "dataStatus_PLC"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("MES标志位", data[tagName + "dataStatus_MES"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("心跳值", heartstat.ToString());
                int PlcFalg = Convert.ToInt16(data[plcDataStatus]);
                int MesFalg = Convert.ToInt16(data[mesDataStatus]);
                //001MEAVN000002C7J0500003
               string ModuleCode = data[tagName + $"\"Module_Code\""].ToString();
                // string ModuleCode = "001MEAVN000002C7N0500002";

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    _log.AddUserLog("贴模组标", "贴模组标", string.Format("贴模组标触发采集信号PlcFalg="+ PlcFalg));
                    var mesStatus=string.Empty;
                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        //验证数据和业务逻辑写在这个里面
                        do
                        {
                            #region  ocv mes交互
                             mesStatus = IniFileAPI.INIGetStringValue(@AppConfig.WebserviceiniPath, "MES状态", "MesStatus", "");
                            if (mesStatus == "ON") // ON 开
                            {
                                if (string.IsNullOrEmpty(ModuleCode) )
                                {
                                    _collectTaskContext.TaskMsgOperator.SetPairText("提示信息", "模组条码为空，请检查数据");
                                    _outStaionResult = false;
                                    _isOutStation = true;
                                    break;
                                }
                                //// 获取打印信息
                                try
                                {
                                    string typem = "1P30S";
                                    string marking = "TMC";
                                    ////调用2次 两个模组号
                                    PasteBoxCall pasteBox = new PasteBoxCall(_collectTaskContext);
                                    miGetSlotDataResponse slotDataResponseA = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌", ModuleCode);
                                    code = slotDataResponseA.@return.code;
                                    msg = slotDataResponseA.@return.message;
                                    if (slotDataResponseA.@return.labelListArray != null)
                                    {
                                        pna = slotDataResponseA.@return.labelListArray[0].value;
                                    }
                                    if (!checkCode("贴模组标", "贴箱体标获取PN接口", code, msg, ModuleCode))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    else
                                    {
                                        _outStaionResult = true;
                                    }

                                    //进站
                                    //miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = PasteBoxCall.ShimEntry(@AppConfig.WebserviceiniPath, "贴模组标进站", ModuleCode, pna);
                                    //if (!checkCode("贴模组标", "贴模组标进站接口", code, msg, ModuleCode))
                                    //{
                                    //    _outStaionResult = false;
                                    //    _isOutStation = true;
                                    //    return;
                                    //}

                                    // 出站
                                    try
                                    {
                                        BlockCall blockCall = new BlockCall(_collectTaskContext);
                                        MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponseA = BlockCall.BlocksfcComplete(@AppConfig.WebserviceiniPath, "Block出站", ModuleCode, pna);
                                        code = sfcCompleteResponseA.@return.code;
                                        msg = sfcCompleteResponseA.@return.message;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                    if (!checkCode("贴模组标", "贴模组标出站接口", code, msg, ModuleCode))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    else
                                    {
                                        _outStaionResult = true;
                                    }

                                    if (!Mesprint(ModuleCode, pna, typem, marking))
                                    {
                                        _collectTaskContext.TaskMsgOperator.SetPairText("打印出错", "请检查打印机");
                                    }
                                    _log.AddUserLog("贴模组标", "贴模组标", string.Format(  "发送打印任务成功,模组号:" + ModuleCode));
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
                        // _outStaionResult = true;
                        //初始化实体类
                        PasteModuleEntity pasteModule = new PasteModuleEntity();
                        pasteModule.ModuleCode = ModuleCode;
                        //上传追溯数据
                        AddEntityAync(pasteModule);

                    }
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst, "", "贴模组标");
                    //根据是否出站成功来回写标准位
                    //mes关闭后默认出战结果是true
                    if (mesStatus == "OFF")
                    {
                        _outStaionResult = true; // 关闭接口后 _outStaionResult 赋值 MES状态赋值ON后注销
                    }
                    if (_outStaionResult)
                    {
                        writeResult = 1;
                    }
                    else
                    {
                        writeResult = 2;
                    }
                    writeTags[mesDataStatus] = Convert.ToInt16(writeResult);
                    _log.AddUserLog("贴模组标", "贴模组标", string.Format("贴模组标触发采集信号PlcFalg=" + writeResult));
                    //写入PLC变量
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("贴模组标", "贴模组标", string.Format("贴模组标resul写入失败"));
                    }

                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("贴模组标", "贴模组标", string.Format("贴模组标标志位写0失败"));
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
                            _log.AddUserLog("贴模组标", "贴模组标", string.Format("贴模组标标志位写4失败"));
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
        HttpResponseResultModel<string> AddEntityAync(PasteModuleEntity pasteModuleEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/PasteModule/insert",
                Data = pasteModuleEntity
            }).Result;
        }
        /// <summary>
        /// 打印方法
        /// </summary>
        /// <param name="sfc">模组号</param>
        /// <param name="pn">pn码</param>
        /// <param name="typem">typem</param>
        /// <param name="marking">marking</param>
        /// <returns></returns>
        public bool Mesprint(string sfc, string pn, string typem, string marking)
        {
            try
            {
                //标签模板文件
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Label", "Meslable.frx");
                //打印机
                var printer = "ZDesigner ZE500R-4 RH-300dpi ZPL";

                var report = new Report();
                report.Load(fileName);

                //设置模板文件中约定的参数
                //report.SetParameterValue("producer", "测试厂商");
                //report.SetParameterValue("customer", "华数锦明");
                //report.SetParameterValue("qty", "3600个");
                //report.SetParameterValue("barcode", DateTime.Now.ToString("yyyy-MM-dd") + new Random().Next(1000, 10000).ToString());
                //report.SetParameterValue("sfc", "001MEAVN000002C770300004");
                report.SetParameterValue("SFC", sfc);
                report.SetParameterValue("PN", pn);
                report.SetParameterValue("TYPEM", typem);
                report.SetParameterValue("MARKING", marking);

                //report.SetParameterValue("qty", "3600个");
                //report.SetParameterValue("barcode", DateTime.Now.ToString("yyyy-MM-dd") + new Random().Next(1000, 10000).ToString());

                //打印机配置
                report.PrintSettings.Printer = printer;
                report.PrintSettings.PageNumbers = "1";
                report.PrintSettings.ShowDialog = false;
                report.PrintSettings.PrintToFile = false;
                report.PrintSettings.PrintToFileName = string.Empty;
                //打印
                report.Print();
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            return true;
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
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "调用成功,模组号:"+sfc));
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
