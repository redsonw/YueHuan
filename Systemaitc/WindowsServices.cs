using Microsoft.Win32;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;

namespace YueHuan.Systemaitc
{
    public class Service
    {
        /// <summary>
        /// 获取指定名称的Windows服务对象
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>服务对象</returns>
        private static ServiceController GetService(string serviceName)
        {
            // 实例化一个ServiceController对象
            ServiceController sc = new(serviceName);
            return sc;
        }

        /// <summary>
        /// 启动指定名称的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void StartService(string serviceName)
        {
            // 获取指定名称的服务对象
            ServiceController sc = GetService(serviceName);

            // 如果服务当前状态不是Running，则启动服务
            if (sc.Status != ServiceControllerStatus.Running)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        /// <summary>
        /// 停止指定名称的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void StopService(string serviceName)
        {
            // 获取指定名称的服务对象
            ServiceController sc = GetService(serviceName);

            // 如果服务当前状态是Running，则停止服务
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }

        /// <summary>
        /// 重启指定名称的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void RestartService(string serviceName)
        {
            // 获取指定名称的服务对象
            ServiceController sc = GetService(serviceName);

            // 如果服务当前状态是Running，则先停止服务，再启动服务
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }
            sc.Start();
            sc.WaitForStatus(ServiceControllerStatus.Running);
        }

        /// <summary>
        /// 获取指定名称的Windows服务的状态
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>服务状态返回running表示正在运行，返回空字符表示没有运行。</returns>
        public static string GetServiceStatus(string serviceName)
        {
            // 获取指定名称的服务对象
            ServiceController sc = GetService(serviceName);

            if (sc == null)
            {
                // 如果服务不存在或未启动，则返回空字符串
                return string.Empty;
            }
            else
            {
                // 返回服务状态
                return sc.Status.ToString();
            }
        }

        /// <summary>
        /// 获取指定名称的Windows服务的配置信息
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>服务配置信息</returns>
        public static string GetServiceConfig(string serviceName)
        {
            // 获取指定名称的服务对象
            ServiceController sc = GetService(serviceName);

            // 返回服务配置信息
            return sc.DisplayName + ": " + sc.ServiceType + ", " + sc.StartType + ", " + sc.CanPauseAndContinue;
        }

        /// <summary>
        /// 检查指定名称的服务是否已经存在
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>服务是否已经存在返回true，不存在则返回false。</returns>
        public static bool IsServiceExists(string serviceName)
        {
            // 获取计算机上所有的服务列表
            ServiceController[] services = ServiceController.GetServices();

            // 检查是否存在指定名称的服务
            return services.Any(s => s.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取指定的Windwos服务信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static ServerInfo? GetServiceExecutablePath(string serviceName)
        {
            ServerInfo serverInfo = new();
            try
            {

                if (IsServiceExists(serviceName))
                {
                    using (ServiceController sc = new ServiceController(serviceName))
                    {
                        // 获取服务的 WMI 配置信息
                        ManagementObject serviceConfig = new($"Win32_Service.Name='{serviceName}'");

                        // 获取服务可执行文件路径
                        serverInfo.PathName = serviceConfig["PathName"].ToString(); // 服务可执行文件的路径
                        serverInfo.DisplayName = serviceConfig["DisplayName"].ToString();
                        //  serverInfo.Description = serviceConfig["Description"].ToString();
                        serverInfo.StartMode = serviceConfig["StartMode"].ToString();
                        serverInfo.ErrorControl = serviceConfig["ErrorControl"].ToString();
                        serverInfo.StartName = serviceConfig["StartName"].ToString();
                        // serverInfo.Dependencies = serviceConfig["Dependencies"].ToString();

                        // 去掉服务路径中的引号和参数
                        serverInfo.PathName = serverInfo.PathName?.Trim('"').Split(' ')[0];

                    }
                }
                else
                {
                    serverInfo = null;
                }
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message, ex);
                Console.WriteLine(ex.ToString());
            }
            return serverInfo;
        }

        /// <summary>
        /// 删除指定的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>删除成功则返回true，删除失败则返回false。</returns>
        public static void DeleteService(string serviceName)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c sc delete \"{serviceName}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Failed to delete service {serviceName}. Error message: {error}");
                }
            }
        }

        /// <summary>
        /// 卸载指定的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>卸载成功则返回true，失败则返回false。</returns>
        public static bool Uninstall(string serviceName)
        {
            if (ServiceController.GetServices().Any(s => s.ServiceName == serviceName))
            {
                using (ServiceController service = new(serviceName))
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    }
                }

                using RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}", true);
                if (key != null)
                {
                    object imagePath = key.GetValue("ImagePath");
                    if (imagePath is string imagePathString && imagePathString.ToLower().Contains("exe"))
                    {
                        try
                        {
                            key.DeleteSubKeyTree("");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting service {serviceName}: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}