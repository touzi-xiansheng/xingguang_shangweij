using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using xingguang_shangweij.Properties;

namespace xingguang_shangweij
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //api相关设置
            string devices = textBox2.Text;
            string key = textBox3.Text;
            string url = "http://api.heclouds.com/devices/" + devices + "/datapoints?";//设备地址，这里显示的是所有数据流
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            SetHeaderValue(request.Headers, "api-key", key);//设备API地址和 首部参数
            request.Host = "api.heclouds.com";
            request.ProtocolVersion = new Version(1, 1);
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            textBox1.Text = retString;
         }

        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)// HTTP协议报文头加入
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        //设置textbox保存上传输入内容
        private Settings settings = new Settings();
        private void Form1_Load(object sender, EventArgs e)
        {
            //填入上次输入内容
            textBox2.Text = settings.设备;
            textBox3.Text = settings.KEY;


            //chart折线图简单设置，未与api的数据对接，仅做简单演示，思路看附带文档
            List<string> xData = new List<string>() { "A", "B", "C", "D" };
            List<int> yData = new List<int>() { 17, 8, 10, 5 };
            chart1.Series[0].Points.DataBindXY(xData, yData);
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series[0].Points.DataBindXY(xData, yData);
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            //串口设置
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();//获取电脑上可用串口号
            comboBox1.Items.AddRange(ports);//给comBox1添加数据
            comboBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;//如果有数据显示第零个（可省略）
            comboBox2.Text = "115200";
            comboBox3.Text = "8";
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //设置textbox保存上传输入内容
            settings.设备 = textBox2.Text;
            settings.KEY = textBox3.Text;
            settings.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "打开串口")
            {
                serialPort1.PortName = comboBox1.Text;//获取comboBox1要打开的串口号
                serialPort1.BaudRate = int.Parse(comboBox2.Text);//获取comboBox2选择的波特率
                serialPort1.DataBits = int.Parse(comboBox3.Text);//设置数据位
                serialPort1.Open();//打开串口
                button2.Text = "关闭串口";//按钮显示关闭串口
            }
            else
            {
                serialPort1.Close();//关闭串口
                button2.Text = "打开串口";//按钮显示打开
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //串口设置，因为不影响正常运行所以注释掉
            string data = string.Empty;
            //while (serialPort1.BytesToRead > 0)
            // {
            data += serialPort1.ReadExisting();  //数据读取,直到读完缓冲区数据
                                                 // }

            //更新界面内容时UI不会卡
            this.Invoke((EventHandler)delegate
            {
                //定义一个textBox控件用于接收消息并显示
                textBox5.AppendText(data + Environment.NewLine);
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String Str = textBox6.Text.ToString();//获取发送文本框里面的数据
          //  try
           // {
             //   if (Str.Length > 0)
               // {
                    serialPort1.Write(Str);//串口发送数据
                //}
           // }
           // catch (Exception) { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
        }
    }
}
