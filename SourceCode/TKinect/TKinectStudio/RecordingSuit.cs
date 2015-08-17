using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKinectStudio
{
    public class RecordingSuit
    {
        public string Name { get; set; }

        public List<Recording> Recordings { get; set; }
 
        public RecordingSuit(string name)
        {
            Name = name;
            Recordings = new List<Recording>();
        }
    }
}
