using Syncfusion.Windows.PdfViewer;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Fluetro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FluentPage : Page
    {
        //Creates a viewmodel which can be used in x:bind instead of binding
        //For binding use public PdfPageViewModel ViewModel { get; private set; }
        public PdfPageViewModel ViewModel
        {
            get => (PdfPageViewModel)DataContext;
            set => DataContext = value;
        }
        //Connects to ViewModel and connects pdfviewer to it
        public FluentPage()
        {
            this.InitializeComponent();
            this.ViewModel = new PdfPageViewModel(pdfViewer);
            this.ViewModel.treeNavigator = BookmarkView;
            pdfViewer.TextSelectionMenu.CopyButton.Visibility = Visibility.Collapsed;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer.SearchPrevText(SearchBox.Text);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer.SearchNextText(SearchBox.Text);
        }
    }
}
