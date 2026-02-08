namespace SystemReboot.Bugs
{
    public class ExampleBug : Bug
    {
        public ExampleBug() : base("") { }

        public override void InitBug()
        {
            
        }

        public override bool CheckComplete()
        {
            return true;
        }
    }
}