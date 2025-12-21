using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_
{
    internal class FileManager
    {
        private Disk currentDisk;
        private Folder baseFolder;
        int selectedIndex = -1;
        List<Disk> drives;
        private Catalog buffer;

        public FileManager()
        {
            drives = new List<Disk>();
        }

        public void AddDisk(Disk disk)
        {
            drives.Add(disk);
        }

        public void SetCurrentDisk(int Index)
        {
            if (Index >= 0 && Index < drives.Count)
            {
                currentDisk = drives[Index];
            }
        }

        public void CreateFile(string Name)
        {
            File newFile = new File(Name);
            if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                currentFolder.AddCatalog(newFile);
                return;
            }
            throw new InvalidDataException("Current catalog is not a folder");
        }

        public void CreateFolder(string Name)
        {
            Folder newFolder = new Folder(Name);
            if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                currentFolder.AddCatalog(newFolder);
                return;
            }
            if (currentDisk.GetCurrentCatalog() == null)
            {
                currentDisk.AddCatalog(newFolder);
                return;
            }
            throw new InvalidDataException("Current catalog is not a folder");
        }

        public void CreateDisk(string name)
        {
            Disk newDisk = new Disk(name);
            drives.Add(newDisk);
        }

        public void GoToSelectedCatalog()
        {
            currentDisk.SetCurrentCatalog(selectedIndex);
            baseFolder = currentDisk.GetPreviousFolder();
            selectedIndex = -1;
        }

        public void GoBack()
        {
            currentDisk.GoBack();
        }

        public void PrintCurrentContent()
        {
            currentDisk.PrintContent();
        }

        public void SetIndex(int index)
        {
            selectedIndex = index;
        }

        public void CopyCatalog()
        {
            if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                List<Catalog> children = currentFolder.GetCatalogs();
                if (selectedIndex >= 0 && selectedIndex < children.Count)
                {
                    buffer = children[selectedIndex];
                }
            }
        }

        public void PasteCatalog()
        {
            if (buffer != null && currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                currentFolder.AddCatalog(buffer);
            }
        }

        public void DeleteCatalog()
        {
            if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                List<Catalog> children = currentFolder.GetCatalogs();
                if (selectedIndex >= 0 && selectedIndex < children.Count)
                {
                    currentFolder.RemoveCatalog(children[selectedIndex]);
                }
            }
        }

        public void RenameCatalog(string newName)
        {
            if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                List<Catalog> children = currentFolder.GetCatalogs();
                if (selectedIndex >= 0 && selectedIndex < children.Count)
                {
                    children[selectedIndex].Rename(newName);
                }
            }
        }

        public void Menu()
        {
            while (true)
            {
                Console.WriteLine("File Manager Menu:");
                Console.WriteLine("1. Create Disk");
                Console.WriteLine("2. Create Folder");
                Console.WriteLine("3. Create File");
                Console.WriteLine("4. Select Catalog");
                Console.WriteLine("5. Go To Selected Catalog");
                Console.WriteLine("6. Go Back");
                Console.WriteLine("7. Copy Catalog");
                Console.WriteLine("8. Paste Catalog");
                Console.WriteLine("9. Delete Catalog");
                Console.WriteLine("10. Rename Catalog");

                Console.Write("Current catalog: ");
                if (currentDisk.GetCurrentCatalog() != null)
                {
                    Console.WriteLine(currentDisk.GetCurrentCatalog().Name);
                }
                else
                {
                    Console.WriteLine("Root");
                }
                Console.WriteLine("Content: ");
                PrintCurrentContent();

                Console.Write("Selected catalog: ");
                if (selectedIndex != -1)
                {
                    Console.WriteLine(currentDisk.GetCurrentCatalogs()[selectedIndex]);
                }
                else
                {
                    Console.WriteLine("None");
                }

                Console.Write("Enter option code: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter disk name: ");
                        string diskName = Console.ReadLine();
                        CreateDisk(diskName);
                        break;
                    case "2":
                        Console.Write("Enter folder name: ");
                        string folderName = Console.ReadLine();
                        CreateFolder(folderName);
                        break;
                    case "3":
                        Console.Write("Enter file name: ");
                        string fileName = Console.ReadLine();
                        CreateFile(fileName);
                        break;
                    case "4":
                        Console.Write("Enter catalog index to select: ");
                        if (int.TryParse(Console.ReadLine(), out int index))
                        {
                            SetIndex(index);
                        }
                        else
                        {
                            Console.WriteLine("Invalid index input.");
                        }
                        break;
                    case "5":
                        GoToSelectedCatalog();
                        break;
                    case "6":
                        GoBack();
                        break;
                    case "7":
                        CopyCatalog();
                        break;
                    case "8":
                        PasteCatalog();
                        break;
                    case "9":
                        DeleteCatalog();
                        break;
                    case "10":
                        Console.Write("Enter new name for the catalog: ");
                        string newName = Console.ReadLine();
                        RenameCatalog(newName);
                        break;
                    default:
                        Console.WriteLine("Invalid option selected.");
                        break;
                }

                Console.Clear();
            }
            
        }
    }
}
