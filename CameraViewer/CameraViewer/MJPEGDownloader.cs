using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CameraViewer
{
    public static class MJPEGDownloader
    {
        public async static Task BeginDownload(string url, Action<byte[]> OnFrameLoaded, int frameBufferSize = 1024, int bufferSize = 1024, CancellationToken? token = null)
        {
            using (var client = new HttpClient())
            {
                using (var stream = await client.GetStreamAsync(url).ConfigureAwait(false))
                {
                    var lenght = 0;
                    var index = 0;
                    var _byte = byte.MinValue;
                    var isGotControlByte = false;
                    var isWriteFrame = false;

                    var frameBuffer = new List<byte>(frameBufferSize);
                    var streamBuffer = new byte[bufferSize];

                    var _token = token.HasValue ? token.Value : CancellationToken.None;

                    while (!_token.IsCancellationRequested)
                    {
                        lenght = await stream.ReadAsync(streamBuffer, 0, bufferSize).ConfigureAwait(false);
                        index = 0;

                        while (index < lenght)
                        {
                            _byte = streamBuffer[index++];
                            
                            if (isWriteFrame)
                            {
                                frameBuffer.Add(_byte);
                            }

                            if (_byte.IsImageControlByte())
                            {
                                isGotControlByte = true;
                            }
                            else if(isGotControlByte)
                            {
                                isGotControlByte = false;

                                if (_byte.IsImageStart())
                                {
                                    isWriteFrame = true;

                                    frameBuffer.Clear();
                                    frameBuffer.Add(Control);
                                    frameBuffer.Add(ImageStart);
                                }
                                else if (isWriteFrame && _byte.IsImageEnd())
                                {
                                    isWriteFrame = false;
                                    
                                    OnFrameLoaded(frameBuffer.ToArray());
                                }
                            }
                        }
                    }
                }
            }
        }

        static bool IsImageControlByte(this byte value) => value == Control;
        static bool IsImageStart(this byte value) => value == ImageStart;
        static bool IsImageEnd(this byte value) => value == ImageEnd;

        static byte Control => 0xff;
        static byte ImageStart => 0xd8;
        static byte ImageEnd => 0xd9;
    }
}
