using System;
using System.Linq;

namespace MyJSON
{
    internal class JsonParse
    {
        #region JSONData

        /// <summary>
        /// JSON数据
        /// </summary>
        internal class JsonData
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
            public JsonData(string json)
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
                    tmp_json += new System.Text.RegularExpressions.Regex("[ \t\r\n]").Replace(json[i].ToString(), "");
                }
                return tmp_json;
            }

        }

        #endregion

        #region 私有变量

        private JsonData data;
        private bool isFormat;
        private int treeLevel;
        private JsonValue jsonValue;
        private bool isEscapeChar;

        #endregion

        #region 构造器

        public JsonParse(string jsonData, bool isFormat = false, bool isEscapeChar = false)
        {
            this.data = new JsonData(jsonData);
            this.isFormat = isFormat;
            this.treeLevel = 0;
            this.isEscapeChar = isEscapeChar;
            this.jsonValue = ParseJson();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 获取解析完毕后的JsonValue类型
        /// </summary>
        /// <returns></returns>
        public JsonValue GetJsonValue()
        {
            if (jsonValue != null) return jsonValue;
            throw new Exception("JsonValue对象不存在，可能是解析失败");
        }

        /// <summary>
        /// 设置是否格式化JSON
        /// </summary>
        /// <param name="isFormat"></param>
        /// <returns></returns>
        public JsonValue SetIsFormat(bool isFormat)
        {
            this.isFormat = isFormat;
            return this;
        }

        #endregion

        #region JSON解析（私有方法）

        /// <summary>
        /// JSON解析入口
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private JsonValue ParseJson()
        {
            JsonValue jsonValue = new JsonValue(JsonType.Object, isFormat, treeLevel);
            switch (data.ReadChar)
            {
                // 对象
                case '{':
                    jsonValue = ParseObject();
                    break;
                // 数组
                case '[':
                    jsonValue = ParseArray();
                    break;

                // 字符串
                case '"':
                    jsonValue = ParseString();
                    break;

                // 数字
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                    jsonValue = ParseNumber();
                    break;

                // 可能是布尔值，需要二次判断确定
                case 'f':
                case 't':
                    jsonValue = ParseBool();
                    if (jsonValue == null)
                    {
                        data.index++;
                        break;
                    }
                    else return jsonValue;


                // 可能是null，需要二次判断
                case 'n':
                    jsonValue = ParseNull();
                    if (jsonValue == null)
                    {
                        data.index++;
                        break;
                    }
                    else return jsonValue;

                default:
                    //break;
                    throw new Exception($"解析入口出现错误，字符 '{data.ReadChar}'  字符引索位置 '{data.index}'");

            }
            return jsonValue;
        }

        /// <summary>
        /// 解析Json对象
        /// </summary>
        private JsonValue ParseObject()
        {
            // 跳过字符 '{'
            data.index++;
            
            if (data.ReadChar != '"')
            {
                throw new FormatException("对象错误的JSON格式,Key字符串没有引号");
            }

            JsonValue jsonObject = new JsonValue(JsonType.Object, isFormat, treeLevel);

            while (true)
            {
                // 读取Key字符串
                string key = ParseString().String;

                // 检查是否有键值Value
                if (data.ReadChar != ':')
                {
                    throw new FormatException($"JSON格式错误,不存在键值Value。Key:{key}");
                }
                // 跳过 ':'
                data.index++;
                jsonObject[key] = ParseJson();

                // 判断是否为对象结束标识 '}' ，是的话跳过字符
                if (data.ReadChar == '}')
                {
                    data.index++;
                    break;
                }
                if (data.ReadChar == ',') // 读取多个Key
                    data.index++;
            }

            return jsonObject;
        }

        /// <summary>
        /// 解析Json数组
        /// </summary>
        private JsonValue ParseArray()
        {
            // 跳过字符 '['
            data.index++;
            
            JsonValue jsonArray = new JsonValue(JsonType.Array, isFormat, treeLevel);

            while (true)
            {

                // 检查数组字符结尾 ']' 如果有则跳过并且结束循环
                if (data.ReadChar == ']')
                {
                    data.index++;
                    break;
                }
                jsonArray.Arrays.Add(ParseJson());

                if (data.ReadChar == ',')
                    data.index++;
            }
            return jsonArray;
        }

        /// <summary>
        /// 字符转义处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string EscapeChar()
        {
            switch (data.ReadChar)
            {
                case '"':
                    data.index++;
                    return "\"";
                case '\\':
                    data.index++;
                    return "\\";
                case '/':
                    data.index++;
                    return "/";
                case '\'':
                    data.index++;
                    return "\'";
                case 'b':
                    data.index++;
                    return "\b";
                case 'f':
                    data.index++;
                    return "\f";
                case 'n':
                    data.index++;
                    return "\n";
                case 'r':
                    data.index++;
                    return "\r";
                case 't':
                    data.index++;
                    return "\t";
                case 'u':
                    string unicode = data.json.Substring(data.index + 1, 4);
                    if (unicode.Length == 4)
                    {
                        data.index += 5;
                        return char.ConvertFromUtf32(int.Parse(unicode, System.Globalization.NumberStyles.HexNumber));
                    }
                    else
                    {
                        string s = data.ReadChar.ToString();
                        data.index++;
                        return s;
                    }

                default:
                    break;

            }
            data.index++;
            return "\\" + data.ReadChar.ToString();
        }

        /// <summary>
        /// 解析Json字符串
        /// </summary>
        private JsonValue ParseString()
        {
            JsonValue jsonString = new JsonValue(JsonType.String, isFormat, treeLevel);
            // 检查是否为字符串开头标识 '"'
            if (data.ReadChar == '"')
            {
                data.index++;

                string src = "";
                while (true)
                {
                    switch (data.ReadChar)
                    {
                        // 检查到字符串结尾标识 '"' 结束循环
                        case '"':
                            data.index++;
                            break;

                        // 字符转义处理
                        case '\\':
                            if (isEscapeChar)
                            {
                                data.index++;
                                src += EscapeChar();
                                continue;
                            }
                            src += data.ReadChar;
                            data.index++;
                            continue;

                        default:
                            src += data.ReadChar;
                            data.index++;
                            continue;
                    }
                    break;
                }
                jsonString.String = src;

            }
            return jsonString;
        }

        /// <summary>
        /// 解析JSON数字
        /// </summary>
        private JsonValue ParseNumber()
        {

            JsonValue jsonNumber = new JsonValue(JsonType.Number, isFormat, treeLevel);
            var start = data.index;
            // 区分一下整数和浮点数
            bool isInt = true;
            while (true)
            {
                switch (data.json[++data.index])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                    case '+':
                    case '.':
                    case 'e':
                    case 'E':
                        continue;
                }
                break;
            }
            string strNum = data.json.Substring(start, data.index - start);

            if (strNum.Contains('.')) isInt = false;
            else isInt = true;

            if (isInt)
            {
                if (long.TryParse(strNum, out long num)) jsonNumber.Value = num;
                else throw new Exception(string.Format("整数解析错误,无法分析字符串 [{0}]", strNum));

            }
            else
            {
                if (double.TryParse(strNum, out double num)) jsonNumber.Value = num;
                else throw new Exception(string.Format("浮点数解析错误,无法分析字符串 [{0}]", strNum));
            }

            return jsonNumber;
        }

        /// <summary>
        /// 解析JSON布尔值
        /// </summary>
        private JsonValue ParseBool()
        {
            JsonValue jsonValue = null;
            switch (data.ReadChar)
            {
                // 可能是false，需要二次判断
                case 'f':
                    if (data.json.Length > data.index + 5)
                    {
                        if (data.json.Substring(data.index, 5).ToLower() == "false")
                        {
                            data.index += 5;
                            jsonValue = new JsonValue(JsonType.Bool, false);
                            break;
                        }
                    }
                    break;
                // 可能是true，需要二次判断
                case 't':
                    if (data.json.Length > data.index + 4)
                    {
                        if (data.json.Substring(data.index, 4).ToLower() == "true")
                        {
                            data.index += 4;
                            jsonValue = new JsonValue(JsonType.Bool, true);
                            break;
                        }
                    }
                    break;
            }
            return jsonValue;
        }

        /// <summary>
        /// 解析JSON Null
        /// </summary>
        private JsonValue ParseNull()
        {
            JsonValue jsonValue = null;
            if (data.json.Length > data.index + 4)
            {
                if (data.json.Substring(data.index, 4).ToLower() == "null")
                {
                    data.index += 4;
                    jsonValue = new JsonValue(JsonType.Null, null);
                }
            }
            return jsonValue;
        }

        #endregion


        public static implicit operator JsonValue(JsonParse v)
        {
            return v.GetJsonValue();
        }
    }
}
