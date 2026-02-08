using System.Collections;
using UnityEngine;

namespace SystemReboot.Computers
{
    public class File : FileSystemNode
    {
        // public string FileType; potential field idea
        // TODO: Add some sort of file content
        // public Contents;
        public File(string path, string name, Folder parent) : base(path, name, parent) { }
        public File(File file) : base(file.PathTo, file.Name, file.Parent) { }
        //public File(string fileType)
        //{
        //    this.FileType = fileType;
        //}
    }
}