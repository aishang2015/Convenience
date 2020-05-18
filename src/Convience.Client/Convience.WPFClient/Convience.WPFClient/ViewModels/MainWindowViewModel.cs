using Convience.WPFClient.Commands;
using Convience.WPFClient.Data.Entity;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Convience.WPFClient.ViewModels
{
    public class MainWindowViewModel
    {
        public ICommand CloseApplicationCommand { get; } = new ShutDownCommand();

        public ObservableCollection<MenuCategory> MenuCategories { get; } = new ObservableCollection<MenuCategory>
            {
                new MenuCategory("仪表盘"),
               //new MenuCategory("Action",
               //    new Menu ("Predator"),
               //    new Menu("Alien"),
               //    new Menu("Prometheus")),
               //new MenuCategory("Comedy",
               //    new Menu("EuroTrip"),
               //    new Menu("EuroTrip")
               //),
            };

        public LoginInfo LoginInfo { get; set; }
    }

    public sealed class MenuCategory
    {
        public MenuCategory(string name, params Menu[] menus)
        {
            Name = name;
            Menus = new ObservableCollection<Menu>(menus);
        }

        public string Name { get; }

        public ObservableCollection<Menu> Menus { get; }
    }

    public sealed class Menu
    {
        public Menu(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
