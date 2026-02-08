using SystemReboot.Computers;

namespace SystemReboot.Bugs
{
    public class MissingLavaVariableBug : Bug
    {
        public MissingLavaVariableBug() : base("My Lava programs are not working") { }

        public override void InitBug()
        {
            Systems.System.Main.SystemVariables.RemoveVariable("LAVA");
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