using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            while(true)
                System.Console.ReadLine();
        }
    }
}
