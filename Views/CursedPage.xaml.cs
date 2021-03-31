using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Fluetro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CursedPage : Page
    {
        public PdfPageViewModel ViewModel
        {
            get => (PdfPageViewModel)DataContext;
            set => DataContext = value;
        }
        public CursedPage()
        {
            this.InitializeComponent();
           // this.ViewModel = new PdfPageViewModel();
           // this.ViewModel.pdfViewer = pdfViewer;
            this.DataContext = ViewModel;
            nv.SelectedItem = Home;
        }
        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            //Opens a file picker.
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.ViewMode = PickerViewMode.List;
            //Filters PDF files in the documents library.
            picker.FileTypeFilter.Add(".pdf");
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;
            //Reads the stream of the loaded PDF document.
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Stream fileStream = stream.AsStreamForRead();
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            //Loads the PDF document.
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(buffer);
            //Creates custom progress ring.
            ProgressRing progressRing = new ProgressRing();
            progressRing.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
            progressRing.Width = 100;
            progressRing.Height = 100;
            //Assigns custom progress ring to the Viewer control.
            pdfViewer.PdfProgressRing = progressRing;
            pdfViewer.LoadDocument(loadedDocument);
        }
        private void OneViewButton_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer.ViewMode = PageViewMode.OnePage;
        }
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer.ViewMode = PageViewMode.Normal;
        }
        private void FitViewButton_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer.ViewMode = PageViewMode.FitWidth;
        }

        private void nv_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if(nv.SelectedItem == Home)
            {
                HomeBar.Visibility = Visibility.Visible;
                FileBar.Visibility = Visibility.Collapsed;
                ViewBar.Visibility = Visibility.Collapsed;
                InkBar.Visibility = Visibility.Collapsed;
            }
            else if(nv.SelectedItem == View)
            {
                ViewBar.Visibility = Visibility.Visible;
                FileBar.Visibility = Visibility.Collapsed;
                HomeBar.Visibility = Visibility.Collapsed;
                InkBar.Visibility = Visibility.Collapsed;
            }
            else if (nv.SelectedItem == Ink)
            {
                InkBar.Visibility = Visibility.Visible;
                ViewBar.Visibility = Visibility.Collapsed;
                FileBar.Visibility = Visibility.Collapsed;
                HomeBar.Visibility = Visibility.Collapsed;
            }
            else if(nv.SelectedItem == File)
            {
             InkBar.Visibility = Visibility.Collapsed;
                FileBar.Visibility = Visibility.Visible;
                HomeBar.Visibility = Visibility.Collapsed;
                ViewBar.Visibility = Visibility.Collapsed;
            }
        }
        private void ColorPicker_ColorChangeCompleted(object sender, Windows.UI.Color value)
        {
            pdfViewer.InkAnnotationSettings.Color = value;
        }
        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            pdfViewer.SearchText(SearchBox.QueryText);
        }
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            //Searches for the text in the PDF Document.
            pdfViewer.SearchPrevText(SearchBox.QueryText);
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            //Searches for the text in the PDF Document.
            pdfViewer.SearchNextText(SearchBox.QueryText);
        }
        private void InkButton_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer.InkAnnotationCommand.Execute(true);
            pdfViewer.InkAnnotationSettings.UseWindowsInkCanvas = InkNative.IsOn;
            pdfViewer.InkAnnotationSettings.Thickness = ThiccSlider.Value;
            pdfViewer.InkAnnotationSettings.Opacity = OpacitySlider.Value;
            pdfViewer.InkAnnotationSettings.Color = Color.FromArgb(255, 0, 0, 0);
            ColorPicker.Color = Color.FromArgb(255, 0, 0, 0);
        }

        private void ThiccSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            try
            {
          
pdfViewer.InkAnnotationSettings.Thickness = ThiccSlider.Value;
            }
            catch
            {
            }
        }

        private void OpacitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            try
            {
                pdfViewer.InkAnnotationSettings.Opacity = OpacitySlider.Value;
            }
            catch
            {
            }
        }
        private void InkNative_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                pdfViewer.InkAnnotationSettings.UseWindowsInkCanvas = InkNative.IsOn;
            }
            catch
            {
            }
        }
    }
}
