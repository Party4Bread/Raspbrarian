using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace raspbrary
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[] info = getResponse(textBox1.Text);
            if ("nope" != info[0])
            {
                textBox2.Text = info[0];
                textBox3.Text = info[1];
                textBox4.Text = info[2];
                Bitmap result = new Bitmap(181, 235);
                using (Graphics g = Graphics.FromImage(result))
                    g.DrawImage(Form1.BitmapFromURL(info[3]), 0, 0, 181, 235);
                pictureBox1.Image = result;
            }
            else
            {
                MessageBox.Show("책을 수동 등록 해야 해!");
            }
        }
        private string[] getResponse(string isbn)
        {
            string URL = "https://openapi.naver.com/v1/search/book.xml",
            clientId = "0WW6gMJM8FBWvjfjBEeD",
            clientpassword = "obvwUCVcli";            
            WebClient wc = new WebClient();
            wc.Headers.Add("X-Naver-Client-Id", clientId);
            wc.Headers.Add("X-Naver-Client-Secret", clientpassword);
            wc.QueryString.Add("query", isbn);
            Stream stream = wc.OpenRead(URL);
            string response = new StreamReader(stream).ReadToEnd();
            wc.Dispose();
            stream.Dispose();
            try
            {
                //9788952210043
                XDocument xd = XDocument.Parse(response);
                XElement title = xd.Root.Element("channel").Element("item").Element("title");
                XElement author = xd.Root.Element("channel").Element("item").Element("author");
                XElement publisher = xd.Root.Element("channel").Element("item").Element("publisher");
                XElement image = xd.Root.Element("channel").Element("item").Element("image");
                return new string[] {title.Value, author.Value,publisher.Value,image.Value};
            }
            catch (Exception e)
            {
                return new string[] { "nope" };
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
