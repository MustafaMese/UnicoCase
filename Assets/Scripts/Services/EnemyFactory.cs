using System.Threading.Tasks;
using Enemies;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyFactory : IEnemyFactory
{
    private GameServices _services;

    public Task Initialize(GameServices services)
    {
        _services = services;
        _services.RegisterService<IEnemyFactory, EnemyFactory>(this);
        return Task.CompletedTask;
    }

    public bool CreateEnemy(EnemyType? type, Vector3 position, int spawnId)
    {
        var enemyData = _services.ResolveData<IEnemyRuntimeData, EnemyRuntimeData>();
        var levelData = _services.ResolveData<ILevelRuntimeData, LevelRuntimeData>();

        if (type != null)
        {
            var prefab = enemyData.EnemyPrefab(type.Value);
            var enemy = Object.Instantiate(prefab);
            enemy.Init(type.Value,
                enemyData.Health(type.Value),
                enemyData.Speed(type.Value),
                enemyData.Damage(type.Value),
                levelData.BlockPrefab().transform.localScale.z,
                position,
                levelData.BoardSize().y,
                spawnId,
                enemyData.JumpPower(type.Value),
                enemyData.JumpCount(type.Value));

            GameManager.Instance.CustomEvent.InvokeCustomEvent(new OnCollisionObjectCreated()
            {
                obj = enemy
            });
            
            return true;
        }

        return false;
    }
}

public interface IEnemyFactory
{
    Task Initialize(GameServices services);
}