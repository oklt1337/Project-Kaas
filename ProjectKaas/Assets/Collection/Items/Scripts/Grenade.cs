using System.Linq;
using Collection.Maps.Scripts;
using Photon.Pun;
using UnityEngine;

namespace Collection.Items.Scripts
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private float range;
        
        private void OnCollisionEnter()
        {
            // Checks every distance to every player to decide hit.
            foreach (var player in PositionManager.PositionManagerInstance.AllPlayers.Where(player => (player.transform.position - transform.position).magnitude < range))
            {
                player.Car.OnHit();
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
