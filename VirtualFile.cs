using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_
{
    internal class VirtualFile : Catalog
    {

        public string data = "";

        public VirtualFile() { }
        public VirtualFile(string name)
        {
            Name = name;
        }
    }
}
