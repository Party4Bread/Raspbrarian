using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using System.Linq;
using SQLite.Net.Attributes;

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

        public Book()
        {
            ISBN = null;
            Name = null;
            Auther = null;
            Publisher = null;
            x = 0;
            y = 0;
            image = null;
        }
        public Book(string ISBN,string Name,string Auther,string Publisher, int x, int y,string image)
        {
            this.x = x;
            this.y = y;
            this.ISBN = ISBN;
            this.Name = Name;
            this.Auther = Auther;
            this.Publisher = Publisher;
            this.image = image;
        }
    }
    class Location
    {
        public int x { get; set; }
        public int y { get; set; }
        public int addr { get; set; }
        public Location()
        {
            x = 0;
            y = 0;
            addr = 0;
        }
        public Location(int x,int y,int addr)
        {
            this.x = x;
            this.y = y;
            this.addr = addr;
        }
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
        public static string Src;
        public static string DBpath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,"db.sqlite");
        public static SQLiteConnection Conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), DBpath);
       
        public static List<Book> FindbyName(string name)
        {
            var query = Conn.Table<Book>();
            List<Book> list = new List<Book>();
            foreach (var message in query)
            {
                if(message.Name.Contains(name,StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(message);
                }
            }
            return list;
        }

        public static Location FindAddress(int x,int y)
        {
            var query = Conn.Table<Location>();
            Location addr=new Location();
            foreach (var message in query)
            {
                if (message.x==x&&message.y==y)
                {
                    addr = message;
                }
            }
            return addr;
        }

        public static List<Book> FindbyAuther(string aut)
        {
            var query = Conn.Table<Book>();
            List<Book> list = new List<Book>();
            foreach (var message in query)
            {
                if (message.Auther.Contains(aut,StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(message);
                }
            }
            return list;
        }

        public static List<Book> FindbyPublisher(string pub)
        {
            var query = Conn.Table<Book>();
            List<Book> list = new List<Book>();
            foreach (var message in query)
            {
                if (message.Publisher.Contains(pub,StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(message);
                }
            }
            return list;
        }

        public static List<Book> Findbyisbn(string isbn)
        {
            var query = Conn.Table<Book>();
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
            Conn.Insert(new Book()
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
                Conn.Insert((Book)obj);
            else if (obj is Location)
                Conn.Insert((Location)obj);
        }
           
        public static void Delete(Book book)
        {
            string query = "DELETE FROM Book WHERE ISBN=" + book.ISBN + " and x=" + book.x + " and y=" + book.y;
            Conn.Execute(query, new Book[1]);
        }

        public static void Delete(int x,int y)
        {
            string query = "DELETE FROM Location WHERE x=" + x + " and y=" + y;
            Conn.Execute(query, new Location[1]);
        }

        public static string ShowMainImage()
        {
            Src = null;
            var query = Conn.Table<Book>();
            List<string> imagesrc = new List<string>();
            var rd = new Random();
            foreach (var book in query)
            {
                imagesrc.Add(book.image);
            }
            string[] imageArray = imagesrc.ToArray();
            Src = (string)(imageArray.GetValue(rd.Next(0, imageArray.Length - 1)));
            return Src;
        }

        public static bool HasMainImage()
        {
            if (Src == null)
                return false;
            return true;
        }

        public static bool Locationexist(int x,int y)
        {
            bool isexist = false;
            var query = Conn.Table<Book>();
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

    internal class Function
    {
        private static string _bookLocationPageName;
        public async static void ShowMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
       public static void SetPageName(string name)
        {
            _bookLocationPageName = name;
        }
        public static string GetPageName()
        {
            return _bookLocationPageName;
        }
    }

    internal class Data
    {
        public static Book Currbook;
        public static Book GetBook()
        {
            return Currbook;
        }
        public static void SetBook(Book book)
        {
            Currbook = book;
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

    public class BookSearch
    {
        private static string API_KEY = "AIzaSyCJYdfg4qMeawcpOEWx4wqSqbCLorbmNoc";
        public static BooksService Service = new BooksService(new BaseClientService.Initializer
        {
            ApplicationName="우리집 책장지기",
            ApiKey = API_KEY
        });

        public static async Task<Volume> SearchISBN(string isbn)
        {
            var result = await Service.Volumes.List(isbn).ExecuteAsync();

            if (result != null && result.Items != null)
            {
                var item = result.Items.FirstOrDefault();
                return item;
            }
            return null;
        }
    }
}

