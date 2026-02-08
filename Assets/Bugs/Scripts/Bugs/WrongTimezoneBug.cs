namespace SystemReboot.Bugs
{
    public class WrongTimezoneBug : Bug
    {
        public WrongTimezoneBug() : base("My clock is 5 hours behind") { }

        public override void InitBug()
        {
            Systems.System.Main.SystemVariables.SetVariable("TIMEZONE", "us");
        }

        public override bool CheckComplete()
        {
            return Systems.System.Main.SystemVariables.GetVariable("TIMEZONE").Item2 == "uk";
        }
    }
}