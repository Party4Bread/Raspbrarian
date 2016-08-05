using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections.Generic;

namespace raspbrary
{
    public partial class Form3 : Form
    {
        Book book;
  
       
        public Form3()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
              book = Book.GetBook(textBox1.Text);
            if (!book.haserror)
            {
                if (book.Nullerror)
                    MessageBox.Show("ISBN을 입력하세요!");
                else
                {
                    textBox2.Text = book.Title;
                    textBox3.Text = book.Author;
                    textBox4.Text = book.Publisher;

                    pictureBox1.Image = book.ConvertToImage(181, 235);
                }
            }
          
            else
            {
                MessageBox.Show("책을 찾을 수 없습니다. 책을 수동 등록하세요.");
            }
           




        }
        /* private Book getResponse(string isbn)
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
                 return new Book (title.Value, author.Value,publisher.Value,image.Value);
             }
             catch (Exception e)
             {
                 return new Book("Error");
             }
         }*/

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" || textBox3.Text != "" || textBox4.Text != "")
            {
                if (book.haserror || book.Nullerror)
                {
                   
                        book = new Book(textBox2.Text, textBox3.Text, textBox4.Text,false);
                   
                }
               
                    
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;
                    result = MessageBox.Show("이 정보가 맞습니까?\r\n" + book.ToString(), "책 정보확인", buttons);
                    if (result == DialogResult.Yes)
                    {
                        //DB등록
                    }


             
            }
            else
                MessageBox.Show("모든 정보를 입력하세요!");

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.StartPosition = FormStartPosition.Manual;
            frm1.Location = new Point(0, 0);
            frm1.Show();
            Dispose();
        }
    }
}

