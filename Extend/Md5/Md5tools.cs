
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 文件类工具
    /// </summary>
    public static class Md5tools
    {
        public static string Md5ToLower(string str)
        {
            byte[] buffer = Encoding.Default.GetBytes(str);
            MD5 md5 = MD5.Create();
            byte[] bufferNew = md5.ComputeHash(buffer);
            string strNew = "";
            for (int i = 0; i < bufferNew.Length; i++)
            {
                strNew += bufferNew[i].ToString("x2");  //对bufferNew字节数组中的每个元素进行十六进制转换然后拼接成strNew字符串
            }
            return strNew;
        }
    }
}