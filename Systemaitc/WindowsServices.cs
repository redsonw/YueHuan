using Microsoft.Win32;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;
using YueHuan.Models;

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
            return new ServiceController(serviceName);
        }

        /// <summary>
        /// 启动指定名称的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void StartService(string serviceName)
        {
            // 获取指定名称的服务对象
            ServiceController sc = GetService(serviceName);

            if (sc.Status != ServiceControllerStatus.Running)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
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

            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
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

            if (sc.Status == ServiceControllerStatus.Running)
            {
                StopService(serviceName);
            }

            StartService(serviceName);
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

            return sc.Status.ToString();
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

            return $"{sc.DisplayName}: {sc.ServiceType}, {sc.StartType}, {sc.CanPauseAndContinue}";
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
        /// <param name="serviceName">服务名称</param>
        /// <returns>服务信息</returns>
        public static ServerInfo? GetServiceExecutablePath(string serviceName)
        {
            try
            {
                // 如果服务不存在或未启动，则返回null
                if (!IsServiceExists(serviceName))
                {
                    return null;
                }

                using (ServiceController sc = GetService(serviceName))
                {
                    // 获取服务可执行文件路径
                    ManagementObject serviceConfig = new($"Win32_Service.Name='{serviceName}'");
                    serviceConfig.Get();
                    string? pathName = serviceConfig["PathName"].ToString();
                    string? displayName = serviceConfig["DisplayName"].ToString();
                    string? startMode = serviceConfig["StartMode"].ToString();
                    string? errorControl = serviceConfig["ErrorControl"].ToString();
                    string? startName = serviceConfig["StartName"].ToString();

                    // 去掉服务路径中的引号和参数
                    pathName = pathName?.Trim('"').Split(' ')[0];

                    return new ServerInfo
                    {
                        PathName = pathName,
                        DisplayName = displayName,
                        StartMode = startMode,
                        ErrorControl = errorControl,
                        StartName = startName
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting service info for {serviceName}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 删除指定的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        public static void DeleteService(string serviceName)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $"delete \"{serviceName}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process process = Process.Start(startInfo)!;
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Failed to delete service {serviceName}. Error message: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting service {serviceName}: {ex.Message}");
            }
        }

        /// <summary>
        /// 卸载指定的Windows服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>卸载成功则返回true，失败则返回false。</returns>
        public static bool Uninstall(string serviceName)
        {
            try
            {
                using ServiceController service = new(serviceName);

                if (service.Status == ServiceControllerStatus.Running)
                {
                    StopService(serviceName);
                }

                using RegistryKey? key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}", true);

                if (key != null)
                {
                    object? imagePath = key.GetValue("ImagePath");

                    if (imagePath is string imagePathString && imagePathString.ToLower().Contains("exe"))
                    {
                        key.DeleteSubKeyTree(string.Empty);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uninstalling service {serviceName}: {ex.Message}");
                return false;
            }
        }
    }
}