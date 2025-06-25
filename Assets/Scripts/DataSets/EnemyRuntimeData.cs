using System.Threading.Tasks;
using Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/EnemyRuntimeData")]
public class EnemyRuntimeData : ScriptableObject, IEnemyRuntimeData
{
    [SerializeField] private EnemyData enemyData;

    private GameServices _services;
    
    public Task Initialize(GameServices services)
    {
        _services = services;
        
        _services.RegisterData<IEnemyRuntimeData, EnemyRuntimeData>(this);
        
        return Task.CompletedTask;
    }
    
    public int Health(EnemyType enemyType)
    {
        foreach (var enemy in enemyData.enemies)
        {
            if (enemy.enemyType == enemyType)
            {
                return enemy.health;
            }
        }
       
        throw new System.Exception($"Enemy type {enemyType} not found in EnemyData.");
    }
    
    public float Speed(EnemyType enemyType)
    {
        foreach (var enemy in enemyData.enemies)
        {
            if (enemy.enemyType == enemyType)
            {
                return enemy.speed;
            }
        }
        
        throw new System.Exception($"Enemy type {enemyType} not found in EnemyData.");
    }
    
    public int Damage(EnemyType enemyType)
    {
        foreach (var enemy in enemyData.enemies)
        {
            if (enemy.enemyType == enemyType)
            {
                return enemy.damage;
            }
        }
        
        throw new System.Exception($"Enemy type {enemyType} not found in EnemyData.");
    }
    
    public Enemy EnemyPrefab(EnemyType enemyType)
    {
        foreach (var enemy in enemyData.enemies)
        {
            if (enemy.enemyType == enemyType)
            {
                return enemy.enemyPrefab;
            }
        }
        
        throw new System.Exception($"Enemy prefab for type {enemyType} not found in EnemyData.");
    }
    
    public EnemyType EnemyType(Enemy enemy)
    {
        foreach (var data in enemyData.enemies)
        {
            if (data.enemyPrefab == enemy)
            {
                return data.enemyType;
            }
        }
        
        throw new System.Exception($"Enemy type for {enemy.name} not found in EnemyData.");
    }

    public float JumpPower(EnemyType type)
    {
        foreach (var enemy in enemyData.enemies)
        {
            if (enemy.enemyType == type)
            {
                return enemy.jumpPower;
            }
        }
        
        throw new System.Exception($"Jump power for enemy type {type} not found in EnemyData.");
    }

    public int JumpCount(EnemyType type)
    {
        foreach (var enemy in enemyData.enemies)
        {
            if (enemy.enemyType == type)
            {
                return enemy.jumpCount;
            }
        }
        
        throw new System.Exception($"Jump count for enemy type {type} not found in EnemyData.");
    }
}

public interface IEnemyRuntimeData
{
    public Task Initialize(GameServices services);
}