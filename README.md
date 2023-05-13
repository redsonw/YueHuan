# C# 基础框架

## 简介
>
> &emsp;&emsp;大家好，我是一名面向搜索引擎编程伪程序员，每次编程都会一通乱搜，而且记性也不太好，每次要写小工具的时候都去搜索太麻烦，所以就有了这个基础框架。<br/>
> &emsp;&emsp;可能有人会问网上大把的框架为什么要自己整个？其实很简单，网上写的很好，但是都没有写说明文件或者代码没有注释，所以我只是做了注释后整合在了一起，有的做了优化、有的直接照搬过来而已。
>
##

## 系统类

### 注册表

#### 数据类型

- 枚举类

##### 注册表域：**`RegDomain`**

<table >
    <tr>
        <th colspan="3">注册表域<th>
    </tr>
    <tr>
        <td>枚举类型</td>
        <td>值</td>
        <td>说明</td>
    </tr>
    <tr>
        <td>ClassesRoot</td>
        <td>0</td>
        <td>对应于HKEY_CLASSES_ROOT 主键</td>
    </tr>
    <tr>
        <td>CurrentUser</td>
        <td>1</td>
        <td>对应于HKEY_CURRENT_USER 主键</td>
    </tr>
    <tr>
        <td>LocalMachine</td>
        <td>2</td>
        <td>对应于HKEY_LOCAL_MACHINE 主键</td>
    </tr>
    <tr>
        <td>User</td>
        <td>3</td>
        <td>对应于HKEY_USER 主键</td>
    </tr>
    <tr>
        <td>CurrentConfig</td>
        <td>4</td>
        <td>对应于HEKY_CURRENT_CONFIG 主键</td>
    </tr>
</table>


##### 注册表值类型：**`RegValueKind`**

| 枚举名称     |  值   | 说明                                                                                                                                                            |
| ------------ | :---: | --------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Unknown      |   0   | 指示一个不受支持的注册表数据类型。例如，不支持 Microsoft Win32 API 注册表数据类型 REG_RESOURCE_LIST。使用此值指定                                               |
| String       |   1   | 指定一个以 Null 结尾的字符串。此值与 Win32 API 注册表数据类型 REG_SZ 等效。                                                                                     |
| ExpandString |   2   | 指定一个以 NULL 结尾的字符串，该字符串中包含对环境变量（如 %PATH%，当值被检索时，就会展开）的未展开的引用。此值与 Win32 API 注册表数据类型 REG_EXPAND_SZ 等效。 |
| Binary       |   3   | 指定任意格式的二进制数据。此值与 Win32 API 注册表数据类型 REG_BINARY 等效。                                                                                     |
| DWord        |   4   | 指定一个 32 位二进制数。此值与 Win32 API 注册表数据类型 REG_DWORD 等效。                                                                                        |
| MultiString  |   5   | 指定一个以 NULL 结尾的字符串数组，以两个空字符结束。此值与 Win32 API 注册表数据类型 REG_MULTI_SZ 等效。                                                         |
| QWord        |   6   | 指定一个 64 位二进制数。此值与 Win32 API 注册表数据类型 REG_QWORD 等效。                                                                                        |

#### 操作类
##### 构造函数
1. `Register()`

| 函数       | 返回类型 | 公开  | 备注                                      |
| ---------- | ------ | ----- | ----------------------------------------- |
| Register |        | [ x ] | 默认表域：LocalMachine 默认表项：SOFTWARE |

2. `Register(string subKey, RegDomain regDomain)`

| 函数       | 返回类型 | 公开  | 备注                                      |
| ---------- | ------ | ----- | ----------------------------------------- |
| Register |        | [ x ] | 默认表域：LocalMachine 默认表项：SOFTWARE |

|参数|数据类型|可空|备注|
|---|---|---|---|
|subKey|string| [ ]|注册表项名称|
|regDomain|RegDomain|[  ]|注册表域|
```C#
Register register = new("SOFTWARE\\",RegDomain.LocalMachine);
```
##### 创建注册表项
1. 

##### 读取注册表项
##### 判断注册表项是否存在
##### 删除注册表项
##### 创建注册表键值
##### 读取注册表键值
##### 判断注册表键值是否存在
##### 删除注册表键值

## 更新日志

### 2023-05-13

####
