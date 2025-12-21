using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileManager fileManager = new FileManager();
            
            fileManager.AddDisk(new Disk("C"));
            fileManager.SetCurrentDisk(0);

            fileManager.CreateFolder("Program Files");
            fileManager.CreateFolder("Users");

            fileManager.Menu();
        }
    }
}
