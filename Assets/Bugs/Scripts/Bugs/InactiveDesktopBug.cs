namespace SystemReboot.Bugs
{
    public class InactiveDesktopBug : Bug
    {
        public InactiveDesktopBug() : base("My desktop is not showing") { }

        public override void InitBug()
        {
            Systems.System.Main.SystemVariables.SetVariable("DESKTOP_STATE", "off");
        }

        public override bool CheckComplete()
        {
            return Systems.System.Main.SystemVariables.GetVariable("DESKTOP_STATE").Item2 == "on";
        }
    }
}