using System.Threading.Tasks;
using CustomEvent;
using UnityEngine;
using Object = UnityEngine.Object;

public class DefenceItemFactory : IDefenceItemFactory
{
    private GameServices _services;

    public Task Initialize(GameServices services)
    {
        _services = services;
        _services.RegisterService<IDefenceItemFactory, DefenceItemFactory>(this);
        return Task.CompletedTask;
    }

    public void CreateDefenceItem(DefenceItemType type, Vector3 position, int spawnId)
    {
        var data = _services.ResolveData<IDefenceItemRuntimeData, DefenceItemRuntimeData>();
        var prefab = data.DefenceItemPrefab(type);
        var defenceItem = Object.Instantiate(prefab);
        
        defenceItem.transform.position = position;
        defenceItem.Init(
            data.Health(type), 
            data.Damage(type),
            data.Range(type),
            data.Interval(type),
            data.DirectionType(type),
            data.ProjectilePrefab(),
            spawnId);

        GameManager.Instance.CustomEvent.InvokeCustomEvent(new OnCollisionObjectCreated()
        {
            obj = defenceItem,
        });
    }
}

public interface IDefenceItemFactory
{
    Task Initialize(GameServices services);
}