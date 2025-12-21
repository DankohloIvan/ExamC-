using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_
{
    internal class Disk
    {
        private Catalog currentCatalog;
        private Folder previousCatalog;
        private string name;
        private List<Catalog> roots;

        public Disk(string Name)
        {
            name = Name;
            roots = new List<Catalog>();
        }

        public void AddCatalog(Catalog catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException("Catalog cannot be null");
            }

            if (currentCatalog == null)
            {
                catalog.SetBase(null);
                roots.Add(catalog);
            }
            else
            {
                catalog.SetBase((Folder)currentCatalog);

                if (currentCatalog is Folder currentFolder)
                {
                    currentFolder.AddCatalog(catalog);
                }
            }
        }

        public void SetCurrentCatalog(int Index)
        {
            if (currentCatalog == null)
            {
                if (Index >= 0 && Index < roots.Count)
                {
                    currentCatalog = roots[Index];
                }
            }
            else
            {
                Folder currentFolder = currentCatalog as Folder;
                List<Catalog> children = currentFolder.GetCatalogs();
                if (Index >= 0 && Index < children.Count)
                {
                    previousCatalog = (Folder)currentCatalog;
                    currentCatalog = children[Index];
                    currentCatalog.SetBase(previousCatalog);
                }
            }
        }

        public void GoBack()
        {
            if (currentCatalog == null)
            {
                return;
            }
            if (currentCatalog.GetBase() == null)
            {
                currentCatalog = null;
                return;
            }
            Catalog prev = currentCatalog.GetBase().GetBase();
            if (prev == null)
            {
                previousCatalog = null;
                currentCatalog = currentCatalog.GetBase();
            }
            else
            {
                previousCatalog = (Folder)prev;
                currentCatalog = currentCatalog.GetBase();
            }
        }

        public Catalog GetCurrentCatalog()
        {
            return currentCatalog;
        }

        public Folder GetPreviousFolder()
        {
            return previousCatalog;
        }

        public List<Catalog> GetRoots()
        {
            return roots;
        }

        public List<Catalog> GetCurrentCatalogs()
        {
            if (currentCatalog == null)
            {
                return roots;
            }
            else if (currentCatalog is Folder currentFolder)
            {
                return currentFolder.GetCatalogs();
            }
            else
            {
                throw new InvalidOperationException("Current catalog is not a folder");
            }
        }

        public void PrintContent()
        {
            if (currentCatalog == null)
            {
                foreach (Catalog root in roots)
                {
                    root.PrintName();
                }
            }
            else if (currentCatalog is Folder currentFolder)
            {
                List<Catalog> catalogs = currentFolder.GetCatalogs();
                foreach (Catalog catalog in catalogs)
                {
                    catalog.PrintName();
                }
            }
            else
            {
                throw new InvalidOperationException("Current catalog is not a folder");
            }
        }
    }
}
