using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Input;
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
           InitializeComponent();
        }
        System.Collections.Generic.List<Book> currentlist = new System.Collections.Generic.List<Book>();
        private void button_Click(object sender, HoldingRoutedEventArgs e)
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
                        case 3:
                            currentlist = DB.Findbyisbn(textBox.Text);
                            foreach (var g in currentlist)
                                listBox.Items.Add(g.Name);
                            break;
                    }
                    
                }
                else
                    Function.ShowMessage("검색 조건을 선택하세요!");
            }
            else
            { 
                Function.ShowMessage("키워드를 입력하세요.\r\n전체 목록을 표시합니다.");
                var query = DB.Conn.Table<Book>();
                foreach (var item in query)
                {
                    currentlist.Add(item);
                    listBox.Items.Add(item.Name);
                }
            }
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, HoldingRoutedEventArgs e)
        {
            CoreApplication.Properties.Clear();
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
                        if (currentlist[listBox.SelectedIndex].image == null)
                            Function.ShowMessage("No Image");
                        btndel.IsEnabled = true;
                        button2.IsEnabled = true;
                    }
                    else
                    {
                        return;
                    }

                }
            }
        }     

        private void button2_Click(object sender, HoldingRoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                CoreApplication.Properties.Clear();
                CoreApplication.Properties.Add("searchType", comboBox.SelectedIndex);
                CoreApplication.Properties.Add("searchKey", textBox.Text);
                CoreApplication.Properties.Add("selsch", listBox.SelectedIndex);
                Frame.Navigate(typeof(BookLocation));
                Function.SetPageName("책 자리 보기");
                Data.SetBook(currentlist[listBox.SelectedIndex]);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            btndel.IsEnabled = false;
            button2.IsEnabled = false;
            object st, sk;
            if(CoreApplication.Properties.TryGetValue("searchType",out st))
            {
                string type = st.ToString();
                comboBox.SelectedIndex = int.Parse(type);
                if (CoreApplication.Properties.TryGetValue("searchKey", out sk))
                {
                    textBox.Text = sk.ToString();
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
                                case 3:
                                    currentlist = DB.Findbyisbn(textBox.Text);
                                    foreach (var g in currentlist)
                                        listBox.Items.Add(g.Name);
                                    break;
                            }
                        }
                        else
                            Function.ShowMessage("검색 조건을 선택하세요!");
                    }
                    else
                    {
                      
                        var query = DB.Conn.Table<Book>();
                        foreach (var item in query)
                        {
                            currentlist.Add(item);
                            listBox.Items.Add(item.Name);
                        }
                    }
                    object selc;
                    int index;
                    if (CoreApplication.Properties.TryGetValue("selsch", out selc))
                    {
                        index = int.Parse(selc.ToString());
                        if (index!=-1&&index<listBox.Items.Count)
                            listBox.SelectedIndex = index;
                    }
                }
            }
        }

        private void btndel_Click(object sender, HoldingRoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                Book temp = currentlist[listBox.SelectedIndex];
                listBox.Items.Clear();
                DB.Delete(temp);
                currentlist.Remove(temp);
                foreach (var g in currentlist)
                    listBox.Items.Add(g.Name);
                if (temp.image == DB.Src)
                    DB.ShowMainImage();
                
            }
        }
    }
}
