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
            hello.RunKinect();

            while (hello.HelloCounter < 10)
            {
            }
        }
    }
}
