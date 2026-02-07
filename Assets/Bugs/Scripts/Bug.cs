using UnityEngine;

namespace SystemReboot.Bugs
{
    public abstract class Bug
    {
        public abstract void InitBug();

        public abstract bool CheckComplete();
    }
}