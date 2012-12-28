using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class Program
    {
        public static string serverIP = "127.0.0.1";
        public static ushort serverPort = 60000;
        public static string password = "";

        private static XNetWork2 _client2;

        static void Main(string[] args)
        {
            _Connect2();

            Console.ReadKey();
        }

        static void _Connect2()
        {
            _Init2();
            RakNet.ConnectionAttemptResult result = _client2.Connect(serverIP, serverPort, password);
            Console.WriteLine("ConnectionAttemptResult result = " + result);
        }

        static void _Init2()
        {
            if (_client2 == null)
            {
                _client2 = new XNetWork2();
                _client2._Init();
            }
        }
    }


}
