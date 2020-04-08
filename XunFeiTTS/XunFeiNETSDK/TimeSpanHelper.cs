using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XunFeiNETSDK
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public class TimeSpanHelper
    {
        /// <summary>
        /// RFC1123
        /// </summary>
        /// <returns></returns>
        public static string GetTimeRFC1123()
        {
            return DateTime.Now.ToString("r");
        }
    }
}
