using CustomEvent;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    [SerializeField] private GameServices _services;
    
    [Header("Helper")]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private BlockSelector _blockSelector;
    [SerializeField] private CollisionManager _collisionManager;
    
    public CustomEventManager CustomEvent;
    
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private async void Initialize()
    {
        
        CustomEvent = new CustomEventManager();
        
        await _services.Initialize();
        await _uiManager.InitializeUI(_services);
        await _enemySpawner.Initialize(_services);
        await _blockSelector.Initialize();
        await _collisionManager.Initialize(_services);
        
        CustomEvent.InvokeCustomEvent(new GameStarted());
        CustomEvent.InvokeCustomEvent(new SelectionStarted());
    }
}