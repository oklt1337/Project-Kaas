using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class BearTrapBehaviour : MonoBehaviour
    {
        [SerializeField] private float duration;
    
        private void OnCollisionEnter(Collision other)
        {
            PhotonNetwork.Destroy(gameObject);

            if (!other.gameObject.CompareTag("Player")) 
                return;
            
            var hitPlayer = other.gameObject.GetComponent<PlayerHandler>();
            hitPlayer.Car.ChangeSpeed(-hitPlayer.Car.MaxSpeed,duration);
        }
    }
}
