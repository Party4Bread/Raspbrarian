﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace raspbrary
{
    public partial class starter : Form
    {
        public starter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void starter_Load(object sender, EventArgs e)
        {

            Form1 frm1 = new Form1();

            frm1.StartPosition = FormStartPosition.Manual;
            frm1.Location = new Point(0, 0);
            frm1.Show();
            this.WindowState = FormWindowState.Minimized;
           
        }
    }
}
