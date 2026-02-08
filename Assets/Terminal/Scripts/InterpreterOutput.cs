namespace SystemReboot.Terminal
{
    public class InterpreterOutput
    {
        public static InterpreterOutput Empty = new InterpreterOutput
        {
            Text = null,
            State = InterpreterOutputState.Empty,
            ChangedDirectory = false,
            NewPath = string.Empty,
            Reboot = false
        };

        public string Text = null;
        public InterpreterOutputState State;
        public bool ChangedDirectory = false;
        public string NewPath;
        public bool Clear = false;
        public bool Reboot = false;
    }
}