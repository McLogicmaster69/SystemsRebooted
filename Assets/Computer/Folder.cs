using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SystemReboot.Computers
{
    public class Folder : FileSystemNode
    {
        public List<FileSystemNode> Children;

        public Folder(string path, string name, Folder parent) : base(path, name, parent)
        {
            this.Children = new List<FileSystemNode>();
        }

        public Folder(string path, string name, List<FileSystemNode> children, Folder parent) : base(path, name, parent)
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

        public bool ContainsFile(string name)
        {
            return this.GetChildrenNameList().Contains(name);
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
            this.CreateChild(new Folder(this.GetPath(), folderName, this));
        }

        public void CreateFolderPath(string path)
        {
            string[] folders = path.Split('/');

            if (folders.Length == 1)
            {
                if (!this.CheckUniqueFileNodeName(folders[0]))
                    CreateFolder(folders[0]);
                return;
            }

            Folder folder = (Folder)this.GetChildByName(folders[0]);

            if (folder == null)
            {
                folder = new Folder(this.GetPath(), folders[0], this);
                this.CreateChild(folder);
            }

            folder.CreateFolderPath(folders, 1);
        }

        public void CreateFolderPath(string[] folders, int index)
        {
            if (index == folders.Length)
                return;

            Folder folder = (Folder)this.GetChildByName(folders[index]);

            if (folder == null)
            {
                folder = new Folder(this.GetPath(), folders[index], this);
                this.CreateChild(folder);
            }

            folder.CreateFolderPath(folders, index + 1);
        }

        public List<string> GetChildrenNameListByPath(string path)
        {
            if (path == "/" || path == ".")
                return this.GetChildrenNameList();

            string[] folders = path.Split('/');
            int index = path[0] == '/' ? 1 : 0;
            
            if (folders[index] == ".")
                return this.GetChildrenNameListByPath(folders, index + 1);

            if (folders[index] == "..")
            {                
                if (Parent == null)
                    return new List<string>();
                return Parent.GetChildrenNameListByPath(folders, index + 1);
            }

            Folder folder = (Folder)this.GetChildByName(folders[index]);
            if (folder == null)
                return new List<string>();

            return folder.GetChildrenNameListByPath(folders, index + 1);
        }

        public List<string> GetChildrenNameListByPath(string[] folders, int index)
        {
            if (index == folders.Length)
                return this.GetChildrenNameList();
            if (index == folders.Length - 1 && string.IsNullOrWhiteSpace(folders[index]))
                return this.GetChildrenNameList();
            if (folders[index] == ".")
                return this.GetChildrenNameListByPath(folders, index + 1);

            if (folders[index] == "..")
            {
                if (Parent == null)
                    return new List<string>();
                return Parent.GetChildrenNameListByPath(folders, index + 1);
            }
            
            Folder folder = (Folder)this.GetChildByName(folders[index]);
            if (folder == null)
                return new List<string>();

            return folder.GetChildrenNameListByPath(folders, index + 1);
        }

        public bool IsValidPath(string path)
        {
            if (string.IsNullOrEmpty(path) || path == "/" || path == "." || path == "..")
                return true;

            string[] folders = path.Split('/');
            int index = path[0] == '/' ? 1 : 0;
            
            return IsValidPath(folders, index);
        }

        public bool IsValidPath(string[] folders, int index)
        {
            
            if (index == folders.Length)
                return true;

            if (index == folders.Length - 1 && string.IsNullOrWhiteSpace(folders[index]))
                return true;

            if (folders[index] == ".")
                return this.IsValidPath(folders, index + 1);

            if (folders[index] == "..")
            {
                if (Parent == null)
                    return false;
                return Parent.IsValidPath(folders, index + 1);
            }

            if (string.IsNullOrEmpty(folders[index]))
                return false;

            if (!ContainsFile(folders[index]))
                return false;

            return ((Folder)this.GetChildByName(folders[index])).IsValidPath(folders, index + 1);
        }

        public void DeleteFolder(string folderName)
        {
            this.DeleteChild(folderName);
        }

        public void CreateFile(string fileName)
        {
            this.CreateChild(new File(this.GetPath(), fileName, this));
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
