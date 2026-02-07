using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

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

        private void InitializeChildrenList()
        {
            this.Children = new ArrayList<FileSystemNode>();
        }

        public string GetPath()
        {
            return Path + Name + "/"; // Path always ends in "/"
        }

        public ArrayList<string> GetChildrenNameList()
        {
            ArrayList<string> nameList = ArrayList<string>();
            foreach (FileSystemNode node in Children) {
                nameList.Add(node.Name);
            }
            return nameList;
        }

        public bool CheckUniqueFileNodeName(string name) 
        {
            if (string.IsNullOrEmpty(name)) throw NullOrEmptyFileSystemNodeException;

            if !(name in this.GetChildrenNameList()) return true;
            else return false;
        }

        public void CreateFolder(string folderName)
        {
            if (!this.Children)
            {
                InitializeChildrenList();
            }
            if (!CheckUniqueFileNodeName(folderName))
            {
                // TODO: Handle non unique folder names
            }
            else { 
                this.Children.Add(new FileSystemNode(this.GetPath(), folderName)); 
            }
        }

        public void DeleteFolder(string folderName) {
            this.Children.Remove(folderName);
        }
    }
}
