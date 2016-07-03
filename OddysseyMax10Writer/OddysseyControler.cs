﻿using OddysseyMax10Writer.SerialState;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddysseyMax10Writer
{
    public delegate void SerialIOEvent(String s);
    public class OddysseyControler : ISerialReadWrite
    {
        private SerialPort mSerialPort;
        private ISerialState mStatus;
        public event SerialIOEvent OnSerialIO;
        
        private string GetPortName()
        {
            string[] devs = SelialPortFinder.GetDeviceNames();
            string p = null;
            foreach (string s in devs)
            {
                if (s.Contains("Silicon Labs")) p = s;
            }
            string[] ports = SerialPort.GetPortNames();
            foreach (string s in ports) {
                if (p.Contains(s)) return s;
            }

            
            return null;
        }
        public void Connect()
        {
            string port = GetPortName();
            if (port == null) throw new Exception("Port NF");
            mSerialPort = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
            mSerialPort.Open();
            mSerialPort.NewLine = "\n";
            mStatus = InitialState.GetInstance();
        }

        public void GoMenu()
        {
           mStatus =  mStatus.Exec(this, TargetState.MENU);

        }

        public void Write(String path)
        {
            GoMenu();
            mStatus = mStatus.Exec(this, TargetState.WRITE);

            Debug.WriteLine("SEND");
            FileStream f = File.OpenRead(path);
            byte[] buf = new byte[1024];
            int l;
            while((l = f.Read(buf, 0, buf.Length)) > 0)
            {
                mSerialPort.Write(buf, 0, l);
            }
            Debug.WriteLine("DONE");
            mStatus = mStatus.Exec(this, TargetState.FINISH_WRITE);
            WriteLine("01");// 保存場所 01~10

            GoMenu();

        }

        public void Program()
        {
            GoMenu();
            mStatus = mStatus.Exec(this, TargetState.PROGRAM);
            WriteLine("01");// 保存場所 01~10
            mStatus = mStatus.Exec(this, TargetState.FINISH_PROGRAM);
            GoMenu();
        }

        public void WriteLine(string s = "")
        {
            mSerialPort.Write(s + "\r");
            
            Debug.WriteLine("W:"+s);
            OnSerialIO(">"+s);
        }

        public string TryReadLine(int ms = 1000)
        {
            int t = mSerialPort.ReadTimeout;
            mSerialPort.ReadTimeout = ms;
            string r = null;
            try
            {
                r = ReadLine();
            }
            catch (TimeoutException e) { }
            mSerialPort.ReadTimeout = t;
            return r;
        }
        

        public string ReadLine()
        {
            
            string r = mSerialPort.ReadLine();
            Debug.WriteLine("R:" + r);
            OnSerialIO(r);
            return r;
        }
    }
}
