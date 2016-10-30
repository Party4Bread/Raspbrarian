using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Async = true;
                using (XmlWriter writer = XmlWriter.Create(s, settings))
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
            
       
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("prefs.xml", CreationCollisionOption.OpenIfExists);


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
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Async = true;
                    using (XmlWriter writer = System.Xml.XmlWriter.Create(s, settings))
                    {
                        writer.WriteStartElement("sheet");
                        writer.WriteElementString("Row", "7");
                        writer.WriteElementString("Line", "7");
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

        private async void button4_Click(object sender, RoutedEventArgs e)
        {
            var fileSavePicker = new Windows.Storage.Pickers.FileSavePicker();
            fileSavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            fileSavePicker.FileTypeChoices.Add("Rasbrary backup file", new List<string>() { ".rbp" });
            // Default file name if the user does not type one in or select a file to replace
            fileSavePicker.SuggestedFileName = "libbackup";
            StorageFile file = await fileSavePicker.PickSaveFileAsync();
                
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                StorageFile db = await StorageFile.GetFileFromPathAsync(DB.DBpath);
                StorageFolder backFolder = await StorageFolder.GetFolderFromPathAsync(file.Path);
                await db.CopyAsync(backFolder,file.Name);
                Windows.Storage.Provider.FileUpdateStatus status =await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Function.ShowMessage("백업완료!");
                }
                else
                {
                    Function.ShowMessage("백업실패....");
                }
            }

        }

        private async void button3_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".rbp");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                StorageFile db = await StorageFile.GetFileFromPathAsync(DB.DBpath);
                StorageFolder dbFolder = await StorageFolder.GetFolderFromPathAsync(db.Path);
                await file.CopyAsync(dbFolder, db.Name);
                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Function.ShowMessage("복구완료!");
                }
                else
                {
                    Function.ShowMessage("복구실패...");
                }
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ManageSlave));
        }
    }
}
