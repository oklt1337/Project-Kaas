using Collection.Maps.Scripts;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Collection.Items.Scripts.Field_Objects
{
    public class HomingRocketBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private NavMeshAgent agent;

        private void FixedUpdate()
        {
            agent.SetDestination(target.transform.position);
        }
        
        /// <summary>
        /// Tells the rocket who shoots it to identify the target.  
        /// </summary>
        /// <param name="user"> Who uses it. </param>
        public void AcquireTarget(PlayerHandler user)
        {
            var nextPlayer = PositionManager.PositionManagerInstance.FindNextPlayer(user);

            target = nextPlayer.gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Checks tag.
            if (!other.gameObject.CompareTag("Player"))
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                // Makes the player tumble on hit.
                var hitPlayer = other.gameObject.GetComponentInParent<PlayerHandler>();
                hitPlayer.Car.OnHit(1f);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
