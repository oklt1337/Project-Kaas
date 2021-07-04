using System.Linq;
using Photon.Pun;
using UnityEngine;
using static Collection.Maps.Scripts.PositionManager;

namespace Collection.Items.Scripts.Field_Objects
{
    public class GrenadeBehaviour : MonoBehaviourPun
    {
        [SerializeField] private float range;
                
        private void OnCollisionEnter()
        {
            // Checks every distance to every player to decide hit.
            foreach (var player in PositionManagerInstance.AllPlayers.Where(player => (player.transform.position - transform.position).magnitude < range))
            {
                player.Car.OnHit();
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
