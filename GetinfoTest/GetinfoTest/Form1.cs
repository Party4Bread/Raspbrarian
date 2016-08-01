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
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace GetinfoTest
{
    public partial class Form1 : Form
    {


        WebClient wc;

        string URL;
        string clientId = "ZTrH5_C5GQuf8FCUp7ol";
        string clientpassword= "XBjBenl1U4";
        string result;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtResult.ReadOnly = true;
          
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
            URL = "https://openapi.naver.com/v1/search/book.xml";
          
            if (!getResponse())
                return;
            else
                MessageBox.Show("성공!");
                
        }
        private bool getResponse()
        {
            wc = new WebClient();

            wc.Headers.Add("X-Naver-Client-Id", clientId);
            wc.Headers.Add("X-Naver-Client-Secret", clientpassword);
            wc.QueryString.Add("query",txtquery.Text);
            Stream stream = wc.OpenRead(URL);
          
            string response = new StreamReader(stream).ReadToEnd();
           
            wc.Dispose();
            stream.Dispose();
            try {
                XDocument xd = XDocument.Parse(response);
                XElement title = xd.Root.Element("channel").Element("item").Element("title");
                XElement author = xd.Root.Element("channel").Element("item").Element("author");
                XElement publisher = xd.Root.Element("channel").Element("item").Element("publisher");

                result += "제목:";
                result += title.Value;

                result += "\r\n작가:";
                result += author.Value;

                result += "\r\n출판사:";
                result += publisher.Value;
                MessageBox.Show(result);
                txtResult.Text = result;
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("책이 없네요...");
            }
           
            




        }
    }
}
