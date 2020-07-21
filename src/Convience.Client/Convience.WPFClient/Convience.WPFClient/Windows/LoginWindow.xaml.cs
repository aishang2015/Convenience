using Convience.WPFClient.Data;
using Convience.WPFClient.Models;
using Convience.WPFClient.Utils;
using Convience.WPFClient.ViewModels;

using JWT.Builder;

using Newtonsoft.Json.Linq;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Convience.WPFClient.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AppDbContext _appDbContext;

        private string _captchaKey;

        public LoginWindow(AppDbContext appDbContext)
        {
            InitializeComponent();
            _appDbContext = appDbContext;
            DataContext = new LoginViewModel();
        }

        private async void LoginLoaded(object sender, RoutedEventArgs e)
        {
            if (IsTokenExpire())
            {
                Visibility = Visibility.Visible;
                await LoadCaptcha();
            }
            else
            {
                ShowMainWindow();
                await LoadCaptcha();
            }
        }

        private async void RefreshCaptcha(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await LoadCaptcha();
        }

        private async Task LoadCaptcha()
        {
            var uri = ConfigurationUtil.GetApiUri() + "/captcha";
            var data = await HttpClientUtil.GetResponseAsync<CaptchaModel>(uri);

            if (data != null)
            {
                byte[] bytes = Convert.FromBase64String(data.CaptchaData);
                MemoryStream memStream = new MemoryStream(bytes);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memStream;
                bitmapImage.EndInit();
                captchaImage.Source = bitmapImage;
                _captchaKey = data.CaptchaKey;
            }
        }

        private async void LoginSubmit(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as LoginViewModel;
            if (string.IsNullOrEmpty(vm.UserName) ||
                string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(vm.CaptchaValue))
            {
                MessageBox.Show("请输入内容！");
                return;
            }

            var uri = ConfigurationUtil.GetApiUri() + "/login";
            var loginData = new LoginModel
            {
                UserName = vm.UserName,
                Password = PasswordBox.Password,
                CaptchaKey = _captchaKey,
                CaptchaValue = vm.CaptchaValue
            };
            var result = await HttpClientUtil.PostRequestAsync<LoginModel, LoginResult>(uri, loginData);
            if (result != null)
            {
                HttpClientUtil.RemoveBearaHeader();
                HttpClientUtil.AddBearaHeader(result.Token);

                var json = new JwtBuilder().Decode(result.Token);
                var jo = JObject.Parse(json);
                var expire = jo["exp"].ToString();

                _appDbContext.LoginInfos.RemoveRange(_appDbContext.LoginInfos);
                _appDbContext.LoginInfos.Add(new Data.Entity.LoginInfo
                {
                    Token = result.Token,
                    Avatar = result.Avatar,
                    Name = result.Name,
                    ExpireTime = DataTimeUtil.ConvertStringToDateTime(expire)
                });
                _appDbContext.SaveChanges();

                ShowMainWindow();
            }
        }

        private void ShowMainWindow()
        {
            Hide();
            var that = this;
            var main = App.GetWindow<MainWindow>();
            main.LogoutEvent += async () =>
            {
                _appDbContext.LoginInfos.RemoveRange(_appDbContext.LoginInfos);
                _appDbContext.SaveChanges();
                Show();

                // 不能再事件中使用wait();
                await LoadCaptcha();
                main.IsCloseApp = false;
                main.Close();
            };
            main.Show();
        }

        private bool IsTokenExpire()
        {
            // 是否包含登录信息
            if (_appDbContext.LoginInfos.Count() > 0)
            {
                // 取得登录信息
                var loginInfo = _appDbContext.LoginInfos.First();

                // 没有过期
                if (loginInfo.ExpireTime > DateTime.Now)
                {
                    HttpClientUtil.RemoveBearaHeader();
                    HttpClientUtil.AddBearaHeader(loginInfo.Token);
                    return false;
                }
                else
                {
                    _appDbContext.LoginInfos.RemoveRange(_appDbContext.LoginInfos);
                    return true;
                }
            }
            return true;
        }

        private void ResetForm(object sender, RoutedEventArgs e)
        {
            DataContext = new LoginViewModel();
        }
    }
}
