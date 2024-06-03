using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookShopManagement.Forms;

namespace BookShopManagement.UserControls
{
    public partial class UC_Home : UserControl
    {
        string userName;
        public UC_Home(string user)
        {
            InitializeComponent();
            userName = user;
        }
        Random rand = new Random();

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Bank_Click(object sender, EventArgs e)
        {
            Bank bank = new Bank(userName);
            bank.ShowDialog();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Bank bank = new Bank(userName);
            bank.ShowDialog();
        }

        /*private void LoadChart()
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadChart();
        }*/


    }
}
