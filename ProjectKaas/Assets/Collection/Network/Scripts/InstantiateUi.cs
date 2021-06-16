using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class InstantiateUi : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private PlayerInputHandler inputHandler;
        private Transform _parent;

        private void Awake()
        {
            _parent = GameObject.FindWithTag("Canvases").transform;

            var ui = Instantiate(prefab, _parent);
            inputHandler.Initialize(ui);
        }
    }
}
