using System.Runtime.InteropServices;
using System.Text;

namespace YueHuan.System
{
    public class IniOperation
    {
        /// <summary>
        /// 为INI文件中指定的节点取得字符串
        /// </summary>
        /// <param name="lpAppName">欲在其中查找关键字的节点名称</param>
        /// <param name="lpKeyName">欲获取的项名</param>
        /// <param name="lpDefault">指定的项没有找到时返回的默认值</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">INI文件完整路径</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量，其中不包括那些NULL中止字符</returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// 修改INI文件中内容
        /// </summary>
        /// <param name="lpApplicationName">欲在其中写入的节点名称</param>
        /// <param name="lpKeyName">欲设置的项名</param>
        /// <param name="lpString">要写入的新字符串</param>
        /// <param name="lpFileName">INI文件完整路径</param>
        /// <returns>不等于0表示成功，等于0表示失败</returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileString(string lpApplicationName, string? lpKeyName, string? lpString, string lpFileName);
        /// <summary>
        /// 读取INI文件值
        /// </summary>
        /// <param name="section">节点名</param>
        /// <param name="key">键</param>
        /// <param name="def">未取到值时返回的默认值</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>读取的值</returns>
        public static string Read(string section, string key, string def, string filePath)
        {
            StringBuilder strBuilder = new(1024);
            GetPrivateProfileString(section, key, def, strBuilder, 1024, filePath);
            return strBuilder.ToString();
        }

        /// <summary>
        /// 写INI文件值
        /// <para>参数：</para>
        /// <paramref name="section"/> 欲在其中写入的节点名称<br/>
        /// <paramref name="key"/> 欲设置的项名<br/>
        /// <paramref name="value"/> 要写入的新字符串<br/>
        /// <paramref name="filePath"/> INI文件完整路径<br/>
        /// </summary>
        /// <param name="section">欲在其中写入的节点名称</param>
        /// <param name="key">欲设置的项名</param>
        /// <param name="value">要写入的新字符串</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>不等于0表示成功，等于0表示失败</returns>
        public static int Write(string section, string? key, string? value, string filePath)
        {
            if (File.Exists(filePath))
            {
                return WritePrivateProfileString(section, key, value, filePath);
            }
            return 0;
        }

        /// <summary>
        /// 删除节
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>不等于0表示成功，等于0表示失败</returns>
        public static int DeleteSection(string section, string filePath)
        {
            return Write(section, null, null, filePath);
        }

        /// <summary>
        /// 删除键的值
        /// </summary>
        /// <param name="section">节点名</param>
        /// <param name="key">键名</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>不等于0表示成功，等于0表示失败</returns>
        public static int DeleteKey(string section, string key, string filePath)
        {
            return Write(section, key, null, filePath);
        }
    }

}
