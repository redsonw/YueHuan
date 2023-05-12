namespace YueHuan.Models
{
    public class ServerInfo
    {
        /// <summary>
        /// 服务可执行文件的路径
        /// </summary>
        public string? PathName; // 服务可执行文件的路径
        /// <summary>
        /// 服务的显示名称
        /// </summary>
        public string? DisplayName;// 服务的显示名称
        /// <summary>
        /// 服务的描述信息
        /// </summary>
        public string? Description;// 服务的描述信息
        /// <summary>
        /// 服务的启动模式
        /// </summary>
        public string? StartMode;// 服务的启动模式
        /// <summary>
        /// 服务出现错误时的操作方式
        /// </summary>
        public string? ErrorControl;// 服务出现错误时的操作方式
        /// <summary>
        /// 服务运行的帐户名称
        /// </summary>
        public string? StartName;// 服务运行的帐户名称
        /// <summary>
        /// 服务依赖的其他服务名称
        /// </summary>
        public string? Dependencies;// 服务依赖的其他服务名称
    }
}
