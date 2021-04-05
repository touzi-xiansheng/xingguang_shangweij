# 1.1准备工作

前言：程序是比赛用的，所以为了有些代码能精简就精简了，可以正常运行，有些地方会有点问题。比如：没有插串口线时，点打开串口会报错，这里可以做一个判断，但是为了减少代码量就省略了。

由于我不想进行窗体之间的跳转（因为这样你还要学习别的东西），所以我使用了**工具箱->容器->TabControl**控件进行窗体内部的切换。

TabControl技巧：在属性`TabPages`中可以设置两个Tab标题，也可以另外添加删除Tab

由于使用了一个窗体，可以进行简单的美化。（窗体属性中设置）

`Text修改为星光计划_上位机`Text是窗体标题（可自行修改）

`StartPosition设置为CenterScreen `设置窗体启动时在屏幕中央

`FormBorderStyle设置为FixedToolWindow`自带样式，个人认为还可以

> C#控件学习，推荐视频
> https://ke.qq.com/course/301616?taid=10053096806062640#term_id=100357491
> 根据自己基础看，我只看了**C#上位机开发玩转UI控件**

> textBox推荐教程
> https://blog.csdn.net/weixin_44077524/article/details/106903917

> C#开发: 通信篇-串口调试助手（强烈推荐）
> https://cloud.tencent.com/developer/article/1593405

# 1.2核心代码

```c#
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

public static void SetHeaderValue(WebHeaderCollection header, string name, string value)// HTTP协议报文头加入
{
    var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
    if (property != null)
    {
        var collection = property.GetValue(header, null) as NameValueCollection;
        collection[name] = value;
    }
}

```

```C#
//串口设置
string[] ports = System.IO.Ports.SerialPort.GetPortNames();//获取电脑上可用串口号
comboBox1.Items.AddRange(ports);//给comBox1添加数据
comboBox1.SelectedIndex = comboBox1.Items.Count > 0 ? 0 : -1;//如果有数据显示第零个（可省略）
comboBox2.Text = "115200";
comboBox3.Text = "8";

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


//串口接收，不确定接收成功，暂不列出
```

# 1.3问题点

api输出数据对接chart控件还未实现，思路如下：

> onenet输出的数据都是json格式，目前有种迂回的方式，将数据显示在DataGridVIew中然后再对接chart控件。（知识比较匮乏，目前思路只有这个，应该有更好的。但是没有系统学过C#和json，无法找到，建议大家自行谷歌或者系统学习下，没准会找到那个点。）
>
> 好的地址
>
> http://c.biancheng.net/view/3037.html
>
> https://blog.csdn.net/weixin_41924879/article/details/104883444
>
> https://www.codeleading.com/article/4376326244/
>
> https://blog.csdn.net/u011523479/article/details/81428493

关于阈值问题

> 实际上设置阈值在onenet文档里面有，**多协议接入->开发指南->EDP->API->API列表->触发器相关内容**这个又是一个坑，用好搜索引擎，尝试不同关键字没准有意想不到的东西（之前我搜到过，onenet上传，使用post方法，在[CSDN](https://www.csdn.net/)中当初没保存，建议去搜索下）(找到了：https://www.cnblogs.com/luxiaoguogege/p/10142053.html)

# 1.4建议反复观看的点

> OneNET基础API调用教程https://v.qq.com/x/page/v0845w7fp6b.html
>
> ONENET学院 https://space.bilibili.com/523880182/
>
> C#控件学习，推荐视频
> https://ke.qq.com/course/301616?taid=10053096806062640#term_id=100357491
> 根据自己基础看，我只看了**C#上位机开发玩转UI控件**
>
> textBox推荐教程
> https://blog.csdn.net/weixin_44077524/article/details/106903917
>
> C#开发: 通信篇-串口调试助手（强烈推荐）
> https://cloud.tencent.com/developer/article/1593405
