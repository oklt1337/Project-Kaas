using UnityEngine;

namespace _Project.Scripts.Utility
{
    public class DontDoL : MonoBehaviour
    {
        private void Awake()
        { 
            DontDestroyOnLoad (transform.gameObject);
        }
    }
}
