using SystemReboot.Computers;

namespace SystemReboot.Bugs
{
    public class TrojanBug : Bug
    {
        public TrojanBug() : base("There is an odd horse on my desktop...") { }

        public override void InitBug()
        {
            Systems.System.Main.FileSystem.FileSystem.CreateFolderPath("etc/horse");

            Systems.System.Main.FileSystem.FileSystem.GetFolderFromPath("etc/horse").CreateFile("trojan.exe");

            Systems.System.Main.SystemVariables.SetVariable("BACKGROUND_INDEX", "1");
        }


        public override bool CheckComplete()
        {
            Folder folder = Systems.System.Main.FileSystem.FileSystem.GetFolderFromPath("etc/horse");
        
            if (folder == null)
                return true;

            return !folder.ContainsFile("trojan.exe");
        }
    }
}