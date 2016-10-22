using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Popups;
using Windows.Devices.Enumeration;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace Rasbrary.uni
{
    #region BaseType
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
    class Location
    {
        public int x { get; set; }
        public int y { get; set; }
        public int addr { get; set; }
    }
    #endregion
    /*
    class Arduino
    {
        I2cDevice arduino;

        private async void send(int devicenum, byte[] data)
        {
            string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            var arduset = new I2cConnectionSettings(devicenum);
            arduino = await I2cDevice.FromIdAsync(devices[0].Id, arduset);
            arduino.Write(data);
        }

        private async Task<byte[]> receive(int devicenum)
        {
            string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            var arduset = new I2cConnectionSettings(devicenum);
            arduino = await I2cDevice.FromIdAsync(devices[0].Id, arduset);
            byte[] buf={0};
            while (buf[0]==0)
                arduino.Read(buf);
            return buf;
        }
    }
    */

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
        public static void Insert(string isbn, string name, string aut, string pub, int x, int y, string imgsrc)
        {
            conn.Insert(new Book()
            {
                Auther = aut,
                image = imgsrc,
                Name = name,
                ISBN = isbn,
                Publisher = pub,
                x = x,
                y = y
            });
        }
        public static void Insert(object obj)
        {
            if (obj is Book)
                conn.Insert((Book)obj);
            else if (obj is Location)
                conn.Insert((Location)obj);
        }
           
        public static void Delete(Book book)
        {
            string query = "DELETE FROM Book WHERE ISBN=" + book.ISBN + " and x=" + book.x + " and y=" + book.y;

            conn.Execute(query, new Book[1]);
        }
       public static void Delete(int x,int y)
        {
            string query = "DELETE FROM Location WHERE x=" + x + " and y=" + y;
            conn.Execute(query, new Location[1]);
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
        public static bool locationexist(int x,int y)
        {
            bool isexist = false;
            var query = conn.Table<Book>();
            foreach (var book in query)
            {
                if (book.x == x && book.y == y)
                {
                    isexist = true;
                }

            }
            return isexist;
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
        public static bool Contains(this string str, string substring,
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
