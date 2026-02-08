using UnityEngine;

namespace SystemReboot.Bugs
{
    public static class BugAssigner
    {
        public static Bug GetRandomBug()
        {
            Bug[] bugs = new Bug[]
            {
                new InactiveDesktopBug(),
                new WrongTimezoneBug()
            };

            return bugs[Random.Range(0, bugs.Length)];
        }
    }
}