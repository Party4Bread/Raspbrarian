using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace raspbrary
{

    public partial class Form2 : Form
    {
       
        public Form2()
        {
            InitializeComponent();
           
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

        private void Form2_Load(object sender, EventArgs e)
        {
            lblAuthor.Text = "";
        }
    }
}
