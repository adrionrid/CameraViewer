using System;
using System.Text;
using System.Threading;

namespace CameraViewer
{
    public class Camera
    {
        public ConnectionSettings ConnectionSettings { get; private set; }
        public CameraSettings CameraSettings { get; private set; }
        public bool IsConnected { get; private set; }

        Action<byte[]> OnFrameLoaded;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public Camera(ConnectionSettings connectionSettings)
        {
            ConnectionSettings = connectionSettings;
        }

        public async void Connect(CameraSettings cameraSettings)
        {
            CameraSettings = cameraSettings;

            try
            {
                if (IsConnected)
                    Disconnect();

                IsConnected = true;
                await MJPEGDownloader.BeginDownload($"{ConnectionSettings.Server}:{ConnectionSettings.Port}" +
                    $"/mobile?login={ConnectionSettings.Login}&channelid={CameraSettings.ChannelId}" +
                    $"&resolutionX={CameraSettings.ResolutionX}&resolutionY={CameraSettings.ResolutionY}&fps={CameraSettings.Fps}",
                    OnFrameLoaded, CameraSettings.ResolutionX * CameraSettings.ResolutionY * 4, 1024, tokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Disconnect()
        {
            IsConnected = false;
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
        }

        public void Add(Action<byte[]> action)
        {
            OnFrameLoaded += action;
        }

        public void Remove(Action<byte[]> action)
        {
            OnFrameLoaded -= action;
        }
    }
}
