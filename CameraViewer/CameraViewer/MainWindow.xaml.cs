using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CameraViewer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConnectionSettings connectionSettings = ConnectionSettings.Default;
        CameraSettings cameraSettings = CameraSettings.Default;

        Camera camera;
        Configuration configuration;

        DispatcherOperation videoUpdater;

        public MainWindow()
        {
            InitializeComponent();

            configuration = ConfigurationDownloader.DownloadFromXml(connectionSettings);
            camera = new Camera(connectionSettings);

            ChannelsSelector.ItemsSource = configuration.Channels.Select(x => x.Name);
            ChannelsSelector.SelectedIndex = 0;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        void ChannelsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Connect();
        }

        void Connect()
        {
            cameraSettings.ChannelId = configuration.Channels[ChannelsSelector.SelectedIndex].Id;
            if (ConnectionButton.IsChecked ?? false)
            {
                camera.Connect(cameraSettings);
            }
            camera.Add(UpdateImage);
        }

        void Disconnect()
        {
            camera.Disconnect();
            video.Source = null;
            camera.Remove(UpdateImage);
        }

        void UpdateImage(byte[] rawImage)
        {
            if (videoUpdater != null)
            {
                videoUpdater.Abort();
                videoUpdater = null;
            }
            videoUpdater = Dispatcher.InvokeAsync((Action)(() => video.Source = ImageConverter.GetBitmapImage(rawImage)));
        }
    }
}
