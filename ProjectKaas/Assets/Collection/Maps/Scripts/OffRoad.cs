using System;
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
            
            playerHandler.Car.ChangeSpeedState(SpeedState.Slowed);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var playerHandler = other.GetComponentInParent<PlayerHandler>();
            
            playerHandler.Car.ChangeSpeedState(SpeedState.None);
        }
    }
}
