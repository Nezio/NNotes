using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Configuration;

namespace NNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string localNNotesPath = localPath + "\\NNotes";
        static string backupFolder = localNNotesPath + "\\backup";
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
            Directory.CreateDirectory(backupFolder);

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

            saveText(textFilePath);
            UpdateBackups();
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
            saveText(textFilePath);
            UpdateBackups();
        }

        private void saveText(string file)
        {
            string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            try
            {
                richText = richText.Remove(richText.Length - 2);
            }
            catch { }
            File.WriteAllText(file, richText);
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

        private void UpdateBackups()
        {
            int maxBackupFiles = 5;
            int backupInterval = 1;

            try
            {
                maxBackupFiles = Int32.Parse(ConfigurationManager.AppSettings["maxBackupFiles"]);
                backupInterval = Int32.Parse(ConfigurationManager.AppSettings["backupInterval"]);
            }
            catch
            {
                MessageBox.Show("Could not load a setting from the app settings config file! Please, check the file!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            var backupFiles = Directory.GetFiles(backupFolder);
            bool doBackup = false;

            if(backupFiles.Length > 0)
            {
                // compare newest backup date with current time to decide if another backup should be created
                // 2021-03-29
                var lastBackupFile = backupFiles[backupFiles.Length - 1];
                string lastBackupFileName = Path.GetFileNameWithoutExtension(lastBackupFile);
                string[] lastBackupDateArray = lastBackupFileName.Split('-');

                DateTime lastBackupDateTime;

                try
                {
                    lastBackupDateTime = new DateTime(Int32.Parse(lastBackupDateArray[0]), Int32.Parse(lastBackupDateArray[1]), Int32.Parse(lastBackupDateArray[2]));
                }
                catch
                {
                    // delete the wrongly named file
                    File.Delete(lastBackupFile);

                    return;
                }

                var daysFromLastBackup = (DateTime.Now - lastBackupDateTime).TotalDays;

                if(daysFromLastBackup >= backupInterval)
                {
                    doBackup = true;
                }

            }

            if (backupFiles.Length == 0 || doBackup)
            {
                // create a new backup
                string newBackupFileName = DateTime.Now.ToString("yyyy'-'MM'-'dd");
                string newBackupFilePath = backupFolder + "\\" + newBackupFileName + ".txt";

                saveText(newBackupFilePath);
            }

            // delete oldest backup file
            if(backupFiles.Length >= maxBackupFiles)
            {
                File.Delete(backupFiles[0]);
            }
        }


    }
}
