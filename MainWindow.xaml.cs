using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteTaker2000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CanvasDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void CanvasDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    AddImageToCanvas(file);
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string text = (string)e.Data.GetData(DataFormats.Text);
                AddTextToCanvas(text);
            }
        }

        private void AddImageToCanvas(string imagePath)
        {
            try
            {
                BitmapImage bmp = new BitmapImage(new Uri(imagePath));
                Image image = new Image
                {
                    Source = bmp,
                    Width = bmp.Width,
                    Height = bmp.Height
                };
                Canvas.SetLeft(image, 50);
                Canvas.SetTop(image, 50);
                mainCanvas.Children.Add(image);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }
        private void AddTextToCanvas(string text)
        {
            TextBlock tb = new TextBlock
            {
                Text = text,
                FontSize = 16,
                Foreground = Brushes.Black

            };
            Canvas.SetLeft(tb, 50);
            Canvas.SetTop(tb, 50);
            mainCanvas.Children.Add(tb);
        }
        private void PasteImageFromClipboard(Point mousePosition)
        {


            if (Clipboard.ContainsImage())
            {
                BitmapSource bmpSrc = Clipboard.GetImage();
                if (bmpSrc != null)
                {
                    Image pastedImage = new Image
                    {
                        Source = bmpSrc,
                        Width = bmpSrc.Width,
                        Height = bmpSrc.Height
                    };

                    Canvas.SetLeft(pastedImage, mousePosition.X);
                    Canvas.SetTop(pastedImage, mousePosition.Y);
                    mainCanvas.Children.Add(pastedImage);
                }
            }

        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Point mousePosition = Mouse.GetPosition(mainCanvas);
                PasteImageFromClipboard(mousePosition);

            }
            base.OnKeyDown(e);
        }
    }
}