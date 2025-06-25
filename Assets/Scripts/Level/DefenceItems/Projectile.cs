using System;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour, ICollisionObject
{
    private bool _isCollided = false;

    private int _damage;
    private int _id;
    private ICollisionObject _collidedObj;
    public int ID => _id;
    
    private Sequence _sequence;
    
    public void Init(int damage, float range, Vector3 direction, int spawnId)
    {
        _damage = damage;
        _id = spawnId;
        
        var targetPosition = transform.position + direction.normalized * range;
        var speed = range;
        
        _sequence = DOTween.Sequence();
        
        _sequence
            .Append(transform.DOMove(targetPosition, speed).SetEase(Ease.Linear))
            .OnComplete(() => 
            { 
                GameManager.Instance.CustomEvent.InvokeCustomEvent(new OnCollisionObjectDestroyed()
                {
                    obj = this
                });
                
                gameObject.SetActive(false);
            });
    }

    public Transform Transform => transform;
    public CollisionType CollisionType => CollisionType.Projectile;
    public void OnCollision(ICollisionObject other)
    {
        if(_isCollided) return;
        
        _isCollided = true;
        _collidedObj = other;
        
    }
    public void ResetValues()
    {
        _isCollided = false;
        _collidedObj = null;
    }
    public void TriggerCollision()
    {
        if(!_isCollided) return;
        
        var targetHealth = _collidedObj as IHealth;
        targetHealth?.TakeHit(_damage);
        gameObject.SetActive(false);
        _sequence?.Kill();
        GameManager.Instance.CustomEvent.InvokeCustomEvent(new OnCollisionObjectDestroyed()
        {
            obj = this
        });
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }
}