using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BroadcastClient
{
    public partial class Form1 : Form
    {
        const string Ip = "239.0.0.1";
        const int Port = 2000;
        UdpClient client;
        IPEndPoint point = null;
        public Form1()
        {
            InitializeComponent();
            ReceivedRTB.Enabled = false;
            comboBox1.Items.AddRange(new string[] {"All", "Advertisement", "Activities", "Overtime work"});
            comboBox1.SelectedIndex = 0;
            Connect();
        }

        private void Connect()
        {
            try
            {
                client = new UdpClient(Port);
                client.JoinMulticastGroup(IPAddress.Parse(Ip));
            }
            catch
            {
                MessageBox.Show("Error!");
                return;
            }
            Task.Run(Receive);
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    do
                    {
                        byte[] buff = client.Receive(ref point);
                        builder.Append(Encoding.UTF8.GetString(buff));
                    }
                    while (client.Available > 0);
                    var q = builder.ToString().Split('\n');
                    if (q[q.Length-1] ==comboBox1.SelectedItem.ToString()||comboBox1.SelectedItem.ToString()=="All")
                    {
                        ReceivedRTB.Text = builder.ToString();
                    }
                }
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                client.DropMulticastGroup(IPAddress.Parse(Ip));
            }
            catch { }
        }
    }
}
