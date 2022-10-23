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
        IPAddress Ip;
        int Port;
        UdpClient client;
        IPEndPoint point = null;
        public Form1()
        {
            InitializeComponent();
            ReceivedRTB.Enabled = false;
            comboBox1.Items.AddRange(new string[] {"All", "Advertisement", "Activities", "Overtime work"});
            comboBox1.SelectedIndex = 0;
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if (ConnectBtn.Text == "Connect")
            {
                if (IPAddress.TryParse(IPTB.Text, out Ip) == false)
                {
                    MessageBox.Show("Wrong IP!");
                    return;
                }
                if (int.TryParse(PortTB.Text, out Port) == false)
                {
                    MessageBox.Show("Wrong port!");
                    return;
                }
                try
                {
                    client = new UdpClient(Port);
                    client.JoinMulticastGroup(Ip);
                    ConnectBtn.Text = "Disconnect";
                }
                catch
                {
                    MessageBox.Show("Not multicast address! Try another one!");
                    return;
                }
                Task.Run(Receive);
            }
            else
            {
                try
                {
                    ConnectBtn.Text = "Connect";
                    client.DropMulticastGroup(Ip);
                    client.Close(); 
                }
                catch { }
            }
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
    }
}
