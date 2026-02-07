using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Computer
{
    class FileSystemNode
    {
        public string Name;
        public string Path = "/"; // Path always ends in "/"
        public ArrayList<FileSystemNode> Children;
        public File Contents;
        public FileSystemNode(string path, string name) { 
            this.Path = path;
            this.Name = name;
        }

        public FileSystemNode(string path, string name, File file)
        {
            this.Path = path;
            this.Name = name;
            this.Contents = file;
        }

        public FileSystemNode(string path, string name, ArrayList<FileSystemNode> children)
        {
            this.Path = path;
            this.Name = name;
            this.Children = children;
        }

        public GetPath()
        {
            return Path + Name + "/"; // Path always ends in "/"
        }
    }
}
