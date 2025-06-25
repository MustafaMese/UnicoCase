using System.Linq;
using DG.Tweening;
using UnityEngine;

public class EnemyAttack
{
    private int _damage;
    private float _interval;
    private Enemy _self;
    private Transform[] _attackPath;
    private Transform _tip;
    
    private Sequence _sequence;
    private bool _isAttacking;

    public EnemyAttack(Enemy enemy, Transform[] attackPath, Transform tip, int damage, float interval)
    {
        _self = enemy;
        _damage = damage;
        _interval = interval;
        _attackPath = attackPath;
        _tip = tip;
        _isAttacking = false;
    }

    public void Attack(ICollisionObject collidedObj)
    {
        if(_isAttacking) return;
        
        _isAttacking = true;
        var path = _attackPath.Select(t => t.position).ToArray();
        
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        
        var targetHealth = collidedObj as IHealth;
        _sequence.Append(_tip.DOPath(path, _interval, PathType.CatmullRom))
            .AppendInterval(0.2f)
            .OnComplete(() =>
            {
                _isAttacking = false;
                targetHealth?.TakeHit(_damage);
                _self.OnActionNeeded?.Invoke();
            });
    }

    public void StopAttack()
    {
        _sequence?.Kill();
        _isAttacking = false;
    }
    
    public void OnDestroy()
    {
        _sequence?.Kill();
    }
}