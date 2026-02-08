namespace SystemReboot.Terminal
{
    public struct InterpreterOutput
    {
        public string Text;
        public InterpreterOutputState State;
        public bool ChangedDirectory;
        public string NewPath;
    }
}