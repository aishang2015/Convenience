using Convience.WPFClient.Models;
using Convience.WPFClient.Utils;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Convience.WPFClient.Windows.Pages
{
    /// <summary>
    /// DashBoardPage.xaml 的交互逻辑
    /// </summary>
    public partial class DashBoardPage : Page
    {
        public DashBoardPage()
        {
            InitializeComponent();
        }

        private async void PageLoaded(object sender, RoutedEventArgs e)
        {
            var uri = ConfigurationUtil.GetApiUri() + "/dashBoard";
            var result = await HttpClientUtil.GetResponseAsync<DashboardModel>(uri);
            DataContext = result;
        }
    }
}
