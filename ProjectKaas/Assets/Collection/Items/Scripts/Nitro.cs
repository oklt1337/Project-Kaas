using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class Nitro : ItemBehaviour
    {
        [SerializeField] private float speedUpTime;
        [SerializeField] private AudioClip clip;
        
        public override void OnUse()
        {
            var boostedPlayer = Owner.GetComponent<PlayerHandler>();
            boostedPlayer.Car.ChangeSpeed(speedUpTime);
            boostedPlayer.Car.PlayAudioClip(clip);
            base.OnUse();
        }
    }
}
