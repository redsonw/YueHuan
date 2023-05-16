namespace YueHuan.System
{
    internal class RunLog
    {
        #region 输出日志到文件
        /// <summary>
        /// 输出日志到文件
        /// </summary>
        /// <param name="Log">日志内容</param>
        public static void WriteLog(string logText)
        {
            string RunPath = Application.StartupPath + "\\LOG"; // 取当前运行目录
            string LogTime = DateTime.Now.ToString("yyyy-MM-dd");
            string LogPath = Path.Combine(RunPath, LogTime + "_LOG" + "_INFO.TXT");
            try
            {
                if (!Directory.Exists(RunPath))
                {
                    Directory.CreateDirectory(RunPath);  // 创建日志文件夹
                }

                StreamWriter sw = new StreamWriter(LogPath, true);
                sw.WriteLine(DateTime.Now.ToString("[hh:mm:ss] ") + logText + "\r");  // 写出日志
                sw.Close();
            }
            catch
            {
                ///
            }

        }
        #endregion
    }
}
