using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace raspbrary
{
    class Book
    {
        
        private string title,author,publisher,imageUrl;
        public bool haserror;
        public bool Nullerror = false;
        static  string ConnectURL = "https://openapi.naver.com/v1/search/book.xml",
                clientId = "0WW6gMJM8FBWvjfjBEeD",
                clientpassword = "obvwUCVcli";
        public Book(string Title,string Author,string Publisher,string ImageUrl,bool error)//일반적 생성자
        {
            title = Title;
            author = Author;
            publisher = Publisher;
            imageUrl = ImageUrl;
            haserror = error;
        }
        public Book(string Title,string Author,string Publisher,bool error)//이미지URL없을시(수동입력)
        {

            title = Title;
            author = Author;
            publisher = Publisher;
            imageUrl = "";
            haserror = error;
        }
        public Book(string ErrorString, bool error)//에러발생시
        {
            if (error)
            {
                if (ErrorString == "Error")
                    haserror = true;
                if (ErrorString == "NullError")
                    Nullerror = true;
            }
        }
      
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }
       public string ImageURL
        {
            get { return imageUrl;}
            set { imageUrl=value;}
        }
      /* public static Image MakeImage(string imageUrl,Size size)//이미지만드는정적메소드
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(imageUrl);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;
                
                Bitmap resizeImage = new Bitmap(DownloadImage, size);
                return resizeImage;
            }
            catch (Exception)
            {
                return null;
            }
        }*/
        public static Image MakeImage(string imageUrl, int Width,int Height)//이미지만드는정적메소드
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(imageUrl);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;
                Size size = new Size(Width, Height);
                Bitmap resizeImage = new Bitmap(DownloadImage, size);
                return resizeImage;
            }
            catch (Exception)
            {
                return null;
            }
        }
      /*  public Image ConvertToImage(Size size)//URL설정된경우 URL->Img
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(imageUrl);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;

                Bitmap resizeImage = new Bitmap(DownloadImage, size);
                return resizeImage;
            }
            catch (Exception)
            {
                return null;
            }

        }*/
        public Image ConvertToImage(int Width, int Height)//이미지만드는정적메소드
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(imageUrl);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;
                Size size = new Size(Width, Height);
                Bitmap resizeImage = new Bitmap(DownloadImage, size);
                return resizeImage;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public override string ToString()
        {
            return "책제목:"+title+"\r\n저자:"+author+"\r\n출판사:"+publisher;
        }
        public static Book GetBook(string isbn)
        {
            if (isbn != "")
            {
                WebClient wc = new WebClient();
                wc.Headers.Add("X-Naver-Client-Id", clientId);
                wc.Headers.Add("X-Naver-Client-Secret", clientpassword);
                wc.QueryString.Add("query", isbn);
                try
                {
                    Stream stream = wc.OpenRead(ConnectURL);
                    string response = new StreamReader(stream).ReadToEnd();
                    wc.Dispose();
                    stream.Dispose();

                    //9788952210043
                    XDocument xd = XDocument.Parse(response);
                    XElement title = xd.Root.Element("channel").Element("item").Element("title");
                    XElement author = xd.Root.Element("channel").Element("item").Element("author");
                    XElement publisher = xd.Root.Element("channel").Element("item").Element("publisher");
                    XElement image = xd.Root.Element("channel").Element("item").Element("image");
                    return new Book(title.Value, author.Value, publisher.Value, image.Value,false);
                }
                catch (WebException we)
                {
                    MessageBox.Show(((HttpWebResponse)we.Response).StatusDescription);
                    return new Book("Error",true);
                }
                catch (Exception)
                {
                    return new Book("Error",true);
                }
            }
            else
            {
               
                return new Book("NullError",true);
            }
            }
        


    }
}
