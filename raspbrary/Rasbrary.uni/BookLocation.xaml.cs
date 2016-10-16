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
using Windows.ApplicationModel.Core;

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
        Button LastButton;
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
            btnConfirm.IsEnabled = false;
            Book currbook = Data.GetBook();
            
            PageName =textBlock.Text = Function.GetPageName();
            
            if (PageName == "책 자리 보기")
            {
                x = currbook.x;
                y = currbook.y;
                txtX.Text += x.ToString();
                txtY.Text += y.ToString();
               
                btnConfirm.Content = "수정하기";
            }
                ReadSize();
        }
       
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
         
            if ((string)btnConfirm.Content == "수정하기")
                
                ChangeLocation();
        }

        private void ChangeLocation()
        {
            Book currbook = Data.GetBook();
            DB.Delete(currbook);
            currbook.x = x;
            currbook.y = y;
            DB.Insert(currbook);
            Function.ShowMessage("수정 완료.");
           
            
        }

        private async void ReadSize()
        {
            double size=0.0;
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
                        btn.Width = size =Math.Round(LocationGrid.Width / (COLUM+1));
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
            if(LastButton!=null)
                LastButton.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
            Button SelectedButton = (Button)e.OriginalSource;
            SelectedButton.Background= new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
            string[] Point=SelectedButton.Name.Split(',');
            x = int.Parse(Point[0]);
            y = int.Parse(Point[1]);
            txtX.Text += Point[0];
            txtY.Text += Point[1];
            LastButton = SelectedButton;
        }
    }
}
