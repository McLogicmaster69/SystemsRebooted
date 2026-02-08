using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SystemReboot.Terminal
{
    public class TerminalUIManager : MonoBehaviour
    {
        public static TerminalUIManager Main { get; private set; }

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _textOutput;

        private string _path;

        public const string DEFAULT_PATH = "/";

        private void Awake()
        {
            Main = this;
        }

        public void InitNewTerminal()
        {
            _path = DEFAULT_PATH;
            _textOutput.text = string.Empty;
            _inputField.text = string.Empty;
        }

        public void OnEditFinish(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                string input = _inputField.text;
                _inputField.text = string.Empty;

                InterpreterOutput interpreterOutput = Interpreter.Interpret(input, _path);

                if (interpreterOutput.Reboot)
                {
                    Systems.System.Main.BootSystem();
                    return;
                }

                if (interpreterOutput.Clear)
                    _textOutput.text = string.Empty;

                if (interpreterOutput.State != InterpreterOutputState.Empty)
                    Print(interpreterOutput.Text);

                if (interpreterOutput.ChangedDirectory)
                    _path = interpreterOutput.NewPath;

                EventSystem.current.SetSelectedGameObject(_inputField.gameObject);
                _inputField.ActivateInputField();
            }
        }

        public void Print(string s)
        {            
            _textOutput.text += $"{s}\n";
        }

        public void SetInputState(bool state)
        {
            _inputField.interactable = state;
            if (!state)
                _inputField.text = string.Empty;
        }
    }
}