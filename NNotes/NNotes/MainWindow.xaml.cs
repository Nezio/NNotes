using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string localNNotesPath = localPath + "\\NNotes";
        //static string localNNotesPath = localPath + "\\NNotes\\Test";
        string textFilePath = localNNotesPath + "\\Text.txt";
        string settingsFilePath = localNNotesPath + "\\Settings.txt";
        string darkTheme = "0";

        SolidColorBrush colorTitle1 = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
        SolidColorBrush colorText1 = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));

        SolidColorBrush colorTitle2 = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
        SolidColorBrush colorText2 = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(localNNotesPath);

            if (File.Exists(settingsFilePath))
            {
                StreamReader file = new StreamReader(settingsFilePath);

                this.Left = Double.Parse(file.ReadLine());
                this.Top = Double.Parse(file.ReadLine());
                this.Width = Double.Parse(file.ReadLine());
                this.Height = Double.Parse(file.ReadLine());
                darkTheme = file.ReadLine();

                file.Close();
            }
            else
            {
                saveSettings();
            }

            updateTheme();

            if (File.Exists(textFilePath))
            {
                string tmpText = File.ReadAllText(textFilePath);
                richTextBox.Document.Blocks.Clear();
                richTextBox.Document.Blocks.Add(new Paragraph(new Run(tmpText)));
            }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;

            imageTheme.Visibility = Visibility.Visible;
        }

        private async void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            imageTheme.Visibility = Visibility.Hidden;

            saveText();
            saveSettings();

            await Task.Delay(2000);
            this.ResizeMode = System.Windows.ResizeMode.NoResize;

        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch { }

        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            saveText();
        }

        private void saveText()
        {
            string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            try
            {
                richText = richText.Remove(richText.Length - 2);
            }
            catch { }
            File.WriteAllText(textFilePath, richText);
        }

        private void saveSettings()
        {
            string x = this.Left.ToString();
            string y = this.Top.ToString();
            string w = this.ActualWidth.ToString();
            string h = this.ActualHeight.ToString();

            string[] lines = { x, y, w, h, darkTheme };
            File.WriteAllLines(settingsFilePath, lines);
        }

        private void updateTheme()
        {
            if (darkTheme == "0")
            {
                label.Background = colorTitle1;
                richTextBox.Background = colorText1;
            }
            else
            {
                label.Background = colorTitle2;
                richTextBox.Background = colorText2;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridBtn.Width = this.Width;
        }

        private void imageClose_MouseEnter(object sender, MouseEventArgs e)
        {
            imageClose.Source = new BitmapImage(new Uri("/Resources/CloseWhiteAlpha90.png", UriKind.Relative));
        }

        private void imageClose_MouseLeave(object sender, MouseEventArgs e)
        {
            imageClose.Source = new BitmapImage(new Uri("/Resources/CloseWhiteAlpha50.png", UriKind.Relative));
        }

        private void imageClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            imageTheme.Source = new BitmapImage(new Uri("/Resources/ThemeWhiteAlpha90.png", UriKind.Relative));
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            imageTheme.Source = new BitmapImage(new Uri("/Resources/ThemeWhiteAlpha50.png", UriKind.Relative));
        }

        private void imageTheme_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (darkTheme == "0")
            {
                darkTheme = "1";
            }
            else
            {
                darkTheme = "0";
            }

            updateTheme();
            saveSettings();
        }


    }
}
