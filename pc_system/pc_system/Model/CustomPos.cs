using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PosTest
{
    class CustomPos
    {
        private System.IO.Ports.SerialPort _port = null;

        public CustomPos(string portName)
        {
            _port = new System.IO.Ports.SerialPort();
            _port.PortName = portName;
            _port.Parity = System.IO.Ports.Parity.None; // 没有奇偶校验位 
            _port.BaudRate = 2400;                      // 数据速度 
            _port.DataBits = 8;                         // 数据位数 
            _port.StopBits = System.IO.Ports.StopBits.One;   // 停止位数
        }

        //发送指令
        private void SendData(byte[] buffer)
        {
            this._port.Open();
            this._port.Write(buffer, 0, buffer.Length);
            this._port.Close();
        }

        // 设置通讯的速率
        public void SetBaudRate(int rate)
        {
            //ACSII码  格式：STX   B   n         0<=n<=5 
            //十进制   格式：[002][066]n         48<=n<=53 
            //十六进制 格式：[02H][42H]n       30H<=n<=35H 
            //说明：改变系统的波特率（上电开机时缺省波特率为：2400bit/s），这个命令一般不需用到，使用缺省设定即可。 
            char c = '0';
            switch (rate)
            {
                case 9600:
                    c = '0';
                    break;
                case 4800:
                    c = '1';
                    break;
                case 2400:
                    c = '2';
                    break;
                case 1200:
                    c = '3';
                    break;
                case 600:
                    c = '4';
                    break;
                case 300:
                    c = '5';
                    break;
                default:
                    throw new System.InvalidOperationException("参数错误");
            }
            byte[] buffer = new byte[3];
            buffer[0] = 2;
            buffer[1] = (byte)'B';
            buffer[2] = (byte)c;
            this.SendData(buffer);
        }

        // 开钱箱   
        public void OpenBox()
        {
            //ACSII码 格式：STX   M 
            //十进制   格式：[002][077] 
            //十六进制 格式：[02H][4DH] 
            //说明：通过顾客显示屏开启钱箱 

            byte[] buffer = new byte[2];
            buffer[0] = 0x2;
            buffer[1] = 0x4D;
            this.SendData(buffer);
        }

        // 初始化 POS 
        public void Init()
        {
            //ASCII码 格式：ESC   @ 
            //十进制   格式：[027][064] 
            //十六进制 格式：[1BH][40H] 
            //说明：恢复到上电开机时的状态。 
            byte[] buffer = new byte[2];
            buffer[0] = 0x1B;
            buffer[1] = 0x40;
            this.SendData(buffer);
        }

        // 清屏幕
        public void Clear()
        {
            // ASCII码 格式：CLR 
            //十进制   格式：[012] 
            //十六进制 格式：[0CH] 
            //说明：清除屏幕上的所有字符。 
            byte[] buffer = new byte[1];
            buffer[0] = 0x0C;
            this.SendData(buffer);
        }

        // 显示信息 
        public void Display(string msg)
        {
            //ASCII码 格式：ESC   Q   A   d1d2d3…dn   CR 
            //十进制   格式：[027][081][065]d1d2d3…dn[013]     48<=dn<=57或dn=45或dn=46 
            //十六进制 格式：[1BH][51H][41H]d1d2d3…dn[0DH]     30H<=dn<=39H或dn=2DH或dn=2EH 
            //                                       
            //说明： 
            //a. 执行该命令时，会以覆盖模式送要显示的数据，这样就不需要在每次送显示数据前都去执行CAN清除光标行命令了。 
            //b. 显示的d1…dn没有小数点时1<=n<=8。 
            //c. 显示的d1…dn有小数点时1<=n<=15（8位数值+7位小数点）。 
            //d. 显示的内容可用CLR或CAN命令清除。 
            int length = msg.Length;
            if (length > 15) length = 15;
            byte[] buffer = new byte[length + 4];
            buffer[0] = 0x1B;
            buffer[1] = 0x51;
            buffer[2] = 0x41;
            int index = 3;
            for (int i = 0; i < length; i++)
            {
                buffer[index + i] = (byte)msg[i];
            }
            buffer[length + 3] = 0x0D;
            this.SendData(buffer);
        }

        // 设置显示模式 
        public void SetDisplayMode(DisplayMode mode)
        {
            //ASCII码 格式：ESC   s   n             0<=n<=4 
            //十进制   格式：[027][115] n           48<=n<=52 
            //十六进制 格式：[1BH][73H] n           30H<=n<=34H 
            //说明：(1)当 n=0，四种字符 全暗。 
            //(2)当 n=1，“单价”字符 亮，其它三种 全暗。 
            //(3)当 n=2，“总计”字符 亮，其它三种 全暗。 
            //(4)当 n=3，“收款”字符 亮，其它三种 全暗。 
            //(5)当 n=4，“找零”字符 亮，其它三种 全暗。 

            byte[] buffer = new byte[3];
            buffer[0] = 0x1B;
            buffer[1] = 0x73;
            buffer[2] = (byte)mode;

            this.SendData(buffer);
        }

        // 控制显示灯状态  
        public void SetDisplayStatus(bool Price, bool Total, bool Gethering, bool Balance)
        {
            //ACSII码 格式：STX   L   d1   d2   d3   d4       d=0、1 
            //十进制   格式：[002][076]d1 d2 d3 d4           d=048、049 
            //十六进制 格式：[02H][4CH]d1 d2 d3 d4           d=30H、31H 
            //说明：控制状态灯相应位的亮灭 
            //当d1=0时，单价灯灭；d1=1时，单价灯亮 
            //当d2=0时，总计灯灭；d1=1时，总计灯亮 
            //当d3=0时，收款灯灭；d1=1时，收款灯亮 
            //当d4=0时，找零灯灭；d1=1时，找零灯亮 

            byte[] buffer = new byte[6];
            buffer[0] = 0x02;
            buffer[1] = 0x4C;
            buffer[2] = (byte)(Price ? 49 : 48);
            buffer[3] = (byte)(Total ? 49 : 48);
            buffer[4] = (byte)(Gethering ? 49 : 48);
            buffer[5] = (byte)(Balance ? 49 : 48);
            this.SendData(buffer);
        }

        // 清除光标行 
        public void ClearLine()
        {
            //ASCII码 格式：CAN 
            //十进制   格式：[024] 
            //十六进制 格式：[18H] 
            //说明：清除光标行（数码行）上的字符，光标移动到第1位置，一般不需使用，只使用ESC   Q   A   d1d2d3…dn   CR命令即可。 

            byte[] buffer = new byte[1];
            buffer[0] = 024;
            this.SendData(buffer);
        }

        // 设置光标状态
        public void SetCursorMode(bool mode)
        {
            // ASCII码 格式：ESC   _   n         0<=n<=1 
            //十进制   格式：[027][095]n         48<=n<=49 
            //十六进制 格式：[1BH][5FH]n       30H<=n<=31H 
            //说明：这个命令一般不需使用。 
            //(1)当n=0时，光标 暗 
            //(2)当n=1时，光标 亮 

            byte[] buffer = new byte[3];
            buffer[0] = 0x1B;
            buffer[1] = 0x5F;
            buffer[2] = (byte)(mode ? 49 : 48);
            this.SendData(buffer);

        }

        // 移动光标位置 
        public void MoveCursor(int position)
        {
            //ASCII码 格式：ESC   I   n           1<=n<=8 
            //十进制   格式：[027][108]n         49<=n<=56 
            //十六进制 格式：[1BH][6CH]n       31H<=n<=38H 
            //说明：这个命令一般不需使用。把光标移动到第n位置。 

            if (position < 1 || position > 8)
            {
                throw new System.InvalidOperationException("位置只能是 1 - 8. ");
            }
            byte[] buffer = new byte[3];
            buffer[0] = 0x1B;
            buffer[1] = 0x6C;
            buffer[2] = (byte)(48 + position);
            this.SendData(buffer);
        }

        public enum DisplayMode
        {
            None = 0x30,        // 无 
            Price = 0x31,       // 价格 
            Total = 0x32,       // 合计 
            Gathering = 0x33,   // 收款 
            Balance = 0x34      // 余额 
        }
    }
}