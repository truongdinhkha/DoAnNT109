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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;
using System.Net.Sockets;

namespace BookShopManagement.Forms
{

    public partial class Bank : Form
    {
        int PanelWidth;
        bool isCollapsed;
        private const int Port_Number = 1234;
        private const string Server_IP = "127.0.0.1";
        TcpClient client;
        string userName;
        public Bank(string user)
        {
            InitializeComponent();
            PanelWidth = panelLeft.Width;
            isCollapsed = false;
            userName = user;
        }
        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(c);
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnHome);

        }
        private void moveSidePanel(Control btn)
        {
            panelSide.Top = btn.Top;
            panelSide.Height = btn.Height;
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

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Clear();
            Button addButton = new Button();
            addButton.Text = "button1";
            addButton.Click += button1_Click;
            addButton.Width = 100;
            addButton.Height = 50;
            flowLayoutPanel1.Controls.Add(addButton);
        }

        private void Bank_Load(object sender, EventArgs e)
        {
            /*
            try
            {
                // Send request to the server to get companies for the selected industry
                string request = "GET_COMPANIES," + selectedIndustry;
                byte[] requestData = Encoding.ASCII.GetBytes(request);
                NetworkStream stream = client.GetStream();
                stream.Write(requestData, 0, requestData.Length);

                // Read response from the server
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Process the response from the server and display it on the user interface
                string[] companies = response.Split(',');
                

                // Close the TCP client connection
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            */
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form_VietBai form = new Form_VietBai(userName);
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reading eva = new Reading();
            flowLayoutPanel1.Controls.Add(eva);
        }
    }
}
