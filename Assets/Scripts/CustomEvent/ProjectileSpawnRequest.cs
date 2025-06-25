using UnityEngine;

namespace CustomEvent
{
    public class ProjectileSpawnRequest : ICustomEvent
    {
        public Projectile prefab;
        public Vector3 position;
        public Vector3 direction;
        public int damage;
        public float range;
    }
}