namespace MyJSON
{
    public class JSON
    {
        /// <summary>
        /// 是否格式化JSON数据,默认true
        /// </summary>
        public bool IsFormat { get; set; } = false;


        /// <summary>
        /// 实例转换JSON(转换JSON)
        /// </summary>
        public static void Dumps()
        {
        }

        /// <summary>
        /// JSON转换实例(解析JSON)
        /// </summary>
        /// <param name="jsonText">待解析JSON文本</param>
        /// <param name="isFormat">是否格式化输出</param>
        /// <param name="isEscapeChar">字符是否转义</param>
        /// <returns></returns>
        public static JsonValue Loads(string jsonText, bool isFormat = false, bool isEscapeChar = false)
        {
            return new JsonParse(jsonText, isFormat, isEscapeChar);
        }
        /// <summary>
        /// JSON转换实例(解析JSON)
        /// </summary>
        /// <param name="jsonText">待解析JSON文本</param>
        /// <param name="isEscapeChar">字符是否转义</param>
        /// <returns></returns>
        public static JsonValue Loads(string jsonText, bool isEscapeChar = false)
        {
            return new JsonParse(jsonText, false, isEscapeChar);
        }
    }

}
