using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_
{
    internal class File : Catalog
    {
        public File() { }
        public File(string name)
        {
            Name = name;
        }
    }
}
