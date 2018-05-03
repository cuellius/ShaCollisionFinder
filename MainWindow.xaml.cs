using System.Threading;
using System.Windows;
using Microsoft.Win32;

namespace ShaCollisionFinder
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            FirstPdfTextBox.AllowableExtensions = SecondPdfTextBox.AllowableExtensions = new[] { ".pdf" };
            FirstJpgTextBox.AllowableExtensions = SecondJpgTextBox.AllowableExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".tif", ".bmp" };
        }

        private void BrowseFirstJpegButtonClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select a first image",
                Filter = "Images (*.jpg; *.jpeg; *.png; *.gif; *.tiff; *.tif; *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.tif;*.bmp|JPEG Image File (*.jpg; *.jpeg)|*.jpg;*.jpeg|Portable Network Graphic (*.png)|*.png|Graphical Interchange File (*.gif)|*.gif|Tagged Image File (*.tiff; *.tif)|*.tiff;*.tif|Bitmap Image File (*.bmp)|*.bmp"
            };
            if (ofd.ShowDialog() == true) FirstJpgTextBox.Text = ofd.FileName;
        }

        private void BrowseSecondJpegButtonClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select a second image",
                Filter = "Images (*.jpg; *.jpeg; *.png; *.gif; *.tiff; *.tif; *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.tif;*.bmp|JPEG Image File (*.jpg; *.jpeg)|*.jpg;*.jpeg|Portable Network Graphic (*.png)|*.png|Graphical Interchange File (*.gif)|*.gif|Tagged Image File (*.tiff; *.tif)|*.tiff;*.tif|Bitmap Image File (*.bmp)|*.bmp"
            };
            if (ofd.ShowDialog() == true) SecondJpgTextBox.Text = ofd.FileName;
        }

        private void BrowseFirstPdfButtonClick(object sender, RoutedEventArgs e)
        {
            var ofd = new SaveFileDialog
            {
                Title = "Select a first output pdf file",
                Filter = "Portable Document Format File (*.pdf)|*.pdf"
            };
            if (ofd.ShowDialog() == true) FirstPdfTextBox.Text = ofd.FileName;
        }

        private void BrowseSecondPdfButtonClick(object sender, RoutedEventArgs e)
        {
            var ofd = new SaveFileDialog
            {
                Title = "Select a second output pdf file",
                Filter = "Portable Document Format File (*.pdf)|*.pdf"
            };
            if (ofd.ShowDialog() == true) SecondPdfTextBox.Text = ofd.FileName;
        }

        private void BuildCollisionsButtonClick(object sender, RoutedEventArgs e)
        {
            var j1 = FirstJpgTextBox.Text;
            var j2 = SecondJpgTextBox.Text;
            var pdf1 = FirstPdfTextBox.Text;
            var pdf2 = SecondPdfTextBox.Text;
            new Thread(() =>
            {
                Core.GenerateFiles(j1, j2, pdf1, pdf2);
                MessageBox.Show("Collisions successfully generated!", "SHA-1 Collision Finder");
            }).Start();
        }
    }
}
