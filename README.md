# MyJSON
JSON解析、JSONParse、ParseJson  

### 说明
这个研究了好几天,在递归遍历全部那儿卡了很久,解析终于完善许多,可以像JavaScript,Python 类似 用JsonValue[key][key][...]这样来取值  
没有Key的数组(比如一开始不是{}对象,而是[]数组类型)需要.GetArray() 来获取数组,然后用.GetArray()[i] 下标来取值  
有Key的数组如："key": [..]就像上面那样取就行  

### JSON文本(测试使用)
在 "...\bin\Debug\" 目录中,有三个测试用的JSON文本,从网上找的生成出来的

### 简易使用的JSON解析类
```C#
            string path;

            // JSON测试文本(网页找的随机生成了个)
            //path = string.Format(@"{0}\1.json", Directory.GetCurrentDirectory());
            //path = string.Format(@"{0}\2.json", Directory.GetCurrentDirectory());
            path = string.Format(@"{0}\3.json", Directory.GetCurrentDirectory());

            // 读取所有文本
            StreamReader sr = new StreamReader(path);
            string json = sr.ReadToEnd();
            Console.WriteLine(json);

            // JSON解析实例化(使用方法和python、json使用方法有点类似，没有键值对的名称数组是需要GetArray()[i]来获取)
            JsonValue jsonValue = JSON.Loads(json);

            Console.WriteLine("********************************************************************************");
            Console.WriteLine(jsonValue);   // JSON对象，已重写内部ToString()，可自动递归出所有解析内容，可直接输出已解析并且格式化后的JSON文本

            // 是否格式化已解析JSON字符串ToString(),默认是true,无需.SetIsFormat(true)，下面只是示范使用
            Console.WriteLine(jsonValue.SetIsFormat(true));   // JSON已解析格式化输出
            Console.WriteLine(jsonValue.SetIsFormat(false));   // JSON已解析去格式化输出


            Console.WriteLine("***************************1.json*********************************************");
            Console.WriteLine(jsonValue.GetArray()); // JSON数组对象
            Console.WriteLine(jsonValue.GetArray()[0]); // JSON数组对象下标为0的数组值
            Console.WriteLine(jsonValue.GetObject()); // JSON对象

            Console.WriteLine(jsonValue.GetArray()[0]["_id"]);   // 获取JSON数组[0] 中的对象 "_id"值
            Console.WriteLine(jsonValue.GetArray()[0]["isActive"]);   // 获取JSON数组[0] 中的对象 "isActive"值(布尔值)

            Console.WriteLine(jsonValue.GetArray()[0]["friends"]);   // 获取JSON数组[0] 中的对象 "friends" 值
            Console.WriteLine(jsonValue.GetArray()[0]["friends"].GetArray());   // 获取JSON数组[0] 中的对象 "friends" 值(List<JsonValue>实例)

            Console.WriteLine(jsonValue.GetArray()[0]["friends"].GetArray()[1]["id"]);   // 获取JSON数组[0]中的对象"friends"的值=>数组下标为1的id
            Console.WriteLine(jsonValue.GetArray()[0]["friends"].GetArray()[1]["name"]); // 获取JSON数组[0]中的对象"friends"的值=>数组下标为1的name

            Console.WriteLine(jsonValue.GetArray()[0]["不存在的"]);   // 不存在的话 返回null
            if (jsonValue.GetArray()[0]["不存在的"] == null)
            {
                Console.WriteLine("null");
            }

            Console.WriteLine("***************************2.json*********************************************");
            Console.WriteLine(jsonValue.GetObject().GetType().Name); // JSON对象类型
            Console.WriteLine(jsonValue.GetType().Name); // JSON对象类型

            Console.WriteLine(jsonValue["_id"]);   // 获取JSON对象中的 "_id"值
            Console.WriteLine(jsonValue["isActive"]);   // 获取JSON对象中的 "isActive"值(布尔值)

            Console.WriteLine(jsonValue["friends"]);   // 获取JSON对象中的"friends" 值
            Console.WriteLine(jsonValue["friends"].GetArray());   // 获取JSON对象中的 "friends" 值(List<JsonValue>实例)
            Console.WriteLine(jsonValue["friends"].GetArray().GetType());   // 获取JSON对象中的 "friends" 值(List<JsonValue>实例名称)

            Console.WriteLine(jsonValue["friends"].GetArray()[1]["id"]);   // 获取JSON对象中的"friends"的值=>数组下标为1的id
            Console.WriteLine(jsonValue["friends"].GetArray()[1]["name"]); // 获取JSON对象中的"friends"的值=>数组下标为1的name

            Console.WriteLine(jsonValue["不存在的"]);   // 不存在的话 返回C# null
            if (jsonValue["不存在的"] == null)
            {
                Console.WriteLine("null");
            }


            Console.WriteLine("***************************3.json*********************************************");
            Console.WriteLine();
            Console.WriteLine(jsonValue["Unicode"]);   // Unicode文本转换测试
            Console.WriteLine();
            Console.WriteLine(jsonValue["objArray"].SetIsFormat(true)); ;
            Console.WriteLine();
            Console.WriteLine(jsonValue["objArray"].SetIsFormat(false)); ;
            Console.WriteLine();
            Console.WriteLine(jsonValue["data"]["length"]);
            Console.WriteLine(jsonValue["data"]["text"]);
```
