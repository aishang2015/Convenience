using System;
using System.Windows;
using System.Windows.Input;

namespace Convience.WPFClient.Commands
{
    public class ShutDownCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var window = parameter as Window;
            window.Close();
            Environment.Exit(0);
        }
    }
}
