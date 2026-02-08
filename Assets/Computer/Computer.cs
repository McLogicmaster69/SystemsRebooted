using Assets.Computer;
using System.Linq;
using UnityEngine;

public class Computer 
{
    Folder fileSystem = new Folder("/", "root");
    public Computer()
    {

    }

    public void CopyFileNode(string pathToFileNode, string pathToDestinationFolder)
    {
        FileSystemNode fileNode = fileSystem.GetChildByPath(pathToFileNode);
        if (fileNode != null) { 
            Folder destinationFolder = (Folder) fileSystem.GetChildByPath(pathToDestinationFolder);
            if (destinationFolder != null) {
                destinationFolder.CreateChild(fileNode);
            }
        }
    }


    public void MoveFileNode(string pathToFileNode, string pathToDestinationFolder)
    {
        Folder oldFolder = (Folder) fileSystem.GetChildByPath(string.Join("/", pathToFileNode.Split("/").SkipLast(1).ToList())); // removing last search element 
        // Empty string at the end if (/path/) might mess stuff up??
        FileSystemNode fileNode = fileSystem.GetChildByPath(pathToFileNode);

        if (oldFolder != null && fileNode != null)
        {
            oldFolder.DeleteChild(fileNode);
            Folder destinationFolder = (Folder) fileSystem.GetChildByPath(pathToDestinationFolder);
            if (destinationFolder != null)
            {
                destinationFolder.CreateChild(fileNode);
            }
        }
    }
}
