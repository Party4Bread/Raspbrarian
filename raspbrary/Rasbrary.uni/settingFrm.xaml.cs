using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace Rasbrary.uni
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class settingFrm : Page
    {
        public settingFrm()
        {
            this.InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            int a = int.Parse(textBox.Text);
            a++;
            textBox.Text = a.ToString();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            int a = int.Parse(textBox.Text);
            a--;
            textBox.Text = a.ToString();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            int a = int.Parse(textBox_Copy.Text);
            a++;
            textBox_Copy.Text = a.ToString();
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            int a = int.Parse(textBox_Copy.Text);
            a--;
            textBox_Copy.Text = a.ToString();
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("prefs.xml");
            using (IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                Stream s = writeStream.AsStreamForWrite();
                System.Xml.XmlWriterSettings settings = new XmlWriterSettings();
                settings.Async = true;
                using (XmlWriter writer = System.Xml.XmlWriter.Create(s, settings))
                {
                    writer.WriteStartElement("sheet");
                    writer.WriteElementString("Row", textBox.Text);
                    writer.WriteElementString("Line", textBox_Copy.Text);
                    writer.Flush();
                    await writer.FlushAsync();
                }
            }
            Function.ShowMessage("변경 완료!");
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            DB.conn.CreateTable<Location>();
       
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("prefs.xml", CreationCollisionOption.OpenIfExists);
            /*            
                        using (IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            Stream s = writeStream.AsStream();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(s.ToString());
                            XElement R = xd.Root.Element("sheet").Element("Row");
                            XElement L = xd.Root.Element("sheet").Element("Line");
                            textBox.Text = R.Value;
                            textBox_Copy.Text = L.Value;
                        }
            */

            // Create an XmlReader
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(await FileIO.ReadTextAsync(file))))
                {
                    reader.ReadToFollowing("Row");
                    textBox.Text = reader.ReadElementContentAsString();
                }
                using (XmlReader reader = XmlReader.Create(new StringReader(await FileIO.ReadTextAsync(file))))
                {
                    reader.ReadToFollowing("Line");
                    textBox_Copy.Text = reader.ReadElementContentAsString();
                }
            }
            catch (Exception ea)
            {
                using (IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    Stream s = writeStream.AsStreamForWrite();
                    System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                    settings.Async = true;
                    using (XmlWriter writer = System.Xml.XmlWriter.Create(s, settings))
                    {
                        writer.WriteStartElement("sheet");
                        writer.WriteElementString("Row", "1");
                        writer.WriteElementString("Line", "1");
                        writer.Flush(); 
                        await writer.FlushAsync();
                    }
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(mainFrm));
        }
    }
}
