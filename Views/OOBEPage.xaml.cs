using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        }
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            FlipViewControl.SelectedIndex = FlipViewControl.SelectedIndex + 1;
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            FlipViewControl.SelectedIndex = FlipViewControl.SelectedIndex - 1;
        }

        private void FlipViewControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                NextBtn.IsEnabled = true;
                PreviousBtn.IsEnabled = true;
                if (FlipViewControl.SelectedIndex == 0)
                {
                    PreviousBtn.IsEnabled = false;
                }

                if (FlipViewControl.SelectedIndex == 3)
                {
                    NextBtn.IsEnabled = false;
                }
            }
            catch
            {

            }
        }
    }
}
