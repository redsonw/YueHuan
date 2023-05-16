using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace YueHuan.NetCommun.FTPFileTransfer
{
    public class GitHubUpdater
    {
        /// <summary>
        /// 声明GitHub存储库URL和本地路径
        /// </summary>
        private string repositoryUrl { get; set; }

        /// <summary>
        /// 本地路径
        /// </summary>
        private string localPath { get; set; }

        /// <summary>
        /// 构造函数，初始化repositoryUrl和localPath字段
        /// </summary>
        /// <param name="repositoryUrl"></param>
        /// <param name="localPath"></param>
        public GitHubUpdater(string repositoryUrl, string localPath)
        {
            this.repositoryUrl = repositoryUrl;
            this.localPath = localPath;
        }

        /// <summary>
        /// 获取所有可用版本的列表，并返回一个字符串列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetVersionList()
        {
            // 创建一个空的字符串列表，用于存储版本号
            List<string> versions = new();

            try
            {
                // 使用HttpClient类来发送GET请求到GitHub API，获取标记信息
                using HttpClient client = new();
                string apiUrl = "https://api.github.com/repos/" + repositoryUrl + "/tags";
                client.DefaultRequestHeaders.Add("User-Agent", "request");
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                response.EnsureSuccessStatusCode();

                // 将API响应的内容读取为字符串，并将其转换为动态对象
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody)!;

                // 遍历标记信息，将每个版本号添加到versions列表中
                foreach (var tag in data)
                {
                    string version = tag.name;
                    versions.Add(version);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving version list: " + ex.Message);
            }

            // 返回包含所有可用版本的字符串列表
            return versions;
        }

        /// <summary>
        /// 检查是否有可用更新，并在有可用更新时执行更新操作
        /// </summary>
        public void CheckAndUpdate()
        {
            // 获取所有可用版本
            List<string> versions = GetVersionList();

            // 如果版本列表不为空，则继续检查更新
            if (versions.Count > 0)
            {
                // 获取最新版本号，即版本列表中的第一个版本
                string latestVersion = versions[0];

                // 如果有可用更新，则下载并安装更新
                if (IsUpdateAvailable(latestVersion))
                {
                    DownloadUpdate(latestVersion);
                    // 可以在这里执行其他更新步骤
                }
                // 否则打印一条消息，表示没有可用更新
                else
                {
                    Console.WriteLine("No updates available.");
                }
            }
            // 如果版本列表为空，则打印一条消息，表示无法检索到版本列表
            else
            {
                Console.WriteLine("Unable to retrieve version list.");
            }
        }

        /// <summary>
        /// 检查是否有新版本可用
        /// </summary>
        /// <param name="latestVersion"></param>
        /// <returns></returns>
        private static bool IsUpdateAvailable(string latestVersion)
        {
            // TODO: 使用自己的逻辑来比较最新版本和当前安装的版本，判断更新是否可用
            // 下面是一个示例，假设当前安装的版本为1.0，最新版本为2.0，则需要进行如下比较：

            // 将最新版本和当前版本转换为数字数组
            int[] latestVersionNumbers = latestVersion.Split('.').Select(int.Parse).ToArray();
            int[] currentVersionNumbers = "1.0".Split('.').Select(int.Parse).ToArray();

            // 从左到右逐个比较版本号的各个组成部分，直到找到不同之处
            for (int i = 0; i < latestVersionNumbers.Length && i < currentVersionNumbers.Length; i++)
            {
                if (latestVersionNumbers[i] > currentVersionNumbers[i])
                {
                    // 如果最新版本的某一部分大于当前版本，则认为有更新可用
                    return true;
                }
                else if (latestVersionNumbers[i] < currentVersionNumbers[i])
                {
                    // 如果最新版本的某一部分小于当前版本，则认为没有更新可用
                    return false;
                }
            }

            // 如果版本号的所有部分都相同，则认为没有更新可用
            return false;
        }

        /// <summary>
        /// 使用指定版本号下载和安装更新
        /// </summary>
        /// <param name="version"></param>
        private void DownloadUpdate(string version)
        {
            try
            {
                // 使用HttpClient类来发送GET请求，获取要下载的ZIP文件
                using HttpClient client = new();
                string downloadUrl = "https://github.com/" + repositoryUrl + "/archive/" + version + ".zip";
                string tempPath = Path.Combine(Path.GetTempPath(), "update.zip");
                HttpResponseMessage response = client.GetAsync(downloadUrl).Result;
                response.EnsureSuccessStatusCode();

                // 将ZIP文件的内容读取为流，并将其写入临时文件中
                Stream contentStream = response.Content.ReadAsStreamAsync().Result;
                using (FileStream fileStream = File.Create(tempPath))
                {
                    contentStream.CopyTo(fileStream);
                }

                // 将下载的ZIP文件解压到本地路径中
                ZipFile.ExtractToDirectory(tempPath, localPath, true);
                Console.WriteLine("Update downloaded and extracted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading update: " + ex.Message);
            }
        }
    }

}
