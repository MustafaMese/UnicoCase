using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CollisionService : ICollisionService
{
    private GameServices _services;

    private Dictionary<Vector2Int, List<ICollisionObject>> CollisionObjectsMap { get; } = new();
    private HashSet<(ICollisionObject, ICollisionObject)> CollisionPairs { get; } = new();
    private List<ICollisionObject> DestroyObjects { get; } = new();
    private List<ICollisionObject> CollisionObjectsList { get; } = new();

    public Task Initialize(GameServices services)
    {
        _services = services;
        _services.RegisterService<ICollisionService, CollisionService>(this);

        GameManager.Instance.CustomEvent.AddCustomEventListener<OnCollisionObjectCreated>(CollisionObjectCreated);
        GameManager.Instance.CustomEvent.AddCustomEventListener<OnCollisionObjectDestroyed>(CollisionObjectDestroyed);

        return Task.CompletedTask;
    }

    private void CollisionObjectDestroyed(OnCollisionObjectDestroyed e)
    {
       DestroyObjects.Add(e.obj);
    }

    private void CollisionObjectCreated(OnCollisionObjectCreated e)
    {
        CollisionObjectsList.Add(e.obj);
    }

    public void Update()
    {
        var board = _services.ResolveService<IBoardService, BoardService>();
        
        CreateMap(board);
        HandleCollision();
        DestroyCollisionObjects();
    }

    private void DestroyCollisionObjects()
    {
        foreach (var obj in DestroyObjects)
        {
            CollisionObjectsList.Remove(obj);
        }
        
        DestroyObjects.Clear();
    }

    private void HandleCollision()
    {
        foreach (var key in CollisionObjectsMap.Keys)
        {
            var collisionObjects  = CollisionObjectsMap[key];
            var targetObjects = GetCollisionObjects(key);
            if (targetObjects.Count() <= 1) continue;

            var projectiles = collisionObjects.Where(x => x.CollisionType == CollisionType.Projectile);
            var enemies = collisionObjects.Where(x => x.CollisionType == CollisionType.Enemy);
            
            ProjectileCollision(projectiles, targetObjects);
            EnemyCollision(enemies, targetObjects);
        }
        
        foreach (var obj in CollisionObjectsList)
        {
            obj.TriggerCollision();
        }
    }

    private IEnumerable<ICollisionObject> GetCollisionObjects(Vector2Int key)
    {
        var list = CollisionObjectsMap[key];
        
        if (CollisionObjectsMap.TryGetValue(key + Vector2Int.up, out var objects))
        {
            list.AddRange(objects);
        }
        
        if (CollisionObjectsMap.TryGetValue(key + Vector2Int.down, out objects))
        {
            list.AddRange(objects);
        }
        
        if (CollisionObjectsMap.TryGetValue(key + Vector2Int.left, out objects))
        {
            list.AddRange(objects);
        }
        
        if (CollisionObjectsMap.TryGetValue(key + Vector2Int.right, out objects))
        {
            list.AddRange(objects);
        }
        
        return list.Distinct();
    }

    private void EnemyCollision(IEnumerable<ICollisionObject> enemies, IEnumerable<ICollisionObject> objects)
    {
        foreach (var enemy in enemies)
        {
            var minDistance = float.MaxValue;
            var nearest = default(ICollisionObject);

            foreach (var obj in objects)
            {
                if(obj.ID == enemy.ID || obj.CollisionType == CollisionType.Projectile) continue;
                
                float squaredDistance = (enemy.Transform.position - obj.Transform.position).sqrMagnitude;
                if (squaredDistance < minDistance && obj.Transform.position.z > enemy.Transform.position.z)
                {
                    minDistance = squaredDistance;
                    nearest = obj;
                }
            }
            
            var collisionPair = (enemy, nearest);
            if (nearest != null && minDistance < 1f && CollisionPairs.Add(collisionPair))
            {
                enemy.OnCollision(nearest);
            }
        }
    }

    private void ProjectileCollision(IEnumerable<ICollisionObject> projectiles, IEnumerable<ICollisionObject> objects)
    {
        foreach (var projectile in projectiles)
        {
            var minDistance = float.MaxValue;
            var nearest = default(ICollisionObject);
            
            foreach (var obj in objects)
            {
                if(obj.CollisionType != CollisionType.Enemy) continue;
                
                float squaredDistance = (obj.Transform.position - projectile.Transform.position).sqrMagnitude;
                if (squaredDistance < minDistance)
                {
                    minDistance = squaredDistance;
                    nearest = obj;
                }
            }
            
            var collisionPair = (projectile, nearest);
            if (nearest != null && minDistance < 0.2f && CollisionPairs.Add(collisionPair))
            {
                projectile.OnCollision(nearest);
            }
        }
    }

    private void CreateMap(BoardService board)
    {
        CollisionPairs.Clear();
        CollisionObjectsMap.Clear();
        foreach (var obj in CollisionObjectsList)
        {
            var key = board.GetBlockKey(obj.Transform.position);
            obj.ResetValues();
            if (!CollisionObjectsMap.TryGetValue(key, out var list))
            {
                CollisionObjectsMap[key] = new List<ICollisionObject> { obj };
            }
            else
            {
                list.Add(obj);
            }
        }
    }
}

public interface ICollisionService
{
    Task Initialize(GameServices services);
    void Update();
}