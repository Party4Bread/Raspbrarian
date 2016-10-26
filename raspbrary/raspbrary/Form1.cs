using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
namespace raspbrary
{
    public partial class Form1 : Form
    {
        string[] url = { "http://cafeptthumb4.phinf.naver.net/20140206_205/zpdl92_1391649626552SIpGd_JPEG/x9788996957652.jpg?type=w740", "http://bimage.interpark.com/goods_image/5/9/1/8/212675918g.jpg","http://bimage.interpark.com/milti/renewPark/main/102563_201607291504118231.jpg" };
      
        public Form1()
        {
            InitializeComponent();
           

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.StartPosition = FormStartPosition.Manual;
            frm2.Location = new Point(0, 0);
            frm2.Show();
            Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Random rd = new Random();
            
            pictureBox1.Image = BitmapFromURL(url[rd.Next(0, url.Length - 1)]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.StartPosition = FormStartPosition.Manual;
            frm3.Location = new Point(0, 0);
            frm3.Show();
            Dispose();
        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        public static Bitmap BitmapFromURL(string URL)
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(URL);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;

                int width = 242;
                int height = 386;
                Size resize = new Size(width, height);
                Bitmap resizeImage = new Bitmap(DownloadImage, resize);
                return resizeImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
