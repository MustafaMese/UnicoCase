using System;
using Enemies;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour, ICollisionObject, IHealth
{
    [SerializeField] private Transform[] _attackPath;
    [SerializeField] private Transform _tip;
    [SerializeField] private Rig _movementRig;
    [SerializeField] private Rig _attackRig;

    private EnemyMovement _enemyMovement;
    private EnemyHealth _enemyHealth;
    private EnemyAttack _enemyAttack;

    private Vector3 _targetPosition;
    private bool _attack;
    private bool _isCollided;
    private ICollisionObject _collidedObj;

    public EnemyType EnemyType { get; private set; }
    public Action OnActionNeeded;

    private int _id;
    public int ID => _id;
    
    public void Init(EnemyType type, int health, float speed, int damage, float blockWidth, Vector3 position, int row,
        int spawnId, float jumpPower, int jumpCount)
    {
        transform.position = position;
        EnemyType = type;
        _id = spawnId;

        _targetPosition = transform.position + row * blockWidth * Vector3.forward;

        _enemyMovement = new EnemyMovement(this, speed, blockWidth, jumpPower, jumpCount);
        _enemyHealth = new EnemyHealth(this, health);
        _enemyAttack = new EnemyAttack(this, _attackPath, _tip, damage, 1.0f);
        
        _attack = false;

        OnActionNeeded += ActionNeeded;
        OnActionNeeded?.Invoke();
    }

    private void ActionNeeded()
    {
        if (CheckHealth())
        {
            DestroyOrder();
            return;
        }

        if (_attack)
        {
            AttackOrder();
            _attack = false;
            return;
        }

        if (IsAtTargetPosition())
        {
            GameManager.Instance.CustomEvent.InvokeCustomEvent(new GameEnd()
            {
                win = false,
            });   
            return;
        }

        MovementOrder();
    }

    private void DestroyOrder()
    {
        GameManager.Instance.CustomEvent.InvokeCustomEvent(new OnCollisionObjectDestroyed()
        {
            obj = this
        });
        
        GameManager.Instance.CustomEvent.InvokeCustomEvent(new EnemyDeath());
        gameObject.SetActive(false);
    }

    private bool CheckHealth()
    {
        return _enemyHealth.IsDead();
    }

    private void AttackOrder()
    {
        _movementRig.weight = 0f;
        _attackRig.weight = 1f;

        _enemyAttack.Attack(_collidedObj);
        _enemyMovement.StopMovement();
    }

    private void MovementOrder()
    {
        _attackRig.weight = 0f;
        _movementRig.weight = 1f;

        _enemyMovement.Jump();
        _enemyAttack.StopAttack();
    }
    
    private bool IsAtTargetPosition()
    {
        return Vector3.Distance(transform.position, _targetPosition) <= 0.1f;
    }

    public void TakeHit(int damage)
    {
        _enemyHealth.TakeHit(damage);
    }
    
    public Transform Transform => transform;
    public CollisionType CollisionType => CollisionType.Enemy;

    public void TriggerCollision()
    {
        if (_isCollided && _collidedObj != null)
        {
            _attack = _collidedObj.CollisionType == CollisionType.DefenceItem; 
        }
        else
        {
            _attack = false;
        }
    }
    
    public void OnCollision(ICollisionObject other)
    {
        _isCollided = true;
        _collidedObj = other;
    }

    public void ResetValues()
    {
        _isCollided = false;
        _collidedObj = null;
        _attack = false;
    }

    private void OnDestroy()
    {
        _enemyAttack.OnDestroy();
        _enemyMovement.OnDestroy();
    }
}