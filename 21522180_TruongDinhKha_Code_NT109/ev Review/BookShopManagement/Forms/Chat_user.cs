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
using Guna.Charts.WinForms;
using System.Web.UI.WebControls;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BookShopManagement.Forms
{
    public partial class Chat_user : Form
    {
        private bool connect = false;
        private const int Port_Number = 5000;
        private const string Server_IP = "127.0.0.1";
        //Socket client;
        NetworkStream stream;

  
        TcpClient client1;
        Dictionary<string, string> users = new Dictionary<string, string>();
        public Chat_user()
        {
            InitializeComponent();
           
        }
        private void connect_Server()
        {
            try
            {
                client1 = new TcpClient("127.0.0.1", 5000);
                button2.Text = "Disconnect";
                connect = true;


                stream = client1.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes("@userName: " + guna2TextBox4.Text);
                stream.Write(buffer, 0, buffer.Length);
                Task.Run(() => receiveMessage());
            }
            catch (Exception ex)
            {
                show($"[{DateTime.Now}] Error connecting to server: {ex.Message}");
            }
        }
        private void receiveMessage()
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (connect)
                {
                    int byteReads = 0;

                    try
                    {
                        byteReads = stream.Read(buffer, 0, buffer.Length);
                    }
                    catch { }

                    if (byteReads == 0)
                    {
                        show($"[{DateTime.Now}] You have disconnect to server");
                        connect = false;
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, byteReads);
                    if (message.Contains("@participants"))
                    {
                        Task.Run(() => showDataGridView(message));
                    }
                    else if (message == "@Server is shutdown")
                    {
                        string[] shutdownMessage = message.Split('@');
                        show($"[{DateTime.Now}] {shutdownMessage[1]}");
                        invokeDisconnect();
                    }
                    else if (message.Contains("@privatechatstart\\"))
                    {
                        string[] fromUser = message.Split('-');


                    }
                    else if (message.Contains("@private"))
                    {

                    }
                    else
                    {
                        show($"[{DateTime.Now}] server: {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                show($"[{DateTime.Now}] Error receive message from server: {ex.Message}");
            }
        }
        private void Chat_user_Load(object sender, EventArgs e)
        {
           
        }
        /*
        private void ReadMessages(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            stream = client.GetStream();

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
                    else
                    {
                 string message = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                    listView1.Items.Add(new ListViewItem("User:") + message);
                    }
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e);
                    break;
                }
            }
        }
        */
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Connect";
            if (string.IsNullOrEmpty(guna2TextBox4.Text))
            {
                MessageBox.Show("Please enter user name!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            if (!connect)
            {
                try
                {
                    connect_Server();
                    button1.Text = "Send";
                }
                catch(Exception ex) { MessageBox.Show("Can't connect to server!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
          

            if (connect)
            {
                try
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(guna2TextBox1.Text);
                    stream.Write(buffer, 0, buffer.Length);
                    show($"[{DateTime.Now}] You: {guna2TextBox1.Text}");
                    guna2TextBox1.Clear();
                }
                catch (Exception ex)
                {
                    show($"Error sending message: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please connect to server!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void show(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => show(message)));
            }
            else
            {
                richTextBox1.AppendText(message + Environment.NewLine);
            }
        }
       
        private void invokeDisconnect()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => invokeDisconnect()));
            }
            else
            {

                Disconnect();
            }
        }
        private void showDataGridView(string message)
        {
            string[] participants = message.Split('/');
            appendDataGridView();
        }

        private void appendDataGridView()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => appendDataGridView()));
            }

        }
    
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Thoát?");
            try
            {
        
                client1?.Close();
                this.Close();
            }
            catch { }
        }
        private void Disconnect()
        {
            if (connect)
            {
                button2.Text = "Connect";
                connect = false;

                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                if (client1 != null)
                {
                    client1.Close();
                    client1 = null;
                }
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

