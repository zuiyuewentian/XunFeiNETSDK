using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using WebSocketSharp;

namespace XunFeiNETSDK
{
    /// <summary>
    /// 讯飞TTS 语音合成
    /// </summary>
    public class XunFeiTTS
    {
        public delegate void Message_Handle(TTS_Data_Model message, string error = null);

        /// <summary>
        /// 消息事件
        /// </summary>
        public event Message_Handle MessageUpdate_Event;

        /// <summary>
        /// baseURL
        /// </summary>
        string baseUrl = "wss://tts-api.xfyun.cn/v2/tts";

        string appId = "";

        string apiKey = "";

        string apiSecret = "";

        /// <summary>
        /// 请求主机
        /// </summary>
        string host = "tts-api.xfyun.cn";

        /// <summary>
        /// 当前时间戳，RFC1123格式 例如：Thu, 01 Aug 2019 01:53:21 GMT
        /// </summary>
        string date = "";

        /// <summary>
        /// 请求行
        /// </summary>
        string requestLine = "GET /v2/tts HTTP/1.1";

        /// <summary>
        /// 配置文件
        /// </summary>
        public ConfigSetting configSetting = new ConfigSetting();


        public WebSocket websocket;

        /// <summary>
        /// TTS 语音合成
        /// </summary>
        /// <param name="appid">appId</param>
        /// <param name="apikey">apiKey</param>
        /// <param name="apisecret">apiSecret</param>
        /// <param name="config">配置文件，不传则选择默认配置</param>
        public XunFeiTTS(string appid, string apikey, string apisecret, ConfigSetting config = null)
        {
            appId = appid;
            apiKey = apikey;
            apiSecret = apisecret;
            if (config != null)
                configSetting = config;

            date = TimeSpanHelper.GetTimeRFC1123();

            string sign = $"host: {host}\ndate: {date}\n{requestLine}";

            string signature = RSAHelper.HMACSha256(apiSecret, sign);
            string authorization_org = $"api_key=\"{apiKey}\", algorithm=\"hmac-sha256\", headers=\"host date request-line\", signature=\"{signature}\"";
            string authorization = Base64Helper.EncodeBase64("utf-8", authorization_org);
            string reqUrl = string.Format(baseUrl + "?authorization={0}&date={1}&host={2}", authorization, HttpUtility.UrlEncode(date).Replace("+", "%20"), host);

            try
            {
                websocket = new WebSocket(reqUrl);
                websocket.OnMessage += Websocket_OnMessage;
                websocket.OnOpen += Websocket_OnOpen;

                websocket.Connect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message">文本内容，最大约2000汉字</param>
        public void SendData(string message)
        {
            if (message.Length > 2000)
            {
                //默认传输都是汉字
                throw new Exception("传入字数过长，不得超过2000汉字！");
            }
            try
            {
                JObject frame = new JObject();
                JObject business = new JObject();
                JObject common = new JObject();
                JObject data = new JObject();
                // 填充common
                common.Add("app_id", appId);
                //填充business
                business.Add("aue", configSetting.aue);
                business.Add("tte", configSetting.tte);
                business.Add("ent", configSetting.ent);
                business.Add("vcn", configSetting.vcn);
                business.Add("pitch", configSetting.pitch);
                business.Add("speed", configSetting.speed);
                business.Add("volume", configSetting.volume);
                business.Add("auf", configSetting.auf);
                business.Add("ram", configSetting.ram);
                business.Add("rdn", configSetting.rdn);
                business.Add("reg", configSetting.reg);
                business.Add("sfl", configSetting.sfl);
                data.Add("status", 2);//数据状态，固定位2
                data.Add("text", Base64Helper.EncodeBase64("utf-8", message));

                frame.Add("common", common);
                frame.Add("business", business);
                frame.Add("data", data);

                byte[] value = Encoding.Default.GetBytes(frame.ToString());

                sendMessage(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        private void sendMessage(byte[] message)
        {
            try
            {
                if (websocket.ReadyState == WebSocketState.Open)
                    websocket.Send(message);
                else
                {
                    new XunFeiTTS(appId, apiKey, apiSecret, configSetting);
                    websocket.Connect();
                    websocket.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Websocket_OnOpen(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Websocket_OnMessage(object sender, MessageEventArgs e)
        {
            string message = e.Data;

            var mdata = JsonConvert.DeserializeObject<TTS_Header_Model>(message);

            if (mdata.code == 0)
            {
                TTS_Data_Model mydata = new TTS_Data_Model();
                mydata.ced = mdata.data.ced;
                mydata.status = mdata.data.status;
                mydata.audio = mdata.data.audio;
                mydata.audioStream = Convert.FromBase64String(mdata.data.audio);

                if (mdata.data.status == 2)
                {
                    websocket.Close();
                }
                if (MessageUpdate_Event != null)
                    MessageUpdate_Event(mydata);
            }
            else
            {
                if (MessageUpdate_Event != null)
                    MessageUpdate_Event(mdata.data, MessageException(mdata));
            }
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="code"></param>
        private string MessageException(TTS_Header_Model mdata)
        {
            if (mdata.code == 10005)
            {
                return "appid授权失败,确认appid是否正确，是否开通了合成服务";
            }
            else if (mdata.code == 10006)
            {
                return "请求缺失必要参数,检查报错信息中的参数是否正确上传";
            }
            else if (mdata.code == 10007)
            {
                return "请求的参数值无效,检查报错信息中的参数值是否在取值范围内";
            }
            else if (mdata.code == 10010)
            {
                return "引擎授权不足,请到控制台提交工单联系技术人员";
            }
            else if (mdata.code == 10109)
            {
                return "请求文本长度非法,检查是否文本长度超出了限制";
            }
            else if (mdata.code == 10019)
            {
                return "session超时,检查是否数据发送完毕但未关闭连接";
            }
            else if (mdata.code == 10101)
            {
                return "引擎回话已结束,检查是否引擎已结束会话但客户端还在发送数据，比如音频数据虽然发送完毕但并未关闭websocket连接，还在发送空的音频等";
            }
            else if (mdata.code == 10313)
            {
                return "appid不能为空,检查common参数是否正确上传，或common中的app_id参数是否正确上传或是否为空";
            }
            else if (mdata.code == 10317)
            {
                return "版本非法,联系技术人员";
            }
            else if (mdata.code == 11200)
            {
                return "没有权限,检查是否使用了未授权的发音人，或者总的调用次数已超越上限";
            }
            else if (mdata.code == 11201)
            {
                return "日流控超限,可联系商务提高每日调用次数";
            }
            else if (mdata.code == 10160)
            {
                return "请求数据格式非法,检查请求数据是否是合法的json";
            }
            else if (mdata.code == 10161)
            {
                return "base64解码失败	,检查发送的数据是否使用了base64编码";
            }
            else if (mdata.code == 10163)
            {
                return "缺少必传参数，或者参数不合法,检查报错信息中的参数是否正确上传";
            }
            else if (mdata.code == 10200)
            {
                return "读取数据超时,检查是否累计10s未发送数据并且未关闭连接";
            }
            else if (mdata.code == 10200)
            {
                return "网络异常,检查网络是否异常";
            }
            return mdata.message;
        }
    }
}
