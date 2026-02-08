using UnityEngine;

namespace SystemReboot.Clients
{
    public class ClientManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _clientSprites;

        private Animator _animator;

        private void Start()
        {
            _animator = Instantiate(_clientSprites[Random.Range(0, _clientSprites.Length)], new Vector3(999f, 999f, 1f), Quaternion.identity, transform).GetComponent<Animator>();
        }

        public void WalkOut()
        {
            _animator.SetTrigger("End");
        }
    }
}