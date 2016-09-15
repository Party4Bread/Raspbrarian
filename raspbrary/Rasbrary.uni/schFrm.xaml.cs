using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace Rasbrary.uni
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class schFrm : Page
    {
        public schFrm()
        {
            this.InitializeComponent();
        }
        System.Collections.Generic.List<Book> currentlist = new System.Collections.Generic.List<Book>();
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                if (comboBox.SelectionBoxItem != null)
                {
                    listBox.Items.Clear();
                    switch (comboBox.SelectedIndex)
                    {
                        case 0:
                            currentlist = DB.FindbyName(textBox.Text);
                            foreach (var g in currentlist)
                                listBox.Items.Add(g.Name);
                            break;
                        case 1:
                            currentlist = DB.FindbyAuther(textBox.Text);
                            foreach (var g in currentlist)
                                listBox.Items.Add(g.Name);
                            break;
                        case 2:
                            currentlist = DB.FindbyPublisher(textBox.Text);
                            foreach (var g in currentlist)
                                listBox.Items.Add(g.Name);
                            break;
                    }
                }
                else
                    Function.ShowMessage("검색 조건을 선택하세요!");
            }
            else
                Function.ShowMessage("키워드를 입력하세요.");
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(mainFrm));
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (currentlist.Count != 0)
            {
                if (listBox.SelectedIndex <= currentlist.Count)
                {
                    if (listBox.SelectedIndex != -1)
                    {
                        textBlock.Text = "책제목:" + currentlist[listBox.SelectedIndex].Name;
                        textBlock1.Text = "저자명:" + currentlist[listBox.SelectedIndex].Auther;
                        image.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri(currentlist[listBox.SelectedIndex].image, System.UriKind.Absolute));
                    }
                    else
                    {
                        return;
                    }

                }
            }
        }


      

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BookLocation));
            Function.SetPageName("책 자리 보기");
            Data.SetBook(currentlist[listBox.SelectedIndex]);
        }

     
           
    }
}
