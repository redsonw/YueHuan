using System.Security.Cryptography;

namespace YueHuan.Encipher
{
    public class KeyFile
    {
        private string keyFile { get; set; } = string.Empty;

        /// <summary>
        /// 创建钥匙文件
        /// </summary>
        public KeyFile(string filePath, string fileName)
        {
            keyFile = Path.Combine(filePath, fileName, ".xml");
        }

        public void Create()
        {
            RSACryptoServiceProvider rsa = new();

            using (StreamWriter writer = new(keyFile, false))  //这个文件要保密...
            {
                writer.WriteLine(rsa.ToXmlString(true));
            }
            using (StreamWriter writer = new(keyFile, false))
            {
                writer.WriteLine(rsa.ToXmlString(false));
            }
        }
    }
}

