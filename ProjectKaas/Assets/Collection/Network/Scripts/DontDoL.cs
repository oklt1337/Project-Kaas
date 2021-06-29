using UnityEngine;

namespace Collection.Network.Scripts
{
    public class DontDoL : MonoBehaviour
    {
        private void Awake()
        { 
            DontDestroyOnLoad (transform.gameObject);
        }
    }
}
