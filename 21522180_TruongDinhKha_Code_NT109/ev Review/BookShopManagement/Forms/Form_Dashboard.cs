using BookShopManagement.UserControls;
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
    public partial class Form_Dashboard : Form
    {
        int PanelWidth;
        bool isCollapsed;
        private const int Port_Number = 1234;
        private const string Server_IP = "127.0.0.1";
        private TcpClient client;
        public Form_Dashboard()
        {
            InitializeComponent();
            timerTime.Start();
            PanelWidth = panelLeft.Width;
            isCollapsed = false;
            UC_Home uch = new UC_Home();
            AddControlsToPanel(uch);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_Dashboard_Load(object sender, EventArgs e)
        {
            /*
            try
            {
                // Tạo kết nối TCP đến server
                client = new TcpClient();
                client.Connect(Server_IP, Port_Number);

                // Gửi yêu cầu lấy danh sách ngành đến server
                string request = "GET_INDUSTRIES";
                byte[] requestData = Encoding.ASCII.GetBytes(request);
                client.GetStream().Write(requestData, 0, requestData.Length);

                // Đọc phản hồi từ server
                byte[] buffer = new byte[1024];
                int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                string[] industryData = response.Split(',');

                for (int i = 0; i < industryData.Length; i += 2)
                {
                    if (int.TryParse(industryData[i], out int industryId))
                    {
                        string industryName = industryData[i + 1];
                        Industry industry = new Industry(industryId, industryName);
                        
                    }
                }

                // Đóng kết nối TCP
                client.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error loading industries: " + ex.Message);
            } 
            */

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                panelLeft.Width = panelLeft.Width + 10;
                if (panelLeft.Width >= PanelWidth)
                {
                    timer1.Stop();
                    isCollapsed = false;
                    this.Refresh();
                }
            }
            else
            {
                panelLeft.Width = panelLeft.Width - 10;
                if (panelLeft.Width <= 59)
                {
                    timer1.Stop();
                    isCollapsed = true;
                    this.Refresh();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        // Di chueyển thanh chuyển hướng
        private void moveSidePanel(Control btn)
        {
            panelSide.Top = btn.Top;
            panelSide.Height = btn.Height;
        }

        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panel1.Controls.Clear();

            panel1.Controls.Add(c);
        }
        private void btnHome_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnHome);

            UC_Home uch = new UC_Home();
            AddControlsToPanel(uch);
        }
        /*moveSidePanel(btnSaleBooks);
        UC_Sales us = new UC_Sales();
        AddControlsToPanel(us);*/

        
        private void button7_Click(object sender, EventArgs e)
        {
           // moveSidePanel(btnSettings);
        }

        private void btnSaleBooks_Click(object sender, EventArgs e)
        {
            Bank us = new Bank();
            us.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(MethodA); 
            a.Start();
        }
        private void MethodA()
        {
        Chat_user chat = new Chat_user();
            chat.ShowDialog();
        }    

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
        
            Thread a = new Thread(MethodB);
            a.Start();
        }
        private void MethodC()
        {
            Form_VietBai form = new Form_VietBai();
            form.ShowDialog();
        }
        private void btnUsers_Click(object sender, EventArgs e)
        {
           
            Thread a = new Thread(MethodB);
            a.Start();
        }
        private void MethodB()
        {
            
        }
    }
}
