using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.Computer
{
    public class FileSystemNode
    {
        public string Name;
        public string PathTo = "/"; // PathTo always ends in "/"

        public FileSystemNode(string path, string name) { 
            this.PathTo = path;
            this.Name = name;
        }

        public string GetPath()
        {
            return PathTo + Name + "/"; // PathTo always ends in "/"
        }

    }
}
