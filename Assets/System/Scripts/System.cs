namespace SystemReboot.Systems
{
    public static class System
    {
        public static SystemVariables SystemVariables { get; private set;}

        public static void InitNewSystem()
        {
            SystemVariables = new SystemVariables();

            // Default Variables
            SystemVariables.SetVariable("DESKTOP_STATE", "on");
        }
    }
}