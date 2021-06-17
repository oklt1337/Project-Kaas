using UnityEngine;

namespace Collection.Network.Scripts.Utility
{
    public class DontDestroyOnLoadHandler : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
