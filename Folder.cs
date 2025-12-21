using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_
{
    internal class Folder : Catalog
    {
        private List<Catalog> catalogs = new List<Catalog>();
        public Folder() { }
        public Folder(string name) {
            Name = name;
        }

        public void AddCatalog(Catalog catalog)
        {
            catalogs.Add(catalog);
        }

        public void RemoveCatalog(Catalog catalog)
        {
            catalogs.Remove(catalog);
        }

        public List<Catalog> GetCatalogs()
        {
            return catalogs;
        }
    }
}
