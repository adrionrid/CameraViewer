using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraViewer
{
    public class CameraSettings
    {
        private static CameraSettings _Default;
        public string ChannelId;
        public int ResolutionX { get; private set; }
        public int ResolutionY { get; private set; }
        public int Fps { get; private set; }

        public CameraSettings(string channelId, int resolutionX, int resolutionY, int fps)
        {
            ChannelId = channelId;
            ResolutionX = resolutionX;
            ResolutionY = resolutionY;
            Fps = fps;
        }
        public static CameraSettings Default =>
            _Default == null ? _Default = new CameraSettings("2016897c-8be5-4a80-b1a3-7f79a9ec729c", 640, 480, 25) : _Default;
    }
}
