using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XunFeiNETSDK
{
    /// <summary>
    /// 讯飞RSA加密
    /// </summary>
    public class RSAHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="apiSecretIsKey"></param>
        /// <param name="buider"></param>
        /// <returns></returns>
        public static string HMACSha256(string apiSecretIsKey, string buider)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(apiSecretIsKey);

            using (HMACSHA256 hMACSHA256 = new HMACSHA256(bytes))
            {
                byte[] date = Encoding.UTF8.GetBytes(buider);
                date = hMACSHA256.ComputeHash(date);
                hMACSHA256.Clear();
                return System.Convert.ToBase64String(date);
            }
        }
    }
}
