using Fluetro.ViewModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.UI.Xaml.Controls;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Storage;
using Syncfusion.Pdf.Interactive;
using Syncfusion.UI.Xaml.Controls.Navigation;

namespace Fluetro
{
    //Viewmodel
    //Base viewmodel which uses the basic viewmode. remember to install fody.propertychanged
    public class PdfPageViewModel : BaseViewModel, IDisposable
    {
        //Bool to toggle native ink with toggle
        public bool _NativeInk = true;
        private PdfBookmarkBase bookmarkBase;
        public TreeView treeNavigator;
        public string ViewString = "fit";
        private string searchString;
        public string CurrentPdfPageNumber;
        public bool Ink { get; set; }
        public Color InkColor = Color.FromArgb(255, 0, 0, 0);
        public bool Rectangle { get; set; }
        public Color RectangleColor = Color.FromArgb(255, 0, 0, 0);
        public bool Ellipse { get; set; }
        public Color EllipseColor = Color.FromArgb(255, 0, 0, 0);
        public bool Line { get; set; }
        public Color LineColor = Color.FromArgb(255, 0, 0, 0);
        public PageViewMode pdfViewMode = PageViewMode.Normal;
        //Listens to when the bool NativeInk has been changed by the UI or code
        //pdf reference from view
        public readonly SfPdfViewerControl pdfViewer;
        public ICommand InkCommand { get; private set; }
        public ICommand RectangleCommand { get; private set; }
        public ICommand lineCommand { get; private set; }
        public ICommand LineColorCommand { get; private set; }
        public ICommand EllipseCommand { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand SaveAsFileCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand OpacityInkSliderCommand { get; private set; }
        public ICommand SizeInkSliderCommand { get; private set; }
        public ICommand OpacityRectangleSliderCommand { get; private set; }
        public ICommand SizeRectangleSliderCommand { get; private set; }
        public ICommand OpacityLineSliderCommand { get; private set; }
        public ICommand SizeLineSliderCommand { get; private set; }
        public ICommand OpacityEllipseSliderCommand { get; private set; }
        public ICommand SizeEllipseSliderCommand { get; private set; }
        public ICommand InkPerformanceCommand { get; private set; }
        public ICommand InkColorCommand { get; private set; }
        public ICommand RectangleColorCommand { get; private set; }
        public ICommand EllipseColorCommand { get; private set; }
        public ICommand PositionUpdatedCommand { get; private set; }

        public bool highlight { get; set; }
        public Color highlightColor = Color.FromArgb(255, 255, 0, 0);
        public ICommand highlightCommand { get; private set; }
        public ICommand highlightColorCommand { get; private set; }
        public ICommand OpacityHighlightSliderCommand { get; private set; }

        public bool Underline { get; set; }
        public Color UnderlineColor = Color.FromArgb(255, 0, 0, 0);
        public ICommand UnderlineCommand { get; private set; }
        public ICommand UnderlineColorCommand { get; private set; }
        public ICommand OpacityUnderlineSliderCommand { get; private set; }

        public bool Strikeout { get; set; }
        public Color StrikeoutColor = Color.FromArgb(255, 0, 0, 0);
        public ICommand StrikeoutCommand { get; private set; }
        public ICommand StrikeoutColorCommand { get; private set; }
        public ICommand OpacityStrikeoutSliderCommand { get; private set; }

        // get set makes it a dependancy property
        public PdfPageViewModel(SfPdfViewerControl pdfViewer)
        {     
            Ink = false;
            Rectangle = false;
            Ellipse = false;
            this.pdfViewer = pdfViewer;
            // Create commands
            InkPerformanceCommand = new RelayCommand<RoutedEventArgs>(InkPerformanceToggledCommand);
            InkCommand = new RelayCommand(InkToggledCommand);
            RectangleCommand = new RelayCommand(RectangleToggledCommand);    
            EllipseCommand = new RelayCommand(EllipseToggledCommand);
            OpenFileCommand = new RelayCommand(OpenFileViewerCommand);
            SaveAsFileCommand = new RelayCommand(SaveAsFileViewerCommand);
            //Allows event parameter to pass through
            SearchCommand = new RelayCommand<AutoSuggestBoxQuerySubmittedEventArgs>(SearchPdfCommand);
            OpacityInkSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityInkCommand);
            SizeInkSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(SizeInkCommand);
            OpacityRectangleSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityRectangleCommand);
            SizeRectangleSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(SizeRectangleCommand);
            OpacityLineSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityLineCommand);
            SizeLineSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(SizeLineCommand);
            OpacityEllipseSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityEllipseCommand);
            SizeEllipseSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(SizeEllipseCommand);
            InkColorCommand = new RelayCommand<ColorChangedEventArgs>(InkColorUpdateCommand);
            RectangleColorCommand = new RelayCommand<ColorChangedEventArgs>(RectangleColorUpdateCommand);
            EllipseColorCommand = new RelayCommand<ColorChangedEventArgs>(EllipseColorUpdateCommand);

            highlightCommand = new RelayCommand(highlightToggledCommand);
            highlightColorCommand = new RelayCommand<ColorChangedEventArgs>(highlightColorUpdateCommand);
            OpacityHighlightSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityhighlightCommand);
            highlight = false;

            lineCommand = new RelayCommand(LineToggledCommand);
            LineColorCommand = new RelayCommand<ColorChangedEventArgs>(LineColorUpdateCommand);
            Line = false;
            OpacityLineSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityLineCommand);
            SizeLineSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(SizeLineCommand);

            StrikeoutCommand = new RelayCommand(StrikeoutToggledCommand);
            StrikeoutColorCommand = new RelayCommand<ColorChangedEventArgs>(StrikeoutColorUpdateCommand);
            Strikeout = false;
            OpacityStrikeoutSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityStrikeoutCommand);

            UnderlineCommand = new RelayCommand(UnderlineToggledCommand);
            UnderlineColorCommand = new RelayCommand<ColorChangedEventArgs>(UnderlineColorUpdateCommand);
            Underline = false;
            OpacityUnderlineSliderCommand = new RelayCommand<RangeBaseValueChangedEventArgs>(OpacityUnderlineCommand);
            PositionUpdatedCommand = new RelayCommand<AutoSuggestBoxQuerySubmittedEventArgs>(PdfPositionCommand);
        }

    

        private void PdfPositionCommand(AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            try
            {
                pdfViewer.GotoPage(int.Parse(e.QueryText));
            }
            catch
            {
                //most likely nullreference
            }
        }
        private void InkColorUpdateCommand(ColorChangedEventArgs e)
        {
            pdfViewer.InkAnnotationSettings.Color = e.NewColor;
            InkColor = e.NewColor;
            OnPropertyChanged(nameof(InkColor));
        }
        private async void RectangleToggledCommand()
        {
            await DisableAnnotations();
            pdfViewer.RectangleAnnotationCommand.Execute(Rectangle);
            Ink = false;
            Ellipse = false;
            Line = false;
            highlight = false;
            Underline = false;
            pdfViewer.RectangleAnnotationSettings.Color = RectangleColor;
        }
        public async Task DisableAnnotations()
        {
            pdfViewer.InkAnnotationCommand.Execute(false);
            pdfViewer.HighlightAnnotationCommand.Execute(false);
            pdfViewer.UnderlineAnnotationCommand.Execute(false);
            pdfViewer.StrikeoutAnnotationCommand.Execute(false);
            pdfViewer.LineAnnotationCommand.Execute(false);
            pdfViewer.RectangleAnnotationCommand.Execute(false);
            pdfViewer.EllipseAnnotationCommand.Execute(false);
            pdfViewer.PopupAnnotationCommand.Execute(false);
        }
        private async void UnderlineToggledCommand()
        {
            await DisableAnnotations();
            pdfViewer.UnderlineAnnotationCommand.Execute(Underline);
            Rectangle = false;
            Ellipse = false;
            Ink = false;
            highlight = false;
            Strikeout = false;
            pdfViewer.UnderlineAnnotationSettings.Color = UnderlineColor;
        }
        private void UnderlineColorUpdateCommand(ColorChangedEventArgs e)
        {
            pdfViewer.UnderlineAnnotationSettings.Color = e.NewColor;
            UnderlineColor = e.NewColor;
            OnPropertyChanged(nameof(InkColor));
        }
        private void OpacityUnderlineCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.UnderlineAnnotationSettings.Opacity = e.NewValue;
        }

        private async void highlightToggledCommand()
        {
            await DisableAnnotations();
            pdfViewer.HighlightAnnotationCommand.Execute(highlight);
            Rectangle = false;
            Ellipse = false;
            Underline = false;
            Strikeout = false;
            Ink = false;
            pdfViewer.HighlightAnnotationSettings.Color = highlightColor;
        }
        private void highlightColorUpdateCommand(ColorChangedEventArgs e)
        {
            pdfViewer.HighlightAnnotationSettings.Color = e.NewColor;
            highlightColor = e.NewColor;
            OnPropertyChanged(nameof(InkColor));
        }
        private void OpacityhighlightCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.HighlightAnnotationSettings.Opacity = e.NewValue;
        }


        private void RectangleColorUpdateCommand(ColorChangedEventArgs e)
        {
            pdfViewer.RectangleAnnotationSettings.Color = e.NewColor;
            RectangleColor = e.NewColor;
            OnPropertyChanged(nameof(InkColor));
        }
        private void SizeRectangleCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.RectangleAnnotationSettings.Thickness = e.NewValue;
        }
        private void OpacityRectangleCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.RectangleAnnotationSettings.Opacity = e.NewValue;
        }
        private async void LineToggledCommand()
        {
            await DisableAnnotations();
            pdfViewer.LineAnnotationCommand.Execute(Line);
            Rectangle = false;
            Ellipse = false;
            Ink = false;
            highlight = false;
            Underline = false;
            Strikeout = false;
            pdfViewer.LineAnnotationSettings.Color = LineColor;
        }
        private void LineColorUpdateCommand(ColorChangedEventArgs e)
        {
            pdfViewer.LineAnnotationSettings.Color = e.NewColor;
            LineColor = e.NewColor;
            OnPropertyChanged(nameof(InkColor));
        }
        private void OpacityLineCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.LineAnnotationSettings.Opacity = e.NewValue;
        }
        private void SizeLineCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.LineAnnotationSettings.Thickness = e.NewValue;
        }
        private async void EllipseToggledCommand()
        {
            await DisableAnnotations();
            pdfViewer.EllipseAnnotationCommand.Execute(Ellipse);
            Ink = false;
            Rectangle = false;
            Line = false;
            highlight = false;
            Underline = false;
            Strikeout = false;
            pdfViewer.EllipseAnnotationSettings.Color = EllipseColor;
        }
        private void EllipseColorUpdateCommand(ColorChangedEventArgs e)
        {
            pdfViewer.EllipseAnnotationSettings.Color = e.NewColor;
            EllipseColor = e.NewColor;
            OnPropertyChanged(nameof(InkColor));
        }
        private void SizeEllipseCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.EllipseAnnotationSettings.Thickness = e.NewValue;
        }
        private void OpacityEllipseCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.EllipseAnnotationSettings.Opacity = e.NewValue;
        }
        private void SizeInkCommand(RangeBaseValueChangedEventArgs e)
        {
                pdfViewer.InkAnnotationSettings.Thickness = e.NewValue;
        }
        private void OpacityInkCommand(RangeBaseValueChangedEventArgs e)
        {
                pdfViewer.InkAnnotationSettings.Opacity = e.NewValue;
        }
        private async void StrikeoutToggledCommand()
        {
            await DisableAnnotations();
            pdfViewer.StrikeoutAnnotationCommand.Execute(Strikeout);
            Rectangle = false;
            Ellipse = false;
            Ink = false;
            highlight = false;
            Underline = false;
            pdfViewer.StrikethroughAnnotationSettings.Color = StrikeoutColor;
        }
        private void StrikeoutColorUpdateCommand(ColorChangedEventArgs e)
        {
            pdfViewer.StrikethroughAnnotationSettings.Color = e.NewColor;
            StrikeoutColor = e.NewColor;
            OnPropertyChanged(nameof(InkColor));
        }
        private void OpacityStrikeoutCommand(RangeBaseValueChangedEventArgs e)
        {
            pdfViewer.StrikethroughAnnotationSettings.Opacity = e.NewValue;
        }
        private async void InkToggledCommand()
        {
            await DisableAnnotations();
            pdfViewer.InkAnnotationCommand.Execute(Ink);
            Rectangle = false;
            Ellipse = false;
            Line = false;
            highlight = false;
            Strikeout = false;
            Underline = false;
            pdfViewer.InkAnnotationSettings.UseWindowsInkCanvas = _NativeInk;
            pdfViewer.InkAnnotationSettings.Color = InkColor;
        }
        private async void InkPerformanceToggledCommand(RoutedEventArgs e)
        {
            //delay due to visual bug or something
            await Task.Delay(300);
            pdfViewer.InkAnnotationSettings.UseWindowsInkCanvas = _NativeInk;
        }
        public async void SearchPdfCommand(AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            searchString = args.QueryText;
            pdfViewer.SearchText(args.QueryText);
        }
        public async void SearchPdfNextCommand(AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            searchString = args.QueryText;
            pdfViewer.SearchNextText(args.QueryText);
        }
        private async void SaveAsFileViewerCommand()
        {
            Stream stream = pdfViewer.Save();
            stream.Position = 0;

            FileSavePicker savePicker = new FileSavePicker();
            savePicker.DefaultFileExtension = ".pdf";
            savePicker.FileTypeChoices.Add("Adobe PDF Document", new List<string>() { ".pdf" });
            StorageFile stFile = await savePicker.PickSaveFileAsync();
            if (stFile != null)
            {
                Windows.Storage.Streams.IRandomAccessStream fileStream = await stFile.OpenAsync(FileAccessMode.ReadWrite);
                Stream st = fileStream.AsStreamForWrite();
                st.SetLength(0);
                st.Write((stream as MemoryStream).ToArray(), 0, (int)stream.Length);
                st.Flush();
                st.Dispose();
                fileStream.Dispose();
            }
        }
       private async void OpenFileViewerCommand()
        {
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
            //Loadds bookmarks
            bookmarkBase = loadedDocument.Bookmarks;
            PdfBookmark bookmark;
            SfTreeNavigatorItem navigatorItem;
            //Runs through initial bookmarks
            for (int i = 0; i < bookmarkBase.Count; i++)
            {
                bookmark = bookmarkBase[i] as PdfLoadedBookmark;
                //  Here we create a rootnode for treeview with the command
                //  It includes the child nodes too
                TreeViewNode rootNode = AddChildBookmarks(bookmark);
                treeNavigator.ItemInvoked += TreeNavigator_ItemInvoked;
                treeNavigator.RootNodes.Add(rootNode);
            }
        }

        private void TreeNavigator_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            //  Gets selected node
            var node = args.InvokedItem as TreeViewNode;
            PdfBookmark bookmark;

            //  Runs through bookmarks
            for (int i = 0; i < bookmarkBase.Count; i++)
            {
                bookmark = bookmarkBase[i] as PdfLoadedBookmark;


                //  Checks if bookmark is the same as the node, then navigates if true
                if(bookmark.Title == node.Content)
                {
                    pdfViewer.GoToBookmark(bookmark);
                    return;
                }
                else if(bookmark.Count != 0 && bookmark.Title != node.Content)
                {
                    // Checks if any of the bookmark children is the same as the node, then navigates if true
                        //  Runs through child bookmarks
                        for (int ii = 0; ii < bookmark.Count; ii++)
                        {
                            if(node.Content == bookmark[ii].Title)
                            {
                               pdfViewer.GoToBookmark(bookmark[ii]);
                               return;
                            }
                        }
                }
            }
        }

        private TreeViewNode AddChildBookmarks(PdfBookmark bookmark)
        {
            //Root nodes of treeviews are the top part which can have content udnerneath. Here we create a root node
            TreeViewNode rootNode = new TreeViewNode() { Content = bookmark.Title };
            if (bookmark != null)
            {
                //  Checks for child bookmarks
                if (bookmark.Count != 0)
                {
                    //  Runs through child bookmarks
                    for (int i = 0; i < bookmark.Count; i++)
                    {
                        TreeViewNode childNode = new TreeViewNode() { Content = bookmark[i].Title };
                        //  Adds child to rootnode
                        rootNode.Children.Add(childNode);
                    }
                }

                return rootNode;
            }
            else
                //Return null if no bookmarks
                return rootNode;
        }
        #region Dispose
        public void Dispose()
        {

        }
        #endregion Dispose
    }
}
