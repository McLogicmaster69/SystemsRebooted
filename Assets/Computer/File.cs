using System.Collections;
using UnityEngine;

namespace Assets.Computer
{
    public class File
    {
        public string Name;
        public string Path;
        public File(string path, string name)
        {
            this.Name = name;
            this.Path = path;
        }


    }
}