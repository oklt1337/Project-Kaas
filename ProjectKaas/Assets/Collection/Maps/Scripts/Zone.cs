using Collection.Cars.Scripts;
using Collection.Cars.Scripts.FormulaCar;
using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Maps.Scripts
{
    public class Zone : MonoBehaviour
    {
        [SerializeField] private byte index;
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") && !other.isTrigger)
                return;

            var player = other.gameObject.GetComponentInParent<PlayerHandler>();
            
            // Checks if the zone of the player is one lower then the own count.
            if (player.Car.ZoneCount + 1 != index) 
                return;
            
            player.Car.OnNextZone();
        }
    }
}
