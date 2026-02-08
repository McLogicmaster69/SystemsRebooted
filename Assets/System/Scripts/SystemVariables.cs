using System.Collections.Generic;
using UnityEngine;

namespace SystemReboot.Systems
{
    public class SystemVariables
    {
        private Dictionary<string, string> _variables;

        public SystemVariables()
        {
            _variables = new Dictionary<string, string>();
        }

        public void SetVariable(string name, string value)
        {
            _variables[name] = value;
        }

        public (bool, string) GetVariable(string name)
        {
            if (ContainsVariable(name))
                return (true, _variables[name]);
            else
                return (false, "");
        }

        public bool ContainsVariable(string name) => _variables.ContainsKey(name);

        public void RemoveVariable(string name)
        {
            _variables.Remove(name);
        }
    }
}