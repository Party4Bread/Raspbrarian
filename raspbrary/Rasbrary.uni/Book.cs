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
