using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Fluetro.ViewModels
{
    public class ProjectsViewModel : BaseViewModel, IDisposable
    {
        public ICommand NavigateCommand { get; set; }
        public ProjectsViewModel()
         {
            NavigateCommand = new RelayCommand(NavigatePageCommand);
         }
        public void NavigatePageCommand()
        {
            Frame rootFrame = new Frame();
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
            rootFrame.Navigate(typeof(FluentPage));
        }
    }
}
