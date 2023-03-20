using DCS.TASK.NET.ViewModel.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Config
{
    internal class SystemConfig
    {
        /// <summary>
        ///配置
        /// </summary>
        static ConfigFile _configFile = new ConfigFile("Config", "SystemConfig");

     

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public static async Task<TaskRootConfig> LoadConfigModelAsync()
        {
             return await Task.Run(() => 
            {
                TaskRootConfig taskRootConfig = null;

                try
                {
                    //加载配置字符串
                    string json = _configFile.ReadString();
                    //字符串不为空加载配置文件
                    if (!string.IsNullOrEmpty(json))
                    {
                        //将Json转换为模型
                        taskRootConfig = JsonConvert.DeserializeObject<TaskRootConfig>(json);
                    }
                }
                catch { }

                return taskRootConfig;
            });
        }

        /// <summary>
        /// 将树节点
        /// </summary>
        /// <param name="rootNode"></param>
        public static TaskRootConfig CreateConfigModel(BaseTreeViewModel rootNode)
        {
            List<BaseTreeViewModel> treeItems = new List<BaseTreeViewModel>();
            //添加根节点
            treeItems.Add(rootNode);
            //获取根节点
            var rootModel = rootNode.NodeModel as TaskRootConfig;
            //循环创建model
            while (treeItems.Count > 0)
            {
                //取出第一个数据
                var frontNode = treeItems[0];
                treeItems.RemoveAt(0);
                //清除数据
                ClearNodeData(frontNode);
                //循环遍历节点
                foreach (var node in frontNode.Children)
                {
                    if (node.Children.Count > 0)
                    {
                        treeItems.Add(node);
                    }
                    //添加节点
                    AddNode(frontNode, node);
                }
            }

            return rootModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskRootConfig"></param>
        /// <returns></returns>
        public static async Task<bool> SaveConfigAsync(TaskRootConfig taskRootConfig)
        {
            //保存数据
            return await Task.Run(() => 
            {
                bool result = false;
                try
                {
                    //将json写入文件
                    var json = JsonConvert.SerializeObject(taskRootConfig);
                    //写入文件
                    _configFile.WriteString(json);
                    //操作成功
                    result = true;
                }
                catch (Exception ex)
                { }

                return result;
            });
        }

        /// <summary>
        /// 清除节点配置数据
        /// </summary>
        private static void ClearNodeData(BaseTreeViewModel parentNode)
        {
            if (parentNode.NodeType == TaskTreeNodeType.TaskRoot)
            {
                var taskRootConfig = parentNode.NodeModel as TaskRootConfig;
                taskRootConfig.TaskDicrectories.Clear();
                }
            else if (parentNode.NodeType == TaskTreeNodeType.TaskDicrectory)
            {
                var taskRootConfig = parentNode.NodeModel as TaskDicrectory;
                taskRootConfig.TaskGroups.Clear();
            }
            else if (parentNode.NodeType == TaskTreeNodeType.TaskCollectGroup ||
                     parentNode.NodeType == TaskTreeNodeType.TaskTimerGroup)
            {
                var taskRootConfig = parentNode.NodeModel as TaskGroupInfo;
                taskRootConfig.CollectTasks.Clear();
            }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNode"></param>
        private static void AddNode(BaseTreeViewModel parentNode, BaseTreeViewModel childNode)
        {
            if (parentNode.NodeType == TaskTreeNodeType.TaskRoot)
            {
                var taskRootConfig = parentNode.NodeModel as TaskRootConfig;
                taskRootConfig.TaskDicrectories.Add(childNode.NodeModel as TaskDicrectory);
            }
            else if (parentNode.NodeType == TaskTreeNodeType.TaskDicrectory)
            {
                var taskRootConfig = parentNode.NodeModel as TaskDicrectory;
                taskRootConfig.TaskGroups.Add(childNode.NodeModel as TaskGroupInfo);
            }
            else if (parentNode.NodeType == TaskTreeNodeType.TaskCollectGroup ||
                     parentNode.NodeType == TaskTreeNodeType.TaskTimerGroup)
            {
                var taskRootConfig = parentNode.NodeModel as TaskGroupInfo;
                taskRootConfig.CollectTasks.Add(childNode.NodeModel as CollectTaskInfo);
            }
        }
    }


    class ConfigFile
    {
        public ConfigFile(string fileName, string directoryName)
        {
            _fileName = fileName;
            _directoryName = directoryName;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool WriteString(string json)
        { 
            bool result = false;

            string filePath = _directoryName + "\\";
            //目录是否存在
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            //文件路径
            filePath += _fileName + ".json";
            //打开文件
            FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate);

            if (fileStream != null)
            {
                //清空文件流
                fileStream.SetLength(0);
                //创建写入流
                StreamWriter streamWriter = new StreamWriter(fileStream);
                //写入字符串
                streamWriter.Write(json);
                streamWriter.Flush();
                //关闭文件流
                streamWriter.Close();
                fileStream.Close();
                //写入成功
                result = true;
            }

            return result;
        }

        /// <summary>
        ///读取文件
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            string json = string.Empty;
            //生成文件路径
            string filePath = $"{_directoryName}\\{_fileName}.json";
            //文件路径
            if (File.Exists(filePath))
            {
                json = File.ReadAllText(filePath, Encoding.UTF8);
            }

            return json;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        string _fileName;

        /// <summary>
        /// 目录名称
        /// </summary>
        string _directoryName;
    }
}
