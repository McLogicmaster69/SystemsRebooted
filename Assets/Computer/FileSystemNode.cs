using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace SystemReboot.Computers
{
    public class FileSystemNode
    {
        public Folder Parent;
        public string Name;
        public string PathTo = "/"; // PathTo always ends in "/"

        public FileSystemNode(string path, string name, Folder parent) { 
            this.PathTo = path;
            this.Name = name;
            this.Parent = parent;
        }

        public string GetPath()
        {
            return PathTo + Name + "/"; // PathTo always ends in "/"
        }

    }
}
