using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ArduinoSerial;



namespace Rasbrary.uni
{
    
    public sealed partial class ManageSlave : Page
    {
        public ManageSlave()
        {
            InitializeComponent();
        }
        private int ROW;
        private int COLUM;
        private int x;
        private int y;
        private int newaddr;
        Button LastButton;
        Arduino test;
        private async void ReadSize()
        {
            var query = DB.Conn.Table<Location>();
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
                        btn.Name = (i + 1) + "," + (j + 1);
                        btn.Height = Math.Round(LocationGrid.Height / (ROW + 1));
                        btn.Width = Math.Round(LocationGrid.Width / (COLUM + 1));
                        btn.Holding += ItemClick;
                        foreach (var message in query)
                        {
                            if ((i + 1) == message.x && (j + 1) == message.y)
                            {
                                btn.Background = new SolidColorBrush(Color.FromArgb(255, 45, 69, 91));
                                btn.IsEnabled = false;
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
        private void ItemClick(object sender, HoldingRoutedEventArgs e)
        {
            if (LastButton != null)
                LastButton.Background = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
            Button SelectedButton = (Button)e.OriginalSource;
            SelectedButton.Background = new SolidColorBrush(Color.FromArgb(132, 15, 29, 169));
            txtl.Text += "(";
            txtl.Text += SelectedButton.Name;
            txtl.Text += ")";
            string[] Point = SelectedButton.Name.Split(',');
            x = int.Parse(Point[0]);
            y = int.Parse(Point[1]);
            LastButton = SelectedButton;
        }

        private void button2_Click(object sender, HoldingRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ReadSize();
            for(int i=1;i<33;i++)
            {
                comboBox.Items.Add(i);
            }
                    
                   
        }
        private void ManageLED(object sender,HoldingRoutedEventArgs e)
        {
            int a = int.Parse(comboBox.SelectedItem.ToString());
            var query = DB.Conn.Table<Location>();
            foreach (var location in query)
            {
                if (location.x == x && location.y == y&&location.addr==a)
                {
                    DB.Delete(x,y);
                }

            }
           
            DB.Conn.Insert(new Location {x=x,y=y,addr=a});
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
