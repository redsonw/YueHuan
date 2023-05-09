using System.Diagnostics;

namespace YueHuan.Systemaitc
{
    public class ProcessExe
    {
        public static bool RunExternalExe(string exeName)
        {
            bool blExe;
            try
            {
                Process process = new();
                ProcessStartInfo startInfo = new(exeName);
                process.StartInfo = startInfo;
                process.Start();
                blExe = true;
            }
            catch (Exception ex)
            {
                blExe = false;
            }
            return blExe;
        }

        public static bool RunExternalExe(string exeName, string[] args)
        {
            bool blExe;
            string brgs = "";
            Process process = new();
            try
            {
                foreach(string a in args)
                {
                    brgs += $"{a} ";
                }
                brgs= brgs.Trim();
                ProcessStartInfo startInfo = new(exeName, brgs);
                process.StartInfo= startInfo;
                process.Start();
                blExe= true;
            }
            catch(Exception ex)
            {
                blExe = false;
            }
            return blExe;
        }

    }
}
