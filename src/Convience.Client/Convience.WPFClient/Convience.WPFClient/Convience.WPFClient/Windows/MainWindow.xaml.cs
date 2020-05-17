using Convience.WPFClient.Data;
using Convience.WPFClient.ViewModels;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Convience.WPFClient.Windows
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _appDbContext;

        public event Action LogoutEvent;

        public bool IsCloseApp = true;

        public MainWindow(AppDbContext appDbContext)
        {
            InitializeComponent();
            _appDbContext = appDbContext;
            var viewModel = new MainWindowViewModel();

            // 取得登录信息
            viewModel.LoginInfo = _appDbContext.LoginInfos.First();
            DataContext = viewModel;
            AvatarImage.Source = new BitmapImage(new Uri($"pack://application:,,,/assets/avatars/{viewModel.LoginInfo.Avatar}.png",
                UriKind.RelativeOrAbsolute));
        }

        private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            pop1.IsOpen = false;
            pop1.IsOpen = true;
        }

        private void Logout(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void AppClose(object sender, EventArgs e)
        {
            if (IsCloseApp)
            {
                Environment.Exit(0);
            }
        }

        private void ListSelect(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var index = ControlList.Items.IndexOf(ControlList.SelectedItem);
            if (index == 2)
            {
                LogoutEvent();
            }
        }
    }
}
