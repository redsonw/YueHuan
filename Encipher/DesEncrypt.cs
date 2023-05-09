using System.Security.Cryptography;
using System.Text;

namespace YueHuan.Encipher
{
    public class DesEncrypt
    {
        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="strPlain">明文</param>
        /// <param name="strDESKey">密钥</param>
        /// <param name="strDESIV">向量</param>
        /// <returns>返回加密后的文本</returns>
        public static string DESEncrypt(string strPlain, string strDESKey, string strDESIV)
        {
            
            byte[] bytesDESKey = Encoding.ASCII.GetBytes(strDESKey);  // 把密钥转换成字节数组

            byte[] bytesDESIV = Encoding.ASCII.GetBytes(strDESIV);  // 把向量转换成字节数组

            DES desEncrypt = DES.Create();  // 声明1个新的DES对象

            MemoryStream msEncrypt = new();  // 开辟一块内存流

            CryptoStream csEncrypt = new(msEncrypt, desEncrypt.CreateEncryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Write);  // 把内存流对象包装成加密流对象

            StreamWriter swEncrypt = new(csEncrypt);  // 把加密流对象包装成写入流对象

            swEncrypt.WriteLine(strPlain);  // 写入流对象写入明文

            swEncrypt.Close();  // 写入流关闭

            csEncrypt.Close();  // 加密流关闭

            byte[] bytesCipher = msEncrypt.ToArray();  // 把内存流转换成字节数组，内存流现在已经是密文了

            msEncrypt.Close();  // 内存流关闭

            return Encoding.Unicode.GetString(bytesCipher);  // 把密文字节数组转换为字符串，并返回
        }

        /// <summary>
        /// DES解密方法
        /// </summary>
        /// <param name="strCipher">密文</param>
        /// <param name="strDESKey">密钥</param>
        /// <param name="strDESIV">向量</param>
        /// <returns>返回解密明文</returns>
        public static string? DESDecrypt(string strCipher, string strDESKey, string strDESIV)
        {
            byte[] bytesDESKey = Encoding.ASCII.GetBytes(strDESKey); // 把密钥转换成字节数组
            
            byte[] bytesDESIV = Encoding.ASCII.GetBytes(strDESIV);   // 把向量转换成字节数组
            
            byte[] bytesCipher = Encoding.Unicode.GetBytes(strCipher);  // 把密文转换成字节数组
            
            DES desDecrypt = DES.Create();  // 声明1个新的DES对象
            
            MemoryStream msDecrypt = new(bytesCipher);  // 开辟一块内存流，并存放密文字节数组
            
            CryptoStream csDecrypt = new(msDecrypt, desDecrypt.CreateDecryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Read);  // 把内存流对象包装成解密流对象
            
            StreamReader srDecrypt = new(csDecrypt);  // 把解密流对象包装成读出流对象
            
            string? strPlainText = srDecrypt.ReadLine();  // 明文=读出流的读出内容
            
            srDecrypt.Close();  // 读出流关闭
            
            csDecrypt.Close();  // 解密流关闭
            
            msDecrypt.Close();  // 内存流关闭
            
            return strPlainText;  // 返回明文
        }
    }
}
