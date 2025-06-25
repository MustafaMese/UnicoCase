using System.Threading.Tasks;
using CustomEvent;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private GameServices _services;
    
    public bool _isInitialized = false;
    
    public Task Initialize(GameServices services)
    {
        _services = services;
        
        GameManager.Instance.CustomEvent.AddCustomEventListener<GameStarted>(OnGameStarted);
        
        return Task.CompletedTask;
    }

    private void FixedUpdate()
    {
        if (!_isInitialized)
        {
            return;
        }
        
        var collisionService = _services.ResolveService<ICollisionService, CollisionService>();
        collisionService.Update();
    }

    private void OnGameStarted(GameStarted e)
    {
        _isInitialized = true;
    }
}