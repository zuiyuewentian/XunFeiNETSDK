using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XunFeiNETSDK
{
    /// <summary>
    /// 返回消息头部
    /// </summary>
    public class TTS_Header_Model
    {
        /// <summary>
        /// 返回码，0表示成功，其它表示异常，详情请参考错误码。
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 本次会话的id，只在第一帧请求时返回
        /// </summary>
        public string sid { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public TTS_Data_Model data { get; set; }
    }

    public class TTS_Data_Model
    {
        /// <summary>
        /// 合成进度，指当前合成文本的字节数 
        /// 注：请注意合成是以句为单位切割的，若文本只有一句话，则每次返回结果的ced是相同的。
        /// </summary>
        public string ced { get; set; }

        /// <summary>
        /// 当前音频流状态，0表示开始合成，1表示合成中，2表示合成结束
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 合成后的音频片段,64位编码
        /// </summary>
        public string audio { get; set; }

        /// <summary>
        /// 音频流
        /// </summary>
        public byte[] audioStream { get; set; }
    }
}
