using System.Security.Cryptography;
using System.Text;

namespace YueHuan.Encipher
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
    }
}

