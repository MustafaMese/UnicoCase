using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameServices : MonoBehaviour
{
    private readonly Dictionary<Type, object> _services = new();
    private readonly Dictionary<Type, object> _dataSets = new();
    public bool IsInitialized { get; set; }
    
    [SerializeField] private LevelRuntimeData _levelRuntimeData;
    [SerializeField] private DefenceItemRuntimeData _defenceItemRuntimeData;
    [SerializeField] private EnemyRuntimeData _enemyRuntimeData;
    
    private SpawnService _spawnService;
    private BoardService _boardService;
    private DefenceItemFactory _defenceItemFactory;
    private EnemyFactory _enemyFactory;
    private CollisionService _collisionService;
    private SceneService _sceneService;

    public async Task Initialize()
    {
        _sceneService = new SceneService();
        
        await _sceneService.Initialize(this);
        await InitializeDataSets();
        await InitializeServices();
    }
    
    
    private async Task InitializeDataSets()
    {
        int levelIndex = _sceneService.CurrentScene;

        await _enemyRuntimeData.Initialize(this);
        await _defenceItemRuntimeData.Initialize(this);
        await _levelRuntimeData.Initialize(this, levelIndex);
    }
    
    private async Task InitializeServices()
    {
        _boardService = new BoardService();
        _defenceItemFactory = new DefenceItemFactory();
        _enemyFactory = new EnemyFactory();
        _collisionService = new CollisionService();
        _spawnService = new SpawnService();

        await Task.WhenAll(
            _boardService.Initialize(this),
            _spawnService.Initialize(this),
            _defenceItemFactory.Initialize(this),
            _enemyFactory.Initialize(this),
            _collisionService.Initialize(this));
        
        IsInitialized = true;
    }

    public void RegisterService<TInterface, TObject>(TObject t)
    {
        _services.Add(typeof(TInterface), t);
    }

    public TObject ResolveService<TInterface, TObject>()
    {
        return (TObject)_services[typeof(TInterface)];
    }

    public void RegisterData<TInterface, TObject>(TObject t)
    {
        _dataSets.Add(typeof(TInterface), t);
    }

    public TObject ResolveData<TInterface, TObject>()
    {
        return (TObject)_dataSets[typeof(TInterface)];
    }
}