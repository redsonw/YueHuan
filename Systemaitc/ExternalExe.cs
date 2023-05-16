using System.Diagnostics;

namespace YueHuan.Systemaitc
{
    public class ProcessExe
    {
        /// <summary>
        /// 运行一个外部程序，不需要参数
        /// </summary>
        /// <param name="exeName"></param>
        /// <returns></returns>
        public static bool RunExternalExe(string exeName)
        {
            try
            {
                Process process = new()
                {
                    StartInfo = new ProcessStartInfo(exeName)
                };
                process.Start();
                return true; // 记录成功标记
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false; // 记录失败标记
            }
        }

        /// <summary>
        /// 运行一个外部程序，需要传递参数
        /// </summary>
        /// <param name="exeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool RunExternalExe(string exeName, string[] args)
        {
            try
            {
                string arguments = string.Join(" ", args); // 将参数数组连接成一个字符串，以空格分隔
                Process process = new()
                {
                    StartInfo = new ProcessStartInfo(exeName, arguments)
                };
                process.Start();
                return true; // 记录成功标记
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false; // 记录失败标记
            }
        }

    }
}