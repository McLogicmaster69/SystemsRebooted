using System.Collections;
using UnityEngine;

namespace Assets.Computer
{
    public class File : FileSystemNode
    {
        // public string FileType; potential field idea
        // TODO: Add some sort of file content
        // public Contents;
        public File(string path, string name) : base(path, name) { }
        public File(File file) : base(file.PathTo, file.Name) { }
        //public File(string fileType)
        //{
        //    this.FileType = fileType;
        //}
    }
}