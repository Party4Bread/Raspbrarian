using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

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
            await searchBookNaverapi(textBox.Text, false);
        }
        private async Task searchBookNaverapi(string data,bool quiet)
        {
            await getResponse(data);
            if ("nope" != book[0])
            {
                textBox1.Text = book[0];
                textBox2.Text = book[1];
                textBox3.Text = book[2];
                image2.Source = new BitmapImage(new Uri(book[3], UriKind.Absolute));
            }
            else
            {
                if(!quiet)
                    Function.ShowMessage("책을 수동 등록 해야 해!");
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
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Properties.Clear();
            Frame.Navigate(typeof(mainFrm));
        }
        private async void confirm(object sender, RoutedEventArgs e)
        {
            
            //CoreApplication.Properties.Add("ISBN", textBox.Text);
            if (textBox1.Text != "" || textBox2.Text != ""||textBox3.Text!="")
            {
                Data.SetBook(new Book { Name = textBox1.Text, Auther = textBox2.Text,Publisher=textBox3.Text,ISBN=textBox.Text,image=book[3]});
                var dialog = new MessageDialog("제목:" + textBox1.Text + "\r\n" + "저자:" + textBox2.Text + "\r\n" + "출판사:" + textBox3.Text, "책 정보 확인");
                dialog.Commands.Add(new UICommand("확인", new UICommandInvokedHandler(checkresponse)));
                dialog.Commands.Add(new UICommand("취소", new UICommandInvokedHandler(checkresponse)));
                await dialog.ShowAsync();
            }
            else
            {
                Function.ShowMessage("책 정보가 확인되지 않습니다.");
            }
        }
        private void checkresponse(IUICommand command)
        {
            if (command.Label == "확인")
            {
                SetLocation();             
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            ReadSize();
            object isbn;
            if (CoreApplication.Properties.TryGetValue("ISBN",out isbn))
            {
                textBox.Text = isbn.ToString();
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
                    Function.ShowMessage("책을 수동 등록 해야 합니다.");
                }
            }
        }

        private async void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox.Text.Length == 13)
            {
                button.IsEnabled = true;
                await searchBookNaverapi(textBox.Text, true);
            }
        }
        public static string PageName;
        private int ROW;
        private int COLUM;
        private int x;
        private int y;
        Button LastButton;
 
 

        private void SetLocation()
        {
           
           
            if (!DB.locationexist(x,y))
            {
                
                Book currbook = Data.GetBook();
                currbook.x = x;
                currbook.y = y;
                DB.Insert(currbook);
                DB.Insert(new Location { x = x, y = y, addr = DB.conn.Table<Location>().Count()+1 });
                Function.ShowMessage("등록 완료.");
                CoreApplication.Properties.Clear();
                Frame.GoBack();
            }
            else
            {
                Function.ShowMessage("자리에 책이 이미 있습니다.\r\n다시 지정해 주세요.");
            }
        }


        private async void ReadSize()
        {
            double size = 0.0;
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("prefs.xml", CreationCollisionOption.OpenIfExists);
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(await FileIO.ReadTextAsync(file))))
                {
                    reader.ReadToFollowing("Row");
                    ROW = int.Parse(reader.ReadElementContentAsString());
                }
                using (XmlReader reader = XmlReader.Create(new StringReader(await FileIO.ReadTextAsync(file))))
                {
                    reader.ReadToFollowing("Line");
                    COLUM = int.Parse(reader.ReadElementContentAsString());
                }
                for (int i = 0; i < ROW; i++)
                {
                    for (int j = 0; j < COLUM; j++)
                    {
                        Button btn = new Button();
                        btn.Content = "";
                        btn.Name = (i + 1).ToString() + "," + (j + 1).ToString();
                        btn.Height = Math.Round(LocationGrid.Height / (ROW + 1));
                        btn.Width = size = Math.Round(LocationGrid.Width / (COLUM + 1));
                        btn.Click += ItemClick;
                        if (PageName == "책 자리 보기")
                        {
                            if ((i + 1) == x && (j + 1) == y)
                            {
                                btn.Background = new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
                                LastButton = btn;
                            }
                        }
                        LocationGrid.Items.Add(btn);
                        Grid.SetRow(btn, i);
                        Grid.SetColumn(btn, j);
                    }
                }

            }
            catch (Exception e)
            {
                Function.ShowMessage("자리표를 불러오는 중 오류가 발생했습니다." + "\r\n" + e.Message);
            }
        }
        private void ItemClick(object sender, RoutedEventArgs e)
        {
            
           
            if (LastButton != null)
                LastButton.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
            Button SelectedButton = (Button)e.OriginalSource;
            SelectedButton.Background = new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
            string[] Point = SelectedButton.Name.Split(',');
            x = int.Parse(Point[0]);
            y = int.Parse(Point[1]);
            LastButton = SelectedButton;
        }
    }
}
