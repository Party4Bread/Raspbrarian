﻿using System.Threading;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace Rasbrary.uni
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class mainFrm : Page
    {
        public mainFrm()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {       
        }

        private void btn_sch_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(schFrm));
        }

        private void btn_reg_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(regFrm));
        }
    }
}