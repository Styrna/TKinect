using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKinectStudio
{
    public class Recording
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string GetFullPath()
        {
            return System.IO.Path.Combine(Path,Name);
        }

        public Recording(string name,string path)
        {
            Name = name;
            Path = path;
        }
    }
}
