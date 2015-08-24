using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKinect
{
    public class Program
    {
        static void Main(string[] args)
        {
            var hello = new HelloKinect();
            hello.HelloDetected += hello_HelloDetected;

            //hello.RunRealKinect();
            //hello.RunKinectReplay();
            hello.RunKinectClient();

            while (hello.HelloCounter < 10)
            {
            }
        }

        static void hello_HelloDetected(object sender, int helloCount)
        {
            Console.WriteLine(string.Format("Hello nr {0} Detected !!!", helloCount));
        }
    }
}
