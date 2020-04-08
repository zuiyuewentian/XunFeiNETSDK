using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XunFeiNETSDK
{
    /// <summary>
    /// 配置 文档参看 https://www.xfyun.cn/doc/tts/online_tts/API.html#%E6%8E%A5%E5%8F%A3%E8%B0%83%E7%94%A8%E6%B5%81%E7%A8%8B
    /// </summary>
    public class ConfigSetting
    {
        /// <summary>
        /// 音频编码 默认"raw" raw：未压缩的pcm
        /// lame：mp3 (当aue=lame时需传参sfl=1)
        /// speex-org-wb;7： 标准开源speex（for speex_wideband，即16k）数字代表指定压缩等级（默认等级为8）
        /// speex-org-nb;7： 标准开源speex（for speex_narrowband，即8k）数字代表指定压缩等级（默认等级为8）
        /// speex;7：压缩格式，压缩等级1~10，默认为7（8k讯飞定制speex）
        /// speex-wb;7：压缩格式，压缩等级1~10，默认为7（16k讯飞定制speex）
        /// </summary>
        public string aue { get; set; } = "raw";

        /// <summary>
        /// 文本编码格式 GB2312 GBK BIG5 GB18030
        /// UNICODE(小语种必须使用UNICODE编码，合成的文本需使用utf16小端的编码方式)
        /// </summary>
        public string tte { get; set; } = "UTF8";

        /// <summary>
        /// 引擎类型，默认为intp65 可选值：
        /// aisound（普通效果）intp65（中文） 
        /// intp65_en（英文）
        /// mtts（小语种，需配合小语种发音人使用） 
        /// xtts（优化效果） 
        /// </summary>
        public string ent { get; set; } = "intp65";

        /// <summary>
        /// 发音人，xiaoyan 基础发音可选值：aisjiuxu，aisxping，aisjinger，aisbabyxu 请到控制台添加试用或购买发音人，添加后即显示发音人参数值
        /// </summary>
        public string vcn { get; set; } = "xiaoyan";

        /// <summary>
        /// 音高，可选值：[0-100]，默认为50
        /// </summary>
        public int pitch { get; set; } = 50;

        /// <summary>
        /// 语速，可选值：[0-100]，默认为50
        /// </summary>
        public int speed { get; set; } = 50;

        /// <summary>
        /// 音量，可选值：[0-100]，默认为50
        /// </summary>
        public int volume { get; set; } = 50;

        /// <summary>
        /// 需要配合aue=lame使用，开启流式返回  mp3格式音频 取值：1 开启
        /// </summary>
        public int sfl { get; set; } = 1;

        /// <summary>
        /// 音频采样率，可选值：audio/L16;rate=8000：合成8K 的音频
        /// audio/L16;rate=16000：合成16K 的音频 
        /// auf不传值：合成16K 的音频
        /// </summary>
        public string auf { get; set; } = "audio/L16;rate=16000";

        /// <summary>
        /// 设置英文发音方式：默认按英文单词发音
        /// 0：自动判断处理，如果不确定将按照英文词语拼写处理（缺省）
        /// 1：所有英文按字母发音
        /// 2：自动判断处理，如果不确定将按照字母朗读
        /// </summary>
        public string reg { get; set; } = "2";

        /// <summary>
        /// 是否读出标点：
        /// 0：不读出所有的标点符号（默认值） 
        /// 1：读出所有的标点符号
        /// </summary>
        public string ram { get; set; } = "0";

        /// <summary>
        /// 合成音频数字发音方式
        /// 0：自动判断（默认值）
        /// 1：完全数值
        /// 2：完全字符串
        /// 3：字符串优先
        /// </summary>
        public string rdn { get; set; } = "0";
    }
}
