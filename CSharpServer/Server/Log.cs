using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class Log
    {
        private static Log _It;

        private FileStream _files;
        private string _FilePath = "Log\\Server.log";

        static Log()
        {
            _It = new Log();
        }

        public Log()
        {
            _FilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _FilePath);
            _files = File.Open(_FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

            Write2File("Application Begin");
        }

        ~Log()
        {
            if (_files != null)
            {
                Write2File("Application End");
                _files.Close();
            }
        }

        public void Write2File(string msg)
        {
            if (_files != null)
            {
                byte[] bs = Encoding.UTF8.GetBytes("[" + DateTime.UtcNow.ToString() + "] " + msg + "\n");
                _files.Write(bs, 0, bs.Length);
                _files.Flush();
            }
        }

        public static void Info(string msg)
        {
            Console.WriteLine(msg);
            _It.Write2File(msg);
        }
    }
}
