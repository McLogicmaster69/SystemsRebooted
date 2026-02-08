using System.Collections;
using SystemReboot.Bugs;
using SystemReboot.Clients;
using SystemReboot.Terminal;
using TMPro;
using UnityEngine;

namespace SystemReboot.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _UI;
        [SerializeField] private GameObject _completeClientButton;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private float _walkInTime = 4f;
        [SerializeField] private float _walkOutTime = 4f;
        [SerializeField] private float _timeBetweenCustomers = 2f;
        [SerializeField] private GameObject _clientPrefab;

        private GameObject _clientObject;

        void Start()
        {
            NextCustomer();
        }

        public void NextCustomer()
        {
            if (_clientObject)
                Destroy(_clientObject);

            StartCoroutine(StartRoutine());
        }

        public void CustomerFinished()
        {
            StartCoroutine(EndRoutine());
        }

        private IEnumerator StartRoutine()
        {
            _messageText.text = string.Empty;
            _completeClientButton.SetActive(false);
            _clientObject = Instantiate(_clientPrefab);
            _UI.SetActive(false);
            // Walk in anim
            yield return new WaitForSeconds(_walkInTime);

            Bug bug = BugAssigner.GetRandomBug();

            _messageText.text = bug.Message;
            _UI.SetActive(true);
            Systems.System.Main.InitNewSystem(bug);
        }

        private IEnumerator EndRoutine()
        {
            _messageText.text = string.Empty;
            _completeClientButton.SetActive(false);
            _clientObject.GetComponent<ClientManager>().WalkOut();
            _UI.SetActive(false);
            // Walk out anim
            yield return new WaitForSeconds(_walkOutTime);

            yield return new WaitForSeconds(_timeBetweenCustomers);
            NextCustomer();
        }
    }
}