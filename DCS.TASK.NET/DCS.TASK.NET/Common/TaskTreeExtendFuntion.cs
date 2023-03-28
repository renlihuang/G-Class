using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{
    /// <summary>
    /// 树节点查找
    /// </summary>
    static class TaskTreeExtendFuntion
    {

        /// <summary>
        /// 查找任务节点
        /// </summary>
        /// <param name="baseTree"></param>
        /// <returns></returns>
        private  static bool IsTaskItem(BaseTreeViewModel baseTree)
        { 
            return (baseTree.NodeType == TaskTreeNodeType.TaskCollectItem ||
                    baseTree.NodeType == TaskTreeNodeType.TaskTimerItem);
        }

        /// <summary>
        /// 查找任务节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<BaseTreeViewModel> FindTaskItems(this BaseTreeViewModel node)
        {
            return FindTaskTreeItems(node, IsTaskItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<BaseTreeViewModel> FindTaskTreeItems(this BaseTreeViewModel node, Func<BaseTreeViewModel, bool> func)
        {
            //要返回的节点
            List<BaseTreeViewModel> resultList = new List<BaseTreeViewModel>();
            //节点队列
            List<BaseTreeViewModel> treeNodes = new List<BaseTreeViewModel>();
            //保存根节点
            treeNodes.Add(node);
            //是否已经查找到
            bool isFind = false;

            while (treeNodes.Count > 0)
            {
                //获取第一个节点
                var frontNode = treeNodes[0];
                treeNodes.RemoveAt(0);
                //循环查找节点
                foreach (var nodeItem in frontNode.Children)
                {
                    //查找
                    if (func(nodeItem))
                    {
                        resultList.Add(nodeItem);
                    }
                    //节点大于0
                    if (nodeItem.Children.Count > 0)
                    {
                        treeNodes.Add(nodeItem);
                    }
                }
                //查找到退出循环
                if (isFind)
                {
                    break;
                }
            }


            return resultList;
        }

        /// <summary>
        /// 按照查找
        /// </summary>
        /// <param name="baseTree"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        static bool FindByItemData(BaseTreeViewModel baseTree, object item)
        {
            //查找
            return baseTree.NodeParam == item && baseTree.NodeType == TaskTreeNodeType.TaskCollectGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemData"></param>
        /// <returns></returns>
        public static BaseTreeViewModel FindTaskTreeNodeByItemData(this BaseTreeViewModel node, object ItemData)
        {
            return FindTaskTreeNode(node, FindByItemData, ItemData);
        }


        /// <summary>
        /// 按条件查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="func"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static BaseTreeViewModel FindTaskTreeNode(this BaseTreeViewModel node,Func<BaseTreeViewModel, object, bool> func, object item)
        {
            //要返回的节点
            BaseTreeViewModel baseTree = null;
            //节点集合
            List<BaseTreeViewModel> treeNodes = new List<BaseTreeViewModel>();
            //保存根节点
            treeNodes.Add(node);
            //是否已经查找到
            bool isFind = false;

            while (treeNodes.Count > 0)
            {
                //获取第一个节点
                var frontNode = treeNodes[0];
                treeNodes.RemoveAt(0);
                //循环查找节点
                foreach (var treeNode in frontNode.Children)
                {
                    //查找
                    if (func(treeNode, item))
                    {
                        baseTree = treeNode;
                        //设置已经查找到
                        isFind = true;
                        break;
                    }
                    //节点大于0
                    if (treeNode.Children.Count > 0)
                    {
                        treeNodes.Add(treeNode);
                    }
                }
                //查找到退出循环
                if (isFind)
                {
                    break;
                }
            }

          
            
            return baseTree;
        }
    }
}
