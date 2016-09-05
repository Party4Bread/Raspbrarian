using Rasbrary.uni;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace Rasbrary.uni
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class regFrm : Page
    {
        public regFrm()
        {
            this.InitializeComponent();
        }
        string[] book;
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await getResponse(textBox.Text);
            if ("nope" != book[0])
            {
                textBox1.Text = book[0];
                textBox2.Text = book[1];
                textBox3.Text = book[2];
                image2.Source = new BitmapImage(new Uri(book[3], UriKind.Absolute));
            }
            else
            {
                var dialog = new MessageDialog("책을 수동 등록 해야 해!");
                await dialog.ShowAsync();
            }
        }
        private async Task getResponse(string isbn)//Task.run() 으로 동기명령수행
        {
            string URL = "https://openapi.naver.com/v1/search/book.xml",
            clientId = "0WW6gMJM8FBWvjfjBEeD",
            clientpassword = "obvwUCVcli";
            HttpWebRequest wReq;
            WebResponse wRes;
            try
            {
                Uri uri = new Uri(URL+"?query="+isbn.ToString()); // string 을 Uri 로 형변환
                wReq = (HttpWebRequest)WebRequest.Create(uri); // WebRequest 객체 형성 및 HttpWebRequest 로 형변환
                wReq.Method = "GET"; // 전송 방법 "GET" or "POST" 
                wReq.Headers["X-Naver-Client-Id"] = clientId;
                wReq.Headers["X-Naver-Client-Secret"] = clientpassword;
                using (wRes = await wReq.GetResponseAsync())
                {                   
                    Stream respPostStream = wRes.GetResponseStream();
                    StreamReader readerPost = new StreamReader(respPostStream, Encoding.GetEncoding("UTF-8"), true);
                    string response = readerPost.ReadToEnd();
                    try
                    {
                        //9788952210043s
                        XDocument xd = XDocument.Parse(response);                        
                        XElement title = xd.Root.Element("channel").Element("item").Element("title");
                        XElement author = xd.Root.Element("channel").Element("item").Element("author");
                        XElement publisher = xd.Root.Element("channel").Element("item").Element("publisher");
                        XElement image = xd.Root.Element("channel").Element("item").Element("image");
                        book = new string[] { title.Value, author.Value, publisher.Value, image.Value };
                    }
                    catch (Exception e)
                    {
                        book = new string[] { "nope" };
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {                        
                    }
                    else
                    {                        
                    }
                }
                else
                {                    
                }
            }            
        }
        private void textBox_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            textBox.Text = "";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(mainFrm));
        }
    }
}
