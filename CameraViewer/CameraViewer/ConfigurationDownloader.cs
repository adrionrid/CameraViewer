using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace CameraViewer
{
    public static class ConfigurationDownloader
    {
        public static Configuration DownloadFromXml(string server, int port, string login)
        {
            var xmlRaw = Download($"{server}:{port}/configex?login={login}");

            using (TextReader reader = new StringReader(xmlRaw))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                return (Configuration)serializer.Deserialize(reader);
            }
        }

        public static Configuration DownloadFromXml(ConnectionSettings connectionSettings) =>
            DownloadFromXml(connectionSettings.Server, connectionSettings.Port, connectionSettings.Login);

        static string Download(string url)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadString(url);
            }
        }
    }
}
