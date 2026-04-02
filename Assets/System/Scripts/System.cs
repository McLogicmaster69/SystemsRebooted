using System.Collections;
using SystemReboot.Bugs;
using SystemReboot.Terminal;
using SystemReboot.Computers;
using UnityEngine;
using UnityEngine.UI;

namespace SystemReboot.Systems
{
    public class System : MonoBehaviour
    {
        public static System Main { get; private set; }

        [SerializeField] private GameObject _desktopUI;
        [SerializeField] private GameObject _completeClientButton;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Sprite[] _backgrounds;

        public SystemVariables SystemVariables { get; private set; }
        public Computer FileSystem { get; private set; }

        private Bug _bug;

        private void Awake()
        {
            Main = this;
        }

        public void InitNewSystem(Bug bug)
        {
            SystemVariables = new SystemVariables();
            FileSystem = new Computer();

            // Default Variables
            SystemVariables.SetVariable("DESKTOP_STATE", "on");
            SystemVariables.SetVariable("TIMEZONE", "uk");
            SystemVariables.SetVariable("LAVA", "/etc/lava");
            SystemVariables.SetVariable("BACKGROUND_INDEX", "0");

            // Default files
            FileSystem.FileSystem.CreateFolderPath("usr/username/desktop");
            FileSystem.FileSystem.CreateFolderPath("usr/username/videos");
            FileSystem.FileSystem.CreateFolderPath("usr/username/photos");
            FileSystem.FileSystem.CreateFolderPath("usr/username/apps");

            FileSystem.FileSystem.CreateFolderPath("etc/lava");
            FileSystem.FileSystem.GetFolderFromPath("etc/lava").CreateFile("lava.exe");

            // Inject bug
            _bug = bug;
            bug.InitBug();

            // Boot System
            BootSystem();
        }

        public void BootSystem()
        {
            StartCoroutine(BootRoutine());
        }

        private void CheckIfBugComplete()
        {
            if (_bug.CheckComplete())
                _completeClientButton.SetActive(true);      
        }

        private IEnumerator BootRoutine()
        {
            _desktopUI.SetActive(false);

            _backgroundImage.sprite = null;
            TerminalUIManager.Main.InitNewTerminal();
            TerminalUIManager.Main.SetInputState(false);
            TerminalUIManager.Main.Print("Booting...");

            yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));

            if (SystemVariables.GetVariable("DESKTOP_STATE").Item2 == "on")
                _desktopUI.SetActive(true);

            yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));

            string s = SystemVariables.GetVariable("BACKGROUND_INDEX").Item2;

            if (int.TryParse(s, out int background_index))
            {
                if (background_index >= 0 && background_index < _backgrounds.Length)
                    _backgroundImage.sprite = _backgrounds[background_index];
                else
                    TerminalUIManager.Main.Print("Error loading background");
            }
            else
                TerminalUIManager.Main.Print("Error loading background");

            yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));
            
            TerminalUIManager.Main.Print("Booting complete");
            TerminalUIManager.Main.SetInputState(true);

            CheckIfBugComplete();
        }
    }
}