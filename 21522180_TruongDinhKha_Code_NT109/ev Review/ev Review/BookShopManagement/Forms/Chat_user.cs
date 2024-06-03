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
using System.Net.Sockets;
using System.Threading;

namespace BookShopManagement.Forms
{
    public partial class Chat_user : Form
    {
        private const int Port_Number = 1235;
        private const string Server_IP = "127.0.0.1";

        public Chat_user()
        {
            InitializeComponent();
        }

        private void Chat_user_Load(object sender, EventArgs e)
        {
           
        }
        private void ReadMessages(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            // Buffer for reading data
            byte[] bytes = new byte[1024];
            int bytesRead;

            while (true)
            {
                try
                {
                    // Read incoming message
                    bytesRead = stream.Read(bytes, 0, bytes.Length);
                    if (bytesRead == 0)
                    {
                        // Server disconnected
                        break;
                    }

                    // Convert the bytes received to a string and display it.
                    string message = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                    listView1.Items.Add(new ListViewItem("User:") + message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e);
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NguoiNhan = guna2TextBox2.Text;
            string TinNhan = guna2TextBox1.Text;

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(Server_IP, Port_Number);

                    NetworkStream stream = client.GetStream();
                    Thread readThread = new Thread(ReadMessages);
                    readThread.Start(client);
                    string fullMessage = "Chat_user:" + "," + NguoiNhan + "," + TinNhan;
                    byte[] data = Encoding.ASCII.GetBytes(fullMessage);
                    stream.Write(data, 0, data.Length);
                    listView1.Items.Add(new ListViewItem("ME:") + TinNhan);
                    guna2TextBox1.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối đến server: " + ex.Message);
                }
            }
        }
    }
}

