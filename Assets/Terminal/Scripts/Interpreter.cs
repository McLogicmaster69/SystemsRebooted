using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemReboot.Terminal
{
    public static class Interpreter
    {
        private static Dictionary<string, Func<string[], string, InterpreterOutput>> _commands = new Dictionary<string, Func<string[], string, InterpreterOutput>>()
        {
            {"echo", CommandEcho},
            {"time", CommandTime},
            {"pwd", CommandPwd}
        };

        public static InterpreterOutput Interpret(string input, string path)
        {
            (bool, string[]) parametersOutput= ConvertToParamters(input);

            if (!parametersOutput.Item1)
                return new InterpreterOutput
                {
                    Text = "Error while parsing",
                    State = InterpreterOutputState.Error
                };

            if (parametersOutput.Item2.Length == 0)
                return new InterpreterOutput
                {
                    State = InterpreterOutputState.Empty
                };

            if (_commands.ContainsKey(parametersOutput.Item2[0]))
            {
                return _commands[parametersOutput.Item2[0]](parametersOutput.Item2, path);
            }
            else
            {
                return new InterpreterOutput
                {
                    Text = "No command found",
                    State = InterpreterOutputState.Error
                };
            }
        }

        private static InterpreterOutput CommandEcho(string[] parameters, string path)
        {
            string output = "";

            for(int i = 1; i < parameters.Length; i++)
            {
                output += $"{parameters[i]} ";
            }

            return new InterpreterOutput
            {
                Text = output,
                State = InterpreterOutputState.Fine
            };
        }

        private static InterpreterOutput CommandTime(string[] parameters, string path)
        {
            return new InterpreterOutput
            {
                Text = DateTime.Now.ToString(),
                State = InterpreterOutputState.Fine
            };
        }

        private static InterpreterOutput CommandPwd(string[] parameters, string path)
        {
            return new InterpreterOutput
            {
                Text = path,
                State = InterpreterOutputState.Fine
            };
        }

        private static (bool, string[]) ConvertToParamters(string input)
        {
            List<string> output = new List<string>();

            string currentParameter = string.Empty;
            bool isString = false;
            bool isVariable = false;

            foreach (char c in input)
            {
                if (c == '%')
                {
                    if (isString)
                    {
                        currentParameter += c.ToString();
                        continue;
                    }

                    if (isVariable)
                    {
                        (bool, string) systemVariable = Systems.System.SystemVariables.GetVariable(currentParameter);
                        output.Add(systemVariable.Item2);
                        currentParameter = string.Empty;
                        isVariable = false;
                        continue;
                    }
                    
                    isVariable = true;
                    continue;
                }

                if (c == '\"')
                {
                    if (isVariable)
                    {
                        currentParameter += c.ToString();
                        continue;
                    }

                    if (isString)
                    {
                        if (!string.IsNullOrEmpty(currentParameter))
                            output.Add(currentParameter);
                        currentParameter = string.Empty;
                        isString = false;
                        continue;
                    }

                    isString = true;
                    continue;
                }

                if (c == ' ')
                {
                    if (isString)
                    {
                        currentParameter += " ";
                        continue;
                    }

                    if (!string.IsNullOrEmpty(currentParameter))
                        output.Add(currentParameter);

                    currentParameter = string.Empty;
                    continue;
                }
                
                currentParameter += c.ToString();
            }

            if (isString)
                return (false, new string[0]);

            if (!string.IsNullOrEmpty(currentParameter))
                output.Add(currentParameter);

            return (true, output.ToArray());
        }
    }
}