using System.Threading.Tasks;
using DefenceItems;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/DefenceItemRuntimeData")]
public class DefenceItemRuntimeData : ScriptableObject, IDefenceItemRuntimeData
{
    [SerializeField] private DefenceItemData defenceItemData;

    private GameServices _services;
    
    public Task Initialize(GameServices services)
    {
        _services = services;
        
        _services.RegisterData<IDefenceItemRuntimeData, DefenceItemRuntimeData>(this);
        
        return Task.CompletedTask;
    }
    
    public int Health(DefenceItemType defenceItemType)
    {
        foreach (var item in defenceItemData.defenceItems)
        {
            if (item.type == defenceItemType)
            {
                return item.health;
            }
        }
        
        throw new System.Exception($"Defence item type {defenceItemType} not found in DefenceItemData.");
    }
    
    public int Damage(DefenceItemType defenceItemType)
    {
        foreach (var item in defenceItemData.defenceItems)
        {
            if (item.type == defenceItemType)
            {
                return item.damage;
            }
        }
       
        throw new System.Exception($"Defence item type {defenceItemType} not found in DefenceItemData.");
    }
    
    public float Range(DefenceItemType defenceItemType)
    {
        foreach (var item in defenceItemData.defenceItems)
        {
            if (item.type == defenceItemType)
            {
                return item.range;
            }
        }
        
        throw new System.Exception($"Defence item type {defenceItemType} not found in DefenceItemData.");
    }
    
    public float Interval(DefenceItemType defenceItemType)
    {
        foreach (var item in defenceItemData.defenceItems)
        {
            if (item.type == defenceItemType)
            {
                return item.interval;
            }
        }
        
        throw new System.Exception($"Defence item type {defenceItemType} not found in DefenceItemData.");
    }
    
    public DefenceItem DefenceItemPrefab(DefenceItemType defenceItemType)
    {
        foreach (var item in defenceItemData.defenceItems)
        {
            if (item.type == defenceItemType)
            {
                return item.defenceItemPrefab;
            }
        }
        
        throw new System.Exception($"Defence item type {defenceItemType} not found in DefenceItemData.");
    }
    
    public DirectionType DirectionType(DefenceItemType defenceItemType)
    {
        foreach (var item in defenceItemData.defenceItems)
        {
            if (item.type == defenceItemType)
            {
                return item.directionType;
            }
        }
       
        throw new System.Exception($"Defence item type {defenceItemType} not found in DefenceItemData.");
    }
    
    public Projectile ProjectilePrefab() => defenceItemData.projectilePrefab;
}

public interface IDefenceItemRuntimeData
{
    public Task Initialize(GameServices services);
}