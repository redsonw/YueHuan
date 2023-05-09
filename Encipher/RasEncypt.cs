using System.Security.Cryptography;

namespace YueHuan.Crypto
{
    /// <summary>
    /// 非对称加密类，使用Ras加密方式
    /// </summary>
    public class RasEncypt
    {
        private static string PublicKey = "";  // 公钥
        private static string PrivateKey = "";  // 私钥
        private static RSACryptoServiceProvider rsaProvider = new(1024);

        /// <summary>
        /// 非对称加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EncryptData(byte[] data)
        {

            RSACryptoServiceProvider rsa = new(1024);

            //将公钥导入到RSA对象中，准备加密；
            rsa.FromXmlString(PublicKey);

            //对数据data进行加密，并返回加密结果；
            //第二个参数用来选择Padding的格式
            byte[] buffer = rsa.Encrypt(data, false);
            return buffer;
        }

        /// <summary>
        /// 非对称解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DecryptData(byte[] data)
        {

            RSACryptoServiceProvider rsa = new(1024);

            //将私钥导入RSA中，准备解密；
            rsa.FromXmlString(PrivateKey);

            //对数据进行解密，并返回解密结果；
            return rsa.Decrypt(data, false);

        }
    }
}
