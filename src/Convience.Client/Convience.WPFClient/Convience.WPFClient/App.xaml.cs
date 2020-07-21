using Convience.WPFClient.Data;
using Convience.WPFClient.Utils;
using Convience.WPFClient.Windows;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

namespace Convience.WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            InitDataBase();
            var main = ServiceProvider.GetRequiredService<LoginWindow>();
            main.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var sqliteConnectionString = ConfigurationUtil.GetConnectionString("Sqlite");
            services.AddDbContext<AppDbContext>(o => o.UseSqlite(sqliteConnectionString));
            services.AddSingleton(typeof(MainWindow));
            services.AddSingleton(typeof(LoginWindow));
        }

        private void InitDataBase()
        {
            var db = ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        public static TWindow GetWindow<TWindow>() where TWindow : Window
        {
            return ServiceProvider.GetRequiredService<TWindow>();
        }
    }
}
