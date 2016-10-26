using System;
using System.Drawing;
using System.Windows.Forms;

namespace raspbrary
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            lblAuthor.Text = "";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.StartPosition = FormStartPosition.Manual;
            frm1.Location = new Point(0, 0);
            frm1.Show();
            Dispose();
        }
    }
}
