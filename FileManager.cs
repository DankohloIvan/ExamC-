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
        public Disk currentDisk;
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
            VirtualFile newFile = new VirtualFile(Name);
            if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                currentFolder.AddCatalog(newFile);
                return;
            }
            else if (currentDisk.GetCurrentCatalog() == null)
            {
                currentDisk.AddCatalog(newFile);
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
            else if (currentDisk.GetCurrentCatalog() == null)
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
            Catalog selected = currentDisk.GetCurrentCatalogs()[selectedIndex];

            if (selected is Folder)
            {
                currentDisk.SetCurrentCatalog(selectedIndex);
            }
            else
            {
                currentDisk.SetCurrentCatalog(selectedIndex);
            }

            selectedIndex = -1;
        }

        public void GoBack()
        {
            if (currentDisk.GetCurrentCatalog() == null)
            {
                currentDisk = null;
            }
            else
            {
                currentDisk.GoBack();
            }
        }

        public void PrintCurrentContent()
        {

            if (currentDisk.GetCurrentCatalog() is VirtualFile)
            {
                VirtualFile file = currentDisk.GetCurrentCatalog() as VirtualFile;
                Console.WriteLine(file.data);
                return;
            }

            if (currentDisk == null)
            {
                for (int i = 0; i < drives.Count; i++)
                {
                    Console.WriteLine($"{i}. {drives[i]}");
                }
            }
            else
            {
                currentDisk.PrintContent();
            }
        }

        public void SetIndex(int index)
        {
            selectedIndex = index;
        }

        public void CopyCatalog()
        {

            if (currentDisk.GetCurrentCatalog() == null)
            {
                List<Catalog> roots = currentDisk.GetRoots();
                if (selectedIndex >= 0 && selectedIndex < roots.Count)
                {
                    buffer = roots[selectedIndex];
                }
            }

            else if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
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

            if (currentDisk.GetCurrentCatalog() == null)
            {
                List<Catalog> roots = currentDisk.GetRoots();
                if (selectedIndex >= 0 && selectedIndex < roots.Count)
                {
                    roots.Remove(roots[selectedIndex]);
                }
            }

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

            if (currentDisk.GetCurrentCatalog() == null)
            {
                List<Catalog> roots = currentDisk.GetRoots();
                if (selectedIndex >= 0 && selectedIndex < roots.Count)
                {
                    roots[selectedIndex].Rename(newName);
                    selectedIndex = -1;
                }
            }

            else if (currentDisk.GetCurrentCatalog() is Folder currentFolder)
            {
                List<Catalog> children = currentFolder.GetCatalogs();
                if (selectedIndex >= 0 && selectedIndex < children.Count)
                {
                    children[selectedIndex].Rename(newName);
                    selectedIndex = -1;
                }
            }
        }

        public void Menu()
        {
            while (true)
            {

                if (currentDisk == null)
                {
                    Console.WriteLine("Available Disks:");
                    for (int i = 0; i < drives.Count; i++)
                    {
                        Console.WriteLine($"{i}. {drives[i].name}");
                    }
                    Console.Write("Select disk index to work with or create a new disk (enter 'c' to create): ");
                    string input = Console.ReadLine();
                    if (input.ToLower() == "c")
                    {
                        Console.Write("Enter disk name: ");
                        string diskName = Console.ReadLine();
                        CreateDisk(diskName);
                    }
                    else if (int.TryParse(input, out int diskIndex))
                    {
                        SetCurrentDisk(diskIndex);
                    }
                    Console.Clear();
                    continue;
                }

                Console.WriteLine("File Manager Menu:");

                if (currentDisk.GetCurrentCatalog() is Folder)
                {
                    FolderMenu();
                }
                else if (currentDisk.GetCurrentCatalog() is VirtualFile)
                {
                    Console.WriteLine("You are currently in a file.");
                    FileMenu();
                }
                else
                {
                    Console.WriteLine("You are at the root of the disk.");
                    FolderMenu();
                }

                Console.Clear();
            }

        }

        public void FolderMenu()
        {
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
        }

        public void FileMenu()
        {
            Console.WriteLine("ESC - Go Back");
            Console.Write("Current catalog: ");
            currentDisk.GetCurrentCatalog().PrintName();
            Console.WriteLine("Content: ");
            PrintCurrentContent();

            Console.Write("Enter option: ");
            ConsoleKey option = Console.ReadKey(true).Key;
            Console.WriteLine();

            switch (option)
            {
                case ConsoleKey.Escape:
                    GoBack();
                    break;

                case ConsoleKey.W:
                    WriteToFile();
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.Clear();
        }

        void WriteToFile()
        {
            if (currentDisk.GetCurrentCatalog() is VirtualFile file)
            {
                Console.WriteLine("Enter data to write to the file: ");



                while (true)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Enter)
                    {
                        file.data += "\n";
                        Console.WriteLine();
                    }
                    else if (key == ConsoleKey.Backspace)
                    {
                        if (file.data.Length > 0)
                        {
                            file.data = file.data.Substring(0, file.data.Length - 1);
                            Console.Write("\b \b");
                        }
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else
                    {
                        bool isCapsLock = Console.CapsLock;

                        if (isCapsLock)
                        {
                            file.data += char.ToUpper((char)key);
                            Console.Write(char.ToUpper((char)key));
                            continue;
                        }
                        else
                        {

                            file.data += char.ToLower((char)key);
                            Console.Write(char.ToLower((char)key));
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Current catalog is not a file.");
            }
        }
    }
}
