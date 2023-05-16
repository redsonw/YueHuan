using Microsoft.Win32;

namespace YueHuan.Systemaitc
{
    public class Registery
    {
        /// <summary>
        /// 枚举类型，表示Windows注册表的基项。
        /// <para>包括以下基项：</para>
        /// <list type="bullet">
        /// <item><term>ClassesRoot</term><description>HKEY_CLASSES_ROOT 主键。</description></item>
        /// <item><term>CurrentUser</term><description>HKEY_CURRENT_USER 主键。</description></item>
        /// <item><term>LocalMachine</term><description>HKEY_LOCAL_MACHINE 主键。</description></item>
        /// <item><term>User</term><description>HKEY_USER 主键。</description></item>
        /// <item><term>CurrentConfig</term><description>HEKY_CURRENT_CONFIG 主键。</description></item>
        /// </list>
        /// </summary>
        public enum RegistryHive
        {
            /// <summary>
            /// 对应于 HKEY_CLASSES_ROOT 主键，包含文件类型和关联信息。
            /// </summary>
            ClassesRoot = 0,

            /// <summary>
            /// 对应于 HKEY_CURRENT_USER 主键，包含当前用户的配置信息。
            /// </summary>
            CurrentUser = 1,

            /// <summary>
            /// 对应于 HKEY_LOCAL_MACHINE 主键，包含本地计算机的硬件和软件配置信息。
            /// </summary>
            LocalMachine = 2,

            /// <summary>
            /// 对应于 HKEY_USERS 主键，包含系统上所有用户的配置信息。
            /// </summary>
            Users = 3,

            /// <summary>
            /// 对应于 HKEY_CURRENT_CONFIG 主键，包含计算机的当前配置信息。
            /// </summary>
            CurrentConfig = 4,
        }

        /// <summary>
        /// 指定在注册表中存储值时所用的数据类型，或标识注册表中某个值的数据类型
        /// <para>主要包括：</para>
        ///  
        /// <paramref name="Unknown"/> = 0 <br/>
        /// <paramref name="String"/> = 1 <br/>
        /// <paramref name="ExpandString"/> = 2 <br/>
        /// <paramref name="Binary"/> = 3 <br/>
        /// <paramref name="DWord"/> = 4 <br/>
        /// <paramref name="MultiString"/> = 5 <br/>
        /// <paramref name="QWord"/> = 6 <br/>
        ///  </summary>
        public enum RegistryValueKind
        {
            /// <summary>
            /// 未知的注册表数据类型。例如，不支持 Microsoft Win32 API 注册表数据类型 REG_RESOURCE_LIST。
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// 以 null 结尾的字符串。等效于 Win32 API 注册表数据类型 REG_SZ。
            /// </summary>
            String = 1,

            /// <summary>
            /// 以 null 结尾的字符串，其中包含对环境变量（如%PATH%）的未展开的引用。等效于 Win32 API 注册表数据类型 REG_EXPAND_SZ。
            /// </summary>
            ExpandString = 2,

            /// <summary>
            /// 任意格式的二进制数据。等效于 Win32 API 注册表数据类型 REG_BINARY。
            /// </summary>
            Binary = 3,

            /// <summary>
            /// 32 位二进制数。等效于 Win32 API 注册表数据类型 REG_DWORD。
            /// </summary>
            DWord = 4,

            /// <summary>
            /// 以 null 结尾的字符串数组，以两个空字符结束。等效于 Win32 API 注册表数据类型 REG_MULTI_SZ。
            /// </summary>
            MultiString = 5,

            /// <summary>
            /// 64 位二进制数。等效于 Win32 API 注册表数据类型 REG_QWORD。
            /// </summary>
            QWord = 6,
        }

        ///  <summary>
        ///  注册表操作类
        ///
        ///  主要包括以下操作：
        ///1.创建注册表项
        ///2.读取注册表项
        ///3.判断注册表项是否存在
        ///4.删除注册表项
        ///5.创建注册表键值
        ///6.读取注册表键值
        ///7.判断注册表键值是否存在
        ///8.删除注册表键值
        ///
        ///  版本:1.0
        ///  </summary>
        #region  字段定义
        ///  <summary>
        ///  注册表项名称
        ///  </summary>
        private string _subkey;
        ///  <summary>
        ///  注册表基项域
        ///  </summary>
        private RegistryHive _domain;
        ///  <summary>
        ///  注册表键值
        ///  </summary>
        private string _regeditkey { get; set; } = string.Empty;
        #endregion

        #region  属性
        ///  <summary>
        ///  设置注册表项名称
        ///  </summary>
        public string SubKey
        {
            get { return _subkey; }
            set { _subkey = value; }
        }

        ///  <summary>
        ///  注册表基项域
        ///  </summary>
        public RegistryHive Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        ///  <summary>
        ///  注册表键值
        ///  </summary>
        public string RegeditKey
        {
            get { return _regeditkey; }
            set { _regeditkey = value; }
        }
        #endregion

        #region  构造函数
        public Registery()
        {
            ///默认注册表项名称
            _subkey = "software\\";
            ///默认注册表基项域
            _domain = RegistryHive.LocalMachine;
        }

        ///  <summary>
        ///  构造函数
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        public Registery(string subKey, RegistryHive regMain)
        {
            ///设置注册表项名称
            _subkey = subKey;
            ///设置注册表基项域
            _domain = regMain;
        }
        #endregion

        #region  公有方法
        #region  创建注册表项
        ///  <summary>
        ///  创建注册表项，默认创建在注册表基项HKEY_LOCAL_MACHINE 下面（请先设置SubKey 属性）
        ///  <para>虚方法，子类可进行重写</para>
        ///  </summary>
        public void CreateSubKey()
        {
            // 判断注册表项名称是否为空，如果为空，返回
            if (string.IsNullOrEmpty(_subkey))
            {
                return;
            }

            // 创建基于注册表基项的节点
            using RegistryKey? key = GetRegMain(_domain);
            // 要创建的注册表项的节点
            if (!IsSubKeyExist())
            {
                key.CreateSubKey(_subkey);
            }
        }

        ///  <summary>
        ///  创建注册表项，默认创建在注册表基项HKEY_LOCAL_MACHINE 下面
        ///  虚方法，子类可进行重写
        ///  例子：如 subkey 是 software\\higame\\，则将创建 HKEY_LOCAL_MACHINE\\software\\higame\\注册表项
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        public void CreateSubKey(string subKey)
        {
            // 判断注册表项名称是否为空，如果为空，返回
            if (string.IsNullOrEmpty(subKey))
            {
                return;
            }

            // 创建基于注册表基项的节点
            using RegistryKey key = GetRegMain(_domain);
            // 要创建的注册表项的节点
            if (!IsSubKeyExist(subKey))
            {
                key.CreateSubKey(subKey);
            }
        }


        ///  <summary>
        ///  创建注册表项，默认创建在注册表基项HKEY_LOCAL_MACHINE 下面
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="regMain">注册表基项域</param>
        public void CreateSubKey(RegistryHive regMain)
        {
            // 判断注册表项名称是否为空，如果为空，返回
            if (string.IsNullOrEmpty(_subkey))
            {
                return;
            }

            // 创建基于注册表基项的节点
            using RegistryKey key = GetRegMain(regMain);
            // 要创建的注册表项的节点
            if (!IsSubKeyExist(regMain))
            {
                key.CreateSubKey(_subkey);
            }
        }

        ///  <summary>
        ///  创建注册表项（请先设置 SubKey 属性）
        ///  虚方法，子类可进行重写
        ///  例子：如 regMain 是 HKEY_LOCAL_MACHINE，subkey 是 software\\higame\\，则将创建 HKEY_LOCAL_MACHINE\\software\\higame\\注册表项
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        public void CreateSubKey(string subKey, RegistryHive regMain)
        {
            // 判断注册表项名称是否为空，如果为空，返回
            if (string.IsNullOrEmpty(subKey))
            {
                return;
            }

            // 创建基于注册表基项的节点
            using RegistryKey key = GetRegMain(regMain);
            // 要创建的注册表项的节点
            if (!IsSubKeyExist(subKey, regMain))
            {
                key.CreateSubKey(subKey);
            }
        }

        #endregion

        #region  判断注册表项是否存在
        ///  <summary>
        ///  判断注册表项是否存在，默认是在注册表基项 HKEY_LOCAL_MACHINE 下判断（请先设置 SubKey 属性）
        ///  虚方法，子类可进行重写
        ///  例子：如果设置了 Domain 和 SubKey 属性，则判断 Domain\\SubKey，否则默认判断 HKEY_LOCAL_MACHINE\\software\\
        ///  </summary>
        ///  <returns>返回注册表项是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsSubKeyExist()
        {
            // 判断注册表项名称是否为空，如果为空，返回 false
            if (string.IsNullOrEmpty(_subkey))
            {
                return false;
            }

            try
            {
                // 检索注册表子项
                using RegistryKey? sKey = OpenSubKey(_subkey, _domain);

                // 如果 sKey 为 null，则说明该注册表项不存在；否则存在
                return sKey != null;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of subkey '{_subkey}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项是否存在失败
        }

        ///  <summary>
        ///  判断注册表项是否存在，默认是在注册表基项 HKEY_LOCAL_MACHINE 下判断
        ///  虚方法，子类可进行重写
        ///  例子：如 subkey 是 software\\higame\\，则将判断 HKEY_LOCAL_MACHINE\\software\\higame\\注册表项是否存在
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <returns>返回注册表项是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsSubKeyExist(string subKey)
        {
            // 判断注册表项名称是否为空，如果为空，返回 false
            if (string.IsNullOrEmpty(subKey))
            {
                return false;
            }

            try
            {
                // 检索注册表子项
                using RegistryKey? sKey = OpenSubKey(subKey);

                // 如果 sKey 为 null，则说明该注册表项不存在；否则存在
                return sKey != null;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of subkey '{subKey}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项是否存在失败
        }

        ///  <summary>
        ///  判断注册表项是否存在
        ///  虚方法，子类可进行重写
        ///  例子：如 regMain 是 HKEY_CLASSES_ROOT，则将判断 HKEY_CLASSES_ROOT\\SubKey 注册表项是否存在
        ///  </summary>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>返回注册表项是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsSubKeyExist(RegistryHive regMain)
        {
            // 判断注册表项名称是否为空，如果为空，返回 false
            if (string.IsNullOrEmpty(_subkey))
            {
                return false;
            }

            try
            {
                // 检索注册表子项
                using RegistryKey? sKey = OpenSubKey(_subkey, regMain);

                // 如果 sKey 为 null，则说明该注册表项不存在；否则存在
                return sKey != null;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of subkey '{_subkey}' under hive '{regMain}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项是否存在失败
        }

        ///  <summary>
        ///  判断注册表项是否存在（请先设置 SubKey 属性）
        ///  虚方法，子类可进行重写
        ///  例子：如 regMain 是 HKEY_CLASSES_ROOT，subkey 是 software\\higame\\，则将判断 HKEY_CLASSES_ROOT\\software\\higame\\注册表项是否存在
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>返回注册表项是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsSubKeyExist(string subKey, RegistryHive regMain)
        {
            // 判断注册表项名称是否为空，如果为空，返回 false
            if (string.IsNullOrEmpty(subKey))
            {
                return false;
            }

            try
            {
                // 检索注册表子项
                using RegistryKey? sKey = OpenSubKey(subKey, regMain);

                // 如果 sKey 为 null，则说明该注册表项不存在；否则存在
                return sKey != null;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of subkey '{subKey}' under hive '{regMain}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项是否存在失败
        }
        #endregion

        #region  删除注册表项
        ///  <summary>
        ///  删除注册表项（请先设置 SubKey 属性）
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <returns>如果删除成功，则返回 true，否则为 false</returns>
        public virtual bool DeleteSubKey()
        {
            ///返回删除是否成功
            bool result = false;

            ///判断注册表项名称是否为空，如果为空，返回 false
            if (_subkey == string.Empty || _subkey == null)
            {
                return false;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegMain(_domain);

            if (IsSubKeyExist())
            {
                try
                {
                    ///删除注册表项
                    key.DeleteSubKey(_subkey);
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            ///关闭对注册表项的更改
            key.Close();
            return result;
        }

        ///  <summary>
        ///  删除注册表项（请先设置 SubKey 属性）
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <returns>如果删除成功，则返回 true，否则为 false</returns>
        public virtual bool DeleteSubKey(string subKey)
        {
            ///返回删除是否成功
            bool result = false;

            ///判断注册表项名称是否为空，如果为空，返回 false
            if (subKey == string.Empty || subKey == null)
            {
                return false;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegMain(_domain);

            if (IsSubKeyExist())
            {
                try
                {
                    ///删除注册表项
                    key.DeleteSubKey(subKey);
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            ///关闭对注册表项的更改
            key.Close();
            return result;
        }

        ///  <summary>
        ///  删除注册表项
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>如果删除成功，则返回 true，否则为 false</returns>
        public virtual bool DeleteSubKey(string subKey, RegistryHive regMain)
        {
            ///返回删除是否成功
            bool result = false;

            ///判断注册表项名称是否为空，如果为空，返回 false
            if (subKey == string.Empty || subKey == null)
            {
                return false;
            }

            ///创建基于注册表基项的节点
            RegistryKey key = GetRegMain(regMain);

            if (IsSubKeyExist(subKey, regMain))
            {
                try
                {
                    ///删除注册表项
                    key.DeleteSubKey(subKey);
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }
            ///关闭对注册表项的更改
            key.Close();
            return result;
        }
        #endregion

        #region  判断键值是否存在
        ///  <summary>
        ///  判断键值是否存在（请先设置 SubKey 和 RegeditKey 属性）
        ///  虚方法，子类可进行重写
        ///1.如果 RegeditKey 为空、null，则返回 false
        ///2.如果 SubKey 为空、 null 或者 SubKey 指定的注册表项不存在，返回 false
        ///  </summary>
        ///  <returns>返回键值是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsRegeditKeyExist()
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(_regeditkey))
            {
                return false;
            }

            try
            {
                // 判断注册表项是否存在
                if (IsSubKeyExist())
                {
                    // 打开注册表项
                    using RegistryKey? key = OpenSubKey();

                    // 键值集合
                    string[] regeditKeyNames = key!.GetValueNames();

                    // 遍历键值集合，如果存在键值，则退出遍历
                    foreach (string regeditKey in regeditKeyNames)
                    {
                        if (string.Compare(regeditKey, _regeditkey, true) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of regedit key '{_regeditkey}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项或键值是否存在失败
        }

        ///  <summary>
        ///  判断键值是否存在（请先设置 SubKey 属性）
        ///  虚方法，子类可进行重写
        ///  如果 SubKey 为空、null 或者 SubKey 指定的注册表项不存在，返回 false
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <returns>返回键值是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsRegeditKeyExist(string name)
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            try
            {
                // 判断注册表项是否存在
                if (IsSubKeyExist())
                {
                    // 打开注册表项
                    using RegistryKey? key = OpenSubKey();

                    // 键值集合
                    string[] regeditKeyNames = key!.GetValueNames();

                    // 遍历键值集合，如果存在键值，则退出遍历
                    foreach (string regeditKey in regeditKeyNames)
                    {
                        if (string.Compare(regeditKey, name, true) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of regedit key '{name}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项或键值是否存在失败
        }

        ///  <summary>
        ///  判断键值是否存在
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="subKey">注册表项名称</param>
        ///  <returns>返回键值是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsRegeditKeyExist(string name, string subKey)
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            try
            {
                // 判断注册表项是否存在
                if (IsSubKeyExist(subKey))
                {
                    // 打开注册表项
                    using RegistryKey? key = OpenSubKey(subKey);

                    // 键值集合
                    string[] regeditKeyNames = key!.GetValueNames();

                    // 遍历键值集合，如果存在键值，则退出遍历
                    foreach (string regeditKey in regeditKeyNames)
                    {
                        if (string.Compare(regeditKey, name, true) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of regedit key '{name}' under subkey '{subKey}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项或键值是否存在失败
        }

        ///  <summary>
        ///  判断键值是否存在
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>返回键值是否存在，存在返回 true，否则返回 false</returns>
        public virtual bool IsRegeditKeyExist(string name, string subKey, RegistryHive regMain)
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            try
            {
                // 判断注册表项是否存在
                if (IsSubKeyExist(subKey, regMain))
                {
                    // 打开注册表项
                    using RegistryKey? key = OpenSubKey(subKey, regMain);

                    // 键值集合
                    string[] regeditKeyNames = key!.GetValueNames();

                    // 遍历键值集合，如果存在键值，则退出遍历
                    foreach (string regeditKey in regeditKeyNames)
                    {
                        if (string.Compare(regeditKey, name, true) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to check existence of regedit key '{name}' under subkey '{subKey}' of hive '{regMain}': {ex.Message}");
            }

            return false; // 返回 false，表明检查注册表项或键值是否存在失败
        }
        #endregion

        #region  设置键值内容
        ///  <summary>
        ///  设置指定的键值内容，不指定内容数据类型（请先设置 RegeditKey 和 SubKey 属性）
        ///  存在该键值则修改键值内容，不存在键值则先创建键值，再设置键值内容
        ///  <para>参数：</para>
        ///  <paramref name="content"/> 键值内容
        ///  </summary>
        ///  <param name="content">键值内容</param>
        ///  <returns>键值内容设置成功，则返回 true，否则返回 false</returns>
        public virtual bool WriteRegeditKey(object content)
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(_regeditkey))
            {
                return false;
            }

            try
            {
                // 判断注册表项是否存在，如果不存在，则直接创建
                if (!IsSubKeyExist(_subkey))
                {
                    CreateSubKey(_subkey);
                }

                // 以可写方式打开注册表项
                using RegistryKey? key = OpenSubKey(true);

                // 如果注册表项打开失败，则返回 false
                if (key == null)
                {
                    return false;
                }

                key.SetValue(_regeditkey, content);
                return true;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to write content '{content}' to regedit key '{_regeditkey}': {ex.Message}");
                return false; // 返回 false，表明写入注册表项失败
            }
        }

        ///  <summary>
        ///  设置指定的键值内容，不指定内容数据类型（请先设置 SubKey 属性）
        ///  存在改键值则修改键值内容，不存在键值则先创建键值，再设置键值内容
        ///  <para>参数：</para>
        ///  <paramref name="name"/> 键值名称 <br/>
        ///  <paramref name="content"/> 键值内容
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="content">键值内容</param>
        ///  <returns>键值内容设置成功，则返回 true，否则返回 false</returns>
        public virtual bool WriteRegeditKey(string name, object content)
        {
            // 判断键值是否存在
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            try
            {
                // 判断注册表项是否存在，如果不存在，则直接创建
                if (!IsSubKeyExist(_subkey))
                {
                    CreateSubKey(_subkey);
                }

                // 以可写方式打开注册表项
                using RegistryKey? key = OpenSubKey(true);

                // 如果注册表项打开失败，则返回 false
                if (key == null)
                {
                    return false;
                }

                key.SetValue(name, content);
                return true;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to write content '{content}' to regedit key '{name}': {ex.Message}");
                return false; // 返回 false，表明写入注册表项失败
            }
        }

        ///  <summary>
        ///  设置指定的键值内容，指定内容数据类型（请先设置 SubKey 属性）
        ///  存在改键值则修改键值内容，不存在键值则先创建键值，再设置键值内容
        ///  <para>参数：</para>
        ///  <paramref name="name"/> 键值名称 <br/>
        ///  <paramref name="content"/> 键值内容 <br/>
        ///  <paramref name="regValueKind"/> 数据类型
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="content">键值内容</param>
        ///  <returns>键值内容设置成功，则返回 true，否则返回 false</returns>
        public bool WriteRegeditKey(string name, object content, RegistryValueKind regValueKind)
        {
            // 判断键值是否为空
            if (name is null or "")
            {
                return false;
            }

            // 判断注册表项是否存在，如果不存在，则直接创建
            if (!IsSubKeyExist(_subkey))
            {
                CreateSubKey(_subkey);
            }

            // 以可写方式打开注册表项
            using RegistryKey? key = OpenSubKey(true);

            // 如果注册表项打开失败，则返回 false
            if (key is null)
            {
                return false;
            }

            try
            {
                key.SetValue(name, content, GetRegValueKind(regValueKind));
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region  读取键值内容
        ///  <summary>
        ///  读取键值内容（请先设置 RegeditKey 和 SubKey 属性）
        ///1.如果 RegeditKey 为空、 null 或者 RegeditKey 指示的键值不存在，返回 null
        ///2.如果 SubKey 为空、 null 或者 SubKey 指示的注册表项不存在，返回 null
        ///3.反之，则返回键值内容
        ///  </summary>
        ///  <returns>返回键值内容</returns>
        public virtual object? ReadRegeditKey()
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(_regeditkey))
            {
                return null;
            }

            try
            {
                // 打开默认的注册表项
                using RegistryKey? key = OpenSubKey();
                if (key != null)
                {
                    // 读取键值内容结果
                    return key.GetValue(_regeditkey);
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to read registry key '{_regeditkey}': {ex.Message}");
            }

            return null; // 返回空对象，表明没有读到键值内容
        }

        ///  <summary>
        ///  读取键值内容（请先设置 SubKey 属性）
        ///1.如果 SubKey 为空、 null 或者 SubKey 指示的注册表项不存在，返回 null
        ///2.反之，则返回键值内容
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <returns>返回键值内容</returns>
        public virtual object? ReadRegeditKey(string name)
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            try
            {
                // 打开默认的注册表项
                using RegistryKey? key = OpenSubKey();
                if (key != null)
                {
                    // 读取键值内容结果
                    return key.GetValue(name);
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to read registry key '{name}': {ex.Message}");
            }

            return null; // 返回空对象，表明没有读到键值内容
        }

        ///  <summary>
        ///  读取键值内容
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="subKey">注册表项名称</param>
        ///  <returns>返回键值内容</returns>
        public virtual object? ReadRegeditKey(string name, string subKey)
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            try
            {
                // 打开注册表项
                using RegistryKey? key = OpenSubKey(subKey);
                if (key != null)
                {
                    // 读取键值内容结果
                    return key.GetValue(name);
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to read registry key '{name}' under subkey '{subKey}': {ex.Message}");
            }
            return null; // 返回空对象，表明没有读到键值内容
        }

        ///  <summary>
        ///  读取键值内容
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>返回键值内容</returns>
        public virtual object? ReadRegeditKey(string name, string subKey, RegistryHive regMain)
        {
            // 判断是否设置键值属性
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            try
            {
                // 打开注册表项
                using RegistryKey? key = OpenSubKey(subKey, regMain);
                if (key != null)
                {
                    // 读取键值内容结果
                    return key.GetValue(name);
                }
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to read registry key '{name}' under subkey '{subKey}' of hive '{regMain}': {ex.Message}");
            }

            return null; // 返回空对象，表明没有读到键值内容
        }
        #endregion

        #region  删除键值
        ///  <summary>
        ///  删除键值（请先设置 RegeditKey 和 SubKey 属性）
        ///1.如果 RegeditKey 为空、 null 或者 RegeditKey 指示的键值不存在，返回 false
        ///2.如果 SubKey 为空、 null 或者 SubKey 指示的注册表项不存在，返回 false
        ///  </summary>
        ///  <returns>如果删除成功，返回 true，否则返回 false</returns>
        public virtual bool DeleteRegeditKey()
        {
            ///删除结果
            bool result = false;

            ///判断是否设置键值属性，如果没有设置，则返回 false
            if (_regeditkey == string.Empty || _regeditkey == null)
            {
                return false;
            }

            ///判断键值是否存在
            if (IsRegeditKeyExist(_regeditkey))
            {
                ///以可写方式打开注册表项
                RegistryKey? key = OpenSubKey(true);
                if (key != null)
                {
                    try
                    {
                        ///删除键值
                        key.DeleteValue(_regeditkey);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        ///关闭对注册表项的更改
                        key.Close();
                    }
                }
            }

            return result;
        }

        ///  <summary>
        ///  删除键值（请先设置 SubKey 属性）
        ///  如果 SubKey 为空、null 或者 SubKey 指示的注册表项不存在，返回 false
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <returns>如果删除成功，返回 true，否则返回 false</returns>
        public virtual bool DeleteRegeditKey(string name)
        {
            ///删除结果
            bool result = false;

            ///判断键值名称是否为空，如果为空，则返回 false
            if (name == string.Empty || name == null)
            {
                return false;
            }

            ///判断键值是否存在
            if (IsRegeditKeyExist(name))
            {
                ///以可写方式打开注册表项
                RegistryKey? key = OpenSubKey(true);
                if (key != null)
                {
                    try
                    {
                        ///删除键值
                        key.DeleteValue(name);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        ///关闭对注册表项的更改
                        key.Close();
                    }
                }
            }

            return result;
        }

        ///  <summary>
        ///  删除键值
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="subKey">注册表项名称</param>
        ///  <returns>如果删除成功，返回 true，否则返回 false</returns>
        public virtual bool DeleteRegeditKey(string name, string subKey)
        {
            ///删除结果
            bool result = false;

            ///判断键值名称和注册表项名称是否为空，如果为空，则返回 false
            if (name == string.Empty || name == null || subKey == string.Empty || subKey == null)
            {
                return false;
            }

            ///判断键值是否存在
            if (IsRegeditKeyExist(name))
            {
                ///以可写方式打开注册表项
                RegistryKey? key = OpenSubKey(subKey, true);
                if (key != null)
                {
                    try
                    {
                        ///删除键值
                        key.DeleteValue(name);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        ///关闭对注册表项的更改
                        key.Close();
                    }
                }
            }

            return result;
        }

        ///  <summary>
        ///  删除键值
        ///  </summary>
        ///  <param name="name">键值名称</param>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>如果删除成功，返回 true，否则返回 false</returns>
        public virtual bool DeleteRegeditKey(string name, string subKey, RegistryHive regMain)
        {
            ///删除结果
            bool result = false;

            ///判断键值名称和注册表项名称是否为空，如果为空，则返回 false
            if (name == string.Empty || name == null || subKey == string.Empty || subKey == null)
            {
                return false;
            }

            ///判断键值是否存在
            if (IsRegeditKeyExist(name))
            {
                ///以可写方式打开注册表项
                RegistryKey? key = OpenSubKey(subKey, regMain, true);
                if (key != null)
                {
                    try
                    {
                        ///删除键值
                        key.DeleteValue(name);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        ///关闭对注册表项的更改
                        key.Close();
                    }
                }
            }

            return result;
        }
        #endregion
        #endregion

        #region  受保护方法
        ///  <summary>
        ///  获取注册表基项域对应顶级节点
        ///  例子：如 regMain 是 ClassesRoot，则返回 Registry.ClassesRoot
        ///  </summary>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>注册表基项域对应顶级节点</returns>
        protected static RegistryKey GetRegMain(RegistryHive regMain)
        {
            return regMain switch
            {
                RegistryHive.ClassesRoot => Registry.ClassesRoot,
                RegistryHive.CurrentUser => Registry.CurrentUser,
                RegistryHive.LocalMachine => Registry.LocalMachine,
                RegistryHive.CurrentConfig => Registry.CurrentConfig,
                _ => Registry.LocalMachine,
            };
        }

        ///  <summary>
        ///  获取在注册表中对应的值数据类型
        ///  例子：如 regValueKind 是 DWord，则返回 RegistryValueKind.DWord
        ///  </summary>
        ///  <param name="regValueKind">注册表数据类型</param>
        ///  <returns>注册表中对应的数据类型</returns>
        protected static Microsoft.Win32.RegistryValueKind GetRegValueKind(RegistryValueKind regValueKind)
        {
            return regValueKind switch
            {
                RegistryValueKind.Unknown => Microsoft.Win32.RegistryValueKind.Unknown,
                RegistryValueKind.String => Microsoft.Win32.RegistryValueKind.String,
                RegistryValueKind.ExpandString => Microsoft.Win32.RegistryValueKind.ExpandString,
                RegistryValueKind.Binary => Microsoft.Win32.RegistryValueKind.Binary,
                RegistryValueKind.DWord => Microsoft.Win32.RegistryValueKind.DWord,
                RegistryValueKind.MultiString => Microsoft.Win32.RegistryValueKind.MultiString,
                RegistryValueKind.QWord => Microsoft.Win32.RegistryValueKind.QWord,
                _ => Microsoft.Win32.RegistryValueKind.String,
            };
        }


        #region  打开注册表项
        ///  <summary>
        ///  打开注册表项节点，以只读方式检索子项
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <returns>如果 SubKey 为空、 null 或者 SubKey 指示注册表项不存在，则返回 null，否则返回注册表节点</returns>
        protected virtual RegistryKey? OpenSubKey()
        {
            // 判断注册表项名称是否为空
            if (string.IsNullOrEmpty(_subkey))
            {
                return null;
            }

            try
            {
                // 创建基于注册表基项的节点
                using RegistryKey key = GetRegMain(_domain);

                // 打开注册表项
                RegistryKey? sKey = key.OpenSubKey(_subkey);

                // 返回注册表节点
                return sKey;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to open subkey '{_subkey}': {ex.Message}");
            }

            return null; // 返回空对象，表明没有打开注册表项
        }

        ///  <summary>
        ///  打开注册表项节点
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="writable">如果需要项的写访问权限，则设置为true</param>
        ///  <returns>如果 SubKey 为空、 null 或者 SubKey 指示注册表项不存在，则返回 null，否则返回注册表节点</returns>
        protected virtual RegistryKey? OpenSubKey(bool writable)
        {
            // 判断注册表项名称是否为空
            if (string.IsNullOrEmpty(_subkey))
            {
                return null;
            }

            try
            {
                // 创建基于注册表基项的节点
                using RegistryKey key = GetRegMain(_domain);

                // 打开注册表项
                RegistryKey? sKey = key.OpenSubKey(_subkey, writable);

                // 返回注册表节点
                return sKey;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to open subkey '{_subkey}' with {(writable ? "write" : "read")} access: {ex.Message}");
            }

            return null; // 返回空对象，表明没有打开注册表项
        }

        ///  <summary>
        ///  打开注册表项节点，以只读方式检索子项
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <returns>如果 SubKey 为空、 null 或者 SubKey 指示注册表项不存在，则返回 null，否则返回注册表节点</returns>
        protected virtual RegistryKey? OpenSubKey(string subKey)
        {
            // 判断注册表项名称是否为空
            if (string.IsNullOrEmpty(subKey))
            {
                return null;
            }

            try
            {
                // 创建基于注册表基项的节点
                using RegistryKey key = GetRegMain(_domain);

                // 打开注册表项
                RegistryKey? sKey = key.OpenSubKey(subKey);

                // 返回注册表节点
                return sKey;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to open subkey '{subKey}': {ex.Message}");
            }

            return null; // 返回空对象，表明没有打开注册表项
        }

        ///  <summary>
        ///  打开注册表项节点，以只读方式检索子项
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="writable">如果需要项的写访问权限，则设置为true</param>
        ///  <returns>如果 SubKey 为空、 null 或者 SubKey 指示注册表项不存在，则返回 null，否则返回注册表节点</returns>
        protected virtual RegistryKey? OpenSubKey(string subKey, bool writable)
        {
            // 判断注册表项名称是否为空
            if (string.IsNullOrEmpty(subKey))
            {
                return null;
            }

            try
            {
                // 创建基于注册表基项的节点
                using RegistryKey key = GetRegMain(_domain);

                // 打开注册表项
                RegistryKey? sKey = key.OpenSubKey(subKey, writable);

                // 返回注册表节点
                return sKey;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to open subkey '{subKey}' with {(writable ? "write" : "read")} access: {ex.Message}");
            }

            return null; // 返回空对象，表明没有打开注册表项
        }

        ///  <summary>
        ///  打开注册表项节点，以只读方式检索子项
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        ///  <returns>如果 SubKey 为空、 null 或者 SubKey 指示注册表项不存在，则返回 null，否则返回注册表节点</returns>
        protected virtual RegistryKey? OpenSubKey(string subKey, RegistryHive regMain)
        {
            // 判断注册表项名称是否为空
            if (string.IsNullOrEmpty(subKey))
            {
                return null;
            }

            try
            {
                // 创建基于指定主键的节点
                using RegistryKey key = GetRegMain(regMain);

                // 打开注册表项
                RegistryKey? sKey = key.OpenSubKey(subKey);

                // 返回注册表节点
                return sKey;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to open subkey '{subKey}' under hive '{regMain}': {ex.Message}");
            }

            return null; // 返回空对象，表明没有打开注册表项
        }

        ///  <summary>
        ///  打开注册表项节点
        ///  虚方法，子类可进行重写
        ///  </summary>
        ///  <param name="subKey">注册表项名称</param>
        ///  <param name="regMain">注册表基项域</param>
        ///  <param name="writable">如果需要项的写访问权限，则设置为true</param>
        ///  <returns>如果 SubKey 为空、 null 或者 SubKey 指示注册表项不存在，则返回 null，否则返回注册表节点</returns>
        protected virtual RegistryKey? OpenSubKey(string subKey, RegistryHive regMain, bool writable)
        {
            // 判断注册表项名称是否为空
            if (string.IsNullOrEmpty(subKey))
            {
                return null;
            }

            try
            {
                // 创建基于指定主键的节点
                using RegistryKey key = GetRegMain(regMain);

                // 打开注册表项
                RegistryKey? sKey = key.OpenSubKey(subKey, writable);

                // 返回注册表节点
                return sKey;
            }
            catch (Exception ex)
            {
                // 捕捉并记录可能出现的异常
                Console.WriteLine($"Failed to open subkey '{subKey}' under hive '{regMain}' with {(writable ? "write" : "read")} access: {ex.Message}");
            }

            return null; // 返回空对象，表明没有打开注册表项
        }
        #endregion
        #endregion
    }
}
