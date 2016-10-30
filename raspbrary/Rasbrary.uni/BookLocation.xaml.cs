using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using System.Xml;
using Windows.UI;
using ArduinoSerial;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rasbrary.uni
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookLocation : Page
    {
        public static string PageName;
        private int ROW;
        private int COLUM;
        private int x;
        private int y;
        private int _inix;
        private int _iniy;
        Button _lastButton;
        Button _originButton;
        public BookLocation()
        {
            InitializeComponent();         
        }
        private void btnexit_Click(object sender, RoutedEventArgs e)
        {           
            Frame.GoBack();         
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {

            btnConfirm.IsEnabled = false;
            Book currbook = Data.GetBook();
            _inix = currbook.x;
            _iniy = currbook.y;
            PageName = textBlock.Text = Function.GetPageName();

            if (PageName == "책 자리 보기")
            {
                x = currbook.x;
                y = currbook.y;
                txtX.Text = "행:" + x.ToString();
                txtY.Text = "열:" + y.ToString();

                btnConfirm.Content = "수정하기";
            }
            ReadSize();
            Arduino head = new Arduino();
            head.connect(null);
            int address = DB.FindAddress(x, y).addr;
            await head.WriteAsync("ledon "+address);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
         
            if ((string)btnConfirm.Content == "수정하기")
                ChangeLocation();
        }

        private void ChangeLocation()
        {
            if (!DB.Locationexist(x, y))
            {
                Book currbook = Data.GetBook();
                DB.Delete(currbook);
                DB.Delete(_inix,_iniy);
                currbook.x = x;
                currbook.y = y;
                DB.Insert(currbook);
                DB.Insert(new Location { x = x, y = y, addr = DB.Conn.Table<Location>().Count() + 1 });
                Function.ShowMessage("수정 완료.");
                Page_Loaded(null, null);
            }
            else
            {
                Function.ShowMessage("자리에 책이 이미 있습니다.\r\n다시 지정해 주세요.");
            }
        }

        private async void ReadSize()
        {
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
                    COLUM=int.Parse(reader.ReadElementContentAsString());
                }
                LocationGrid.Items.Clear();
                for (int i = 0; i < ROW; i++)
                {
                    for (int j = 0; j < COLUM; j++)
                    {
                        Button btn = new Button();
                        btn.Content = "";
                        btn.Name = (i + 1) + "," + (j + 1);
                        btn.Height = Math.Round(LocationGrid.Height/(ROW+1));
                        btn.Width = Math.Round(LocationGrid.Width / (COLUM+1));
                        btn.Click += ItemClick;
                        if (PageName == "책 자리 보기")
                        {
                            if ((i + 1) == x && (j + 1) == y)
                            {
                                btn.Background = new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
                                _originButton = btn;
                            }
                        }
                        LocationGrid.Items.Add(btn);
                        Grid.SetRow(btn, i);
                        Grid.SetColumn(btn, j);     
                    }
                }
               
            }
            catch(Exception e)
            {
                Function.ShowMessage("자리표를 불러오는 중 오류가 발생했습니다."+"\r\n"+e.Message);
            }
        }
        private void ItemClick(object sender, RoutedEventArgs e)
        {
            btnConfirm.IsEnabled = true;
            txtX.Text = "행:";
            txtY.Text = "열:";
            if(_lastButton!=null)
                _lastButton.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
            Button selectedButton = (Button)e.OriginalSource;
            selectedButton.Background= new SolidColorBrush(Color.FromArgb(132, 27, 186, 154));
            _originButton.Background = new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
            string[] point=selectedButton.Name.Split(',');
            x = int.Parse(point[0]);
            y = int.Parse(point[1]);
            txtX.Text += point[0];
            txtY.Text += point[1];
            _lastButton = selectedButton;
        }
    }
}
