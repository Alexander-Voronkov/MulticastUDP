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
        const int ReceiverPort = 2000;
        const string ReceiverIP = "239.0.0.1";
        UdpClient server;
        
        public Form1()
        {
            InitializeComponent();
            SendBtn.Enabled = false;
            MessageRTB.Enabled = false; 
            comboBox1.Items.AddRange(new string[] { "Advertisement", "Activities", "Overtime work" });
            comboBox1.SelectedIndex = 0;
            Connect();
        }

        private void Connect()
        {
            try
            {
                server = new UdpClient();
                multicastIP = IPAddress.Parse(ReceiverIP);
            }
            catch
            {
                MessageBox.Show("Error!");
                this.Close();
                return;
            }
            SendBtn.Enabled = true;
            MessageRTB.Enabled = true;
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] buff = Encoding.UTF8.GetBytes(MessageRTB.Text+"\n"+comboBox1.SelectedItem.ToString());
                server.Send(buff, buff.Length, new IPEndPoint(multicastIP,ReceiverPort));
            }
            catch { }
        }
    }
}
