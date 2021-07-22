using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Maps.Scripts
{
    public class OffRoad : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var playerHandler = other.GetComponentInParent<PlayerHandler>();

            if (playerHandler.ItemState != ItemState.Nitro)
            {
                playerHandler.Car.ChangeSpeed(-playerHandler.Car.MaxSpeed * 0.5f, 1f);
            }
        }
    }
}
