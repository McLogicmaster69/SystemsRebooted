using System.Linq;
using UnityEngine;

namespace SystemReboot.Computers
{
    public class Computer 
    {
        public Folder FileSystem { get; private set; }

        public Computer()
        {
            FileSystem = new Folder("/", "root", null);
        }

        public void CopyFileNode(string pathToFileNode, string pathToDestinationFolder)
        {
            FileSystemNode fileNode = FileSystem.GetChildByPath(pathToFileNode);
            if (fileNode != null) { 
                Folder destinationFolder = (Folder) FileSystem.GetChildByPath(pathToDestinationFolder);
                if (destinationFolder != null) {
                    destinationFolder.CreateChild(fileNode);
                }
            }
        }


        public void MoveFileNode(string pathToFileNode, string pathToDestinationFolder)
        {
            Folder oldFolder = (Folder) FileSystem.GetChildByPath(string.Join("/", pathToFileNode.Split("/").SkipLast(1).ToList())); // removing last search element 
            // Empty string at the end if (/path/) might mess stuff up??
            FileSystemNode fileNode = FileSystem.GetChildByPath(pathToFileNode);

            if (oldFolder != null && fileNode != null)
            {
                oldFolder.DeleteChild(fileNode);
                Folder destinationFolder = (Folder) FileSystem.GetChildByPath(pathToDestinationFolder);
                if (destinationFolder != null)
                {
                    destinationFolder.CreateChild(fileNode);
                }
            }
        }
    }
}