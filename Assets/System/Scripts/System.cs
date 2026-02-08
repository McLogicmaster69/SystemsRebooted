using System.Collections;
using SystemReboot.Bugs;
using SystemReboot.Terminal;
using SystemReboot.Computers;
using UnityEngine;

namespace SystemReboot.Systems
{
    public class System : MonoBehaviour
    {
        public static System Main { get; private set; }

        [SerializeField] private GameObject _desktopUI;
        [SerializeField] private GameObject _completeClientButton;

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

            // Default files
            FileSystem.FileSystem.CreateFolderPath("usr/username/desktop");
            FileSystem.FileSystem.CreateFolderPath("usr/username/videos");
            FileSystem.FileSystem.CreateFolderPath("usr/username/photos");
            FileSystem.FileSystem.CreateFolderPath("usr/username/apps");

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

            TerminalUIManager.Main.InitNewTerminal();
            TerminalUIManager.Main.SetInputState(false);
            TerminalUIManager.Main.Print("Booting...");

            yield return new WaitForSeconds(Random.Range(0.4f, 1.2f));

            if (SystemVariables.GetVariable("DESKTOP_STATE").Item2 == "on")
                _desktopUI.SetActive(true);

            yield return new WaitForSeconds(Random.Range(0.4f, 1.2f));
            
            TerminalUIManager.Main.Print("Booting complete");
            TerminalUIManager.Main.SetInputState(true);

            CheckIfBugComplete();
        }
    }
}