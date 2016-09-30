using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Popups;
using Windows.Devices.SerialCommunication;
using Windows.Devices.Enumeration;
using System.Threading;
using System.Collections.ObjectModel;
using Windows.Storage.Streams;
using System.Threading.Tasks;

namespace Rasbrary.uni
{
    class Book
    {
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Auther { get; set; }
        public string Publisher { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string image { get; set; }
    }
   
    class Arduino
    {
        private SerialDevice serialPort = null;
        DataWriter dataWriteObject = null;
        DataReader dataReaderObject = null;
        private ObservableCollection<DeviceInformation> listOfDevices;
        private CancellationTokenSource ReadCancellationTokenSource;

        public async void connect()
        {
            listOfDevices = new ObservableCollection<DeviceInformation>();
            ReadCancellationTokenSource = new CancellationTokenSource();
            DeviceInformationCollection dis;
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                dis = await DeviceInformation.FindAllAsync(aqs);
                DeviceInformation entry = dis[0];            
                serialPort = await SerialDevice.FromIdAsync(entry.Id);
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                Listen();
            }
            catch (Exception ex)
            {
            }
        }

        private async void Listen()
        {
            try
            {
                if (serialPort != null)
                {
                    dataReaderObject = new DataReader(serialPort.InputStream);  
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                    CloseDevice();
            }
            finally
            {
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        private void CloseDevice()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;
            listOfDevices.Clear();
            connect();
        }

        public async Task WriteAsync(string data)
        {
            dataWriteObject = new DataWriter(serialPort.OutputStream);
            Task<uint> storeAsyncTask;
            dataWriteObject.WriteString(data);
            storeAsyncTask = dataWriteObject.StoreAsync().AsTask();
            uint bytesWritten = await storeAsyncTask;
        }

        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<uint> loadAsyncTask;
            uint ReadBufferLength = 1024;
            cancellationToken.ThrowIfCancellationRequested();
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);
            // Launch the task and wait
            uint bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                string recive = dataReaderObject.ReadString(bytesRead);
            }
        }
    }


    class DB
    {
        public static string src;
        public static string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,"db.sqlite");
        public static SQLiteConnection conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
       
        public static List<Book> FindbyName(string name)
        {
            var query = conn.Table<Book>();
            List<Book> L = new List<Book>();
            foreach (var message in query)
            {
                if(message.Name.Contains(name,StringComparison.OrdinalIgnoreCase))
                {
                    L.Add(message);
                }
            }
            return L;
        }
        public static List<Book> FindbyAuther(string aut)
        {
            var query = conn.Table<Book>();
            List<Book> L = new List<Book>();
            foreach (var message in query)
            {
                if (message.Auther.Contains(aut,StringComparison.OrdinalIgnoreCase))
                {
                    L.Add(message);
                }
            }
            return L;
        }
        public static List<Book> FindbyPublisher(string pub)
        {
            var query = conn.Table<Book>();
            List<Book> L = new List<Book>();
            foreach (var message in query)
            {
                if (message.Publisher.Contains(pub,StringComparison.OrdinalIgnoreCase))
                {
                    L.Add(message);
                }
            }
            return L;
        }
        public static List<Book> Findbyisbn(string isbn)
        {
            var query = conn.Table<Book>();
            List<Book> L = new List<Book>();
            foreach (var message in query)
            {
                if (message.ISBN==isbn)
                {
                    L.Add(message);
                }
            }
            return L;
        }
        public static void Insert(string isbn,string name,string aut, string pub, int x,int y,string imgsrc)
        { 
            conn.Insert(new Book()
            {
                Auther=aut,
                image=imgsrc,
                Name=name,
                ISBN=isbn,
                Publisher=pub,
                x=x,
                y=y
            });
        }
        public static void Insert(Book newbook)
        {
            conn.Insert(newbook);
        }
        public static void Delete(Book book)
        {
            string query = "DELETE FROM Book WHERE ISBN=" + book.ISBN + " and x=" + book.x + " and y=" + book.y;

            conn.Execute(query, new Book[1]);
        }
        public static string ShowMainImage()
        {
            src = null;
            var query = conn.Table<Book>();
            string[] Imagesrc;
            List<string> imagesrc = new List<string>();
           
            Random rd = new Random();
            foreach (var book in query)
            {
                imagesrc.Add(book.image);
            }
            Imagesrc = imagesrc.ToArray();
            src = (string)(Imagesrc.GetValue(rd.Next(0, Imagesrc.Length - 1)));
            
                return src;
 
        }
        public static bool HasMainImage()
        {
            if (src == null)
                return false;
            else
                return true;
        }

    }
    class Function
    {
        private static string BookLocationPageName;
        
        public async static void ShowMessage(string Message)
        {
            var dialog = new MessageDialog(Message);
            await dialog.ShowAsync();
        }
       public static void SetPageName(string Name)
        {
            BookLocationPageName = Name;
        }
        public static string GetPageName()
        {
            return BookLocationPageName;
        }
      
    }
    class Data
    {
        public static Book currbook;
        public static Book GetBook()
        {
            return currbook;
        }
        public static void SetBook(Book book)
        {
            currbook = book;
        }
       
    }
    public static class StringExtensions
    {
        public static bool Contains(this String str, String substring,
                                    StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                                "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                            "comp");

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}
