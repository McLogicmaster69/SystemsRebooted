using SystemReboot.Computers;

namespace SystemReboot.Bugs
{
    public class WrongLavaVariableBug : Bug
    {
        public WrongLavaVariableBug() : base("My Lava programs are not working") { }

        public override void InitBug()
        {
            Systems.System.Main.SystemVariables.SetVariable("LAVA", "/usr/lava");
        }

        public override bool CheckComplete()
        {
            Folder folder = Systems.System.Main.FileSystem.FileSystem.GetFolderFromPath(Systems.System.Main.SystemVariables.GetVariable("LAVA").Item2);
        
            if (folder == null)
                return false;

            return folder.ContainsFile("lava.exe");
        }
    }
}