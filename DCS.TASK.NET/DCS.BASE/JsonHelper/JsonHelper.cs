using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace DCS.BASE
{
    /// <summary>
    /// json帮助类，序列化，反序列化
    /// </summary>
    public static class JsonHelper
    {



        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T message)
        {

            //Newtonsoft.Json的用法
            var str_Message = message is string ? message.ToString() : JsonConvert.SerializeObject(message, Formatting.None,
                 new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return str_Message;


        }

        /// <summary>
        /// 对象转JSON字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T message)
        {

            //Newtonsoft.Json的用法
            var str_Message = message is string ? message.ToString() : JsonConvert.SerializeObject(message, Formatting.None,
                 new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return str_Message;
        }

        /// <summary>
        /// 字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string message)
        {
            //Newtonsoft.Json的用法
            var obj = JsonConvert.DeserializeObject<T>(message);
            return obj;



        }

        public static string JsonParse(JObject json)
        {
            string jsonString = json.ToString(Formatting.None);
            return jsonString;
        }

        public static JObject JsonMerge(JObject jObject1, params JObject[] jsons)
        {
            foreach (var json in jsons)
            {
                if (json != null)
                {
                    jObject1.Merge(json, new JsonMergeSettings() { MergeNullValueHandling = MergeNullValueHandling.Merge, MergeArrayHandling = MergeArrayHandling.Concat });
                }
            }
            return jObject1;
        }

        public static JObject JsonMerge(JObject jObject1, params string[] jsons)
        {
            foreach (var jsonString in jsons)
            {
                if (!string.IsNullOrEmpty(jsonString))
                {
                    JObject json = JObject.Parse(jsonString);
                    jObject1.Merge(json, new JsonMergeSettings() { MergeNullValueHandling = MergeNullValueHandling.Merge, MergeArrayHandling = MergeArrayHandling.Concat });
                }
            }
            return jObject1;
        }

        public static JObject JsonMerge(string jsonString, params string[] jsons)
        {
            var jObject= JObject.Parse(jsonString);
            foreach (var json in jsons)
            {
                if (json != null)
                {
                    JObject jObject1 = JObject.Parse(json);
                    jObject.Merge(jObject1, new JsonMergeSettings() { MergeNullValueHandling = MergeNullValueHandling.Merge, MergeArrayHandling = MergeArrayHandling.Concat });
                }
            }
            return jObject;
        }
    }
}
