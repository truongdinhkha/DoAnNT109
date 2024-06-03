using BookShopManagement.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Net;

namespace BookShopManagement
{
    public partial class Form1 : Form
    {
        // Địa chỉ IP và cổng của server
        private const string ServerIP = "127.0.0.1";
        private const int ServerPort = 1234;
        private TcpClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Tạo kết nối TCP đến server
            client = new TcpClient();
            client.Connect(ServerIP, ServerPort);

            // Gửi yêu cầu đăng nhập đến server
            string request = "LOGIN/" + textBox1.Text + "/" + textBox2.Text;
            byte[] requestData = Encoding.ASCII.GetBytes(request);
            client.GetStream().Write(requestData, 0, requestData.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            if (response == "LOGIN_SUCCESS")
            {

                MessageBox.Show("Đăng nhập thành công!");

                Form_Dashboard form_Dashboard = new Form_Dashboard();
                form_Dashboard.ShowDialog();
                this.Close();
            }
            else
            {
                // Đăng nhập thất bại
                MessageBox.Show("Đăng nhập thất bại!");

            }


            client.Close();         
        }
        private void Sign_up_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.ShowDialog();
        }
        private void Close_bttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}

