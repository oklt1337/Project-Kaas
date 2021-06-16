using Photon.Pun;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    public class PlayerInputHandler : MonoBehaviourPun
    { 
        public bool Drive { get; private set; }

        // Update is called once per frame
        void Update()
        {
            Drive = Input.touchCount > 0;
        }
    }
}
