using System.Linq;

namespace MyJSON
{
    public class JSONTools
    {
        #region JSON工具

        /// <summary>
        /// 树等级缩进
        /// </summary>
        public static string TreeLevel(int level)
        {
            string indentStr = string.Empty;
            for (int i = 0; i < level; i++)
            {
                indentStr += "  ";
            }
            return indentStr;
        }

        #endregion
    }
}
