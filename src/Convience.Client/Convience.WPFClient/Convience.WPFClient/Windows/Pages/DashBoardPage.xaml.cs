using Convience.WPFClient.Models;
using Convience.WPFClient.Utils;
using System.Windows;
using System.Windows.Controls;

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
