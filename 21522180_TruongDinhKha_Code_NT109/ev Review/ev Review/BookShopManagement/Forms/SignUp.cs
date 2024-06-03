using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace BookShopManagement.Forms
{
    public partial class SignUp : Form
    {
        private const int Port_Number = 1234;
        private const string Server_IP = "127.0.0.1";

        private Connect connect;
        private SqlConnection connection;

        public SignUp()
        {
            InitializeComponent();
            connect = new Connect();
            connection = connect.GetConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Lấy thông tin đăng ký từ các TextBox
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Gửi yêu cầu đăng ký tới server Socket
            string requestData = $"REGISTER/{username}/{password}";
            byte[] requestBytes = Encoding.ASCII.GetBytes(requestData);

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(Server_IP, Port_Number);

                    NetworkStream stream = client.GetStream();

                    // Gửi yêu cầu đăng ký
                    stream.Write(requestBytes, 0, requestBytes.Length);

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);

                    // Xử lý phản hồi từ server và hiển thị kết quả lên giao diện người dùng
                    if (response == "REGISTER_SUCCESS")
                    {
                        MessageBox.Show("Đăng ký thành công!");
                    }
                    else if (response == "REGISTER_FAILURE_USERNAME")
                    { 
                         MessageBox.Show("Tên đăng nhập đã được sử dụng!");
                    }
                    else
                    {
                        MessageBox.Show("Đăng ký thất bại!");
                    }

                    stream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối đến server: " + ex.Message);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 Sign_in = new Form1();
            Sign_in.ShowDialog();
            this.Close();
        }
    }
}
