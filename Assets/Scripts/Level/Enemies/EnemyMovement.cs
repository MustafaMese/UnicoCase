using DG.Tweening;
using UnityEngine;

public class EnemyMovement
{
    private Enemy _self;
    private float _speed;
    private float _blockWidth;

    private bool _isMoving = false;
    private Sequence _sequence;
    private float _jumpPower;
    private int _jumpCount;

    public EnemyMovement(Enemy enemy, float speed, float blockWidth, float jumpPower, int jumpCount)
    {
        _self = enemy;
        _speed = speed;
        _blockWidth = blockWidth;
        _jumpPower = jumpPower;
        _jumpCount = jumpCount;
    }

    public void Jump()
    {
        if(_isMoving) return;
        
        _isMoving = true;
        float jumpDistance = 0.25f;
        float duration = jumpDistance / _speed * _blockWidth;
        var targetPosition = _self.transform.position + Vector3.forward * jumpDistance;

        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        
        _sequence.Append(
                _self.transform.DOJump(targetPosition, _jumpPower, _jumpCount, duration)
                .SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                _isMoving = false;
                _self.OnActionNeeded?.Invoke();
            });
    }

    public void StopMovement()
    {
        _sequence?.Kill();
        _isMoving = false;
    }

    public void OnDestroy()
    {
        _sequence?.Kill();
    }
}