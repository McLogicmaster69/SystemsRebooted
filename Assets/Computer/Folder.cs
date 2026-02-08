using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Computer
{
    public class Folder : FileSystemNode
    {
        public List<FileSystemNode> Children;

        public Folder(string path, string name) : base(path, name)
        {
            this.Children = new List<FileSystemNode>();
        }

        public Folder(string path, string name, List<FileSystemNode> children) : base(path, name)
        {
         
            this.Children = children;
        }

        public List<string> GetChildrenNameList()
        {
            List<string> nameList = new List<string>();
            foreach (FileSystemNode node in Children)
            {
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

        public void DeleteChild(FileSystemNode node)
        {
            if (!this.Children.Any() || !this.Children.Contains(node))
            {
                // TODO: Handle non existent delete request
            }
            this.Children.Remove(node);
        }

        public void CreateFolder(string folderName)
        {
            this.CreateChild(new Folder(this.GetPath(), folderName));
        }

        public void DeleteFolder(string folderName)
        {
            this.DeleteChild(folderName);
        }

        public void CreateFile(string fileName)
        {
            this.CreateChild(new File(this.GetPath(), fileName));
        }

        public void CopyFile(File file)
        {
            this.CreateChild(new File(file));
        }

        public void DeleteFile(string fileName)
        {
            this.DeleteChild(fileName);
        }

        public FileSystemNode GetChildByName(string name)
        {
            FileSystemNode child = this.Children.FirstOrDefault(node => node.Name == name);
            if (child != null)
            {
                return child;
            } else
            {
                // TODO: error child not found
                return null;
            }
        }

        public FileSystemNode GetChildByPath(string path)
        {
            string[] splitPath = path.Split('/'); // Empty string at the end??
            FileSystemNode currentItem = this.GetChildByName(splitPath[0]);
            if (splitPath.Length == 1) {
                return currentItem;
            }
            if (currentItem is Folder)
            {
                return ((Folder) currentItem).GetChildByPath(String.Join("/", splitPath.Skip(1).ToList()));
            }
            else
            {
                // TODO: error incorrect path
                return null;
            }
        }
    }
}
