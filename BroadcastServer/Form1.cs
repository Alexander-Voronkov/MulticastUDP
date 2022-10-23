using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BroadcastServer
{
    public partial class Form1 : Form
    {
        IPAddress multicastIP;
        int localPort;
        UdpClient server;
        public Form1()
        {
            InitializeComponent();
            SendBtn.Enabled = false;
            MessageRTB.Enabled = false; 
            comboBox1.Items.AddRange(new string[] { "Advertisement", "Activities", "Overtime work" });
            comboBox1.SelectedIndex = 0;
        }

        private void BindBtn_Click(object sender, EventArgs e)
        {
            if (BindBtn.Text == "Bind")
            {
                if (IPAddress.TryParse(multicastIPTB.Text, out multicastIP) == false)
                {
                    MessageBox.Show("Wrong IP!");
                    return;
                }
                if (int.TryParse(PortTB.Text, out localPort) == false)
                {
                    MessageBox.Show("Wrong port!");
                    return;
                }
                try
                {
                    server = new UdpClient();
                    BindBtn.Text = "Unbind";
                }
                catch
                {
                    MessageBox.Show("Not multicast address! Try another one!");
                    return;
                }
                SendBtn.Enabled = true;
                MessageRTB.Enabled = true;
            }
            else
            {
                try
                {
                    server.Close();
                }
                catch { }
                BindBtn.Text = "Bind";
                SendBtn.Enabled = false;
                MessageRTB.Enabled = false;
            }
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buff = Encoding.UTF8.GetBytes(MessageRTB.Text+"\n"+comboBox1.SelectedItem.ToString());
                server.Send(buff, buff.Length, new IPEndPoint(multicastIP,1000));
            }
            catch { }
        }
    }
}
