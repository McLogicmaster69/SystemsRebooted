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


        public bool MoveFileNode(string pathToFileNode, string pathToDestinationFolder)
        {
            string[] splitPath = pathToFileNode.Split('/');
            Folder oldFolder = FileSystem.GetFolderFromPath(string.Join("/", splitPath.SkipLast(1).ToList()));

            FileSystemNode fileNode = oldFolder.GetChildByName(pathToFileNode.Split('/')[splitPath.Length - 1]);

            if (oldFolder != null && fileNode != null)
            {
                Folder destinationFolder = FileSystem.GetFolderFromPath(string.Join("/", pathToDestinationFolder.Split("/").SkipLast(1).ToList()));
                if (destinationFolder != null)
                {
                    oldFolder.DeleteChild(fileNode);
                    destinationFolder.CreateChild(fileNode);
                    return true;
                }
            }
            
            return false;
        }

        public bool RemoveFile(string pathToFileNode)
        {
            string[] splitPath = pathToFileNode.Split('/');
            Folder folder = FileSystem.GetFolderFromPath(string.Join("/", splitPath.SkipLast(1).ToList()));

            FileSystemNode fileNode = folder.GetChildByName(pathToFileNode.Split('/')[splitPath.Length - 1]);

            if (folder != null && fileNode != null)
            {
                folder.DeleteChild(fileNode);
                return true;
            }

            return false;
        }
    }
}