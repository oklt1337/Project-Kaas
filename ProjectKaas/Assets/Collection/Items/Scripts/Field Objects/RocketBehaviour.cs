using System;
using Collection.NetworkPlayer.Scripts;
using UnityEngine;

namespace Collection.Items.Scripts.Field_Objects
{
    public class RocketBehaviour : MonoBehaviour
    {
        [SerializeField] private int speed;
        [SerializeField] private Vector3 direction;
        [SerializeField] private Transform thisTransform;

        private void Start()
        {
            var parentTrans = gameObject.GetComponentInParent<Transform>();
            CalculateFlyPath(parentTrans.position);
        }

        public void FixedUpdate()
        {
            thisTransform.position += direction * (Time.deltaTime * speed);
        }

        /// <summary>
        /// Calculates the path 
        /// </summary>
        /// <param name="user"> The user that launches the rocket. </param>
        private void CalculateFlyPath(Vector3 user)
        {
            direction = (thisTransform.position - user).normalized;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player"))
                return;
            
            var hitPlayer = other.gameObject.GetComponent<PlayerHandler>();
        }
    }
}
