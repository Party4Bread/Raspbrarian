using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using Windows.Storage;
using System.Xml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;

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
        public BookLocation()
        {
            InitializeComponent();
            
        
        }

        private void btnexit_Click(object sender, RoutedEventArgs e)
        {
           
            Frame.GoBack();
           
               
        }

       

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Book currbook = Data.GetBook();
            PageName =textBlock.Text = Function.GetPageName();
            if (PageName == "책 자리 정하기")
                btnConfirm.Content = "설정";
            if (PageName == "책 자리 보기")
            {
                x = currbook.x;
                y = currbook.y;
                txtX.Text += x.ToString();
                txtY.Text += y.ToString();
            }
                ReadSize();

        }
        private void SetLocation()
        {
            Book currbook = Data.GetBook();
            currbook.x = x;
            currbook.y = y;
            DB.Insert(currbook);
            
            Function.ShowMessage("등록 완료.");
            Frame.GoBack();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnConfirm.Content == "설정")
                SetLocation();

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
                for (int i = 0; i < ROW; i++)
                {
                    for (int j = 0; j < COLUM; j++)
                    {
                        Button btn = new Button();
                        btn.Content = "";
                        btn.Name = (i + 1).ToString() + "," + (j + 1).ToString();
                        btn.Height = Math.Round(LocationGrid.Height/(ROW+1));
                        btn.Width = Math.Round(LocationGrid.Width / (COLUM+1));
                        if(PageName=="책 자리 정하기")
                        btn.Click += LocationGrid_ItemClick;
                        if (PageName == "책 자리 보기")
                        {
                            if ((i+1) == x && (j+1) == y)
                                btn.Background = new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
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


        private void LocationGrid_ItemClick(object sender, RoutedEventArgs e)
        {
            txtX.Text = "행:";
            txtY.Text = "열:";
            Button SelectedButton = (Button)e.OriginalSource;
           SelectedButton.Background= new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
            string[] Point=SelectedButton.Name.Split(',');
            x = int.Parse(Point[0]);
            y = int.Parse(Point[1]);
            txtX.Text += Point[0];
            txtY.Text += Point[1];
        }
    }
}
