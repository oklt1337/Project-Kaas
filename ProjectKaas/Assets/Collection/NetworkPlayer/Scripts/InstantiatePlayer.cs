using Collection.Network.Scripts;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    public class InstantiatePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        private void Awake()
        {
            MasterManager.NetworkInstantiation(prefab, transform.position, Quaternion.identity);
        }
    }
}
