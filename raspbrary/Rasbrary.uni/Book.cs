using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    class DB
    {
        public static string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,"db.sqlite");
        public static SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new
            SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

        public static List<Book> FindbyName(string name)
        {
            var query = conn.Table<Book>();
            List<Book> L = new List<Book>();
            foreach (var message in query)
            {
                if(message.Name.IndexOf(name)!=-1)
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
                if (message.Auther.IndexOf(aut) != -1)
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
                if (message.Publisher.IndexOf(pub) != -1)
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
        
    }
}
