using System.Security.Cryptography;
using System.Text;

namespace YueHuan.Crypto
{
    public class Encypt
    {
        #region SHA1加密字符串
        /// <summary>
        /// SHA1加密字符串
        /// </summary>
        /// <param name="Source">源字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Hash(string Source)
        {
            var buffer = Encoding.UTF8.GetBytes(Source);
            var data = SHA1.HashData(buffer);

            StringBuilder sub = new();
            foreach (var t in data)
            {
                sub.Append(t.ToString("X2"));
            }

            return sub.ToString();
        }
        #endregion

        #region MD5加密字符串
        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="Source">源字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5(string Source)
        {
            byte[] Result = MD5.HashData(Encoding.Default.GetBytes(Source));
            return Encoding.Default.GetString(Result);
        }
        #endregion


        /// <summary>
        /// 默认密钥向量
        /// </summary>
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        #region DES加密字符串
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey[..8]);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                using (DES dCSP = DES.Create())
                {
                    MemoryStream mStream = new MemoryStream();
                    CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    return Convert.ToBase64String(mStream.ToArray());
                }
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DES DCSP = DES.Create();
                MemoryStream mStream = new();
                CryptoStream cStream = new(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion

    }
}

