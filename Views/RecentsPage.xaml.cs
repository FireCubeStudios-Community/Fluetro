using Fluetro.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Fluetro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecentsPage : Page
    {
        public RecentsPage()
        {
            this.InitializeComponent();
            ObservableCollection<RecentFiles> DocList = new ObservableCollection<RecentFiles>();
            DocList.Add(new RecentFiles()
            {
                FileName = "electron_apps",
                Date = "11/20/2020",
                FileType = "PDF",
                Images = "Assets/Electron.png"
            });
            DocList.Add(new RecentFiles()
            {
                FileName = "work_project",
                Date = "11/20/2020",
                FileType = "EPUB",
                Images = "Assets/Electron 2.png"
            });
            DocList.Add(new RecentFiles()
            {
                FileName = "news article",
                Date = "11/20/2020",
                FileType = "PDF",
                Images = "Assets/News Article.png"
            });
            DocList.Add(new RecentFiles()
            {
                FileName = "electron_apps",
                Date = "11/20/2020",
                FileType = "PDF",
                Images = "Assets/Blindness.png"
            });
            DocList.Add(new RecentFiles()
            {
                FileName = "work_project",
                Date = "11/20/2020",
                FileType = "EPUB",
                Images = "Assets/Electron 2.png"
            });
            DocsGrid.ItemsSource = DocList;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
