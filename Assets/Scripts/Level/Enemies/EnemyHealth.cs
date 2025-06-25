public class EnemyHealth
{
    private int _health;
    private Enemy _self;

    public EnemyHealth(Enemy enemy, int health)
    {
        _self = enemy;
        _health = health;
    }

    public bool IsDead()
    {
       return _health <= 0;
    }

    public void TakeHit(int damage)
    {
        _health -= damage;
        if (IsDead())
        {
            _self.OnActionNeeded?.Invoke();
        }
    }
}