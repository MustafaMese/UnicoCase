using System.Threading.Tasks;
using CustomEvent;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameServices _services;
    private bool _isInitialized;
    private float _spawnInterval;
    private float _duration;
    private bool _isLimitExceed;

    private bool _allEnemiesCreated;
    private int _enemiesCreatedCount;
    private int _deadEnemyCount;

    public async Task Initialize(GameServices services)
    {
        _services = services;
        _spawnInterval = _services.ResolveData<ILevelRuntimeData, LevelRuntimeData>().EnemySpawnInterval();
        _duration = 0;
        
        GameManager.Instance.CustomEvent.AddCustomEventListener<GameStarted>(OnGameStarted);
        GameManager.Instance.CustomEvent.AddCustomEventListener<EnemyDeath>(OnEnemyDeath);
        await Task.CompletedTask;
    }

    private void OnEnemyDeath(EnemyDeath e)
    {
        _deadEnemyCount++;
        
        if(!_isLimitExceed) return;

        var enemyCount = _services.ResolveData<ILevelRuntimeData, LevelRuntimeData>().Enemies().Count;
        if(enemyCount <= _deadEnemyCount)
        {
            GameManager.Instance.CustomEvent.InvokeCustomEvent(new GameEnd()
            {
                win = true
            });
        }
    }

    private void Update()
    {
        if(!_isInitialized) return;

        _duration -= Time.deltaTime;
        if (_duration <= 0)
        {
            _duration = _spawnInterval;
            
            var spawnService = _services.ResolveService<ISpawnService, SpawnService>();
            _isLimitExceed = spawnService.TrySpawnEnemy();
            if (_isLimitExceed)
            {
                _isInitialized = false;
            }
        }
    }

    private void OnGameStarted(GameStarted e)
    {
        var data = _services.ResolveData<ILevelRuntimeData, LevelRuntimeData>();
        
        _spawnInterval = data.EnemySpawnInterval();
        _duration = 0;
        _isInitialized = true;
    }
}