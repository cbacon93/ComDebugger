using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace ComDebugger
{
    public partial class Form1 : Form
    {
        //public static Form1 instance;
        public SerialPort sp = new SerialPort();
        public bool running = false;
        private Thread readThr;

        delegate void SetTextCallback(string text);

        public Form1()
        {
            readThr = new Thread(readThread);
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            comboBox3.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sp.PortName = textBox1.Text;
                sp.BaudRate = Int32.Parse(textBox3.Text);
                sp.DataBits = Int32.Parse(textBox5.Text);

                switch (comboBox1.SelectedItem.ToString().ToLower())
                {
                    default:
                    case ("none"):
                        sp.Parity = Parity.None;
                        break;
                    case ("even"):
                        sp.Parity = Parity.Even;
                        break;
                    case ("mark"):
                        sp.Parity = Parity.Mark;
                        break;
                    case ("odd"):
                        sp.Parity = Parity.Odd;
                        break;
                    case ("space"):
                        sp.Parity = Parity.Space;
                        break;
                }


                
                

                switch (comboBox2.SelectedItem.ToString().ToLower())
                {
                    default:
                    case ("none"):
                        sp.StopBits = StopBits.None;
                        break;
                    case ("one"):
                        sp.StopBits = StopBits.One;
                        break;
                    case ("two"):
                        sp.StopBits = StopBits.Two;
                        break;
                    case ("one point five"):
                        sp.StopBits = StopBits.OnePointFive;
                        break;
                }


                switch (comboBox3.SelectedItem.ToString().ToLower())
                {
                    default:
                    case ("none"):
                        sp.Handshake = Handshake.None;
                        break;
                    case ("request to send"):
                        sp.Handshake = Handshake.RequestToSend;
                        break;
                    case ("request to send xonxoff"):
                        sp.Handshake = Handshake.RequestToSendXOnXOff;
                        break;
                    case ("xonxoff"):
                        sp.Handshake = Handshake.XOnXOff;
                        break;
                }
                

                sp.ReadTimeout = 5000;
                sp.WriteTimeout = 5000;

                sp.Open();
                running = true;
                readThr.Start();
            }
            catch (Exception ex)
            {
                AddText(ex.Message + "\n" + ex.StackTrace + "\n");
                button2_Click(sender, e);
            }
        }


        public void readThread() {
            while (running)
            {
                try
                {
                    string msg = sp.ReadLine();
                    AddText(msg + "\n");
                }
                catch (Exception e) { }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                running = false;
                sp.Close();
                readThr.Join();
                readThr = new Thread(readThread);
            }
            catch (Exception ex) { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sp != null)
            {
                try
                {
                    sp.WriteLine(textBox2.Text);
                }
                catch (Exception ex)
                {
                    AddText(ex.Message + "\n" + ex.StackTrace + "\n");
                }
            }
        }


        private void AddText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AddText);
                Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.Text += text;
            }
            
        }

    }
}
