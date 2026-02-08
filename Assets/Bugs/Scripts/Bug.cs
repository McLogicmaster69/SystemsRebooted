using UnityEngine;

namespace SystemReboot.Bugs
{
    public abstract class Bug
    {
        public readonly string Message;

        public Bug(string message)
        {
            Message = message;
        }

        public abstract void InitBug();

        public abstract bool CheckComplete();
    }
}