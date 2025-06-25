using System.Threading.Tasks;
using CustomEvent;
using UnityEngine;

public class SpawnService : ISpawnService
{
    private GameServices _services;
    private int _spawnedEnemyCount;
    private int _spawnId;

    public Task Initialize(GameServices services)
    {
        _services = services;
        _services.RegisterService<ISpawnService, SpawnService>(this);
        _spawnedEnemyCount = 0;
        _spawnId = 0;

        GameManager.Instance.CustomEvent.AddCustomEventListener<SelectionCompleted>(TrySpawnDefenceItem);
        GameManager.Instance.CustomEvent.AddCustomEventListener<ProjectileSpawnRequest>(TrySpawnProjectile);

        return Task.CompletedTask;
    }

    private void TrySpawnProjectile(ProjectileSpawnRequest e)
    {
        _spawnId++;

        var projectile = Object.Instantiate(e.prefab);
        projectile.transform.position = e.position;
        projectile.Init(e.damage, e.range, e.direction, _spawnId);

        GameManager.Instance.CustomEvent.InvokeCustomEvent(new OnCollisionObjectCreated()
        {
            obj = projectile
        });
    }

    private void TrySpawnDefenceItem(SelectionCompleted e)
    {
        var boardService = _services.ResolveService<IBoardService, BoardService>();
        var defenceItemFactory = _services.ResolveService<IDefenceItemFactory, DefenceItemFactory>();
        var position = boardService.GetTilePosition(e.Tile);
        if (boardService.IsDefenceItemPositionValid(e.Tile) && position != null)
        {
            _spawnId++;
            defenceItemFactory.CreateDefenceItem(e.Type, position.Value, _spawnId);
        }
        else
        {
            GameManager.Instance.CustomEvent.InvokeCustomEvent(new SelectionFailed()
            {
                Type = e.Type,
            });
        }
    }

    public bool TrySpawnEnemy()
    {
        var levelData = _services.ResolveData<ILevelRuntimeData, LevelRuntimeData>();
        var boardService = _services.ResolveService<IBoardService, BoardService>();
        var enemyFactory = _services.ResolveService<IEnemyFactory, EnemyFactory>();

        var position = boardService.GetRandomSpawnableEnemyTile();
        if (position != null && levelData.HasEnemies())
        {
            _spawnId++;
            var type = levelData.Enemies()?[_spawnedEnemyCount];
            var success = enemyFactory.CreateEnemy(type, position.Value, _spawnId);
            if (success)
            {
                _spawnedEnemyCount++;
            }
        }

        return _spawnedEnemyCount >= levelData.Enemies().Count;
    }

    public int SpawnedEnemyCount => _spawnedEnemyCount;
}

public interface ISpawnService
{
    Task Initialize(GameServices services);
}