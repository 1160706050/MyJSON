using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyJSON
{

    /// <summary>
    /// JSON数据类型
    /// </summary>
    public enum JsonType { Object, Array, String, Number, Bool, Null, }

    /// <summary>
    /// JSON解析结构类
    /// </summary>
    public class JsonValue
    {
        #region 公开字段

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
        /// JSON数组类型
        /// </summary>
        public List<JsonValue> Arrays { get; set; } = new List<JsonValue>();

        /// <summary>
        /// 字典类型
        /// </summary>
        public Dictionary<string, JsonValue> Dict { get; set; } = new Dictionary<string, JsonValue>();

        /// <summary>
        /// 是否格式化JSON数据,默认true
        /// </summary>
        public bool IsFormat { get; set; } = false;

        /// <summary>
        /// JSON树等级
        /// </summary>
        public int TreeLevel { get; set; } = 0;

        #endregion

        #region 构造器

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
        public JsonValue(JsonType type, bool IsFormat, int TreeLevel)
        {
            this.Type = type;
            this.IsFormat = IsFormat;
            this.TreeLevel = TreeLevel;
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
            this.IsFormat = IsFormat;
            this.TreeLevel = TreeLevel;
            this.Value = value;
        }

        #endregion

        #region 公开方法

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
        /// 设置JSON已解析文本格式化输出
        /// </summary>
        public JsonValue SetIsFormat(bool value)
        {
            IsFormat = value;
            return this;
        }

        /// <summary>
        /// 设置JSON已解析文本格式化输出
        /// </summary>
        public JsonValue SetTreeLevel(int value)
        {
            this.TreeLevel = value;
            return this;
        }
        /// <summary>
        /// 嵌套递归获取JSON，输出字符串类型文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Type == JsonType.Object)
            {
                TreeLevel++;
                string str = string.Empty;
                for (int i = 0; i < Dict.Count; i++)
                {
                    KeyValuePair<string, JsonValue> dictObj = Dict.ElementAt(i);
                    if (IsFormat)
                    {
                        str += JSONTools.TreeLevel(TreeLevel);
                    }
                    if (dictObj.Value == null)
                    {
                        str += "\"" + dictObj.Key + "\":";
                        if (IsFormat)
                        {
                            str += " ";
                        }
                        str += "null";
                    }
                    else
                    {
                        str += "\"" + dictObj.Key + "\":";
                        if (IsFormat)
                        {
                            str += " ";
                        }
                        str += dictObj.Value.SetTreeLevel(TreeLevel);
                    }
                    if (i < (Dict.Count - 1))
                    {
                        str += ",";
                        if (IsFormat)
                        {
                            str += "\n";
                        }
                    }
                }
                string endString = "{";
                if (IsFormat)
                {
                    endString += "\n";
                    endString += str;
                    endString += "\n" + JSONTools.TreeLevel(TreeLevel - 1);
                }
                else endString += str;

                return endString += "}";
            }
            if (Type == JsonType.Array)
            {
                TreeLevel++;
                string str = "";
                for (int i = 0; i < Arrays.Count; i++)
                {
                    if (IsFormat)
                    {
                        str += JSONTools.TreeLevel(TreeLevel);
                    }
                    JsonValue jsonValue = Arrays[i];
                    if (jsonValue == null)
                    {
                        str += "null";
                    }
                    else str += jsonValue.SetTreeLevel(TreeLevel);
                    if (i < (Arrays.Count - 1))
                    {
                        str += ",";
                        if (IsFormat)
                        {
                            str += "\n";
                        }
                    }

                }
                string endString = "[";
                if (IsFormat)
                {
                    endString += "\n";
                    endString += str;
                    endString += "\n" + JSONTools.TreeLevel(TreeLevel - 1);
                }
                else endString += str;

                return (endString += "]");
            }
            if (Type == JsonType.String)
            {
                return "\"" + String.ToString() + "\"";
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

        #endregion

        #region 索引访问器

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
        /// 索引访问器
        /// </summary>
        public JsonValue this[int index]
        {
            get
            {
                if (Arrays.Count > index)
                {
                    return Arrays[index];
                }
                throw (new Exception($"GetArray索引：'{index}' 不存在"));
            }
            set
            {
                Arrays[index] = value;
            }
        }

        #endregion

        #region 隐式转换

        public static implicit operator JsonValue(Dictionary<string, JsonValue> v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Object);
            jsonValue.Dict = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(List<JsonValue> v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Array);
            jsonValue.Arrays = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(string v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.String);
            jsonValue.String = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(int v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Number);
            jsonValue.Value = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(long v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Number);
            jsonValue.Value = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(uint v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Number);
            jsonValue.Value = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(ulong v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Number);
            jsonValue.Value = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(float v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Number);
            jsonValue.Value = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(double v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Number);
            jsonValue.Value = v;
            return jsonValue;
        }
        public static implicit operator JsonValue(bool v)
        {
            JsonValue jsonValue = new JsonValue(JsonType.Number);
            jsonValue.Value = v;
            return jsonValue;
        }

        public static implicit operator List<JsonValue>(JsonValue v)
        {
            return v.GetArray();
        }
        public static implicit operator string(JsonValue v)
        {
            return v.ToString();
        }
        public static implicit operator int(JsonValue v)
        {
            return (int)v.Value;
        }
        public static implicit operator uint(JsonValue v)
        {
            return (uint)v.Value;
        }
        public static implicit operator long(JsonValue v)
        {
            return (long)v.Value;
        }
        public static implicit operator ulong(JsonValue v)
        {
            return (ulong)v.Value;
        }
        public static implicit operator float(JsonValue v)
        {
            return (long)v.Value;
        }
        public static implicit operator double(JsonValue v)
        {
            return (ulong)v.Value;
        }
        public static implicit operator bool(JsonValue v)
        {
            return (bool)v.Value;
        }

        #endregion
    }

}
