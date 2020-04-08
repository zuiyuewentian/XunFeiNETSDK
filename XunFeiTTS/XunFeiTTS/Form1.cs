using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XunFeiNETSDK;

namespace XunFeiTTS
{
    public partial class Form1 : Form
    {
        XunFeiNETSDK.XunFeiTTS xunFeiNetSdk;

        public Form1()
        {
            InitializeComponent();

            string appId = ConfigurationSettings.AppSettings["appId"];
            string apiKey = ConfigurationSettings.AppSettings["apiKey"];
            string apiSecret = ConfigurationSettings.AppSettings["apiSecret"];
            xunFeiNetSdk = new XunFeiNETSDK.XunFeiTTS(appId, apiKey, apiSecret);
            xunFeiNetSdk.MessageUpdate_Event += XunFeiNetSdk_MessageUpdate_Event;
        }

        /// <summary>
        /// 缓存返回数据
        /// </summary>
        byte[] data = new byte[0];

        private void XunFeiNetSdk_MessageUpdate_Event(TTS_Data_Model message, string error = null)
        {

            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            try
            {
                //合成结束
                if (message.status == 2)
                {
                    data = data.Concat(message.audioStream).ToArray();
                    var mWavWriter = new WaveFileWriter("test.wav", new WaveFormat(16000, 1));
                    mWavWriter.Write(data, 0, data.Length);
                    mWavWriter.Close();
                    MessageBox.Show("合成成功");
                    data = new byte[0];
                }
                else
                {
                    data = data.Concat(message.audioStream).ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            xunFeiNetSdk.SendData("积极参与全球卫生治理，为世界公共卫生事业作贡献，是责任也是义务，符合人类共同利益。只有团结协作，人类才能获取制胜之力，才能共享生命安全");
        }

    
    }
}
