using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SystemReboot.Terminal
{
    public static class Interpreter
    {
        private static Dictionary<string, Func<string[], string, InterpreterOutput>> _commands = new Dictionary<string, Func<string[], string, InterpreterOutput>>()
        {
            {"echo", CommandEcho},
            {"time", CommandTime},
            {"pwd", CommandPwd},
            {"addvar", CommandAddVar},
            {"setvar", CommandSetVar},
            {"reboot", CommandReboot},
            {"cls", CommandCls},
            {"ls", CommandLs},
            {"cd", CommandCd}
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
                return InterpreterOutput.Empty;

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
            return new InterpreterOutput
            {
                Text = CombineParamters(parameters, 1),
                State = InterpreterOutputState.Fine
            };
        }

        private static InterpreterOutput CommandTime(string[] parameters, string path)
        {
            string timeZone = Systems.System.Main.SystemVariables.GetVariable("TIMEZONE").Item2;

            string currentTime;

            switch (timeZone)
            {
                case "uk":
                    currentTime = DateTime.Now.ToString();
                    break;
                case "us":
                    currentTime = DateTime.Now.AddHours(-5).ToString();
                    break;
                default:
                    currentTime = "Error unknown timezone";
                    break;
            }

            return new InterpreterOutput
            {
                Text = currentTime,
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

        private static InterpreterOutput CommandAddVar(string[] parameters, string path)
        {
            if (parameters.Length < 2)
            {
                return new InterpreterOutput
                {
                    Text = "Unexpected number of parameters",
                    State = InterpreterOutputState.Error
                };
            }

            if (Systems.System.Main.SystemVariables.ContainsVariable(parameters[1]))
            {
                return new InterpreterOutput
                {
                    Text = "System already contains variable",
                    State = InterpreterOutputState.Error
                };
            }

            string value =  CombineParamters(parameters, 2);
            Systems.System.Main.SystemVariables.SetVariable(parameters[1], value);
            return new InterpreterOutput
            {
                Text = $"Added variable {parameters[1]} with value \"{value}\""
            };
        }

        private static InterpreterOutput CommandSetVar(string[] parameters, string path)
        {
            if (parameters.Length < 3)
            {
                return new InterpreterOutput
                {
                    Text = "Unexpected number of parameters",
                    State = InterpreterOutputState.Error
                };
            }

            if (!Systems.System.Main.SystemVariables.ContainsVariable(parameters[1]))
            {
                return new InterpreterOutput
                {
                    Text = "Unknown system variable",
                    State = InterpreterOutputState.Error
                };
            }

            string value =  CombineParamters(parameters, 2);
            Systems.System.Main.SystemVariables.SetVariable(parameters[1], value);
            return new InterpreterOutput
            {
                Text = $"Set variable {parameters[1]} to value \"{value}\""
            };
        }

        private static InterpreterOutput CommandReboot(string[] parameters, string path)
        {
            return new InterpreterOutput
            {
                Reboot = true
            };
        }

        private static InterpreterOutput CommandCls(string[] parameters, string path)
        {
            return new InterpreterOutput
            {
                Clear = true
            };
        }

        private static InterpreterOutput CommandLs(string[] parameters, string path)
        {
            if (parameters.Length > 2)
                return new InterpreterOutput
                {
                    Text = "Error unexpected amount of parameters",
                    State = InterpreterOutputState.Error
                };

            List<string> nodes;
            string outputPath;

            if (parameters.Length == 1)
            {
                nodes = Systems.System.Main.FileSystem.FileSystem.GetChildrenNameListByPath(path);
                outputPath = path;
            }
            else if (parameters[1] == "/")
            {
                nodes = Systems.System.Main.FileSystem.FileSystem.GetChildrenNameList();
                outputPath = "/";
            }
            else
            {
                nodes = Systems.System.Main.FileSystem.FileSystem.GetChildrenNameListByPath(path + parameters[1]);
                outputPath = path + parameters[1];
            }

            if (nodes.Count == 0)
                return new InterpreterOutput
                {
                    Text = $"No files or folders found in \"{outputPath}\""
                };
            
            string output = $"Files and folders in \"{outputPath}\":\n";
            for (int i = 0; i < nodes.Count; i++)
            {
                if (i == nodes.Count - 1)
                    output += $"- {nodes[i]}";
                else
                    output += $"- {nodes[i]}\n";
            }

            return new InterpreterOutput
            {
                Text = output,
                State = InterpreterOutputState.Fine
            };
        }

        private static InterpreterOutput CommandCd(string[] parameters, string path)
        {
            if (parameters.Length != 2)
                return new InterpreterOutput
                {
                    Text = "Error unexpected amount of parameters",
                    State = InterpreterOutputState.Error
                };

            if (parameters[1] == ".")
                return InterpreterOutput.Empty;

            if (parameters[1] == "..")
                return new InterpreterOutput
                {
                    ChangedDirectory = true,
                    NewPath = CombinePath(path, "..")
                };

            if (parameters[1] == "/")
                return new InterpreterOutput
                {
                    ChangedDirectory = true,
                    NewPath = "/"
                };

            if (Systems.System.Main.FileSystem.FileSystem.IsValidPath(path + parameters[1]))
                return new InterpreterOutput
                {
                    ChangedDirectory = true,
                    NewPath = CombinePath(path, parameters[1])
                };
                
            return new InterpreterOutput
            {
                Text = "Error invalid path",
                State = InterpreterOutputState.Error
            };
        }

        private static string CombinePath(string path1, string path2)
        {
            List<string> path = new List<string>();

            foreach (string s in path1.Split('/'))
            {
                if (!string.IsNullOrEmpty(s))
                    path.Add(s);
            }
            
            Debug.Log($"{path1} :: {path2}");

            foreach(string s in path2.Split('/'))
            {
                if (s == "." || string.IsNullOrEmpty(s))
                    continue;
                if (s == "..")
                    path.RemoveAt(path.Count - 1);
                else
                    path.Add(s);
            }

            string combined = "";

            foreach (string s in path)
            {
                combined += $"{s}/";
            }

            return $"/{combined}";
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
                        (bool, string) systemVariable = Systems.System.Main.SystemVariables.GetVariable(currentParameter);
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

        private static string CombineParamters(string[] parameters, int startIndex)
        {
            string output = "";

            for(int i = startIndex; i < parameters.Length; i++)
            {
                output += parameters[i];
                if (i + 1 != parameters.Length)
                    output += " ";
            }

            return output;
        }
    }
}