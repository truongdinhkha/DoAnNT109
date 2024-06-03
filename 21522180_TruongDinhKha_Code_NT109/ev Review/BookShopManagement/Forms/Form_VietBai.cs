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

namespace BookShopManagement
{
    public partial class Form_VietBai : Form
    {
        private const int Port_Number = 1234;
        private const string Server_IP = "127.0.0.1";
        private Connect connect;
        private SqlConnection connection;
        string ConnectionString = @"Data Source=DINH-KHA\SQLEXPRESS;Initial Catalog=model;Integrated Security=True";
        public Form_VietBai()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string company_name = textBox1.Text;
            string title = textBox2.Text;
            string content = richTextBox1.Text;
            int star = Int32.Parse(textBox3.Text);
            string date = dateTimePicker1.Text;

            string request = $"New_Post/{company_name}/{title}/{content}/{star}/{date}";
            byte[] requestBytes = Encoding.ASCII.GetBytes(request);
            //string Name_Company,string title,string content,string Star,string Date,string ID_Nguoi_Dung
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

                    if (response == "POST_SUCCESS")
                    {
                        MessageBox.Show("Đăng bài thành công!");
                    }
                    else if (response == "POST_FAILURE")
                    {
                        MessageBox.Show("Đăng bài thất bại!");
                    }
                    else
                    {
                        MessageBox.Show("Khong xu ly duoc");
                    }
                    stream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối đến server: " + ex.Message);
                }
            }


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
