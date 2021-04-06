using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fluetro.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OOBEPage : Page
    {
        
        public OOBEPage()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar; titleBar.ButtonBackgroundColor = Colors.Transparent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = new Frame();
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
            rootFrame.Navigate(typeof(HomePage));
        }

        private void Light_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Light_Checked(object sender, RoutedEventArgs e)
        {
            this.RequestedTheme = ElementTheme.Light;
        }

        private void Dark_Checked(object sender, RoutedEventArgs e)
        {
            this.RequestedTheme = ElementTheme.Dark;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = new Frame();
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
            rootFrame.Navigate(typeof(HomePage));
        }
    }
}
