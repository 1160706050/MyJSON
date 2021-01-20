using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyJSON
{
    partial class JSON
    {
        #region JSONData

        /// <summary>
        /// JSON数据
        /// </summary>
        private class Data
        {
            /// <summary>
            /// JSON文本(去除格式化)
            /// </summary>
            public string json;

            /// <summary>
            /// JSON文本下标索引
            /// </summary>
            public int index;

            /// <summary>
            /// JSON当前字符
            /// </summary>
            public char ReadChar => json[index];

            /// <summary>
            /// JSON构造器，初始JSON文本并去除格式化
            /// </summary>
            /// <param name="json">JSON文本</param>
            public Data(string json)
            {
                this.json = DelFormatJSON(json);
                this.index = 0;
            }

            /// <summary>
            /// 删除格式化JSON文本
            /// </summary>
            /// <returns>删除格式化后的JSON</returns>
            private string DelFormatJSON(string json)
            {
                string tmp_json = string.Empty;
                for (int i = 0; i < json.Length; i++)
                {
                    if (json[i] == '"')
                    {
                        tmp_json += '"';
                        i++;
                        for (; json[i] != '"'; ++i)
                            tmp_json += json[i];
                    }
                    tmp_json += new Regex("[ \t\r\n]").Replace(json[i].ToString(), "");
                }
                return tmp_json;
            }

        }

        /// <summary>
        /// JSON数据类型
        /// </summary>
        public enum JsonType { Object, Array, String, Number, Bool, Null, }

        /// <summary>
        /// JSON解析结构类
        /// </summary>
        public class JsonValue
        {
            /// <summary>
            /// JSON类型
            /// </summary>
            public JsonType Type { get; }

            /// <summary>
            /// JSON字符串
            /// </summary>
            public string String { get; set; }

            /// <summary>
            /// 值(Number,Bool,Null)
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// 是否格式化JSON数据,默认true
            /// </summary>
            private bool isFormat = true;

            /// <summary>
            /// 设置JSON已解析文本格式化输出,默认true
            /// </summary>
            public JsonValue SetIsFormat(bool value)
            {
                isFormat = value;
                return this;
            }

            /// <summary>
            /// JSON数组类型
            /// </summary>
            public List<JsonValue> Arrays = new List<JsonValue>();

            /// <summary>
            /// 字典类型
            /// </summary>
            private Dictionary<string, JsonValue> Dict = new Dictionary<string, JsonValue>();

            /// <summary>
            /// JSON对象类构造器
            /// </summary>
            /// <param name="type">JSON内置的JsonType类型</param>
            public JsonValue(JsonType type)
            {
                this.Type = type;
                this.Value = string.Empty;
            }

            /// <summary>
            /// JSON对象类构造器
            /// </summary>
            /// <param name="type">JSON内置的JsonType类型</param>
            /// <param name="value">值(Number,Bool,Null)</param>
            public JsonValue(JsonType type, object value)
            {
                this.Type = type;
                this.Value = value;
            }

            /// <summary>
            /// 嵌套递归获取JSON
            /// </summary>
            private string GetObjectString()
            {

                if (Type == JsonType.Object)
                {
                    string str = string.Empty;
                    foreach (string key1 in Dict.Keys)
                    {
                        str += string.Format("\"{0}\":{1},", key1, Dict[key1].GetObjectString());
                    }
                    return "{" + str.TrimEnd(new char[] { ',' }) + "}";
                }
                if (Type == JsonType.Array)
                {
                    string str = "";
                    foreach (JsonValue jsonValue in Arrays)
                    {
                        str += $"{jsonValue.GetObjectString()},";
                    }
                    return string.Format("[{0}]", str.TrimEnd(','));
                }
                if (Type == JsonType.String)
                {
                    return string.Format("\"{0}\"", String.ToString());
                }
                if (Type == JsonType.Number)
                {
                    return Value.ToString();
                }
                if (Type == JsonType.Bool)
                {
                    return Value.ToString().ToLower();
                }
                if (Type == JsonType.Null)
                {
                    return "null";
                }
                return null;
            }

            /// <summary>
            /// 获取JsonValue类型数组
            /// </summary>
            /// <returns>返回List数组的JsonValue类型</returns>
            public List<JsonValue> GetArray()
            {
                if (Type == JsonType.Array)
                {
                    return Arrays;
                }
                return null;
            }

            /// <summary>
            /// 获取JsonValue类型对象
            /// </summary>
            /// <returns>返回JsonValue类型对象</returns>
            public JsonValue GetObject()
            {
                if (Type == JsonType.Object)
                {
                    return this;
                }
                return null;
            }

            /// <summary>
            /// 索引访问器
            /// </summary>
            public JsonValue this[string key]
            {
                get
                {
                    if (Dict.ContainsKey(key))
                    {
                        return Dict[key];
                    }
                    return null;
                }
                set
                {
                    Dict[key] = value;
                }
            }

            /// <summary>
            /// 输出字符串类型文本
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                if (Type == JsonType.Object)
                {
                    if (isFormat)
                    {
                        return JSONTools.JsonFormat(GetObjectString());
                    }
                    return GetObjectString();
                }
                if (Type == JsonType.Array)
                {
                    if (isFormat)
                    {
                        return JSONTools.JsonFormat(GetObjectString());
                    }
                    return GetObjectString();
                }
                if (Type == JsonType.String)
                {
                    return String.ToString();
                }
                if (Type == JsonType.Number)
                {
                    return Value.ToString();
                }
                if (Type == JsonType.Bool)
                {
                    return Value.ToString().ToLower();
                }
                if (Type == JsonType.Null)
                {
                    return Value.ToString().ToLower();
                }
                return base.ToString();

            }
        }

        #endregion


    }



}
