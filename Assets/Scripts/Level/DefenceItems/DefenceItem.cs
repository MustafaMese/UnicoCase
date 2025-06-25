using CustomEvent;
using UnityEngine;

namespace DefenceItems
{
    public class DefenceItem : MonoBehaviour, ICollisionObject, IHealth
    {
        [SerializeField] private Transform _firePoint;

        private int _health;
        private int _damage;
        private float _range;
        private float _interval;
        private DirectionType _directionType;
        private float _time;
        private bool _isActive;
        private Projectile _projectile;

        private int _id;
        public int ID => _id;
        
        public void Init(int health, int damage, float range, float interval, DirectionType directionType,
            Projectile prefab, int spawnId)
        {
            _health = health;
            _damage = damage;
            _range = range;
            _interval = interval;
            _directionType = directionType;
            _time = 0;
            _isActive = true;
            _projectile = prefab;
            _id = spawnId;
        }

        private void Update()
        {
            if (!_isActive)
                return;

            _time -= Time.deltaTime;
            if (_time < 0)
            {
                Fire();
                _time = _interval;
            }
        }

        private void Fire()
        {
            if ((_directionType & DirectionType.Forward) != 0)
            {
                CreateProjectile(-Vector3.forward);
            }

            if ((_directionType & DirectionType.Backward) != 0)
            {
                CreateProjectile(-Vector3.back);
            }

            if ((_directionType & DirectionType.Left) != 0)
            {
                CreateProjectile(Vector3.left);
            }

            if ((_directionType & DirectionType.Right) != 0)
            {
                CreateProjectile(Vector3.right);
            }
        }

        private void CreateProjectile(Vector3 direction)
        {
            GameManager.Instance.CustomEvent.InvokeCustomEvent(new ProjectileSpawnRequest()
            {
                prefab = _projectile,
                position = _firePoint.position,
                direction = direction,
                damage = _damage,
                range = _range
            });
        }

        public void TakeHit(int damage)
        {
            _health -= damage;
            if(_health <= 0)
            {
                _isActive = false;
                gameObject.SetActive(false);
                
                GameManager.Instance.CustomEvent.InvokeCustomEvent(new OnCollisionObjectDestroyed()
                {
                    obj = this
                });
            }
        }
        
        public Transform Transform => transform;
        public CollisionType CollisionType => CollisionType.DefenceItem;
        public void OnCollision(ICollisionObject other) { }
        public void ResetValues() {}
        public void TriggerCollision() {}
    }
}