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
        public string PathTo = "/"; // PathTo always ends in "/"
        public List<FileSystemNode> Children;
        public File Contents;
        public FileSystemNode(string path, string name) { 
            this.PathTo = path;
            this.Name = name;
        }

        public FileSystemNode(string path, string name, File file)
        {
            this.PathTo = path;
            this.Name = name;
            this.Contents = file;
        }

        public FileSystemNode(string path, string name, List<FileSystemNode> children)
        {
            this.PathTo = path;
            this.Name = name;
            this.Children = children;
        }

        private void InitializeChildrenList()
        {
            this.Children = new List<FileSystemNode>();
        }

        public string GetPath()
        {
            return PathTo + Name + "/"; // PathTo always ends in "/"
        }

        public List<string> GetChildrenNameList()
        {
            List<string> nameList = new List<string>();
            foreach (FileSystemNode node in Children) {
                nameList.Add(node.Name);
            }
            return nameList;
        }

        public bool CheckUniqueFileNodeName(string name) 
        {
            if (string.IsNullOrEmpty(name)) throw new NullOrEmptyFileSystemNodeException();

            if (!(this.GetChildrenNameList().Contains(name))) return true;
            else return false;
        }

        public void CreateChild(FileSystemNode node)
        {
            if (!this.Children.Any())
            {
                InitializeChildrenList();
            }
            if (!CheckUniqueFileNodeName(node.Name))
            {
                // TODO: Handle non unique folder names
            }
            else
            {
                this.Children.Add(node);
            }
        }

        public void DeleteChild(string name)
        {
            if (!this.Children.Any() || !this.GetChildrenNameList().Contains(name))
            {
                // TODO: Handle non existent delete request
            }
            this.Children.RemoveAll(node => node.Name == name);
        }

        public void CreateFolder(string folderName)
        {
            this.CreateChild(new FileSystemNode(this.GetPath(), folderName));
        }

        public void DeleteFolder(string folderName) {
            this.DeleteChild(folderName);
        }

        public void CreateFile(string fileName)
        {
            this.CreateChild(new FileSystemNode(this.GetPath(), fileName, new File()));
        }

        public void CreateFile(string fileName, File fileContents)
        {
            this.CreateChild(new FileSystemNode(this.GetPath(), fileName, fileContents));
        }

        public void DeleteFile(string fileName) {
            this.DeleteChild(fileName);
        }
    }
}
