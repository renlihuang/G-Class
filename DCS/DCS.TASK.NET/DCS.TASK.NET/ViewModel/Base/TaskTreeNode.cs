using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.Base
{
    /// <summary>
    /// 创建节点工厂
    /// </summary>
    class TreeNodeFactory
    {
        /// <summary>
        /// 是否是系统管理员
        /// </summary>
        public static bool IsManage { get; set; }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static BaseTreeViewModel Create(TaskTreeNodeType nodeType, string nodeName, BaseTreeViewModel parentNode = null)
        {
            BaseTreeViewModel treeNode = null;
            //状态图标
            string statusIcon = string.Empty;
            //节点图标
            string nodeIcon = string.Empty;
            //是否可见
            bool isVisible = false;

            switch (nodeType)
            {
                case TaskTreeNodeType.TaskDicrectory:
                case TaskTreeNodeType.TaskRoot:
                case TaskTreeNodeType.TaskTimerGroup:
                    {
                        if (nodeType == TaskTreeNodeType.TaskRoot)
                        {
                            nodeIcon = statusIcon = "DesktopMacDashboard";
                        }

                        if (nodeType == TaskTreeNodeType.TaskDicrectory)
                        {
                            nodeIcon = statusIcon = "TextBoxOutline";
                        }

                        if (nodeType == TaskTreeNodeType.TaskTimerGroup)
                        {
                            nodeIcon = statusIcon = "StoreTime";
                        }
                    }
                    break;

                case TaskTreeNodeType.TaskCollectGroup:
                case TaskTreeNodeType.TaskCollectItem:
                case TaskTreeNodeType.TaskTimerItem:
                    {
                        isVisible = true;

                        if (nodeType == TaskTreeNodeType.TaskCollectGroup)
                        {
                            nodeIcon = "TelevisionClean";
                        }
                        else
                        {
                            nodeIcon = "WeatherTime";
                        }
                    }
                    break;
            }


            //创建节点
            treeNode = new BaseTreeViewModel()
            {
                Name = nodeName,
                StatusIcon = statusIcon,
                NodeIcon = nodeIcon,
                ParentNode = parentNode,
                NodeType = nodeType,
                StatusIconVisible = isVisible,
                ToolBarIsVisible = !IsManage
            };

            treeNode.Status = false;

            return treeNode;
        }
    }
}
