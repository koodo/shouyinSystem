using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pc_system.View
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Hide();
            panel1.Show();
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            button1.Image = pc_system.Resource1.交接班h;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.Image = pc_system.Resource1.交接班f;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            panel3.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_MouseMove(object sender, MouseEventArgs e)
        {
            button4.Image = pc_system.Resource1.新增会员h;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.Image = pc_system.Resource1.新增会员f;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button6_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            button2.Image = pc_system.Resource1.订货h;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.Image = pc_system.Resource1.订货f;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.Image = pc_system.Resource1.商品编辑f;
        }

        private void button3_MouseMove(object sender, MouseEventArgs e)
        {
            button3.Image = pc_system.Resource1.商品编辑h;
        }

        private void button5_MouseMove(object sender, MouseEventArgs e)
        {
            button5.Image = pc_system.Resource1.系统设置h;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.Image = pc_system.Resource1.系统设置f;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            panel3.Show();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
