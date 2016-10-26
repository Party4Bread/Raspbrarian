using System;
using System.Windows.Forms;
using System.Drawing;

namespace raspbrary
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            starter start = new starter();
            start.StartPosition = FormStartPosition.Manual;
            start.Location = new Point(0, 0);
            Application.Run(start);
        }
    }
}
