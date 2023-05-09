using System.Security.Cryptography;

namespace YueHuan.Crypto
{
    public class FileEncrypt
    {
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="pathInput">文件路径</param>
        /// <param name="pathOutput">输出路径</param>
        /// <param name="desKey">密钥</param>
        /// <param name="desIV">向量</param>
        public static void EncryptData(String pathInput, String pathOutput, byte[] desKey, byte[] desIV)
        {
            // 创建文件流以处理输入和输出文件。
            FileStream fin = new(pathInput, FileMode.Open, FileAccess.Read);
            FileStream fout = new(pathOutput, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            // 创建变量来辅助读取和写入。
            byte[] bin = new byte[100]; // 这是加密的中间存储。
            long rdlen = 0;              // 这是写入的总字节数。
            long totlen = fin.Length;    // 这是输入文件的总长度。
            int len;                     // 每次写入的字节数。

            DES des = DES.Create();
            CryptoStream encStream = new(fout, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);

            // 从输入文件读取，然后加密并写入输出文件。
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen += len;
            }

            encStream.Close();
            fout.Close();
            fin.Close();
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="pathInput">密文路径</param>
        /// <param name="pathOutput">输出路径</param>
        /// <param name="desKey">密钥</param>
        /// <param name="desIV">向量</param>
        public static void DecryptData(String pathInput, String pathOutput, byte[] desKey, byte[] desIV)
        {
            // 创建文件流以处理输入和输出文件。
            FileStream fin = new(pathInput, FileMode.Open, FileAccess.Read);
            FileStream fout = new(pathOutput, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            // 创建变量来辅助读取和写入。
            byte[] bin = new byte[100]; // 这是加密的中间存储。
            long rdlen = 0;              // 这是写入的总字节数。
            long totlen = fin.Length;    // 这是输入文件的总长度。
            int len;                     // 每次写入的字节数。

            DES des = DES.Create();
            CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write);

            // 从输入文件读取，然后加密并写入输出文件。
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen += len;
            }

            encStream.Close();
            fout.Close();
            fin.Close();
        }
    }
}
