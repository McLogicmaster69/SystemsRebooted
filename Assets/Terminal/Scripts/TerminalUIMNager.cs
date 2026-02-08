using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SystemReboot.Terminal
{
    public class TerminalUIMNager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _textOutput;

        private string _path = "/";

        void Start()
        {
            Systems.System.InitNewSystem();
        }

        public void OnEditFinish(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                string input = _inputField.text;
                _inputField.text = string.Empty;

                InterpreterOutput interpreterOutput = Interpreter.Interpret(input, _path);

                _textOutput.text += $"{interpreterOutput.Text}\n";

                if (interpreterOutput.ChangedDirectory)
                    _path = interpreterOutput.NewPath;

                EventSystem.current.SetSelectedGameObject(_inputField.gameObject);
                _inputField.ActivateInputField();
            }
        }
    }
}