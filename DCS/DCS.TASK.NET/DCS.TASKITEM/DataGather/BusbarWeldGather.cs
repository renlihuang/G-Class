using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.TASKITEM.DataGather
{
    internal class BusbarWeldGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        private static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        private static bool _outStaionResult = false;

        /// <summary>
        /// csvlist
        /// </summary>
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();

        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public BusbarWeldGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        private RequestToHttpHelper _requestToHttpHelper;

        //采集任务
        private static CollectTaskContext _collectTaskContext;

        //需要采集的参数，根据业务增加
        private string _plcDataStatus;

        private string _mesDataStatus;
        private string Apihost;

        //心跳变量
        private int heartstat;

        //日志帮助类
        private static ILogOperator _log;

        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;

            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            var opc = _collectTaskContext.OpcOperator;

            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();

            string tagName = "\"TMCBusbarDB\".";

            data[tagName + $"\"dataStatus_PLC\""] = null;
            data[tagName + $"\"dataStatus_MES\""] = null;
            data[tagName + $"\"dataStatus_Heart\""] = null;
            _mesDataStatus = tagName + $"\"dataStatus_MES\"";
            data[tagName + $"\"BusbarSpeed\""] = null;
            data[tagName + $"\"Phto_A_offset\""] = null;
            data[tagName + $"\"Phto_B_offset\""] = null;
            data[tagName + $"photo_1"] = null;
            data[tagName + $"photo_2"] = null;
            data[tagName + $"photo_3"] = null;
            data[tagName + $"photo_4"] = null;
            data[tagName + $"Mark"] = null;

            for (int i = 0; i < 6; i++)
            {
                data[tagName + $"\"setvalue_Group1\".pos{i + 1}"] = null;
            }
            for (int i = 0; i < 6; i++)
            {
                data[tagName + $"\"setvalue_Group2\".pos{i + 1}"] = null;
            }
            for (int i = 0; i < 6; i++)
            {
                data[tagName + $"\"setvalue_Group3\".pos{i + 1}"] = null;
            }
            for (int i = 0; i < 6; i++)
            {
                data[tagName + $"\"setvalue_Group4\".pos{i + 1}"] = null;
            }
            for (int i = 0; i < 6; i++)
            {
                data[tagName + $"\"setvalue_Group5\".pos{i + 1}"] = null;
            }

            for (int a = 0; a < 6; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"OutputPower_group{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"OutputPower_group{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"OutputPower_group{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"OutputPower_group{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"OutputPower_group{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"OutputPower_group{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }

            data[tagName + $"\"Module-acode\""] = null;
            data[tagName + $"\"Module-bcode\""] = null;
            data[tagName + $"\"CurrentUserName\""] = null;
            data[tagName + $"\"RFID\""] = null;

            for (int a = 0; a < 6; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"OutputPower_MAX{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"OutputPower_MAX{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"OutputPower_MAX{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"OutputPower_MAX{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"OutputPower_MAX{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"OutputPower_MAX{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }

            for (int a = 0; a < 6; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"OutputPower_MIN{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"OutputPower_MIN{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"OutputPower_MIN{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"OutputPower_MIN{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"OutputPower_MIN{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"OutputPower_MIN{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }

            if (opc.ReadNodes(data))
            {
                heartstat = Convert.ToInt16(data[tagName + $"\"dataStatus_Heart\""].ToString());
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
                    _log.AddUserLog("Busbar焊接", "Busbar焊接心跳", string.Format("Busbar焊接心跳写入失败"));
                }
                _collectTaskContext.TaskMsgOperator.SetPairText("心跳值", heartstat.ToString());
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}
                int PlcFalg = Convert.ToInt16(data[tagName + $"\"dataStatus_PLC\""].ToString());
                int MesFalg = Convert.ToInt16(data[tagName + $"\"dataStatus_MES\""].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_PLC\"".ToString(), PlcFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_MES\"".ToString(), MesFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("当前进度", MesFalg.ToString() == "1" ? "mes处理成功" : "等待plc值");
                string BusbarSpeed = data[tagName + $"\"BusbarSpeed\""].ToString();
                string Phto_A_offset = data[tagName + $"\"Phto_A_offset\""].ToString();
                string Phto_B_offset = data[tagName + $"\"Phto_B_offset\""].ToString();
                string photo1 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_1"])));
                string photo2 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_2"])));
                string photo3 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_3"])));
                string photo4 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_4"])));
                string Mark = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "Mark"])));
                string setvalue_Group1 = "";
                for (int i = 0; i < 6; i++)
                {
                    setvalue_Group1 += data[tagName + $"\"setvalue_Group1\".pos{i + 1}"].ToString() + ";";
                }
                string setvalue_Group2 = "";
                for (int i = 0; i < 6; i++)
                {
                    setvalue_Group2 += data[tagName + $"\"setvalue_Group2\".pos{i + 1}"].ToString() + ";";
                }
                string setvalue_Group3 = "";
                for (int i = 0; i < 6; i++)
                {
                    setvalue_Group3 += data[tagName + $"\"setvalue_Group3\".pos{i + 1}"].ToString() + ";";
                }
                string setvalue_Group4 = "";
                for (int i = 0; i < 6; i++)
                {
                    setvalue_Group4 += data[tagName + $"\"setvalue_Group4\".pos{i + 1}"].ToString() + ";";
                }
                string setvalue_Group5 = "";
                for (int i = 0; i < 6; i++)
                {
                    setvalue_Group5 += data[tagName + $"\"setvalue_Group5\".pos{i + 1}"].ToString() + ";";
                }

                #region OutputPower_group

                string OutputPower_group1 = "";
                string OutputPower_group1_pos1 = "";
                string OutputPower_group1_pos2 = "";
                string OutputPower_group1_pos3 = "";
                string OutputPower_group1_pos4 = "";
                string OutputPower_group1_pos5 = "";
                string OutputPower_group1_pos6 = "";
                string OutputPower_group2 = "";
                string OutputPower_group2_pos1 = "";
                string OutputPower_group2_pos2 = "";
                string OutputPower_group2_pos3 = "";
                string OutputPower_group2_pos4 = "";
                string OutputPower_group2_pos5 = "";
                string OutputPower_group2_pos6 = "";
                string OutputPower_group3 = "";
                string OutputPower_group3_pos1 = "";
                string OutputPower_group3_pos2 = "";
                string OutputPower_group3_pos3 = "";
                string OutputPower_group3_pos4 = "";
                string OutputPower_group3_pos5 = "";
                string OutputPower_group3_pos6 = "";
                string OutputPower_group4 = "";
                string OutputPower_group4_pos1 = "";
                string OutputPower_group4_pos2 = "";
                string OutputPower_group4_pos3 = "";
                string OutputPower_group4_pos4 = "";
                string OutputPower_group4_pos5 = "";
                string OutputPower_group4_pos6 = "";
                string OutputPower_group5 = "";
                string OutputPower_group5_pos1 = "";
                string OutputPower_group5_pos2 = "";
                string OutputPower_group5_pos3 = "";
                string OutputPower_group5_pos4 = "";
                string OutputPower_group5_pos5 = "";
                string OutputPower_group5_pos6 = "";
                string OutputPower_group6 = "";
                string OutputPower_group6_pos1 = "";
                string OutputPower_group6_pos2 = "";
                string OutputPower_group6_pos3 = "";
                string OutputPower_group6_pos4 = "";
                string OutputPower_group6_pos5 = "";
                string OutputPower_group6_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    OutputPower_group1_pos1 += data[tagName + $"\"OutputPower_group1\".pos1.hole{b + 1}"] + ";";
                    OutputPower_group1_pos2 += data[tagName + $"\"OutputPower_group1\".pos2.hole{b + 1}"] + ";";
                    OutputPower_group1_pos3 += data[tagName + $"\"OutputPower_group1\".pos3.hole{b + 1}"] + ";";
                    OutputPower_group1_pos4 += data[tagName + $"\"OutputPower_group1\".pos4.hole{b + 1}"] + ";";
                    OutputPower_group1_pos5 += data[tagName + $"\"OutputPower_group1\".pos5.hole{b + 1}"] + ";";
                    OutputPower_group1_pos6 += data[tagName + $"\"OutputPower_group1\".pos6.hole{b + 1}"] + ";";

                    OutputPower_group2_pos1 += data[tagName + $"\"OutputPower_group2\".pos1.hole{b + 1}"] + ";";
                    OutputPower_group2_pos2 += data[tagName + $"\"OutputPower_group2\".pos2.hole{b + 1}"] + ";";
                    OutputPower_group2_pos3 += data[tagName + $"\"OutputPower_group2\".pos3.hole{b + 1}"] + ";";
                    OutputPower_group2_pos4 += data[tagName + $"\"OutputPower_group2\".pos4.hole{b + 1}"] + ";";
                    OutputPower_group2_pos5 += data[tagName + $"\"OutputPower_group2\".pos5.hole{b + 1}"] + ";";
                    OutputPower_group2_pos6 += data[tagName + $"\"OutputPower_group2\".pos6.hole{b + 1}"] + ";";

                    OutputPower_group3_pos1 += data[tagName + $"\"OutputPower_group3\".pos1.hole{b + 1}"] + ";";
                    OutputPower_group3_pos2 += data[tagName + $"\"OutputPower_group3\".pos2.hole{b + 1}"] + ";";
                    OutputPower_group3_pos3 += data[tagName + $"\"OutputPower_group3\".pos3.hole{b + 1}"] + ";";
                    OutputPower_group3_pos4 += data[tagName + $"\"OutputPower_group3\".pos4.hole{b + 1}"] + ";";
                    OutputPower_group3_pos5 += data[tagName + $"\"OutputPower_group3\".pos5.hole{b + 1}"] + ";";
                    OutputPower_group3_pos6 += data[tagName + $"\"OutputPower_group3\".pos6.hole{b + 1}"] + ";";

                    OutputPower_group4_pos1 += data[tagName + $"\"OutputPower_group4\".pos1.hole{b + 1}"] + ";";
                    OutputPower_group4_pos2 += data[tagName + $"\"OutputPower_group4\".pos2.hole{b + 1}"] + ";";
                    OutputPower_group4_pos3 += data[tagName + $"\"OutputPower_group4\".pos3.hole{b + 1}"] + ";";
                    OutputPower_group4_pos4 += data[tagName + $"\"OutputPower_group4\".pos4.hole{b + 1}"] + ";";
                    OutputPower_group4_pos5 += data[tagName + $"\"OutputPower_group4\".pos5.hole{b + 1}"] + ";";
                    OutputPower_group4_pos6 += data[tagName + $"\"OutputPower_group4\".pos6.hole{b + 1}"] + ";";

                    OutputPower_group5_pos1 += data[tagName + $"\"OutputPower_group5\".pos1.hole{b + 1}"] + ";";
                    OutputPower_group5_pos2 += data[tagName + $"\"OutputPower_group5\".pos2.hole{b + 1}"] + ";";
                    OutputPower_group5_pos3 += data[tagName + $"\"OutputPower_group5\".pos3.hole{b + 1}"] + ";";
                    OutputPower_group5_pos4 += data[tagName + $"\"OutputPower_group5\".pos4.hole{b + 1}"] + ";";
                    OutputPower_group5_pos5 += data[tagName + $"\"OutputPower_group5\".pos5.hole{b + 1}"] + ";";
                    OutputPower_group5_pos6 += data[tagName + $"\"OutputPower_group5\".pos6.hole{b + 1}"] + ";";

                    OutputPower_group6_pos1 += data[tagName + $"\"OutputPower_group6\".pos1.hole{b + 1}"] + ";";
                    OutputPower_group6_pos2 += data[tagName + $"\"OutputPower_group6\".pos2.hole{b + 1}"] + ";";
                    OutputPower_group6_pos3 += data[tagName + $"\"OutputPower_group6\".pos3.hole{b + 1}"] + ";";
                    OutputPower_group6_pos4 += data[tagName + $"\"OutputPower_group6\".pos4.hole{b + 1}"] + ";";
                    OutputPower_group6_pos5 += data[tagName + $"\"OutputPower_group6\".pos5.hole{b + 1}"] + ";";
                    OutputPower_group6_pos6 += data[tagName + $"\"OutputPower_group6\".pos6.hole{b + 1}"] + ";";
                }
                OutputPower_group1 = OutputPower_group1_pos1 + OutputPower_group1_pos2
                    + OutputPower_group1_pos3 + OutputPower_group1_pos4 + OutputPower_group1_pos5
                    + OutputPower_group1_pos6;
                OutputPower_group2 = OutputPower_group2_pos1 + OutputPower_group2_pos2
                    + OutputPower_group2_pos3 + OutputPower_group2_pos4 + OutputPower_group2_pos5
                    + OutputPower_group2_pos6;
                OutputPower_group3 = OutputPower_group3_pos1 + OutputPower_group3_pos2
                    + OutputPower_group3_pos3 + OutputPower_group3_pos4 + OutputPower_group3_pos5
                    + OutputPower_group3_pos6;
                OutputPower_group4 = OutputPower_group4_pos1 + OutputPower_group4_pos2
                    + OutputPower_group4_pos3 + OutputPower_group4_pos4 + OutputPower_group4_pos5
                    + OutputPower_group4_pos6;
                OutputPower_group5 = OutputPower_group5_pos1 + OutputPower_group5_pos2
                    + OutputPower_group5_pos3 + OutputPower_group5_pos4 + OutputPower_group5_pos5
                    + OutputPower_group5_pos6;
                OutputPower_group6 = OutputPower_group6_pos1 + OutputPower_group6_pos2
                    + OutputPower_group6_pos3 + OutputPower_group6_pos4 + OutputPower_group6_pos5
                    + OutputPower_group6_pos6;

                #endregion OutputPower_group

                #region OutputPower_MAX

                string OutputPower_MAX1 = "";
                string OutputPower_MAX1_pos1 = "";
                string OutputPower_MAX1_pos2 = "";
                string OutputPower_MAX1_pos3 = "";
                string OutputPower_MAX1_pos4 = "";
                string OutputPower_MAX1_pos5 = "";
                string OutputPower_MAX1_pos6 = "";
                string OutputPower_MAX2 = "";
                string OutputPower_MAX2_pos1 = "";
                string OutputPower_MAX2_pos2 = "";
                string OutputPower_MAX2_pos3 = "";
                string OutputPower_MAX2_pos4 = "";
                string OutputPower_MAX2_pos5 = "";
                string OutputPower_MAX2_pos6 = "";
                string OutputPower_MAX3 = "";
                string OutputPower_MAX3_pos1 = "";
                string OutputPower_MAX3_pos2 = "";
                string OutputPower_MAX3_pos3 = "";
                string OutputPower_MAX3_pos4 = "";
                string OutputPower_MAX3_pos5 = "";
                string OutputPower_MAX3_pos6 = "";
                string OutputPower_MAX4 = "";
                string OutputPower_MAX4_pos1 = "";
                string OutputPower_MAX4_pos2 = "";
                string OutputPower_MAX4_pos3 = "";
                string OutputPower_MAX4_pos4 = "";
                string OutputPower_MAX4_pos5 = "";
                string OutputPower_MAX4_pos6 = "";
                string OutputPower_MAX5 = "";
                string OutputPower_MAX5_pos1 = "";
                string OutputPower_MAX5_pos2 = "";
                string OutputPower_MAX5_pos3 = "";
                string OutputPower_MAX5_pos4 = "";
                string OutputPower_MAX5_pos5 = "";
                string OutputPower_MAX5_pos6 = "";
                string OutputPower_MAX6 = "";
                string OutputPower_MAX6_pos1 = "";
                string OutputPower_MAX6_pos2 = "";
                string OutputPower_MAX6_pos3 = "";
                string OutputPower_MAX6_pos4 = "";
                string OutputPower_MAX6_pos5 = "";
                string OutputPower_MAX6_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    OutputPower_MAX1_pos1 += data[tagName + $"\"OutputPower_MAX1\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MAX1_pos2 += data[tagName + $"\"OutputPower_MAX1\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MAX1_pos3 += data[tagName + $"\"OutputPower_MAX1\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MAX1_pos4 += data[tagName + $"\"OutputPower_MAX1\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MAX1_pos5 += data[tagName + $"\"OutputPower_MAX1\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MAX1_pos6 += data[tagName + $"\"OutputPower_MAX1\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MAX2_pos1 += data[tagName + $"\"OutputPower_MAX2\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MAX2_pos2 += data[tagName + $"\"OutputPower_MAX2\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MAX2_pos3 += data[tagName + $"\"OutputPower_MAX2\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MAX2_pos4 += data[tagName + $"\"OutputPower_MAX2\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MAX2_pos5 += data[tagName + $"\"OutputPower_MAX2\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MAX2_pos6 += data[tagName + $"\"OutputPower_MAX2\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MAX3_pos1 += data[tagName + $"\"OutputPower_MAX3\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MAX3_pos2 += data[tagName + $"\"OutputPower_MAX3\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MAX3_pos3 += data[tagName + $"\"OutputPower_MAX3\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MAX3_pos4 += data[tagName + $"\"OutputPower_MAX3\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MAX3_pos5 += data[tagName + $"\"OutputPower_MAX3\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MAX3_pos6 += data[tagName + $"\"OutputPower_MAX3\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MAX4_pos1 += data[tagName + $"\"OutputPower_MAX4\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MAX4_pos2 += data[tagName + $"\"OutputPower_MAX4\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MAX4_pos3 += data[tagName + $"\"OutputPower_MAX4\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MAX4_pos4 += data[tagName + $"\"OutputPower_MAX4\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MAX4_pos5 += data[tagName + $"\"OutputPower_MAX4\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MAX4_pos6 += data[tagName + $"\"OutputPower_MAX4\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MAX5_pos1 += data[tagName + $"\"OutputPower_MAX5\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MAX5_pos2 += data[tagName + $"\"OutputPower_MAX5\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MAX5_pos3 += data[tagName + $"\"OutputPower_MAX5\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MAX5_pos4 += data[tagName + $"\"OutputPower_MAX5\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MAX5_pos5 += data[tagName + $"\"OutputPower_MAX5\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MAX5_pos6 += data[tagName + $"\"OutputPower_MAX5\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MAX6_pos1 += data[tagName + $"\"OutputPower_MAX6\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MAX6_pos2 += data[tagName + $"\"OutputPower_MAX6\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MAX6_pos3 += data[tagName + $"\"OutputPower_MAX6\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MAX6_pos4 += data[tagName + $"\"OutputPower_MAX6\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MAX6_pos5 += data[tagName + $"\"OutputPower_MAX6\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MAX6_pos6 += data[tagName + $"\"OutputPower_MAX6\".pos6.hole{b + 1}"] + ";";
                }
                OutputPower_MAX1 = OutputPower_MAX1_pos1 + OutputPower_MAX1_pos2
                    + OutputPower_MAX1_pos3 + OutputPower_MAX1_pos4 + OutputPower_MAX1_pos5
                    + OutputPower_MAX1_pos6;
                OutputPower_MAX2 = OutputPower_MAX2_pos1 + OutputPower_MAX2_pos2
                    + OutputPower_MAX2_pos3 + OutputPower_MAX2_pos4 + OutputPower_MAX2_pos5
                    + OutputPower_MAX2_pos6;
                OutputPower_MAX3 = OutputPower_MAX3_pos1 + OutputPower_MAX3_pos2
                    + OutputPower_MAX3_pos3 + OutputPower_MAX3_pos4 + OutputPower_MAX3_pos5
                    + OutputPower_MAX3_pos6;
                OutputPower_MAX4 = OutputPower_MAX4_pos1 + OutputPower_MAX4_pos2
                    + OutputPower_MAX4_pos3 + OutputPower_MAX4_pos4 + OutputPower_MAX4_pos5
                    + OutputPower_MAX4_pos6;
                OutputPower_MAX5 = OutputPower_MAX5_pos1 + OutputPower_MAX5_pos2
                    + OutputPower_MAX5_pos3 + OutputPower_MAX5_pos4 + OutputPower_MAX5_pos5
                    + OutputPower_MAX5_pos6;
                OutputPower_MAX6 = OutputPower_MAX6_pos1 + OutputPower_MAX6_pos2
                    + OutputPower_MAX6_pos3 + OutputPower_MAX6_pos4 + OutputPower_MAX6_pos5
                    + OutputPower_MAX6_pos6;

                #endregion OutputPower_MAX

                #region OutputPower_MIN

                string OutputPower_MIN1 = "";
                string OutputPower_MIN1_pos1 = "";
                string OutputPower_MIN1_pos2 = "";
                string OutputPower_MIN1_pos3 = "";
                string OutputPower_MIN1_pos4 = "";
                string OutputPower_MIN1_pos5 = "";
                string OutputPower_MIN1_pos6 = "";
                string OutputPower_MIN2 = "";
                string OutputPower_MIN2_pos1 = "";
                string OutputPower_MIN2_pos2 = "";
                string OutputPower_MIN2_pos3 = "";
                string OutputPower_MIN2_pos4 = "";
                string OutputPower_MIN2_pos5 = "";
                string OutputPower_MIN2_pos6 = "";
                string OutputPower_MIN3 = "";
                string OutputPower_MIN3_pos1 = "";
                string OutputPower_MIN3_pos2 = "";
                string OutputPower_MIN3_pos3 = "";
                string OutputPower_MIN3_pos4 = "";
                string OutputPower_MIN3_pos5 = "";
                string OutputPower_MIN3_pos6 = "";
                string OutputPower_MIN4 = "";
                string OutputPower_MIN4_pos1 = "";
                string OutputPower_MIN4_pos2 = "";
                string OutputPower_MIN4_pos3 = "";
                string OutputPower_MIN4_pos4 = "";
                string OutputPower_MIN4_pos5 = "";
                string OutputPower_MIN4_pos6 = "";
                string OutputPower_MIN5 = "";
                string OutputPower_MIN5_pos1 = "";
                string OutputPower_MIN5_pos2 = "";
                string OutputPower_MIN5_pos3 = "";
                string OutputPower_MIN5_pos4 = "";
                string OutputPower_MIN5_pos5 = "";
                string OutputPower_MIN5_pos6 = "";
                string OutputPower_MIN6 = "";
                string OutputPower_MIN6_pos1 = "";
                string OutputPower_MIN6_pos2 = "";
                string OutputPower_MIN6_pos3 = "";
                string OutputPower_MIN6_pos4 = "";
                string OutputPower_MIN6_pos5 = "";
                string OutputPower_MIN6_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    OutputPower_MIN1_pos1 += data[tagName + $"\"OutputPower_MIN1\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MIN1_pos2 += data[tagName + $"\"OutputPower_MIN1\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MIN1_pos3 += data[tagName + $"\"OutputPower_MIN1\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MIN1_pos4 += data[tagName + $"\"OutputPower_MIN1\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MIN1_pos5 += data[tagName + $"\"OutputPower_MIN1\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MIN1_pos6 += data[tagName + $"\"OutputPower_MIN1\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MIN2_pos1 += data[tagName + $"\"OutputPower_MIN2\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MIN2_pos2 += data[tagName + $"\"OutputPower_MIN2\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MIN2_pos3 += data[tagName + $"\"OutputPower_MIN2\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MIN2_pos4 += data[tagName + $"\"OutputPower_MIN2\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MIN2_pos5 += data[tagName + $"\"OutputPower_MIN2\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MIN2_pos6 += data[tagName + $"\"OutputPower_MIN2\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MIN3_pos1 += data[tagName + $"\"OutputPower_MIN3\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MIN3_pos2 += data[tagName + $"\"OutputPower_MIN3\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MIN3_pos3 += data[tagName + $"\"OutputPower_MIN3\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MIN3_pos4 += data[tagName + $"\"OutputPower_MIN3\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MIN3_pos5 += data[tagName + $"\"OutputPower_MIN3\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MIN3_pos6 += data[tagName + $"\"OutputPower_MIN3\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MIN4_pos1 += data[tagName + $"\"OutputPower_MIN4\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MIN4_pos2 += data[tagName + $"\"OutputPower_MIN4\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MIN4_pos3 += data[tagName + $"\"OutputPower_MIN4\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MIN4_pos4 += data[tagName + $"\"OutputPower_MIN4\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MIN4_pos5 += data[tagName + $"\"OutputPower_MIN4\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MIN4_pos6 += data[tagName + $"\"OutputPower_MIN4\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MIN5_pos1 += data[tagName + $"\"OutputPower_MIN5\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MIN5_pos2 += data[tagName + $"\"OutputPower_MIN5\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MIN5_pos3 += data[tagName + $"\"OutputPower_MIN5\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MIN5_pos4 += data[tagName + $"\"OutputPower_MIN5\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MIN5_pos5 += data[tagName + $"\"OutputPower_MIN5\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MIN5_pos6 += data[tagName + $"\"OutputPower_MIN5\".pos6.hole{b + 1}"] + ";";

                    OutputPower_MIN6_pos1 += data[tagName + $"\"OutputPower_MIN6\".pos1.hole{b + 1}"] + ";";
                    OutputPower_MIN6_pos2 += data[tagName + $"\"OutputPower_MIN6\".pos2.hole{b + 1}"] + ";";
                    OutputPower_MIN6_pos3 += data[tagName + $"\"OutputPower_MIN6\".pos3.hole{b + 1}"] + ";";
                    OutputPower_MIN6_pos4 += data[tagName + $"\"OutputPower_MIN6\".pos4.hole{b + 1}"] + ";";
                    OutputPower_MIN6_pos5 += data[tagName + $"\"OutputPower_MIN6\".pos5.hole{b + 1}"] + ";";
                    OutputPower_MIN6_pos6 += data[tagName + $"\"OutputPower_MIN6\".pos6.hole{b + 1}"] + ";";
                }
                OutputPower_MIN1 = OutputPower_MIN1_pos1 + OutputPower_MIN1_pos2
                    + OutputPower_MIN1_pos3 + OutputPower_MIN1_pos4 + OutputPower_MIN1_pos5
                    + OutputPower_MIN1_pos6;
                OutputPower_MIN2 = OutputPower_MIN2_pos1 + OutputPower_MIN2_pos2
                    + OutputPower_MIN2_pos3 + OutputPower_MIN2_pos4 + OutputPower_MIN2_pos5
                    + OutputPower_MIN2_pos6;
                OutputPower_MIN3 = OutputPower_MIN3_pos1 + OutputPower_MIN3_pos2
                    + OutputPower_MIN3_pos3 + OutputPower_MIN3_pos4 + OutputPower_MIN3_pos5
                    + OutputPower_MIN3_pos6;
                OutputPower_MIN4 = OutputPower_MIN4_pos1 + OutputPower_MIN4_pos2
                    + OutputPower_MIN4_pos3 + OutputPower_MIN4_pos4 + OutputPower_MIN4_pos5
                    + OutputPower_MIN4_pos6;
                OutputPower_MIN5 = OutputPower_MIN5_pos1 + OutputPower_MIN5_pos2
                    + OutputPower_MIN5_pos3 + OutputPower_MIN5_pos4 + OutputPower_MIN5_pos5
                    + OutputPower_MIN5_pos6;
                OutputPower_MIN6 = OutputPower_MIN6_pos1 + OutputPower_MIN6_pos2
                    + OutputPower_MIN6_pos3 + OutputPower_MIN6_pos4 + OutputPower_MIN6_pos5
                    + OutputPower_MAX6_pos6;

                #endregion OutputPower_MIN

                string Module_acode = data[tagName + $"\"Module-acode\""].ToString();
                string Module_bcode = data[tagName + $"\"Module-bcode\""].ToString();
                string CurrentUserName = data[tagName + $"\"CurrentUserName\""].ToString();
                string rfid = data[tagName + $"\"RFID\""].ToString();

                string Setvalue_Group1 = string.Empty;
                string Setvalue_Group2 = string.Empty;
                string Setvalue_Group3 = string.Empty;
                string Setvalue_Group4 = string.Empty;
                string Setvalue_Group5 = string.Empty;
                for (int i = 0; i < 6; i++)
                {
                    Setvalue_Group1 += data[tagName + $"\"setvalue_Group1\".pos{i + 1}"].ToString() + ";";
                    Setvalue_Group2 += data[tagName + $"\"setvalue_Group2\".pos{i + 1}"].ToString() + ";";
                    Setvalue_Group3 += data[tagName + $"\"setvalue_Group3\".pos{i + 1}"].ToString() + ";";
                    Setvalue_Group4 += data[tagName + $"\"setvalue_Group4\".pos{i + 1}"].ToString() + ";";
                    Setvalue_Group5 += data[tagName + $"\"setvalue_Group5\".pos{i + 1}"].ToString() + ";";
                }
                Setvalue_Group1 = Setvalue_Group1.Substring(0, Setvalue_Group1.Length - 1);
                Setvalue_Group2 = Setvalue_Group2.Substring(0, Setvalue_Group2.Length - 1);
                Setvalue_Group3 = Setvalue_Group3.Substring(0, Setvalue_Group3.Length - 1);
                Setvalue_Group4 = Setvalue_Group4.Substring(0, Setvalue_Group4.Length - 1);
                Setvalue_Group5 = Setvalue_Group5.Substring(0, Setvalue_Group5.Length - 1);

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        //验证数据和业务逻辑写在这个里面
                        do
                        {
                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //保存出站结果
                        _outStaionResult = true;
                        //初始化实体类
                        dicLst.Clear();
                        dicLst.Add(data);
                        _log.ToCSVData(dicLst, "", "Busbar焊接");
                        BusbarWeldEntity busbar = new BusbarWeldEntity();
                        busbar.BusbarSpeed = BusbarSpeed;
                        busbar.Phto_A_offset = Phto_A_offset;
                        busbar.Phto_B_offset = Phto_B_offset;
                        busbar.Photo1 = photo1;
                        busbar.Photo2 = photo2;
                        busbar.Photo3 = photo3;
                        busbar.Photo4 = photo4;
                        busbar.Mark = Mark;
                        busbar.Setvalue_Group1 = Setvalue_Group1;
                        busbar.Setvalue_Group2 = Setvalue_Group2;
                        busbar.Setvalue_Group3 = Setvalue_Group3;
                        busbar.Setvalue_Group4 = Setvalue_Group4;
                        busbar.Setvalue_Group5 = Setvalue_Group5;
                        //busbar.Module_A = Module_A;
                        //busbar.Module_B = Module_B;
                        busbar.OutputPower_group1 = OutputPower_group1;
                        busbar.OutputPower_group2 = OutputPower_group2;
                        busbar.OutputPower_group3 = OutputPower_group3;
                        busbar.OutputPower_group4 = OutputPower_group4;
                        busbar.OutputPower_group5 = OutputPower_group5;
                        busbar.OutputPower_group6 = OutputPower_group6;
                        busbar.Module_acode = Module_acode;
                        busbar.Module_bcode = Module_bcode;
                        busbar.CurrentUserName = CurrentUserName;
                        busbar.RFID = rfid;
                        busbar.OutputPower_MAX1 = OutputPower_MAX1;
                        busbar.OutputPower_MAX2 = OutputPower_MAX2;
                        busbar.OutputPower_MAX3 = OutputPower_MAX3;
                        busbar.OutputPower_MAX4 = OutputPower_MAX4;
                        busbar.OutputPower_MAX5 = OutputPower_MAX5;
                        busbar.OutputPower_MAX6 = OutputPower_MAX6;
                        busbar.OutputPower_MIN1 = OutputPower_MIN1;
                        busbar.OutputPower_MIN2 = OutputPower_MIN2;
                        busbar.OutputPower_MIN3 = OutputPower_MIN3;
                        busbar.OutputPower_MIN4 = OutputPower_MIN4;
                        busbar.OutputPower_MIN5 = OutputPower_MIN5;
                        busbar.OutputPower_MIN6 = OutputPower_MIN6;
                        //上传追溯数据
                        HttpResponseResultModel<BusbarWeldEntity> result = AddEntityAync(busbar);
                        string s = busbar.ToJson();
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", s);
                    }
                    //根据是否出站成功来回写标准位
                    if (_outStaionResult)
                    {
                        writeResult = 1;
                    }
                    else
                    {
                        writeResult = 2;
                    }
                    writeTags[_mesDataStatus] = Convert.ToInt16(writeResult);
                    //写入PLC变量
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接resul写入失败"));
                    }
                    else
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接resul写入2失败"));
                        }
                        else
                        {
                            //标记结束出站
                            _isOutStation = false;
                        }
                    }
                }
                if (PlcFalg == 3)
                {
                    _isOutStation = false;

                    writeTags[_mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写4失败"));
                        }
                    }
                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[_mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写0失败"));
                    }
                    _isOutStation = false;
                }
                bool isget = MesFalg == 8 || MesFalg == 0;
                //busbar写入数据相关
                if (PlcFalg == 5 && isget)
                {
                    short writeResult = -1;
                    //模组号可以上面取一下 MoudlecodeA = Module_acode, MoudlecodeB = Module_bcode
                    HttpResponseResultModel<List<SeekSiteEntity>> weldData = GetDataAync(new SeekSiteEntity { MoudlecodeA = Module_acode, MoudlecodeB = Module_bcode });
                    List<SeekSiteEntity> weldDatas = weldData.BackResult;
                    if (weldDatas.Count > 0)
                    {
                        char[] cPhoto_1 = weldDatas[0].Photo_1.ToCharArray();
                        char[] cPhoto_2 = weldDatas[0].Photo_2.ToCharArray();
                        char[] cPhoto_3 = weldDatas[0].Photo_3.ToCharArray();
                        char[] cPhoto_4 = weldDatas[0].Photo_4.ToCharArray();
                        char[] cMark = weldDatas[0].Mark.ToCharArray();
                        for (int i = 0; i < cPhoto_1.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_1\"[{i}]"] = Convert.ToByte(cPhoto_1[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        writeTags.Clear();

                        for (int i = 0; i < cPhoto_2.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_2\"[{i}]"] = Convert.ToByte(cPhoto_2[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        writeTags.Clear();
                        for (int i = 0; i < cPhoto_3.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_3\"[{i}]"] = Convert.ToByte(cPhoto_3[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        writeTags.Clear();
                        for (int i = 0; i < cPhoto_4.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_4\"[{i}]"] = Convert.ToByte(cPhoto_4[i]);
                        }
                        for (int i = 0; i < cMark.Length; i++)
                        {
                            writeTags[tagName + $"\"Mark\"[{i}]"] = Convert.ToByte(cMark[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        else
                        {
                            //标记结束出站
                            _isOutStation = false;
                        }
                        writeTags.Clear();
                    }
                    else
                    {
                        _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("焊钳寻址数据获取失败,请检查模组码是否正确!"));
                    }
                    //表示已经调用过出战
                    _isOutStation = true;
                    //保存出站结果
                    _outStaionResult = true;

                    //根据是否出站成功来回写标准位
                    if (_outStaionResult)
                    {
                        writeResult = 5;
                    }
                    else
                    {
                        writeResult = 6;
                    }
                    writeTags[_mesDataStatus] = Convert.ToInt16(writeResult);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写入" + writeResult + "失败"));
                    }
                }
                if (PlcFalg == 7)
                {
                    _isOutStation = false;

                    writeTags[_mesDataStatus] = Convert.ToInt16(8);
                    if (MesFalg != 8)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写8失败"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="busbarWeldEntity"></param>
        /// <returns></returns>
        private HttpResponseResultModel<BusbarWeldEntity> AddEntityAync(BusbarWeldEntity busbarWeldEntity)
        {
            return _requestToHttpHelper.PostAsync<BusbarWeldEntity>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/BusbarWeld/insert",
                Data = busbarWeldEntity
            }).Result;
        }

        /// <summary>
        /// 调用api获取焊前寻址数据
        /// </summary>
        /// <param name="seekSiteEntity"></param>
        /// <returns></returns>
        private HttpResponseResultModel<List<SeekSiteEntity>> GetDataAync(SeekSiteEntity seekSiteEntity)
        {
            return _requestToHttpHelper.GetAsync<List<SeekSiteEntity>>(new HttpRequestModel
            {
                Host = Apihost,
                Path = $"/SeekSite/GetList?MoudlecodeA=" + seekSiteEntity.MoudlecodeA + "&MoudlecodeB=" + seekSiteEntity.MoudlecodeB,
                Data = seekSiteEntity
            }).Result;
        }

        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}