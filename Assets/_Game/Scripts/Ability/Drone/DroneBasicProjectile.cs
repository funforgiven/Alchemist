using System;
using UnityEngine;

namespace Ability.Drone
{
    public class DroneBasicProjectile : MonoBehaviour
    {
        public float speed = 100;
        public float damage = 5;
        public float lifetime = 5;

        private void Update()
        {
            var movement = Vector3.forward * speed * Time.deltaTime;
            transform.Translate(movement, Space.Self);

            lifetime -= Time.deltaTime;
            if(lifetime <= 0)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var stats = other.GetComponent<PlayerStats>();
            if (!stats) return;
            
            stats.ModifyHealthOffset(-damage);
            Destroy(gameObject);
        }
    }
}