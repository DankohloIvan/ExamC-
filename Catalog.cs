using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_
{
    internal class Catalog
    {
        public string Name { get; set; }
        private Folder baseCatalog;
        public Catalog() { }

        public override string ToString()
        {
            return Name;
        }

        public void PrintName()
        {
            Console.WriteLine(Name);
        }

        public void SetBase(Folder BaseCatalog)
        {
            baseCatalog = BaseCatalog;
        }

        public Folder GetBase()
        {
            return baseCatalog;
        }

        public void Rename(string newName)
        {
            Name = newName;
        }
    }
}
