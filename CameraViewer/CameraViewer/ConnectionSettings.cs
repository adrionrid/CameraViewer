using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraViewer
{
    public class ConnectionSettings
    {
        private static ConnectionSettings _Default;
        public string Server { get; private set; }
        public int Port { get; private set; }
        public string Login { get; private set; }

        public ConnectionSettings(string server, int port, string login)
        {
            Server = server;
            Port = port;
            Login = login;
        }

        public static ConnectionSettings Default =>
            _Default == null ? _Default = new ConnectionSettings("http://demo.macroscop.com", 8080, "root") : _Default;
    }
}
