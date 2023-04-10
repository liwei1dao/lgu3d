
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 文件类工具
    /// </summary>
    public static class TimeTools
    {

        ///获取当前时间戳 bflag 秒/毫秒
        public static long GetTimeStamp(bool bflag)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long ret = 0;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds);
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds);

            return ret;
        }
    }
}