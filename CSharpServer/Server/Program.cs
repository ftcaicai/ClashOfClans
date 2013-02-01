using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test();
            test.Startup(60000, 100);
            test.Stop();
        }
    }
}
