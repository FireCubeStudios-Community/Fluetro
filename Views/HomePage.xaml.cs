using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fluetro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar; titleBar.ButtonBackgroundColor = Colors.Transparent;
            MyFrame.Navigate(typeof(ProjectsPage));
            Nav.SelectedItem = Home;
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (Home.IsSelected)
            {
                MyFrame.Navigate(typeof(ProjectsPage), null, new DrillInNavigationTransitionInfo());
            }
            else if (Search.IsSelected)
            {
                MyFrame.Navigate(typeof(SearchPage), null, new DrillInNavigationTransitionInfo());
            }
            else
            {
                MyFrame.Navigate(typeof(RecentsPage), null, new DrillInNavigationTransitionInfo());
            }
        }
    }
}
