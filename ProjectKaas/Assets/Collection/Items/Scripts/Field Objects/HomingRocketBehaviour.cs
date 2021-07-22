using System;
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

        private void Start()
        {
            agent.updateRotation = true;
        }

        private void Update()
        {
            if (target == null)
                return;

            var targetPos = target.transform.position;
            agent.SetDestination(targetPos);

            var relativePos = targetPos - transform.position;

            // the second argument, upwards, defaults to Vector3.up
            var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.localRotation = Quaternion.Euler(rotation.x + 90 , rotation.y , rotation.z);
        }

        /// <summary>
        /// Tells the rocket who shoots it to identify the target.  
        /// </summary>
        /// <param name="user"> Who uses it. </param>
        public void AcquireTarget(PlayerHandler user)
        {
            var nextPlayer = PositionManager.PositionManagerInstance.FindNextPlayer(user);
            target = nextPlayer.Car.gameObject;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Checks tag.
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            // Makes the player tumble on hit.
            var hitPlayer = other.gameObject.GetComponentInParent<PlayerHandler>();
            hitPlayer.Car.OnHit(1f);
            PhotonNetwork.Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
