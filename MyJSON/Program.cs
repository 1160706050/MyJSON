using System;
using System.IO;
using static MyJSON.JSON;

namespace MyJSON
{
    class Program
    {
        /// <summary>
        /// 控制台测试入口，解决方案属性中改的控制台，是为了方便测试JSON类。
        /// </summary>
        /// <param name = "args" ></ param >
        static void Main(string[] args)
        {
            string path;

            // JSON测试文本(网页找的随机生成了个)
            path = string.Format(@"{0}\1.json", Directory.GetCurrentDirectory());
            //path = string.Format(@"{0}\2.json", Directory.GetCurrentDirectory());
            //path = string.Format(@"{0}\3.json", Directory.GetCurrentDirectory());

            // 读取所有文本
            StreamReader sr = new StreamReader(path);
            string json = sr.ReadToEnd();

            Console.WriteLine("\r\n***************************源文本*********************************************\r\n");
            Console.WriteLine(json);
            Console.WriteLine("\r\n***************************1.json*********************************************\r\n");
            // JSON解析实例化()
            JsonValue jsonValue = JSON.Loads(json, true, false);
            //Console.WriteLine(jsonValue[0].ToString()); // JSON数组对象下标为0的数组值

            //Console.WriteLine(jsonValue[0]["_id"].ToString());   // 获取JSON数组[0] 中的对象 "_id"值
            //Console.WriteLine(jsonValue[0]["isActive"].ToString());   // 获取JSON数组[0] 中的对象 "isActive"值(布尔值)

            Console.WriteLine(jsonValue[0]["friends"].ToString());   // 获取JSON数组[0] 中的对象 "friends" 值


            //Console.WriteLine(jsonValue[0]["不存在的"]);   // 不存在的话 返回null
            //if (jsonValue[0]["不存在的"] == null)
            //{
            //    Console.WriteLine("null");
            //}

            //Console.WriteLine("\r\n***************************2.json*********************************************\r\n");
            //Console.WriteLine(jsonValue.GetObject().GetType().Name); // JSON对象类型
            //Console.WriteLine(jsonValue.GetType().Name); // JSON对象类型

            //Console.WriteLine(jsonValue["_id"]);   // 获取JSON数组[0] 中的对象 "_id"值
            //Console.WriteLine(jsonValue["isActive"]);   // 获取JSON数组[0] 中的对象 "isActive"值(布尔值)

            //Console.WriteLine(jsonValue["friends"]);   // 获取JSON数组[0] 中的对象 "friends" 值
            //Console.WriteLine(jsonValue["friends"]);   // 获取JSON数组[0] 中的对象 "friends" 值(List<JsonValue>实例)
            //Console.WriteLine(jsonValue["friends"].GetType());   // 获取JSON数组[0] 中的对象 "friends" 值(List<JsonValue>实例名称)

            //Console.WriteLine(jsonValue["friends"][1]["id"]);   // 获取JSON数组[0]中的对象"friends"的值=>数组下标为1的id
            //Console.WriteLine(jsonValue["friends"][1]["name"]); // 获取JSON数组[0]中的对象"friends"的值=>数组下标为1的name

            //Console.WriteLine(jsonValue["不存在的"]);   // 不存在的话 返回C# null
            //if (jsonValue["不存在的"] == null)
            //{
            //    Console.WriteLine("null");
            //}


            //Console.WriteLine("\r\n***************************3.json*********************************************\r\n");
            //Console.WriteLine();
            //Console.WriteLine(jsonValue["Unicode"]);   // Unicode文本转换测试
            //Console.WriteLine();
            //Console.WriteLine(jsonValue["objArray"].SetIsFormat(true)); ;
            //Console.WriteLine();
            //Console.WriteLine(jsonValue["objArray"].SetIsFormat(false)); ;
            //Console.WriteLine();
            //Console.WriteLine(jsonValue["data"]["length"]);
            //Console.WriteLine(jsonValue["data"]["text"]);
            Console.ReadKey();
        }
    }
}
