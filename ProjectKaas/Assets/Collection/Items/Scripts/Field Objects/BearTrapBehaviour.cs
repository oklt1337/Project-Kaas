using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class BearTrapBehaviour : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private AudioClip clip;
    
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 3)
                return;
            
            PhotonNetwork.Destroy(gameObject);

            if (!other.gameObject.CompareTag("Player") || !other.isTrigger) 
                return;
            
            var hitPlayer = other.gameObject.GetComponentInParent<PlayerHandler>();
            hitPlayer.Car.PlayAudioClip(clip);
            hitPlayer.Car.OnHit(duration);
        }
    }
}
