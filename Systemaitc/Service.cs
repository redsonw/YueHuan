using System.ServiceProcess;

namespace YueHuan.Systemaitc
{
    public class Service
    {
        /// <summary>
        /// 监测服务是否启动
        /// <para>参数：</para>
        /// <paramref name="serviceName"/> 需要查询的服务名称
        /// </summary>
        /// <param name="serviceName">需要查询的服务名称</param>
        /// <returns>默认false，服务已启动则返回true。</returns>
        public static bool Exists(string serviceName)
        {
            bool IsService = false;

            // 获取所有服务
            ServiceController[] services = ServiceController.GetServices(); 
            try
            {
                // 遍历服务列表
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.ToUpper() == serviceName.ToUpper())
                    {
                        IsService = true;
                        break;
                    }
                }
                return IsService;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 启动服务
        /// <para>参数：</para>
        /// <paramref name="serviceName"/> 要启动的服务名称
        /// </summary>
        /// <param name="serviceName"> 要启动的服务名称</param>
        /// <returns>默认false，启动成功返回true。</returns>
        public static bool Start(string serviceName)
        {
            bool Isbn = false;

            try
            {
                if (Exists(serviceName))
                {
                    ServiceController star_service = new(serviceName);
                    if (star_service.Status != ServiceControllerStatus.Running &&
                    star_service.Status != ServiceControllerStatus.StartPending)
                    {
                        star_service.Start();

                        for (int i = 0; i < 60; i++)
                        {
                            star_service.Refresh();
                            Thread.Sleep(1000);
                            if (star_service.Status == ServiceControllerStatus.Running)
                            {
                                Isbn = true;
                                break;
                            }
                            if (i == 59)
                            {
                                Isbn = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return Isbn;
        }

        // 停止服务
        public static bool Stop(string serviceName)
        {
            bool isbn = false;

            try
            {
                if (Exists(serviceName))
                {
                    ServiceController star_service = new ServiceController(serviceName);
                    if (star_service.Status == ServiceControllerStatus.Running)
                    {
                        star_service.Stop();

                        for (int i = 0; i < 60; i++)
                        {
                            star_service.Refresh();
                            Thread.Sleep(1000);
                            if (star_service.Status == ServiceControllerStatus.Stopped)
                            {
                                isbn = true;
                                break;
                            }
                            if (i == 59)
                            {
                                isbn = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return isbn;
        }

    }
}
