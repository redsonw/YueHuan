namespace YueHuan.Byteset
{
    public class Hexadecimal
    {
        private string filePath; // 文件路径
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public Hexadecimal(string filePath)
        {
            this.filePath = filePath; // 初始化文件路径
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns>读取成功返回byte[]，失败返回null</returns>
        public virtual byte[]? ReadFile()
        {
            if (File.Exists(filePath)) // 判断文件是否存在
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filePath); // 读取文件
                    return bytes;
                }
                catch (Exception e)
                {
                    Console.WriteLine("读取文件失败: " + e.Message); // 如果读取文件失败，输出错误信息
                    return null;
                }
            }
            else
            {
                Console.WriteLine("文件不存在"); // 如果文件不存在，输出提示信息
                return null;
            }
        }

        /// <summary>
        /// 查找十六进制字符串
        /// <br/>
        /// <br/><paramref name="hexValue"/>    十六进制字节，格式：00 00 1F 2A
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>成功返回true，失败返回false。</returns>
        public virtual bool FindHex(string hexString)
        {
            byte[]? bytes = ReadFile();

            if (bytes == null)
            {
                Console.WriteLine("无法查找十六进制: 文件不存在或读取失败"); // 如果读取文件失败，输出提示信息
                return false;
            }

            string[] hexValuesSplit = hexString.Split(' '); // 将十六进制字符串分隔成一个个字节
            byte[] hexValues = new byte[hexValuesSplit.Length]; // 存储分隔后的十六进制字节

            for (int i = 0; i < hexValuesSplit.Length; i++) // 循环遍历每个字节的十六进制字符串
            {
                hexValues[i] = Convert.ToByte(hexValuesSplit[i], 16); // 转换成十六进制字节
            }

            for (int i = 0; i < bytes.Length - hexValues.Length + 1; i++)
            {
                bool match = true;
                for (int j = 0; j < hexValues.Length; j++)
                {
                    if (bytes[i + j] != hexValues[j]) // 如果对应的字节不匹配
                    {
                        match = false;
                        break;
                    }
                }
                if (match) // 如果全部匹配
                {
                    Console.WriteLine("已找到十六进制: " + hexString); // 输出匹配的十六进制字符串
                    return true;
                }
            }
            Console.WriteLine("未找到十六进制: " + hexString); // 如果未找到，输出提示信息
            return false;

        }

        /// <summary>
        /// 查找十六进制数
        /// <br/>
        /// <br/><paramref name="hexValue"/>    十六进制字节，格式：0x00001F2A
        /// </summary>
        /// <param name="hexValues"></param>
        /// <returns>成功返回true,失败返回false。</returns>
        public virtual bool FindHex(byte[] hexValues)
        {
            byte[]? bytes = ReadFile();

            if (bytes == null)
            {
                Console.WriteLine("无法查找十六进制: 文件不存在或读取失败"); // 如果读取文件失败，输出提示信息
                return false;
            }

            for (int i = 0; i < bytes.Length - hexValues.Length + 1; i++)
            {
                bool match = true;
                for (int j = 0; j < hexValues.Length; j++)
                {
                    if (bytes[i + j] != hexValues[j]) // 如果对应的字节不匹配
                    {
                        match = false;
                        break;
                    }
                }
                if (match) // 如果全部匹配
                {
                    Console.WriteLine("已找到十六进制: " + BitConverter.ToString(hexValues).Replace("-", " ")); // 输出匹配的十六进制字符串
                    return true;
                }
            }
            Console.WriteLine("未找到十六进制: " + BitConverter.ToString(hexValues).Replace("-", " ")); // 如果未找到，输出提示信息
            return false;
        }

        /// <summary>
        /// 查找十六进制字节
        /// <br/>
        /// <br/><paramref name="hexValue"/>    十六进制字节，格式：0x00,0x00,0x1F,0x2A
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns>成功返回true，失败返回false。</returns>
        public virtual bool FindHex(uint hexValue)
        {
            byte[]? bytes = ReadFile();

            if (bytes == null)
            {
                Console.WriteLine("无法查找十六进制: 文件不存在或读取失败"); // 如果读取文件失败，输出提示信息
                return false;
            }

            byte[] hexValues = BitConverter.GetBytes(hexValue); // 将十六进制uint转换成字节
            Array.Reverse(hexValues); // 反转字节顺序

            return FindHex(hexValues);
        }
        /// <summary>
        /// 替换十六进制字符串
        /// <br/>
        /// <br/><paramref name="oldHexString"/>    欲替换的十六进制字符串，格式：00 00 1F 2A
        /// <br/><paramref name="newHexString"/>    新的十六进制字符串，格式：00 00 1F 2A
        /// </summary>
        /// <param name="oldHexString"></param>
        /// <param name="newHexString"></param>
        /// <returns></returns>
        public virtual bool ReplaceHex(string oldHexString, string newHexString)
        {
            byte[]? bytes = ReadFile();

            if (bytes == null)
            {
                Console.WriteLine("无法替换十六进制: 文件不存在或读取失败"); // 如果读取文件失败，输出提示信息
                return false;
            }

            string[] oldHexValuesSplit = oldHexString.Split(' '); // 将原十六进制字符串分隔成一个个字节
            byte[] oldHexValues = new byte[oldHexValuesSplit.Length]; // 存储分隔后的原十六进制字节

            for (int i = 0; i < oldHexValuesSplit.Length; i++) // 循环遍历每个字节的原十六进制字符串
            {
                oldHexValues[i] = Convert.ToByte(oldHexValuesSplit[i], 16); // 转换成原十六进制字节
            }

            string[] newHexValuesSplit = newHexString.Split(' '); // 将新十六进制字符串分隔成一个个字节
            byte[] newHexValues = new byte[newHexValuesSplit.Length]; // 存储分隔后的新十六进制字节

            for (int i = 0; i < newHexValuesSplit.Length; i++) // 循环遍历每个字节的新十六进制字符串
            {
                newHexValues[i] = Convert.ToByte(newHexValuesSplit[i], 16); // 转换成新十六进制字节
            }

            bool found = false;

            for (int i = 0; i < bytes.Length - oldHexValues.Length + 1; i++)
            {
                bool match = true;
                for (int j = 0; j < oldHexValues.Length; j++)
                {
                    if (bytes[i + j] != oldHexValues[j]) // 如果对应的字节不匹配
                    {
                        match = false;
                        break;
                    }
                }
                if (match) // 如果全部匹配
                {
                    Buffer.BlockCopy(newHexValues, 0, bytes, i, newHexValues.Length); // 替换原十六进制
                    found = true;
                }
            }

            if (!found) // 如果未找到需要替换的十六进制
            {
                Console.WriteLine("未找到需要替换的十六进制: " + oldHexString); // 输出未找到的原十六进制字符串
                return false;
            }

            try
            {
                File.WriteAllBytes(filePath, bytes); // 保存替换后的文件
                Console.WriteLine("已替换十六进制: " + oldHexString + " => " + newHexString); // 输出替换后的原和新十六进制字符串
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("文件保存失败: " + e.Message); // 如果保存文件失败，输出错误信息
                return false;
            }
        }
        /// <summary>
        /// 替换十六进制数 
        /// <br/>
        /// <br/><paramref name="oldHexValues"/>    欲替换的十六进制数，格式：0x00001F2A<br/>
        /// <paramref name="newHexValues"/> 新的十六进制数，格式：0x00001F2A<br/>
        /// </summary>
        /// <param name="oldHexValues">欲替换的十六进制数</param>
        /// <param name="newHexValues">新的十六进制数</param>
        /// <returns>成功返回true，失败返回false。</returns>
        public virtual bool ReplaceHex(byte[] oldHexValues, byte[] newHexValues)
        {
            byte[]? bytes = ReadFile();

            if (bytes == null)
            {
                Console.WriteLine("无法替换十六进制: 文件不存在或读取失败"); // 如果读取文件失败，输出提示信息
                return false;
            }

            bool found = false;

            for (int i = 0; i < bytes.Length - oldHexValues.Length + 1; i++)
            {
                bool match = true;
                for (int j = 0; j < oldHexValues.Length; j++)
                {
                    if (bytes[i + j] != oldHexValues[j]) // 如果对应的字节不匹配
                    {
                        match = false;
                        break;
                    }
                }
                if (match) // 如果全部匹配
                {
                    Buffer.BlockCopy(newHexValues, 0, bytes, i, newHexValues.Length); // 替换原十六进制
                    found = true;
                }
            }

            if (!found) // 如果未找到需要替换的十六进制
            {
                Console.WriteLine("未找到需要替换的十六进制: " + BitConverter.ToString(oldHexValues).Replace("-", " ")); // 输出未找到的原十六进制字符串
                return false;
            }

            try
            {
                File.WriteAllBytes(filePath, bytes); // 保存替换后的文件
                Console.WriteLine("已替换十六进制: " + BitConverter.ToString(oldHexValues).Replace("-", " ") + " => " + BitConverter.ToString(newHexValues).Replace("-", " ")); // 输出替换后的原和新十六进制字符串
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("文件保存失败: " + e.Message); // 如果保存文件失败，输出错误信息
                return false;
            }
        }
        /// <summary>
        /// 替换十六进制字节 <br/>
        /// <br/><paramref name="oldHexValue"/> 欲替换的十六进制字节，格式：0x00,0x00,0x1F,0x2A
        /// <br/><paramref name="newHexValue"/> 新的十六进制字节，格式：0x00,0x00,0x1F,0x2A
        /// </summary>
        /// <param name="oldHexValue"></param>
        /// <param name="newHexValue"></param>
        /// <returns>成功返回true，失败返回false。</returns>
        public virtual bool ReplaceHex(uint oldHexValue, uint newHexValue)
        {
            byte[] oldHexValues = BitConverter.GetBytes(oldHexValue); // 将原十六进制uint转换成字节
            Array.Reverse(oldHexValues); // 反转字节顺序

            byte[] newHexValues = BitConverter.GetBytes(newHexValue); // 将新十六进制uint转换成字节
            Array.Reverse(newHexValues); // 反转字节顺序

            return ReplaceHex(oldHexValues, newHexValues);
        }

        public virtual bool SaveFile()
        {
            byte[]? bytes = ReadFile();

            if (bytes == null)
            {
                Console.WriteLine("无法保存文件: 文件不存在或读取失败"); // 如果读取文件失败，输出提示信息
                return false;
            }

            try
            {
                File.WriteAllBytes(filePath, bytes); // 保存文件
                Console.WriteLine("文件保存成功"); // 输出成功信息
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("文件保存失败: " + e.Message); // 如果保存文件失败，输出错误信息
                return false;
            }
        }
    }
}
