using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class Nitro : ItemBehaviour
    {
        [SerializeField] private float speedUpValue;
        [SerializeField] private float speedUpTime;
        
        public override void OnUse()
        {
            var boostedPlayer = Owner.GetComponent<PlayerHandler>();
            boostedPlayer.ItemState = ItemState.Nitro;
            boostedPlayer.Car.ChangeSpeed(speedUpValue,speedUpTime);
            base.OnUse();
        }
    }
}
